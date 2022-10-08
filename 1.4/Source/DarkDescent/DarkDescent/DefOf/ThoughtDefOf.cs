using System;
using RimWorld;

namespace DarkDescent
{
	[DefOf]
	public static class ThoughtDefOf
	{
		static ThoughtDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf));
		}

		public static ThoughtDef DarkDescent_KnowGuestDrained;
		public static ThoughtDef DarkDescent_KnowColonistDrained;
		public static ThoughtDef DarkDescent_MyDrained;
	}
}
