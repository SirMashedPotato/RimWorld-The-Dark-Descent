using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	class CompProperties_TeslaDischarge : CompProperties
	{
		public CompProperties_TeslaDischarge()
		{
			this.compClass = typeof(Comp_TeslaDischarge);
		}
		public HediffDef Hediff;
		public float Radius = 3f;
		public float cooldown = 1f;
		public int Damage = 20;
		public SoundDef Sound;

		//specific implants
		public float fasterCooldown = 0.5f;
		public float largerRadius = 6f;
		public int higherDamage = 40;
		public int lowerDamage = 10;
		public float lowerRadius = 2f;
	}
}