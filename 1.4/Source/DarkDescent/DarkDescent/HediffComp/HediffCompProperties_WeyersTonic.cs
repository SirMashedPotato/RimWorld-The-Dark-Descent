using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace DarkDescent
{
    class HediffCompProperties_WeyersTonic : HediffCompProperties
    {
        public HediffCompProperties_WeyersTonic()
        {
            this.compClass = typeof(HediffComp_WeyersTonic);
        }
        public int years = 1;
    }
}
