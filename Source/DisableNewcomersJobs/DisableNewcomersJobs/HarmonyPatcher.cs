using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DisableNewcomersJobs
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            var harmony = new Harmony("iforgotmysocks." + Assembly.GetExecutingAssembly().GetName().Name);
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) } )]
    public class PawnGenerator_GeneratePawn
    {
        public static void Postfix(ref Pawn __result)
        {
            if (__result?.Faction != Faction.OfPlayer) return;
            Helper.AdjustWorkForPawn(__result);
        }
    }

    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnKindDef), typeof(Faction) })]
    public class PawnGenerator_GeneratePawn2
    {
        public static void Postfix(ref Pawn __result)
        {
            if (__result?.Faction != Faction.OfPlayer) return;
            Helper.AdjustWorkForPawn(__result);
        }
    }

    [HarmonyPatch(typeof(Thing), "SetFaction")]
    public class Thing_SetFaction
    {
        public static void Postfix(Thing __instance)
        { 
            if (__instance?.Faction != Faction.OfPlayer || !(__instance is Pawn pawn)) return;
            Helper.AdjustWorkForPawn(pawn);
        }
    }
}
