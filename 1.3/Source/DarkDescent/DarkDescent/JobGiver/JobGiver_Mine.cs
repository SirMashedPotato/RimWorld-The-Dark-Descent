using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobGiver_Mine : ThinkNode_JobGiver
    {
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> predicate = (Thing t) => t.def.category == ThingCategory.Building;
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

		//copied from WorkGiver_Miner
		public static void ResetStaticData()
		{
			NoPathTrans = "NoPath".Translate();
		}

		private static string NoPathTrans;

		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine))
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = designation.target.Cell + GenAdj.AdjacentCells[i];
					if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Mineable firstMineable = designation.target.Cell.GetFirstMineable(pawn.Map);
					if (firstMineable != null)
					{
						yield return firstMineable;
					}
				}
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
		}

		public Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.def.mineable)
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null || !ForbidUtility.InAllowedArea(t.Position, pawn))
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
					if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.Walkable(t.Map) && !intVec2.Standable(t.Map))
					{
						Thing thing = null;
						List<Thing> thingList = intVec2.GetThingList(t.Map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
							{
								thing = thingList[k];
								break;
							}
						}
						if (thing != null)
						{
							Job job = HaulAIUtility.HaulAsideJobFor(pawn, thing);
							if (job != null)
							{
								return job;
							}
							JobFailReason.Is(NoPathTrans, null);
							return null;
						}
					}
				}
				JobFailReason.Is(NoPathTrans, null);
				return null;
			}
			return JobMaker.MakeJob(RimWorld.JobDefOf.Mine, t, 20000, true);
		}

	}
}
