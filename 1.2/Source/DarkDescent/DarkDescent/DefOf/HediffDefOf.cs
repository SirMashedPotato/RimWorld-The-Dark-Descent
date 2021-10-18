using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	[DefOf]
	public static class HediffDefOf
	{
		static HediffDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
		}

		public static HediffDef DarkDescent_VitaeExtracted;
		public static HediffDef DarkDescent_OrgoneExtracted;
		public static HediffDef DarkDescent_ServantAwakening;
		public static HediffDef DarkDescent_StoredVitae;
		public static HediffDef DarkDescent_ServantQuality;
		public static HediffDef DarkDescent_ServantQuality_Legendary;
		public static HediffDef DarkDescent_HiddenManhunter;

		//Implants
		public static HediffDef DarkDescent_TeslaLarger;
		public static HediffDef DarkDescent_TeslaFaster;
		public static HediffDef DarkDescent_TeslaStronger;
		public static HediffDef DarkDescent_TeslaSustainer;

		public static HediffDef DarkDescent_Dampener;
		public static HediffDef DarkDescent_Inhibitor;
	}
}
