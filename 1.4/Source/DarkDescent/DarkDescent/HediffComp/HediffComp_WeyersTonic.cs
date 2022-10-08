using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using Verse;
using System;
using System.Linq;
using System.Collections.Generic;
using Verse.AI;
using Verse.AI.Group;
using System.Text;

namespace DarkDescent
{
    class HediffComp_WeyersTonic : HediffComp
    {

        public HediffCompProperties_WeyersTonic Props
        {
            get
            {
                return (HediffCompProperties_WeyersTonic)this.props;
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
        }
    }
}
