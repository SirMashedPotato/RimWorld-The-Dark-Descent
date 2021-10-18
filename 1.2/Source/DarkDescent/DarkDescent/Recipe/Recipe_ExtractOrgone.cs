using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace DarkDescent
{
	public class Recipe_ExtractOrgone : RecipeWorker
	{
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				GenSpawn.Spawn(ThingDefOf.DarkDescent_Orgone, billDoer.Position, billDoer.Map, WipeMode.Vanish);
				if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_OrgoneExtracted) != null)
				{
					pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_OrgoneExtracted).Severity += 0.3f;
				}
				else
				{
					pawn.health.AddHediff(HediffDefOf.DarkDescent_OrgoneExtracted).Severity = 0.2f;
				}
			}
			HealthUtility.GiveInjuriesOperationFailureMinor(pawn, pawn.def.race.body.AllParts.RandomElement());
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.OrganHarvesting);
			}
			giveThoughtDrained(pawn);
			base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraMiniOrHomeFaction, -70, "DarkDescent_OrgoneExtracted"); // 1.2
			//base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraHomeFaction, -70, "DarkDescent_OrgoneExtracted"); // 1.1
		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return ((pawn.Faction != billDoerFaction && pawn.Faction != null) || pawn.IsQuestLodger()) && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}
		public static void giveThoughtDrained(Pawn victim)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			ThoughtDef thoughtDef = null;
			if (victim.IsColonist)
			{
				thoughtDef = ThoughtDefOf.DarkDescent_KnowColonistDrained;
			}
			else if (victim.HostFaction == Faction.OfPlayer)
			{
				thoughtDef = ThoughtDefOf.DarkDescent_KnowGuestDrained;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
			{
				if (pawn.needs.mood != null)
				{
					if (pawn == victim)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DarkDescent_MyDrained, null);
					}
					else if (thoughtDef != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, null);
					}
				}
			}
		}

	}
}
