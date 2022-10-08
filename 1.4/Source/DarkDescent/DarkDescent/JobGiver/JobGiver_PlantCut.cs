using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
	class JobGiver_PlantCut : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> predicate = (Thing t) => t.def.category == ThingCategory.Plant;
			Thing thing = PotentialWorkThingsGlobal(pawn).RandomElement();
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

		//copied from WorkGiver_PlantCut
		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.designationsByDef[DesignationDefOf.CutPlant])
			{
				yield return designation.target.Thing;
			}
            foreach (Designation designation2 in pawn.Map.designationManager.designationsByDef[DesignationDefOf.HarvestPlant])
			{
				yield return designation2.target.Thing;
			}
            yield break;
		}

		public Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.category != ThingCategory.Plant)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (t.IsForbidden(pawn) || !ForbidUtility.InAllowedArea(t.Position, pawn))
			{
				return null;
			}
			if (t.IsBurning())
			{
				return null;
			}
			foreach (Designation designation in pawn.Map.designationManager.AllDesignationsOn(t))
			{
				if (designation.def == DesignationDefOf.HarvestPlant && pawn.def.defName == "DarkDescent_Wretch")
				{
					if (!((Plant)t).HarvestableNow)
					{
						return null;
					}
					return JobMaker.MakeJob(RimWorld.JobDefOf.HarvestDesignated, t);
				}
				else
				if (designation.def == DesignationDefOf.CutPlant)
				{
					return JobMaker.MakeJob(RimWorld.JobDefOf.CutPlantDesignated, t);
				}
			}
			return null;
		}
	}
}
