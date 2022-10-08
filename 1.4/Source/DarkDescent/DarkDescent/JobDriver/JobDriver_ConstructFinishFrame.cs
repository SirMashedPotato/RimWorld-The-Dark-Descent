using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
	class JobDriver_ConstructFinishFrame : JobDriver
	{
		//adapted from JobDriver_ConstructFinishFrame
		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;

			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil build = new Toil();
			build.initAction = delegate ()
			{
				GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
			};
			build.tickAction = delegate ()
			{
				Pawn actor = build.actor;
				Frame frame = this.Frame;
				float num = Utility.GetWork_Construct(actor);
				if (frame.Stuff != null)
				{
					num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				float workToBuild = frame.WorkToBuild;
				if (actor.Faction == Faction.OfPlayer)
				{
					float statValue = Utility.GetWork_Construct(actor);
					if (!TutorSystem.TutorialMode && Rand.Value < 1f - Mathf.Pow(statValue, num / workToBuild))
					{
						frame.FailConstruction(actor);
						this.ReadyForNextToil();
						return;
					}
				}
				if (frame.def.entityDefToBuild is TerrainDef)
				{
					this.Map.snowGrid.SetDepth(frame.Position, 0f);
				}
				frame.workDone += num;
				if (frame.workDone >= workToBuild)
				{
					frame.CompleteConstruction(actor);
					this.ReadyForNextToil();
					return;
				}
			};
			build.WithEffect(() => ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			build.FailOn(() => !GenConstruct.CanConstruct(this.Frame, this.pawn, false, false));
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = JobEndInterval;
			//build.activeSkill = (() => SkillDefOf.Construction);
			yield return build;
			yield break;
		}

		private const int JobEndInterval = 5000;
	}
}

