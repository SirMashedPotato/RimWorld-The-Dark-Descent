using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace DarkDescent
{
    class HediffCompProperties_HarvesterDecoction : HediffCompProperties
    {
        public HediffCompProperties_HarvesterDecoction()
        {
            this.compClass = typeof(HediffComp_HarvesterDecoction);
        }
        public PawnKindDef ServantKind = null;
        public HediffDef hediffToApply = null;
        public bool applyAwakeningHediff = true;
    }
}
