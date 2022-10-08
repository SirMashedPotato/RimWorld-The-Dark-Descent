using Verse;

namespace DarkDescent
{
    class ModSettings_Utility
    {
        /* Regen Docoction */

        public static int DarkDescent_SettingRegenTicks()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingRegenTicks;
        }

        /* Ravenous Hunger */

        public static bool DarkDescent_SettingRavenousHunger()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingRavenousHunger;
        }

        public static int DarkDescent_SettingRavenousHungerMin()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingRavenousHungerMin;
        }

        public static float DarkDescent_SettingRavenousHungerMin_Float()
        {
            float f = DarkDescent_SettingRavenousHungerMin();
            return f / 100;
        }

        /* Incite */

        public static bool DarkDescent_SettingIncite()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingIncite;
        }

        public static int DarkDescent_SettingInciteMin()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingInciteMin;
        }

        public static float DarkDescent_SettingInciteMin_Float()
        {
            float f = DarkDescent_SettingInciteMin();
            return f / 100;
        }

        public static int DarkDescent_SettingInciteChance()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingInciteChance;
        }

        public static float DarkDescent_SettingInciteChance_Float()
        {
            float f = DarkDescent_SettingInciteChance();
            return f / 100;
        }

        public static int DarkDescent_SettingInciteRadius()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingInciteRadius;
        }

        /* Vitae Thirst */

        public static bool DarkDescent_SettingVitaeThirst()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingVitaeThirst;
        }

        public static int DarkDescent_SettingVitaeThirstTicks()
        {
            return LoadedModManager.GetMod<DarkDescent_Mod>().GetSettings<DarkDescent_ModSettings>().DarkDescent_SettingVitaeThirstTicks;
        }
    }
}
