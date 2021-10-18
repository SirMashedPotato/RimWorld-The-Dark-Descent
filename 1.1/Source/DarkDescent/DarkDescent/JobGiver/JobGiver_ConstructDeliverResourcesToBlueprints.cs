using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobGiver_ConstructDeliverResourcesToBlueprints : JobGiver_ConstructDeliverResources
    {
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.Message("Trying to trigger");
			Predicate<Thing> predicate = (Thing t) => t.def.IsBlueprint && HasJobOnThing(pawn, t);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Blueprint), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false), 100f, predicate);
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

		//copied from WorkGiver_ConstructDeliverResourcesToFrames

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

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
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(blueprint, pawn, false, false))
			{
				return null;
			}
			if (JobGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
			{
				return null;
			}
			Job job2 = base.ResourceDeliverJobFor(pawn, blueprint, true);
			if (job2 != null)
			{
				return job2;
			}
			Job job3 = this.NoCostFrameMakeJobFor(pawn, blueprint);
			if (job3 != null)
			{
				return job3;
			}
			return null;
		}

		private Job NoCostFrameMakeJobFor(Pawn pawn, IConstructible c)
		{
			if (c is Blueprint_Install)
			{
				return null;
			}
			if (c is Blueprint && c.MaterialsNeeded().Count == 0)
			{
				Job job = JobMaker.MakeJob(RimWorld.JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				return job;
			}
			return null;
		}
	}
}
