using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	[DefOf]
	public static class JobDefOf
	{
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}

		public static JobDef DarkDescent_ExtractVitae;
		public static JobDef DarkDescent_VitaeResurrect;
		public static JobDef DarkDescent_ServantRepair;
		public static JobDef DarkDescent_ServantDeconstruct;
		public static JobDef DarkDescent_ServantFinishFrame;
	}
}
