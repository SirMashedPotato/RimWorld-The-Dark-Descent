﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobGiver_AIGotoNearestHostile_ButSmartly : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			float num = 600f;
			Thing thing = null;
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(pawn) && AttackTargetFinder.IsAutoTargetable(attackTarget))
				{
					Thing thing2 = (Thing)attackTarget;
					if (ForbidUtility.InAllowedArea(thing2.Position, pawn))
					{
						int num2 = thing2.Position.DistanceToSquared(pawn.Position);
						if ((float)num2 < num && pawn.CanReach(thing2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							num = (float)num2;
							thing = thing2;
						}
					}
				}
			}
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(RimWorld.JobDefOf.AttackMelee, thing);
				job.checkOverrideOnExpire = true;
				job.expiryInterval = 500;
				job.collideWithPawns = true;
				return job;
			}
			return null;
		}
	}
}
