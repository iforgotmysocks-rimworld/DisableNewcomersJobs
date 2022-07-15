using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DisableNewcomersJobs
{
    internal class DisableWorkWC : WorldComponent
    {
        private int ticks = 0;
        private readonly int checkTick = 300;
        private List<string> checkedPawnIds = new List<string>();
        private bool addedExisting;

        public DisableWorkWC(World world) : base(world)
        {
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref checkedPawnIds, "checkedPawnIds", LookMode.Value);
            Scribe_Values.Look(ref addedExisting, "addedExisting", false);
        }

        public override void WorldComponentTick()
        {
            if (ticks >= checkTick)
            {
                ticks = 0;
                CheckPawnChanges();
            }

            ticks++;
        }

        private void CheckPawnChanges()
        {
            var pawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction?.Where(x => x?.RaceProps?.Humanlike == true);

            if (!addedExisting)
            {
                if (pawns.Any()) checkedPawnIds = pawns.Select(x => x.ThingID).ToList();
                addedExisting = true;
            }

            if (!pawns.Any()) return;
            foreach (var pawn in pawns.Where(x => !checkedPawnIds.Contains(x.ThingID))) AdjustWorkForPawn(pawn);
            checkedPawnIds = pawns.Select(x => x.ThingID).ToList();
        }

        private void AdjustWorkForPawn(Pawn pawn)
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
