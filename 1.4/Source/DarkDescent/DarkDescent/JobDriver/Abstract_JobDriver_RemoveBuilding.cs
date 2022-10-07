﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    public abstract class JobDriver_RemoveBuilding : JobDriver
    {
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		protected abstract DesignationDef Designation { get; }

		protected abstract float TotalNeededWork { get; }

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, (this.Target is Building_Trap) ? PathEndMode.OnCell : PathEndMode.Touch);
			Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate ()
			{
				this.totalNeededWork = this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickAction = delegate ()
			{
				this.workLeft -= Utility.GetWork_Construct(pawn);
				this.TickAction();
				if (this.workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
			yield return doWork;
			yield return new Toil
			{
				initAction = delegate ()
				{
					this.FinishedRemoving();
					base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		protected virtual void FinishedRemoving()
		{
		}

		protected virtual void TickAction()
		{
		}

		private float workLeft;

		private float totalNeededWork;
	}
}
