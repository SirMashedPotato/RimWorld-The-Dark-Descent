using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using Verse;
using System;
using System.Linq;
using System.Collections.Generic;
using Verse.AI;
using Verse.AI.Group;
using System.Text;

namespace DarkDescent
{
    class HediffComp_HarvesterDecoction : HediffComp
    {

        public HediffCompProperties_HarvesterDecoction Props
        {
            get
            {
                return (HediffCompProperties_HarvesterDecoction)this.props;
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            Pawn pawn = parent.pawn;
            if (parent.Severity + severityAdjustment >= 1f)
            {
                MakeTheChange(pawn);
            }
            base.CompPostTick(ref severityAdjustment);
        }

        public Pawn MakeTheChange(Pawn originalPawn)
        {
            originalPawn.DropAndForbidEverything();
            originalPawn.Strip();
            float vitaeValue = getVitaeValue(originalPawn);
            originalPawn.Kill(null);
            Pawn newPawn = PawnGenerator.GeneratePawn(Props.ServantKind, Faction.OfPlayer);
            newPawn.Name = originalPawn.Name;
            newPawn.gender = originalPawn.gender;
            PawnUtility.TrySpawnHatchedOrBornPawn(newPawn, originalPawn.Corpse);
            originalPawn.Corpse.Destroy();
            if (this.Props.applyAwakeningHediff)
            {
                newPawn.health.AddHediff(HediffDefOf.DarkDescent_ServantAwakening).Severity = 0.9f;
            }
            newPawn.health.AddHediff(HediffDefOf.DarkDescent_StoredVitae).Severity = vitaeValue;
            return newPawn;
        }

        public float getVitaeValue(Pawn pawn)
        {
            if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_VitaeExtracted))
            {
                return 1f - pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_VitaeExtracted).Severity;
            }
            return 1f;
        }
    }

}
