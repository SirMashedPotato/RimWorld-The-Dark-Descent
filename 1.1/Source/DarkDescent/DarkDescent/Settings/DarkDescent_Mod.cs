using UnityEngine;
using Verse;

namespace DarkDescent
{
    class DarkDescent_Mod : Mod
    {
        DarkDescent_ModSettings settings;
        public DarkDescent_Mod(ModContentPack contentPack) : base(contentPack)
        {
            this.settings = GetSettings<DarkDescent_ModSettings>();
        }

        public override string SettingsCategory() => "Dark Descent";

        private int page = 0;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);

            //get page
            switch (page)
            {
                case 1:
                    listing_Standard = SettingsPage_Ravenous(listing_Standard);
                    break;

                case 2:
                    listing_Standard = SettingsPage_VitaeThirst(listing_Standard);
                    break;

                default:
                    listing_Standard = SettingsPage_Default(listing_Standard);
                    break;
            }

            listing_Standard = SettingsButtons(listing_Standard);

            listing_Standard.End();
            base.DoSettingsWindowContents(inRect);
        }

        private Listing_Standard SettingsButtons(Listing_Standard listing_Standard)
        {
            listing_Standard.Gap();
            listing_Standard.GapLine();
            //pages

            if (page != 0)
            {
                Rect rectPageDefault = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageDefault, "DarkDescent_PageDefault".Translate());
                if (Widgets.ButtonText(rectPageDefault, "DarkDescent_PageDefault".Translate(), true, true, true))
                {
                    page = 0;
                }
                listing_Standard.Gap();
            }

            if (page != 1)
            {
                Rect rectPageRavenous = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageRavenous, "DarkDescent_PageRavenousHunger".Translate());
                if (Widgets.ButtonText(rectPageRavenous, "DarkDescent_PageRavenousHunger".Translate(), true, true, true))
                {
                    page = 1;
                }
                listing_Standard.Gap();
            }

            if (page != 2)
            {
                Rect rectPageVitae = listing_Standard.GetRect(30f);
                TooltipHandler.TipRegion(rectPageVitae, "DarkDescent_PageVitaeThirst".Translate());
                if (Widgets.ButtonText(rectPageVitae, "DarkDescent_PageVitaeThirst".Translate(), true, true, true))
                {
                    page = 2;
                }
                listing_Standard.Gap();
            }
            listing_Standard.GapLine();

            //reset
            Rect rectDefault = listing_Standard.GetRect(30f);
            TooltipHandler.TipRegion(rectDefault, "DarkDescent_Reset".Translate());
            if (Widgets.ButtonText(rectDefault, "DarkDescent_Reset".Translate(), true, true, true))
            {
                DarkDescent_ModSettings.ResetSettings(settings);
            }

            return listing_Standard;
        }

        //Specific pages

        private Listing_Standard SettingsPage_Default(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("DarkDescent_PageDefault".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();

            //DarkDescent_SettingRegenTicks
            listing_Standard.Label("DarkDescent_SettingRegenTicks".Translate() + " (" + settings.DarkDescent_SettingRegenTicks + ")");
            settings.DarkDescent_SettingRegenTicks = (int)listing_Standard.Slider(settings.DarkDescent_SettingRegenTicks, 1, 10000);

            return listing_Standard;
        }

        private Listing_Standard SettingsPage_Ravenous(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("DarkDescent_PageRavenousHunger".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();

            //DarkDescent_SettingRavenousHunger
            listing_Standard.CheckboxLabeled("DarkDescent_SettingRavenousHunger".Translate(), ref settings.DarkDescent_SettingRavenousHunger);

            if (settings.DarkDescent_SettingRavenousHunger)
            {
                listing_Standard.GapLine();
                listing_Standard.Gap();

                //DarkDescent_SettingRavenousHungerMin
                listing_Standard.Label("DarkDescent_SettingRavenousHungerMin".Translate() + " (" + settings.DarkDescent_SettingRavenousHungerMin + "%)");
                settings.DarkDescent_SettingRavenousHungerMin = (int)(listing_Standard.Slider(settings.DarkDescent_SettingRavenousHungerMin, 0, 100));
                listing_Standard.Gap();

                //DarkDescent_SettingIncite
                listing_Standard.CheckboxLabeled("DarkDescent_SettingIncite".Translate(), ref settings.DarkDescent_SettingIncite);

                if (settings.DarkDescent_SettingIncite)
                {
                    listing_Standard.GapLine();
                    listing_Standard.Gap();

                    //DarkDescent_SettingInciteMin
                    listing_Standard.Label("DarkDescent_SettingInciteMin".Translate() + " (" + settings.DarkDescent_SettingInciteMin + "%)");
                    settings.DarkDescent_SettingInciteMin = (int)(listing_Standard.Slider(settings.DarkDescent_SettingInciteMin, 0, 100));
                    listing_Standard.Gap();

                    //DarkDescent_SettingInciteChance
                    listing_Standard.Label("DarkDescent_SettingInciteChance".Translate() + " (" + settings.DarkDescent_SettingInciteChance + "%)");
                    settings.DarkDescent_SettingInciteChance = (int)(listing_Standard.Slider(settings.DarkDescent_SettingInciteChance, 0, 100));
                    listing_Standard.Gap();

                    //DarkDescent_SettingInciteRadius
                    listing_Standard.Label("DarkDescent_SettingInciteRadius".Translate() + " (" + settings.DarkDescent_SettingInciteRadius + ")");
                    settings.DarkDescent_SettingInciteRadius = (int)(listing_Standard.Slider(settings.DarkDescent_SettingInciteRadius, 0, 100));
                    listing_Standard.Gap();
                }
            }
            return listing_Standard;
        }

        private Listing_Standard SettingsPage_VitaeThirst(Listing_Standard listing_Standard)
        {
            listing_Standard.Label("DarkDescent_PageVitaeThirst".Translate());
            listing_Standard.GapLine();
            listing_Standard.Gap();

            //DarkDescent_SettingVitaeThirst
            listing_Standard.CheckboxLabeled("DarkDescent_SettingVitaeThirst".Translate(), ref settings.DarkDescent_SettingVitaeThirst);

            if (settings.DarkDescent_SettingVitaeThirst)
            {
                listing_Standard.GapLine();
                listing_Standard.Gap();

                //DarkDescent_SettingVitaeThirstTicks
                listing_Standard.Label("DarkDescent_SettingVitaeThirstTicks".Translate() + " (" + settings.DarkDescent_SettingVitaeThirstTicks + ")");
                settings.DarkDescent_SettingVitaeThirstTicks = (int)(listing_Standard.Slider(settings.DarkDescent_SettingVitaeThirstTicks, 0, 10000));
                listing_Standard.Gap();
            }
            return listing_Standard;
        }
    }
}
