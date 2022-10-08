using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
    class CompUseEffect_ActivateServant : CompUseEffect
    {
		public CompProperties_UseEffect_ActivateServant Props
		{
			get
			{
				return (CompProperties_UseEffect_ActivateServant)this.props;
			}
		}
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.ServantKind != null)
			{
				Pawn servant = PawnGenerator.GeneratePawn(this.Props.ServantKind, usedBy.Faction);
				PawnUtility.TrySpawnHatchedOrBornPawn(servant, usedBy);

				//optionals
				if(this.Props.bond) servant.relations.AddDirectRelation(PawnRelationDefOf.Bond, usedBy);
				if (this.Props.hediffToApply != null)
				{
					servant.health.AddHediff(this.Props.hediffToApply);
				}
				if (this.Props.applyAwakeningHediff)
				{
					servant.health.AddHediff(HediffDefOf.DarkDescent_ServantAwakening).Severity = 0.9f;
				}
				if(this.Props.drainToActivator != null)
				{
					if (usedBy.health.hediffSet.GetFirstHediffOfDef(this.Props.drainToActivator) != null)
					{
						usedBy.health.hediffSet.GetFirstHediffOfDef(this.Props.drainToActivator).Severity += this.Props.drainToActivatorSeverity;
					}
					else
					{
						usedBy.health.AddHediff(this.Props.drainToActivator).Severity = this.Props.drainToActivatorSeverity;
					}
				}
				if (this.Props.useQuality)
				{
					parent.TryGetQuality(out QualityCategory qc);
					float q = (float)qc;
					q /= 10f;
                    if (qc == QualityCategory.Legendary)
                    {
						servant.health.AddHediff(HediffDefOf.DarkDescent_ServantQuality_Legendary).Severity = q + 0.1f;
						return;
					}
					servant.health.AddHediff(HediffDefOf.DarkDescent_ServantQuality).Severity = q + 0.1f;
				}
				foreach (ConceptDef cd in Props.conceptDefs)
				{
					LessonAutoActivator.TeachOpportunity(cd, OpportunityType.Important);
				}
			}
			parent.Destroy();
		}
	}
}