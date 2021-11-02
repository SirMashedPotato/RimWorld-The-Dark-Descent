using System.Collections.Generic;
using Verse;
using RimWorld;

namespace DarkDescent
{
	public class Recipe_HarvestVitaeComplete : RecipeWorker
	{
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				Hediff vitaeHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_StoredVitae);
				while (vitaeHediff.Severity > 0.0f)
				{
					GenSpawn.Spawn(ThingDefOf.DarkDescent_Vitae, billDoer.Position, billDoer.Map, WipeMode.Vanish);
					vitaeHediff.Severity -= 0.1f;
				}
                if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_StoredVitae) == null)
                {
					pawn.health.AddHediff(HediffDefOf.DarkDescent_StoredVitae).Severity = 0.001f;
				}
			}
			HealthUtility.GiveInjuriesOperationFailureMinor(pawn, pawn.def.race.body.AllParts.RandomElement());
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
			}
		}
	}
}