using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobGiver_DeconstructBuilding : JobGiver_RemoveBuilding
    {
        // copied from WorkGiver_Deconstruct
        protected override DesignationDef Designation
        {
            get
            {
                return DesignationDefOf.Deconstruct;
            }
        }
        protected override JobDef RemoveBuildingJob
        {
            get
            {
                return JobDefOf.DarkDescent_ServantDeconstruct;
            }
        }
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building building = t.GetInnerIfMinified() as Building;
            return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced) && ForbidUtility.InAllowedArea(t.Position, pawn);
        }
    }
}
