using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace DisableNewcomersJobs
{
    internal class ModSettings : Verse.ModSettings
    {
        public static List<WorkTypeSet> workTypeSets;
        private Vector2 scrollPos;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref workTypeSets, "workTypeSets", LookMode.Deep);
        }

        public void DoWindowContents(Rect rect)
        {
            var options = new Listing_Standard();
            var viewRect = new Rect(0f, 0f, rect.width - 60, 1400 + (50 * workTypeSets.Count));
            var smallerOutRect = new Rect(rect.x, rect.y, rect.width, rect.height - 60);

            Widgets.BeginScrollView(smallerOutRect, ref scrollPos, viewRect);
            options.Begin(viewRect);

            foreach (var def in DefDatabase<WorkTypeDef>.AllDefs)
            {
                var savedDef = workTypeSets.FirstOrDefault(x => x.workTypeDefName == def.defName);
                Text.Font = GameFont.Medium;
                options.Label(def.defName);
                Text.Font = GameFont.Small;
                var enabled = savedDef != null;
                options.CheckboxLabeled("Disable job automatically", ref enabled);
                var intVal = savedDef?.minLevel ?? 0;
                if (enabled)
                {
                    options.Label($"Set level for disableing: {intVal}");
                    var buffer = string.Empty;
                    intVal = Convert.ToInt32(options.Slider(intVal, 0, 20));
                }
                options.Gap();

                if (!enabled) workTypeSets.RemoveAll(x => x.workTypeDefName == def.defName);
                else
                {
                    var existing = workTypeSets.FirstOrDefault(x => x.workTypeDefName == def.defName);
                    if (existing == null) workTypeSets.Add(new WorkTypeSet() { workTypeDefName = def.defName, minLevel = intVal });
                    else existing.minLevel = intVal;
                }
            }

            options.End();
            Widgets.EndScrollView();
        }

        internal static void Initialize()
        {
            if (workTypeSets != null) return;
            workTypeSets = new List<WorkTypeSet>();
        }
    }

    [StaticConstructorOnStartup]
    class Startup { static Startup() { ModSettings.Initialize(); } }
}
