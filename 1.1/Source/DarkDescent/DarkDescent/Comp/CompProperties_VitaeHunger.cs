using Verse;
using RimWorld;

namespace DarkDescent
{
    class CompProperties_VitaeHunger : CompProperties
    {
        public CompProperties_VitaeHunger()
        {
            this.compClass = typeof(Comp_VitaeHunger);
        }
        public float minSeverity = 0.001f;
        public float resetSeverity = 0.2f;
        //public int ticksUntilEnrage = 1000;
        public bool consumeForHunger = true;
        public float consumeAmount = 0.10f;
        public HediffDef hediff = null;
        public MentalStateDef mentalState = null; //no default value, game gets angry otherwise
    }
}
