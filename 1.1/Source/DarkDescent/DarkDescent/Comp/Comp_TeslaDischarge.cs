using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	class Comp_TeslaDischarge : ThingComp
	{
		public CompProperties_TeslaDischarge Props
		{
			get
			{
				return (CompProperties_TeslaDischarge)this.props;
			}
		}

		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostPostApplyDamage(dinfo, totalDamageDealt);
			Pawn pawn = parent as Pawn;
			if (!pawn.health.hediffSet.HasHediff(Props.Hediff))
			{
				init(pawn);
				DoDischarge(pawn);
				pawn.health.AddHediff(Props.Hediff).Severity = actualCooldown;
			}
		}

		//vars
		private float actualRadius;
		private int actualDamage;
		private float actualCooldown;

		private void init(Pawn pawn)
		{
			actualRadius = Props.Radius;
			actualDamage = Props.Damage;
			actualCooldown = Props.cooldown;
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_TeslaFaster)) actualCooldown = Props.fasterCooldown;
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_TeslaLarger)) actualRadius = Props.largerRadius;
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_TeslaStronger)) actualDamage = Props.higherDamage;

			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_TeslaSustainer))
			{
				actualDamage -= Props.lowerDamage;
				actualRadius -= Props.lowerRadius;
			}
		}

		private void DoDischarge(Pawn pawn)
		{
			GenExplosion.DoExplosion(pawn.Position, pawn.Map, actualRadius, DamageDefOf.EMP, pawn, actualDamage, -1, Props.Sound);
		}
	}
}
