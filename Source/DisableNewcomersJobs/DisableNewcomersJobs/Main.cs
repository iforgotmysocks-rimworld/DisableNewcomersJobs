﻿using Verse;

namespace DisableNewcomersJobs
{
    public class Main : Mod
    {
        public Main(ModContentPack content) : base(content)
        {
            GetSettings<ModSettings>();
        }

        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            GetSettings<ModSettings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Disable Newcomer's Jobs";
        }
    }
}
