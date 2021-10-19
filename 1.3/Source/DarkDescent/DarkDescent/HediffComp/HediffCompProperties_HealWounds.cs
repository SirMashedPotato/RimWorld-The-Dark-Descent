using System;
using Verse;
using RimWorld;


namespace DarkDescent
{
    class HediffCompProperties_HealWounds : HediffCompProperties
    {
        public HediffCompProperties_HealWounds()
        {
            this.compClass = typeof(HediffComp_HealWounds);
        }

        //public int ticks = 500;
        public float baseNumber = 3f;
        public float tend = 0.5f;
        public float severity = 0.5f;
    }
}
