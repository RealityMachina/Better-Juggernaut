using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Harmony;
using BattleTech;

namespace BetterJuggernaut
{
    public class SprintAndPunch
    {
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("Battletech.realitymachina.BetterJuggernaut");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence), "GenerateMeleePath")]
    public static class BattleTech_GenerateMeleePath_Patch
    {
        static bool Prefix(MechMeleeSequence __instance)
        {
            //no easy way to avoid incompatibilites here: we're completely overriding the logic so we can do what we need to do
            return false;
        }
    }

    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesMovement", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesMovement_PostFix
    {
        static void Postfix(MechMeleeSequence __instance, bool __result)
        {
            if (__instance.OwningMech.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
            {
                __result = false; //ignore movement cost with juggernaut active: combined with bulwark, this will auto brace the mech.
            }
            // otherwise we do nothing to alter the result
        }
    }

    [HarmonyPatch(typeof(MechMeleeSequence), "GenerateMeleePath")]
    public static class BattleTech_GenerateMeleePath_Postfix
    {
        static void Postfix(MechMeleeSequence __instance)
        {
            Type meleeSeqType = typeof(MechMeleeSequence);
            FieldInfo field1 = meleeSeqType.GetField("MoveIsCharge", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo field2 = meleeSeqType.GetField("moveSequence", BindingFlags.NonPublic | BindingFlags.Instance);
            ActorMovementSequence repSeq = new ActorMovementSequence(__instance.OwningMech)
            {
                IgnoreEndSmoothing = false,
                meleeType = __instance.selectedMeleeType
            };
            // __instance.MoveIsCharge = false;
            field1.SetValue(__instance, false); //this should set moveischarge to false?
            if (__instance.OwningMech.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
            {
                __instance.OwningMech.Pathing.SetSprinting(); //sprint if juggernaut is active
            }
            else{
                __instance.OwningMech.Pathing.SetWalking();
            }
            
            __instance.OwningMech.Pathing.SetMeleeTarget(__instance.MeleeTarget as AbstractActor);
            __instance.OwningMech.Pathing.SelectMeleeDest(__instance.DesiredMeleePosition);
            __instance.OwningMech.Pathing.Update(__instance.MeleeTarget.CurrentPosition, false);
            field2.SetValue(__instance, repSeq);
    
          //  __instance.moveSequence = new ActorMovementSequence(__instance.OwningMech);
          //  __instance.moveSequence.IgnoreEndSmoothing = __instance.MoveIsCharge;
         //   __instance.moveSequence.meleeType = __instance.selectedMeleeType;
            __instance.OwningMech.Pathing.ClearMeleeTarget();
        }
    }
}
