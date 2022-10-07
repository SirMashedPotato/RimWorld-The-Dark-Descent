using RimWorld;
using System.Collections.Generic;
using Verse;
using UnityEngine;

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

			if (pawn.health.hediffSet.HasHediff(HediffDefOf.DarkDescent_TeslaSustainer) 
				|| (LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Vanilla Expanded Framework") && pawn.health.hediffSet.HasHediff(HediffDef.Named("DarkDescent_TeslaCannon"))))
			{
				actualDamage -= Props.lowerDamage;
				actualRadius -= Props.lowerRadius;
			}
		}

		private void DoDischarge(Pawn pawn)
		{
			GenExplosion.DoExplosion(pawn.Position, pawn.Map, actualRadius, DamageDefOf.EMP, pawn, actualDamage, -1, Props.Sound);
		}
		/*
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
			Pawn p = parent as Pawn;
            if (!p.Dead && p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_TeslaBigBoom) != null)
            {
				yield return new Command_Action
				{
					defaultLabel = "DarkDescent_DetonatePiggy".Translate(),
					defaultDesc = "DarkDescent_DetonatePiggyDescription".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Gizmos/DarkDescent_PiggyGoBoom", true),
					action = delegate ()
					{
						GenExplosion.DoExplosion(p.Position, p.Map, 20, DamageDefOf.EMP, p, 20, -1, Props.Sound);
						GenExplosion.DoExplosion(p.Position, p.Map, 15, DamageDefOf.Bomb, p, 20, -1, Props.Sound);
					}
				};
			}
        }
		*/
    }
}
