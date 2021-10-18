using Verse;
using RimWorld;

namespace DarkDescent
{
    class CompProperties_RavenousHunger : CompProperties
    {
        public CompProperties_RavenousHunger()
        {
            this.compClass = typeof(Comp_RavenousHunger);
        }
        //public float minSeverity = 0.2f;
        //public float inciteMinSeverity = 0.6f;
        public bool affectOthers = false;
        //public float affectOthersChance = 0.3f;
        //public int inciteRadius = 3;
        public int inciteRadius = ModSettings_Utility.DarkDescent_SettingInciteRadius();
        public MentalStateDef mentalState = null; //no default value, game gets angry otherwise
    }
}
