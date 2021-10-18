using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace DarkDescent
{
    class JobDriver_ExtractVitae : JobDriver
    {
        private Pawn Target
        {
            get
            {
                return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
            }
        }
        private Thing Item
        {
            get
            {
                return this.job.GetTarget(TargetIndex.B).Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(DurationTicks, TargetIndex.None);
            toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return toil;
            yield return Toils_General.Do(new Action(this.ExtractVitae));
            yield break;
        }

        private void ExtractVitae()
        {
            Hediff hediff;
            if (!this.Target.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_VitaeExtracted))
            {
                hediff = this.Target.health.AddHediff(HediffDefOf.DarkDescent_VitaeExtracted);
                hediff.Severity = 0.1f;
                GenSpawn.Spawn(ThingDefOf.DarkDescent_Vitae, this.Target.Position, this.Target.Map, WipeMode.Vanish);
            }
            else hediff = this.Target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_VitaeExtracted);
            for(int i = 1; i != 2; i++) GenSpawn.Spawn(ThingDefOf.DarkDescent_Vitae, this.Target.Position, this.Target.Map, WipeMode.Vanish);
            while(hediff.Severity < 1.0f)
            {
                GenSpawn.Spawn(ThingDefOf.DarkDescent_Vitae, this.Target.Position, this.Target.Map, WipeMode.Vanish);
                hediff.Severity += 0.1f;
            }
            if (this.Target.Dead)
            {
                ThoughtUtility.GiveThoughtsForPawnExecuted(this.Target, this.pawn, PawnExecutionKind.GenericBrutal);
            }
            this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
        }
        private const TargetIndex PawnInd = TargetIndex.A;
        private const TargetIndex ItemInd = TargetIndex.B;
        private const int DurationTicks = 600;
    }
}