using RimWorld;
using Verse;

namespace DisableNewcomersJobs
{
    public class WorkTypeSet : IExposable
    {
        public int minLevel = 0;
        public string workTypeDefName;

        public void ExposeData()
        {
            Scribe_Values.Look(ref minLevel, "minLevel", 0);
            Scribe_Values.Look(ref workTypeDefName, "workTypeDef");
        }
    }
}