using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace DarkDescent
{
	public class CompTargetable_SinglePawnHumanlike : CompTargetable
	{
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}


		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				canTargetAnimals = false,
				canTargetHumans = true,
				canTargetMechs = false,
				onlyTargetIncapacitatedPawns = true,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
