using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using System.Linq;
using RimWorld.Planet;


namespace DarkDescent
{
    class Comp_RavenousHunger : ThingComp
    {
		public CompProperties_RavenousHunger Props
		{
			get
			{
				return (CompProperties_RavenousHunger)this.props;
			}
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			Pawn pawn = this.parent as Pawn;
			if (pawn.Dead || !ModSettings_Utility.DarkDescent_SettingRavenousHunger())
			{
				return;
			}
			//wakeup if getting too hungry
			if (!pawn.Awake() && pawn.needs.food.CurLevelPercentage <= (ModSettings_Utility.DarkDescent_SettingRavenousHungerMin_Float()+ 0.2f) && !pawn.Downed)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
			//make manhunter check
			if (HungerCheck(pawn))
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.mentalState);
				if (!pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_HiddenManhunter))
				{
					pawn.health.AddHediff(HediffDefOf.DarkDescent_HiddenManhunter);
				}
				if (ModSettings_Utility.DarkDescent_SettingIncite() && this.Props.affectOthers)
				{
					InciteOthers(pawn);
				}
			}
			//end manhunter if fully fed
			if(pawn.needs.food.CurLevelPercentage >= 0.9f && this.Props.mentalState == this.Props.mentalState)
			{
				pawn.mindState.mentalStateHandler.Reset();
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_HiddenManhunter))
                {
					pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_HiddenManhunter));
				}
			}
		}

		public bool HungerCheck(Pawn pawn, bool isSource = true)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_ServantAwakening) || pawn.Downed) return false;
			if (isSource)
			{
				return pawn.needs.food.CurLevelPercentage <= GetThreshold(pawn, ModSettings_Utility.DarkDescent_SettingRavenousHungerMin_Float());
			}
			return (pawn.needs.food.CurLevelPercentage <= GetThreshold(pawn, ModSettings_Utility.DarkDescent_SettingInciteMin_Float()) && Rand.Chance(ModSettings_Utility.DarkDescent_SettingInciteChance_Float()));
		}

		public float GetThreshold(Pawn pawn, float initial)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_Dampener)) initial /= 2f;
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_Inhibitor)) initial = 0f;
			return initial;
		}

		public void InciteOthers(Pawn pawn)
		{
			int radius = GenRadial.NumCellsInRadius(Props.inciteRadius);
			for(int i = 0; i != radius; i++)
			{
				IntVec3 tile = pawn.Position + GenRadial.RadialPattern[i];
				if (tile.InBounds(pawn.Map))
				{
					HashSet<Thing> hashSet = new HashSet<Thing>(tile.GetThingList(pawn.Map).Where(x => x.def == ThingDefOf.DarkDescent_Wretch || x.def == ThingDefOf.DarkDescent_Engineer));
					foreach(Pawn p in hashSet)
					{
						if(HungerCheck(p, false))
						{
							p.mindState.mentalStateHandler.TryStartMentalState(this.Props.mentalState);
						}
					}
				}
			}
		}
	}
}
