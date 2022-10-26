using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using System.Linq;

namespace DarkDescent
{
    class JobDriver_VitaeResurrect : JobDriver
    {
        
        private Corpse Corpse
        {
            get
            {
                return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
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
            return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
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
            yield return Toils_General.Do(new Action(this.Resurrect));
            yield break;
        }

        private void Resurrect()
        {
            Pawn innerPawn = this.Corpse.InnerPawn;
            /* servant check */
            if (Utility.IsNotServant(innerPawn))
            {
                ResurrectionUtility.Resurrect(innerPawn);
                innerPawn.health.AddHediff(HediffDefOf.DarkDescent_ServantAwakening).Severity = 1f;
                Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
                this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else
            {
                ResurrectionUtility.ResurrectWithSideEffects(innerPawn);
                Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
                this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
        }

        private const TargetIndex CorpseInd = TargetIndex.A;

        private const TargetIndex ItemInd = TargetIndex.B;

        private const int DurationTicks = 900;
    }
}
