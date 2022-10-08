using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	[DefOf]
	public static class ThingDefOf
	{
		static ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

		public static ThingDef DarkDescent_Vitae;
		public static ThingDef DarkDescent_Orgone;
		public static ThingDef DarkDescent_Wretch;
		public static ThingDef DarkDescent_Engineer;
	}
}
