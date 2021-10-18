﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobDriver_Repair : JobDriver
    {
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil repair = new Toil();
			repair.initAction = delegate ()
			{
				this.ticksToNextRepair = 80f;
			};
			repair.tickAction = delegate ()
			{
				Pawn actor = repair.actor;
				float num = Utility.GetWork_Construct(actor);
				this.ticksToNextRepair -= num;
				if (this.ticksToNextRepair <= 0f)
				{
					this.ticksToNextRepair += 20f;
					this.TargetThingA.HitPoints++;
					this.TargetThingA.HitPoints = Mathf.Min(this.TargetThingA.HitPoints, this.TargetThingA.MaxHitPoints);
					this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)this.TargetThingA);
					if (this.TargetThingA.HitPoints == this.TargetThingA.MaxHitPoints)
					{
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					}
				}
			};
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			yield return repair;
			yield break;
		}

		protected float ticksToNextRepair;

		private const float WarmupTicks = 80f;

		private const float TicksBetweenRepairs = 20f;
	}
}
