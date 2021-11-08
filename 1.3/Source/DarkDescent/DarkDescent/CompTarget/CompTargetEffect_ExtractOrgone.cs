using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
	class CompTargetEffect_ExtractOrgone : CompTargetEffect
	{
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			if (!user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, -1, null, true))
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DarkDescent_ExtractOrgone, target, this.parent);
			job.count = 1;
			user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}
}
