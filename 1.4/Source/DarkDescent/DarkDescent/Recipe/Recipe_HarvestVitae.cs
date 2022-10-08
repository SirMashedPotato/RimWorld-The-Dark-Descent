using System.Collections.Generic;
using Verse;
using RimWorld;

namespace DarkDescent
{
	public class Recipe_HarvestVitae : RecipeWorker
	{
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				Hediff vitaeHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarkDescent_StoredVitae);
				while (vitaeHediff.Severity > 0.1f)
				{
					GenSpawn.Spawn(ThingDefOf.DarkDescent_Vitae, billDoer.Position, billDoer.Map, WipeMode.Vanish);
					vitaeHediff.Severity -= 0.1f;
				}
			}
			HealthUtility.GiveRandomSurgeryInjuries(pawn, 5, pawn.def.race.body.AllParts.RandomElement());
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
			}
		}
	}
}