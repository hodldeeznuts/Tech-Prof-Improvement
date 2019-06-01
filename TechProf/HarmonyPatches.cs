using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TechProf
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.harmony.rimworld.mod.techprof");

            harmony.Patch(
                    original: AccessTools.Method(
                        type: typeof(ResearchManager),
                        name: nameof(ResearchManager.ResearchPerformed)
                    ),
                    prefix: new HarmonyMethod(patchType, nameof(ResearchPerformed))
                );
        }

        static void ResearchPerformed(ResearchManager __instance, ref float amount, Pawn researcher)
        {
            // If pawn has an intellectual inspiration
            if (researcher.InspirationDef != DefDatabase<InspirationDef>.GetNamed("Frenzy_Research"))
            {
                return;
            }

            // Throw exception if InspirationDef is missing an instance of its Worker
            if (!(researcher.InspirationDef.Worker is InspirationWorker_Research))
            {
                throw new Exception("InspirationDef is missing instance of InspirationWorker_Research.");
            }

            // Find the instance of the worker
            InspirationWorker_Research worker = researcher.InspirationDef.Worker as InspirationWorker_Research;

            // Track the research points consumed
            worker.UseInspirationResearchPoints(amount);

            // If the workers research point balance is less than or equal to 0, forcefully end the inspiration
            if (worker.ResearchPointsLeft <= 0)
            {
                researcher.mindState.inspirationHandler.EndInspiration(researcher.InspirationDef);
            }

            // Double the research point ouput
            amount *= 2;
        }
    }
}
