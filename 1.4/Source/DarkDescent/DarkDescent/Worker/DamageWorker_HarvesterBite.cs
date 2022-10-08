using Verse;

namespace DarkDescent
{
    class DamageWorker_HarvesterBite : DamageWorker_AddInjury
	{
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			if (CanInfectPawn(pawn))
			{
				DoVitaeStuff(dinfo.Instigator as Pawn, pawn, (dinfo.Amount / 150));
			}
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}

        public static bool CanInfectPawn(Pawn pawn)
        {
			return (!pawn.RaceProps.Animal && !pawn.RaceProps.IsMechanoid && pawn.kindDef != PawnKindDefOf.DarkDescent_Harvester);
        }

		public static void DoVitaeStuff(Pawn parent, Pawn target, float drainAmount)
		{
			Hediff gainHediff = parent.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_StoredVitae);
			if (gainHediff.Severity >= 1.0f) return;
			if (!target.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_VitaeExtracted))
			{
				target.health.AddHediff(HediffDefOf.DarkDescent_VitaeExtracted).Severity = 0.001f;
			}
			Hediff drainHediff = target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_VitaeExtracted);
			drainAmount = GetDrain(gainHediff, drainHediff, drainAmount);
			gainHediff.Severity += drainAmount;
			drainHediff.Severity += drainAmount;
			return;

		}

		public static float GetDrain(Hediff parent, Hediff target, float initial)
		{
			//if (initial == 1f) return initial;
			float cap;
			if(parent.Severity >= target.Severity)
			{
				cap = 1f - parent.Severity;
			}
			else
			{
				cap = 1f - target.Severity;
			}
			if(initial <= cap) return initial;
			return cap;
		}
    }
}