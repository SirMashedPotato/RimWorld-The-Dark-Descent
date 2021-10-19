using System;
using Verse;

namespace DarkDescent
{
	// Despite name, triggers for all non-orbservants
	public class HediffGiver_RandomHumanlike : HediffGiver
	{
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Utility.IsNotServant(pawn) && Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		public float mtbDays;


	}
}
