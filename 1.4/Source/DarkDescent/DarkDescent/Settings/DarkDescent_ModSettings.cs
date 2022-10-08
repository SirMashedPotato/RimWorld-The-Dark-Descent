using Verse;

namespace DarkDescent
{
    class DarkDescent_ModSettings : ModSettings
    {
        //settings
        public int DarkDescent_SettingRegenTicks = DarkDescent_SettingRegenTicks_def;

        public bool DarkDescent_SettingRavenousHunger = DarkDescent_SettingRavenousHunger_def;
        public int DarkDescent_SettingRavenousHungerMin = DarkDescent_SettingRavenousHungerMin_def; //convert to float in getter

        public bool DarkDescent_SettingIncite = DarkDescent_SettingIncite_def;
        public int DarkDescent_SettingInciteMin = DarkDescent_SettingInciteMin_def; //convert to float in getter
        public int DarkDescent_SettingInciteChance = DarkDescent_SettingInciteChance_def; //convert to float in getter
        public int DarkDescent_SettingInciteRadius = DarkDescent_SettingInciteRadius_def;

        public bool DarkDescent_SettingVitaeThirst = DarkDescent_SettingVitaeThirst_def;
        public int DarkDescent_SettingVitaeThirstTicks = DarkDescent_SettingVitaeThirstTicks_def;

        //Def Values
        public static readonly int DarkDescent_SettingRegenTicks_def = 500;

        public static readonly bool DarkDescent_SettingRavenousHunger_def = true;
        public static readonly int DarkDescent_SettingRavenousHungerMin_def = 20;

        public static readonly bool DarkDescent_SettingIncite_def = true;
        public static readonly int DarkDescent_SettingInciteMin_def = 60;
        public static readonly int DarkDescent_SettingInciteChance_def = 30;
        public static readonly int DarkDescent_SettingInciteRadius_def = 3;

        public static readonly bool DarkDescent_SettingVitaeThirst_def = true;
        public static readonly int DarkDescent_SettingVitaeThirstTicks_def = 1000;

        //Save settings
        public override void ExposeData()
        {
            Scribe_Values.Look(ref DarkDescent_SettingRegenTicks, "DarkDescent_SettingRegenTicks", DarkDescent_SettingRegenTicks_def);
            Scribe_Values.Look(ref DarkDescent_SettingRavenousHunger, "DarkDescent_SettingRavenousHunger", DarkDescent_SettingRavenousHunger_def);
            Scribe_Values.Look(ref DarkDescent_SettingRavenousHungerMin, "DarkDescent_SettingRavenousHungerMin", DarkDescent_SettingRavenousHungerMin_def);
            Scribe_Values.Look(ref DarkDescent_SettingIncite, "DarkDescent_SettingIncite", DarkDescent_SettingIncite_def);
            Scribe_Values.Look(ref DarkDescent_SettingInciteMin, "DarkDescent_SettingInciteMin", DarkDescent_SettingInciteMin_def);
            Scribe_Values.Look(ref DarkDescent_SettingInciteChance, "DarkDescent_SettingInciteChance", DarkDescent_SettingInciteChance_def);
            Scribe_Values.Look(ref DarkDescent_SettingInciteRadius, "DarkDescent_SettingInciteRadius", DarkDescent_SettingInciteRadius_def);
            Scribe_Values.Look(ref DarkDescent_SettingVitaeThirst, "DarkDescent_SettingVitaeThirst", DarkDescent_SettingVitaeThirst_def);
            Scribe_Values.Look(ref DarkDescent_SettingVitaeThirstTicks, "DarkDescent_SettingVitaeThirstTicks", DarkDescent_SettingVitaeThirstTicks_def);
            base.ExposeData();
        }

        public static void ResetSettings(DarkDescent_ModSettings settings)
        {
            settings.DarkDescent_SettingRegenTicks = DarkDescent_SettingRegenTicks_def;

            settings.DarkDescent_SettingRavenousHunger = DarkDescent_SettingRavenousHunger_def;
            settings.DarkDescent_SettingRavenousHungerMin = DarkDescent_SettingRavenousHungerMin_def;

            settings.DarkDescent_SettingIncite = DarkDescent_SettingIncite_def;
            settings.DarkDescent_SettingInciteMin = DarkDescent_SettingInciteMin_def;
            settings.DarkDescent_SettingInciteChance = DarkDescent_SettingInciteChance_def;
            settings.DarkDescent_SettingInciteRadius = DarkDescent_SettingInciteRadius_def;

            settings.DarkDescent_SettingVitaeThirst = DarkDescent_SettingVitaeThirst_def;
            settings.DarkDescent_SettingVitaeThirstTicks = DarkDescent_SettingVitaeThirstTicks_def;
        }
    }
}
