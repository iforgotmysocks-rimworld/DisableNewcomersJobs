using System.Linq;
using Verse;

namespace DisableNewcomersJobs
{
    public class Helper
    {
        public static void AdjustWorkForPawn(Pawn pawn)
        {
            if (pawn?.RaceProps?.Humanlike != true) return;
            foreach (var workTypeSet in ModSettings.workTypeSets.Reverse<WorkTypeSet>())
            {
                var workTypeDef = DefDatabase<WorkTypeDef>.AllDefs.FirstOrDefault(x => x.defName == workTypeSet.workTypeDefName);

                if (workTypeDef == null)
                {
                    Log.Message($"worktype def {workTypeSet.workTypeDefName} was null");
                    ModSettings.workTypeSets.Remove(workTypeSet);
                    continue;
                }

                if (workTypeDef.relevantSkills.All(x => workTypeSet.minLevel >= (pawn?.skills?.GetSkill(x)?.Level ?? 1000))) pawn.workSettings.SetPriority(workTypeDef, 0);
            }
        }
    }
}
