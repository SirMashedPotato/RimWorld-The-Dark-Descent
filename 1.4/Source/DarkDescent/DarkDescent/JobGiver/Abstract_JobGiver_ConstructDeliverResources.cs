using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    public abstract class JobGiver_ConstructDeliverResources : ThinkNode_JobGiver
    {
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		//copied from JobGiver_ConstructDeliverResources
		// Token: 0x06003116 RID: 12566 RVA: 0x000E8E09 File Offset: 0x000E7009
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x00113FC3 File Offset: 0x001121C3
		public static void ResetStaticData()
		{
			JobGiver_ConstructDeliverResources.MissingMaterialsTranslated = "MissingMaterials".Translate();
			JobGiver_ConstructDeliverResources.ForbiddenLowerTranslated = "ForbiddenLower".Translate();
			JobGiver_ConstructDeliverResources.NoPathTranslated = "NoPath".Translate();
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x00114001 File Offset: 0x00112201
		private static bool ResourceValidator(Pawn pawn, ThingDefCountClass need, Thing th)
		{
			return th.def == need.thingDef && !th.IsForbidden(pawn) && pawn.CanReserve(th, 1, -1, null, false);
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x00114034 File Offset: 0x00112234
		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return this.InstallJob(pawn, blueprint_Install);
			}
			bool flag = false;
			ThingDefCountClass thingDefCountClass = null;
			List<ThingDefCountClass> list = c.MaterialsNeeded();
			int count = list.Count;
			int i = 0;
			while (i < count)
			{
				ThingDefCountClass need = list[i];
				if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need, pawn))
				{
					flag = true;
					thingDefCountClass = need;
					break;
				}
				Thing foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing r) => JobGiver_ConstructDeliverResources.ResourceValidator(pawn, need, r), null, 0, -1, false, RegionType.Set_Passable, false);
				if (foundRes != null)
				{
					int resTotalAvailable;
					this.FindAvailableNearbyResources(foundRes, pawn, out resTotalAvailable);
					int num;
					Job job;
					HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need, c, resTotalAvailable, canRemoveExistingFloorUnderNearbyNeeders, out num, out job);
					if (job != null)
					{
						return job;
					}
					hashSet.Add((Thing)c);
					Thing thing = hashSet.MinBy((Thing nee) => IntVec3Utility.ManhattanDistanceFlat(foundRes.Position, nee.Position));
					hashSet.Remove(thing);
					int num2 = 0;
					int j = 0;
					do
					{
						num2 += JobGiver_ConstructDeliverResources.resourcesAvailable[j].stackCount;
						j++;
					}
					while (num2 < num && j < JobGiver_ConstructDeliverResources.resourcesAvailable.Count);
					JobGiver_ConstructDeliverResources.resourcesAvailable.RemoveRange(j, JobGiver_ConstructDeliverResources.resourcesAvailable.Count - j);
					JobGiver_ConstructDeliverResources.resourcesAvailable.Remove(foundRes);
					Job job2 = JobMaker.MakeJob(RimWorld.JobDefOf.HaulToContainer);
					job2.targetA = foundRes;
					job2.targetQueueA = new List<LocalTargetInfo>();
					for (j = 0; j < JobGiver_ConstructDeliverResources.resourcesAvailable.Count; j++)
					{
						job2.targetQueueA.Add(JobGiver_ConstructDeliverResources.resourcesAvailable[j]);
					}
					job2.targetB = thing;
					if (hashSet.Count > 0)
					{
						job2.targetQueueB = new List<LocalTargetInfo>();
						foreach (Thing t in hashSet)
						{
							job2.targetQueueB.Add(t);
						}
					}
					job2.targetC = (Thing)c;
					job2.count = num;
					job2.haulMode = HaulMode.ToContainer;
					return job2;
				}
				else
				{
					flag = true;
					thingDefCountClass = need;
					i++;
				}
			}
			if (flag)
			{
				JobFailReason.Is(string.Format("{0}: {1}", JobGiver_ConstructDeliverResources.MissingMaterialsTranslated, thingDefCountClass.thingDef.label), null);
			}
			return null;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x00114368 File Offset: 0x00112568
		private void FindAvailableNearbyResources(Thing firstFoundResource, Pawn pawn, out int resTotalAvailable)
		{
			int num = Mathf.Min(firstFoundResource.def.stackLimit, pawn.carryTracker.MaxStackSpaceEver(firstFoundResource.def));
			resTotalAvailable = 0;
			JobGiver_ConstructDeliverResources.resourcesAvailable.Clear();
			JobGiver_ConstructDeliverResources.resourcesAvailable.Add(firstFoundResource);
			resTotalAvailable += firstFoundResource.stackCount;
			if (resTotalAvailable < num)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(firstFoundResource.Position, firstFoundResource.Map, 5f, false))
				{
					if (resTotalAvailable >= num)
					{
						break;
					}
					if (thing.def == firstFoundResource.def && GenAI.CanUseItemForWork(pawn, thing))
					{
						JobGiver_ConstructDeliverResources.resourcesAvailable.Add(thing);
						resTotalAvailable += thing.stackCount;
					}
				}
			}
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x00114440 File Offset: 0x00112640
		private HashSet<Thing> FindNearbyNeeders(Pawn pawn, ThingDefCountClass need, IConstructible c, int resTotalAvailable, bool canRemoveExistingFloorUnderNearbyNeeders, out int neededTotal, out Job jobToMakeNeederAvailable)
		{
			neededTotal = need.count;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			Thing thing = (Thing)c;
			foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 8f, true))
			{
				if (neededTotal >= resTotalAvailable)
				{
					break;
				}
				if (this.IsNewValidNearbyNeeder(thing2, hashSet, c, pawn))
				{
					Blueprint blueprint = thing2 as Blueprint;
					if (blueprint == null || !JobGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						int num = GenConstruct.AmountNeededByOf((IConstructible)thing2, need.thingDef);
						if (num > 0)
						{
							hashSet.Add(thing2);
							neededTotal += num;
						}
					}
				}
			}
			Blueprint blueprint2 = c as Blueprint;
			if (blueprint2 != null && blueprint2.def.entityDefToBuild is TerrainDef && canRemoveExistingFloorUnderNearbyNeeders && neededTotal < resTotalAvailable)
			{
				foreach (Thing thing3 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 3f, false))
				{
					if (this.IsNewValidNearbyNeeder(thing3, hashSet, c, pawn))
					{
						Blueprint blueprint3 = thing3 as Blueprint;
						if (blueprint3 != null)
						{
							Job job = this.RemoveExistingFloorJob(pawn, blueprint3);
							if (job != null)
							{
								jobToMakeNeederAvailable = job;
								return hashSet;
							}
						}
					}
				}
			}
			jobToMakeNeederAvailable = null;
			return hashSet;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x001145B4 File Offset: 0x001127B4
		private bool IsNewValidNearbyNeeder(Thing t, HashSet<Thing> nearbyNeeders, IConstructible constructible, Pawn pawn)
		{
			return t is IConstructible && t != constructible && !(t is Blueprint_Install) && t.Faction == pawn.Faction && !t.IsForbidden(pawn) && !nearbyNeeders.Contains(t) && GenConstruct.CanConstruct(t, pawn, false, false);
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x00114606 File Offset: 0x00112806
		protected static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			return blue.def.entityDefToBuild is TerrainDef && pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position);
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x00114638 File Offset: 0x00112838
		protected Job RemoveExistingFloorJob(Pawn pawn, Blueprint blue)
		{
			if (!JobGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue))
			{
				return null;
			}
			if (!pawn.CanReserve(blue.Position, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(RimWorld.JobDefOf.RemoveFloor, blue.Position);
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x0011468C File Offset: 0x0011288C
		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
			{
				JobFailReason.Is(JobGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
				return null;
			}
			if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false))
			{
				JobFailReason.Is(JobGiver_ConstructDeliverResources.NoPathTranslated, null);
				return null;
			}
			if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn);
				if (pawn2 != null)
				{
					JobFailReason.Is("ReservedBy".Translate(pawn2.LabelShort, pawn2), null);
				}
				return null;
			}
			Job job = JobMaker.MakeJob(RimWorld.JobDefOf.HaulToContainer);
			job.targetA = miniToInstallOrBuildingToReinstall;
			job.targetB = install;
			job.count = 1;
			job.haulMode = HaulMode.ToContainer;
			return job;
		}

		// Token: 0x04001B69 RID: 7017
		private static List<Thing> resourcesAvailable = new List<Thing>();

		// Token: 0x04001B6A RID: 7018
		private const float MultiPickupRadius = 5f;

		// Token: 0x04001B6B RID: 7019
		private const float NearbyConstructScanRadius = 8f;

		// Token: 0x04001B6C RID: 7020
		private static string MissingMaterialsTranslated;

		// Token: 0x04001B6D RID: 7021
		private static string ForbiddenLowerTranslated;

		// Token: 0x04001B6E RID: 7022
		private static string NoPathTranslated;
	}
}
