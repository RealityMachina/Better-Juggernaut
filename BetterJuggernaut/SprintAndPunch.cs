using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using BattleTech;
using BattleTech.UI;
using System.IO;

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
        static bool Prefix(MechMeleeSequence __instance, ref bool __result)
        {
            if (__instance == null)
            {
                throw new ArgumentNullException(nameof(__instance));
            }

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
                            logwriter?.WriteLine("Should be returning false for ConsumesFiring.");
                            __result = false; //ignore firing cost with juggernaut active:
                            __instance.OwningMech.BracedLastRound = true;
                            __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                            return false; //overriding if we have this...
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning true for ConsumesFiring.");
                        __result = true;
                    }
                }
                return true; //otherwise skip
            }
        }
    }

    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesFiring", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesFiring_Postfix
    {
        static void Postfix(MechMeleeSequence __instance, ref bool __result)
        {
            if (__instance == null)
            {
                throw new ArgumentNullException(nameof(__instance));
            }

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
                            logwriter?.WriteLine("Should be returning false for ConsumesFiring.");
                            __result = false; //ignore firing cost with juggernaut active:
                                              //__instance.OwningMech.BracedLastRound = true;
                                              /// __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                            SprintAndPunch.justPunched = foundJug;
                            SprintAndPunch.jugMech = __instance.OwningMech;
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning true for ConsumesFiring.");
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
                            logwriter?.WriteLine("Should be returning false for ConsumesMovement.");
                            __result = false; //ignore firing cost with juggernaut active:
                            foundJug = true;
                            return false; //overriding if we have this...
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning true for ConsumesMovement.");
                        __result = true;
                    }

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
                            logwriter?.WriteLine("Should be returning false for ConsumesMovement.");
                            __result = false; //ignore firing cost with juggernaut active:
                                              //__instance.OwningMech.BracedLastRound = true;
                                              /// __instance.OwningMech.ApplyInstabilityReduction(StabilityChangeSource.Bracing);
                            foundJug = true;
                            SprintAndPunch.justPunched = foundJug;
                            SprintAndPunch.jugMech = __instance.OwningMech;
                        }

                    }
                    if (!foundJug) //assume melee cost like normal
                    {
                        logwriter?.WriteLine("Should be returning true for COnsumesMovement.");
                        __result = true;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(BattleTech.UI.CombatHUDMechwarriorTray))]
    [HarmonyPatch("ResetMechwarriorButtons")]
    public static class BattleTech_ResetMechwarriorButtons_Postfix
    {
        static void Postfix(CombatHUDMechwarriorTray __instance, AbstractActor actor)
        {
            Mech mech = actor as Mech;

            if (SprintAndPunch.justPunched && mech != null && mech == SprintAndPunch.jugMech)
            {
                __instance.FireButton.ResetButtonIfNotActive(actor);
                SprintAndPunch.justPunched = false;
                SprintAndPunch.jugMech = null;
            }
        }
    }

}

