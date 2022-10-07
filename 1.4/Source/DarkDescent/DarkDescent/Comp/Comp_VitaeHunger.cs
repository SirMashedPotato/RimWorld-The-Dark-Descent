using RimWorld;
using System;
using Verse;

namespace DarkDescent
{
	class Comp_VitaeHunger : ThingComp
	{
		public CompProperties_VitaeHunger Props
		{
			get
			{
				return (CompProperties_VitaeHunger)this.props;
			}
		}

		private int currentTicks = 0;

		public override void CompTickRare()
		{
			base.CompTickRare();
			Pawn pawn = this.parent as Pawn;
			if (pawn.Dead || !ModSettings_Utility.DarkDescent_SettingVitaeThirst())
			{
				return;
			}
			//check hunger level, consume hediff if too low
			if (pawn.needs.food.CurLevelPercentage <= 0f)
			{
				ConsumeVitae(pawn);
			}
			//make manhunter check
			if (HungerCheck(pawn))
			{
				currentTicks++;
				if(currentTicks >= GetTicks(pawn))
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.mentalState);
					currentTicks = 0;
				}
			}
			//end manhunter if fully fed
			if (ResetCheck(pawn) && this.Props.mentalState == this.Props.mentalState && pawn.Faction != null)
			{
				pawn.mindState.mentalStateHandler.Reset();
			}
		}

		public void ConsumeVitae(Pawn pawn)
		{
			if (Props.consumeForHunger && pawn.health.hediffSet.HasHediff(Props.hediff))
			{
				Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
				float severityGain = Props.consumeAmount;
				if (hediff.Severity == Props.minSeverity)
				{
					return;
				} 
				else
				{
					if (hediff.Severity <= Props.consumeAmount)
					{
						severityGain = hediff.Severity - Props.minSeverity;
					}
				}
				hediff.Severity -= severityGain;
				pawn.needs.food.CurLevelPercentage += severityGain;
				return;
			}
			Log.Message(pawn + " has no hediff " + Props.hediff + ", adding back.");
			pawn.health.AddHediff(Props.hediff).Severity = Props.minSeverity;
		}

		public bool HungerCheck(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_ServantAwakening) || pawn.Downed) return false;
			if (pawn.health.hediffSet.HasHediff(Props.hediff))
			{
				Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
				return hediff.Severity <= GetThreshold(pawn, Props.minSeverity);
			}
			Log.Message(pawn + " has no hediff " + Props.hediff + ", adding back.");
			pawn.health.AddHediff(Props.hediff).Severity = Props.minSeverity;
			return false;
		}

		public bool ResetCheck(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(Props.hediff))
			{
				Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
				return hediff.Severity >= Props.resetSeverity;
			}
			Log.Message(pawn + " has no hediff " + Props.hediff + ", adding back.");
			pawn.health.AddHediff(Props.hediff).Severity = Props.minSeverity;
			return true;
		}

		public float GetThreshold(Pawn pawn, float initial)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_Dampener)) initial /= 2f;
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_Inhibitor)) initial = -1f;
			return initial;
		}

		public int GetTicks(Pawn pawn)
		{
			int ticks = ModSettings_Utility.DarkDescent_SettingVitaeThirstTicks();
			if(pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_Dampener)) ticks *= 2;
			return ticks;
		}
	}
}
