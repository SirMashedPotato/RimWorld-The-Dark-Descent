using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobGiver_Flick : ThinkNode_JobGiver
    {
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> predicate = (Thing t) => t.def.category == ThingCategory.Building && HasJobOnThing(pawn, t);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false), 100f, predicate);
			Job result;
			if (thing == null)
			{
				result = null;
			}
			else
			{
				result = JobOnThing(pawn, thing);
			}
			return result;
		}

		//copied from WorkGiver_Flick

		public bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && pawn.CanReserve(t, 1, -1, null, forced) && ForbidUtility.InAllowedArea(t.Position, pawn);
		}

		public Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(RimWorld.JobDefOf.Flick, t);
		}
	}
}
