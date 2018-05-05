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

 
    [HarmonyPatch(typeof(BattleTech.MechMeleeSequence))]
    [HarmonyPatch("ConsumesFiring", PropertyMethod.Getter)]
    public static class BattleTech_ConsumesMovement_PostFix
    {
        static void Postfix(MechMeleeSequence __instance, bool __result)
        {
            if (__instance.OwningMech.StatCollection.GetValue<int>("MeleeHitPushBackPhases") > 0)
            {
                __result = false; //ignore firing cost with juggernaut active: let a mech brace or otherwise fire at point blank range if they wish to.
            }
            // otherwise we do nothing to alter the result
        }
    }
    
}
