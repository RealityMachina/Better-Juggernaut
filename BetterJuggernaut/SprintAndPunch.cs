using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using BattleTech;
using System.IO;

namespace BetterJuggernaut
{
    public class SprintAndPunch
    {
        public static string LogPath;
        public static string ModDirectory;

        public static void Init()
        {
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
        static bool Prefix(MechMeleeSequence __instance, bool __result)
        {

            using (var logwriter = File.AppendText(SprintAndPunch.LogPath))
            {

                logwriter?.WriteLine("Default prefix result of ConsumesFiring is {0}.", __result);
                // __result = true;
                Pilot curPilot = __instance.OwningMech.GetPilot();
                if (curPilot != null && curPilot.PassiveAbilities.Count > 0) //.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
                {
                    logwriter?.WriteLine("Found pilot with abilities.");
                    bool foundJug = false;
                    for (int i = 0; i < curPilot.PassiveAbilities.Count && !foundJug; i++)
                    {
                        
                        if (curPilot.PassiveAbilities[i].Def.Description.Id == "AbilityDefGu8")
                        {
                            logwriter?.WriteLine("Should be returning true.");
                            __result = false; //ignore firing cost with juggernaut active:
                            __instance.OwningMech.BracedLastRound = true;
                            __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning false.");
                        __result = true;
                    }
                    return false; //overriding if we have a pilot...
                }
                return true; //otherwise skip
            }
        }
    }

    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesFiring", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesFiring_Postfix
    {
        static void Postfix(MechMeleeSequence __instance, bool __result)
        {

            using (var logwriter = File.AppendText(SprintAndPunch.LogPath))
            {

                logwriter?.WriteLine("Default prefix result of ConsumesFiring is {0}.", __result);
                // __result = true;
                Pilot curPilot = __instance.OwningMech.GetPilot();
                if (curPilot != null && curPilot.PassiveAbilities.Count > 0) //.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
                {
                    logwriter?.WriteLine("Found pilot with abilities.");
                    bool foundJug = false;
                    for (int i = 0; i < curPilot.PassiveAbilities.Count && !foundJug; i++)
                    {

                        if (curPilot.PassiveAbilities[i].Def.Description.Id == "AbilityDefGu8")
                        {
                            logwriter?.WriteLine("Should be returning true.");
                            __result = false; //ignore firing cost with juggernaut active:
                                              //__instance.OwningMech.BracedLastRound = true;
                                              /// __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning false.");
                        __result = true;
                    }
                }
            }
        }
    }

    //movement
    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesMovement", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesMovement_Prefix
    {
        static bool Prefix(MechMeleeSequence __instance, bool __result)
        {

            using (var logwriter = File.AppendText(SprintAndPunch.LogPath))
            {

                logwriter?.WriteLine("Default prefix result of ConsumesMovement is {0}.", __result);
                // __result = true;
                Pilot curPilot = __instance.OwningMech.GetPilot();
                if (curPilot != null && curPilot.PassiveAbilities.Count > 0) //.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
                {
                    logwriter?.WriteLine("Found pilot with abilities.");
                    bool foundJug = false;
                    for (int i = 0; i < curPilot.PassiveAbilities.Count && !foundJug; i++)
                    {

                        if (curPilot.PassiveAbilities[i].Def.Description.Id == "AbilityDefGu8")
                        {
                            logwriter?.WriteLine("Should be returning true.");
                            __result = false; //ignore firing cost with juggernaut active:
                            __instance.OwningMech.BracedLastRound = true;
                            __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning false.");
                        __result = true;
                    }
                    return false; //overriding if we have a pilot...
                }
                return true; //otherwise skip
            }
        }
    }

    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesMovement", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesMovement_Postfix
    {
        static void Postfix(MechMeleeSequence __instance, bool __result)
        {

            using (var logwriter = File.AppendText(SprintAndPunch.LogPath))
            {

                logwriter?.WriteLine("Default prefix result of ConsumesMovement is {0}.", __result);
                // __result = true;
                Pilot curPilot = __instance.OwningMech.GetPilot();
                if (curPilot != null && curPilot.PassiveAbilities.Count > 0) //.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
                {
                    logwriter?.WriteLine("Found pilot with abilities.");
                    bool foundJug = false;
                    for (int i = 0; i < curPilot.PassiveAbilities.Count && !foundJug; i++)
                    {

                        if (curPilot.PassiveAbilities[i].Def.Description.Id == "AbilityDefGu8")
                        {
                            logwriter?.WriteLine("Should be returning true.");
                            __result = false; //ignore firing cost with juggernaut active:
                                              //__instance.OwningMech.BracedLastRound = true;
                                              /// __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning false.");
                        __result = true;
                    }
                }
            }
        }
    }
}
