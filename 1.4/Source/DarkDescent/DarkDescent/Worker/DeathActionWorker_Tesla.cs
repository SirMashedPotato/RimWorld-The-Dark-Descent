using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	public class DeathActionWorker_Tesla : DeathActionWorker
	{
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 4.9f, DamageDefOf.EMP, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
		}
	}
}
