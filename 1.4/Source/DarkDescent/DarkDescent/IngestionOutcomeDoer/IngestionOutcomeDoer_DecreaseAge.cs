using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace DarkDescent
{
    class IngestionOutcomeDoer_DecreaseAge : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if (!pawn.health.hediffSet.HasHediff(associatedHediff))
            {
                long ticks = years * 3600000;
                if (pawn.ageTracker.AgeBiologicalTicks - ticks > 0)
                {
                    pawn.ageTracker.AgeBiologicalTicks -= ticks;
                }
            }
        }

        public int years = 1;

        public HediffDef associatedHediff;
    }
}
