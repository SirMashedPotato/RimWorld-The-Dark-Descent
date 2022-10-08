using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobDriver_Deconstruct : DarkDescent.JobDriver_RemoveBuilding
    {
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true), MinDeconstructWork, MaxDeconstructWork);
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Building == null || !base.Building.DeconstructibleBy(this.pawn.Faction));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
		}

		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
		}

		private const float MaxDeconstructWork = 3000f;

		private const float MinDeconstructWork = 20f;
	}
}
