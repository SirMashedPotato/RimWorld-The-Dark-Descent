using System;
using Verse;
using RimWorld;

namespace DarkDescent
{
	[DefOf]
	public static class ResearchProjectDefOf
	{
		static ResearchProjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
		}
		public static ResearchProjectDef DarkDescent_ImprovedVitaeResearch;
		public static ResearchProjectDef DarkDescent_ImprovedOrgoneResearch;
		public static ResearchProjectDef DarkDescent_AdvancedExtractorResearch;
	}
}