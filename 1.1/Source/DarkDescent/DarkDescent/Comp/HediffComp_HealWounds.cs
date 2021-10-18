using Verse;
using RimWorld;
using System.Linq;

namespace DarkDescent
{
    class HediffComp_HealWounds : HediffComp
    {
        public int ticks = 0;

        public HediffCompProperties_HealWounds Props
        {
            get
            {
                return (HediffCompProperties_HealWounds)this.props;
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            Pawn pawn = base.Pawn;
            if (ticks >= ModSettings_Utility.DarkDescent_SettingRegenTicks())
            {
                if (!pawn.Dead)
                {
                    for(int i = 0; i != Props.baseNumber; i++)
                    {
                        doHealing(pawn);
                    }
                }
                ticks = 0;
            }
            ticks++;
        }

        private void doHealing(Pawn pawn)
        {
            Hediff hediff;
            if (!(from hd in pawn.health.hediffSet.hediffs
                  where hd.def.displayWound && !hd.IsPermanent()
                  select hd).TryRandomElement(out hediff))
            {
                return;
            }
            if (pawn.health.hediffSet.PartIsMissing(hediff.Part) && !hediff.IsTended())
            {
                hediff.Tended(Props.tend);
                return;
            }
            hediff.Severity -= Props.severity;
        }
    }
}
