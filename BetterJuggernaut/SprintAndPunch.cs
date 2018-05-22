using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using BattleTech;
using BattleTech.UI;
using System.IO;

//This version lets consumesfiring and consumesmovement be true by default again

namespace BetterJuggernaut
{
    public class SprintAndPunch
    {
        public static string LogPath;
        public static string ModDirectory;
        public static bool justPunched; //used to keep track of whether we should restore the fire button or not
        public static Mech jugMech; //we use this to verify we have the right mech to alter
        public static void Init()
        {
            justPunched = false;
            ModDirectory = Path.Combine(Path.GetDirectoryName(VersionManifestUtilities.MANIFEST_FILEPATH), @"..\..\..\Mods\");
            LogPath = Path.Combine(ModDirectory, "BetterJuggernaut.log");
            File.CreateText(SprintAndPunch.LogPath);//create new text on startup
            var harmony = HarmonyInstance.Create("Battletech.realitymachina.BetterJuggernaut");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesFiring", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesFiring_Prefix
    {
        static bool Prefix(MechMeleeSequence __instance)
        {
            if (__instance == null)
            {
                throw new ArgumentNullException(nameof(__instance));
            }

            using (var logwriter = File.AppendText(SprintAndPunch.LogPath))
            {


                Pilot curPilot = __instance.OwningMech.GetPilot();
                if (curPilot != null && curPilot.PassiveAbilities.Count > 0) //.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
                {
                    logwriter?.WriteLine("Found pilot with abilities.");
                    bool foundJug = false;
                    for (int i = 0; i < curPilot.PassiveAbilities.Count && !foundJug; i++)
                    {
                        
                        if (curPilot.PassiveAbilities[i].Def.Description.Id == "AbilityDefGu8")
                        {
                            logwriter?.WriteLine("Found Juggernaut, apply bracing.");
                            __instance.OwningMech.BracedLastRound = true;
                            __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);

                        }

                    }
                }
                return true; //otherwise skip
            }
        }
    }




}

