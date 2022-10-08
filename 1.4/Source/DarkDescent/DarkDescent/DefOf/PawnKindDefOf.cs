using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	[DefOf]
	public static class PawnKindDefOf
	{
		static PawnKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf));
		}

		public static PawnKindDef DarkDescent_Wretch;
		public static PawnKindDef DarkDescent_Tesla;
		public static PawnKindDef DarkDescent_Grunt;
		public static PawnKindDef DarkDescent_Brute;
		public static PawnKindDef DarkDescent_Harvester;
		public static PawnKindDef DarkDescent_Engineer;
		public static PawnKindDef DarkDescent_Hound;
		public static PawnKindDef DarkDescent_Goliath;
	}
}
