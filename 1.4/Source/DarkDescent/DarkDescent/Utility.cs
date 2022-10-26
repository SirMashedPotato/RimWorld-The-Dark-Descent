using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq;

namespace DarkDescent
{
    class Utility
    {
        public static float GetWork_Construct(Pawn actor)
        {
            return actor.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) * actor.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);
        }

        public static bool IsNotServant(Pawn pawn)
        {
            return !servantDefs.Contains(pawn.kindDef) && pawn.RaceProps.FleshType.defName != "DarkDescent_OrbServantFlesh";
        }

        public static bool PawnIsDrainer (Pawn pawn)
        {
            return ModsConfig.IdeologyActive && pawn.ideo.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail("DarkDescent_Draining_Acceptable"));
        }

        public static bool PawnIsOrbSeeker(Pawn pawn)
        {
            return ModsConfig.IdeologyActive && pawn.ideo.Ideo.HasPrecept(DefDatabase<PreceptDef>.GetNamedSilentFail("DarkDescent_OrbSeeker_Increased"));
        }

        public static bool PawnIsMithraic(Pawn pawn)
        {
            return ModsConfig.IdeologyActive && pawn.ideo.Ideo.memes.Contains(DefDatabase<MemeDef>.GetNamedSilentFail("DarkDescent_Mithraic"));
        }

        private static readonly PawnKindDef[] servantDefs =
            {PawnKindDefOf.DarkDescent_Brute, PawnKindDefOf.DarkDescent_Grunt , PawnKindDefOf.DarkDescent_Harvester,
            PawnKindDefOf.DarkDescent_Tesla, PawnKindDefOf.DarkDescent_Wretch, PawnKindDefOf.DarkDescent_Engineer,
            PawnKindDefOf.DarkDescent_Hound, PawnKindDefOf.DarkDescent_Goliath};
    }
}
