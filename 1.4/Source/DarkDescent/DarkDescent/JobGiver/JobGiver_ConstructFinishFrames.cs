using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobGiver_ConstructFinishFrames : ThinkNode_JobGiver
    {
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> predicate = (Thing t) => t.def.IsFrame && HasJobOnThing(pawn, t);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false), 100f, predicate);
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

		//copied from WorkGiver_ConstructFinishFrames

		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null && ForbidUtility.InAllowedArea(t.Position, pawn);
		}

		public Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Frame frame = t as Frame;
			if (frame == null)
			{
				return null;
			}
			if (frame.MaterialsNeeded().Count > 0)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, false, forced))
			{
				return null;
			}
			BuildableDef buildable = frame.def.entityDefToBuild;
			if (buildable is ThingDef)
            {
				ThingDef thingDef = buildable as ThingDef;
				Thing thing = new Thing();
				thing.def = thingDef;
				if (thing.def.comps.Any(x=> x.compClass.Name == "CompQuality"))
                {
					return null;
				}
			}
			return JobMaker.MakeJob(JobDefOf.DarkDescent_ServantFinishFrame, frame);
		}
	}
}
