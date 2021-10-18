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
            return servantDefs.Contains(pawn.kindDef) || pawn.RaceProps.FleshType.defName == "DarkDescent_OrbServantFlesh";
        }

        private static readonly PawnKindDef[] servantDefs =
            {PawnKindDefOf.DarkDescent_Brute, PawnKindDefOf.DarkDescent_Grunt , PawnKindDefOf.DarkDescent_Harvester,
            PawnKindDefOf.DarkDescent_Tesla, PawnKindDefOf.DarkDescent_Wretch, PawnKindDefOf.DarkDescent_Engineer,
            PawnKindDefOf.DarkDescent_Hound};
    }
}
