using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
	class JobGiver_Refuel : ThinkNode_JobGiver
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

		//copied from WorkGiver_Refuel
		public virtual JobDef JobStandard
		{
			get
			{
				return RimWorld.JobDefOf.Refuel;
			}
		}
		public virtual JobDef JobAtomic
		{
			get
			{
				return RimWorld.JobDefOf.RefuelAtomic;
			}
		}
		public virtual bool CanRefuelThing(Thing t)
		{
			return true;// !(t is Building_Turret);
		}
		public bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced) && ForbidUtility.InAllowedArea(t.Position, pawn);
		}
		public Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}

	}
}
