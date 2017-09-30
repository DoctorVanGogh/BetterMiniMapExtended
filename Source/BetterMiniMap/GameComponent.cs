﻿using UnityEngine;
using Verse;
using RimWorld;
using Harmony;
using RimWorld.Planet;
using System.Linq;

namespace BetterMiniMap
{
    [StaticConstructorOnStartup]
    class MiniMap_GameComponent : GameComponent
    {
        private static MiniMapWindow miniMap;

        private static bool initialized;

        // used to toggle minimap
        private static bool researchPal; 
        private static bool relationsTab;

        public MiniMap_GameComponent(Game g) => Initialize();

        public MiniMap_GameComponent() => Initialize();

        private static void Initialize()
        {
            if (!initialized)
            {
                // used for toggling minimap 
                researchPal = ModLister.AllInstalledMods.FirstOrDefault(m => m.Name == "ResearchPal")?.Active == true;
                relationsTab = ModLister.AllInstalledMods.FirstOrDefault(m => m.Name == "Relations Tab")?.Active == true;
                initialized = true;
            }
        }

        // NOTE: not sure if I like this...
        static MiniMap_GameComponent()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.whyisthat.betterminimap.gamecomponent");
            harmony.Patch(AccessTools.Method(typeof(UIRoot_Play), nameof(UIRoot_Play.Init)), null, new HarmonyMethod(typeof(MiniMap_GameComponent), nameof(AddDefaultWindow)));
            harmony.Patch(AccessTools.Method(typeof(MainTabsRoot), nameof(MainTabsRoot.ToggleTab)), null, new HarmonyMethod(typeof(MiniMap_GameComponent), nameof(ToggleMiniMap)));
            harmony.Patch(AccessTools.Method(typeof(MainButtonWorker_ToggleWorld), nameof(MainButtonWorker_ToggleWorld.Activate)), null, new HarmonyMethod(typeof(MiniMap_GameComponent), nameof(ToggleMiniMap_WorldTab)));
        }
        
        // NOTE: this is done here to avoid lazy loading.
        static void AddDefaultWindow()
        {
#if DEBUG
            Log.Message("MiniMap_GameComponent.Initilize");
#endif
            miniMap = new MiniMapWindow();
            Find.WindowStack.Add(miniMap);
        }

        static void ToggleMiniMap(MainTabsRoot __instance, MainButtonDef newTab)
        {
#if DEBUG
            Log.Message($"MainTabsRoot.ToggleTab: {newTab} {__instance.OpenTab != null}");
#endif
            if (miniMap.Active)
            {
                if (__instance.OpenTab != null)
                {
                    switch (newTab.defName)
                    {
                        case "Research":
                            if (researchPal) miniMap.Toggle(false);
                            break;
                        case "Factions":
                            if (relationsTab) miniMap.Toggle(false);
                            break;
                        default:
                            miniMap.Toggle(!WorldRendererUtility.WorldRenderedNow);
                            break;
                    }
                }
                else
                    miniMap.Toggle(!WorldRendererUtility.WorldRenderedNow);
            }
        }

        static void ToggleMiniMap_WorldTab()
        {
#if DEBUG
            Log.Message($"MainButtonWorker_ToggleWorld.Activate: {Find.World.renderer.wantedMode}");
#endif
            if (miniMap.Active)
                miniMap.Toggle(!WorldRendererUtility.WorldRenderedNow);
        }

        public override void GameComponentOnGUI()
        {
            base.GameComponentOnGUI();
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F8)
            {
                miniMap.Toggle(Find.WindowStack.Windows.IndexOf(miniMap) == -1); // TODO: this is a bit nasty...
                miniMap.Active = false;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<MiniMapWindow>(ref miniMap, "miniMap");
            // NOTE: handles adding mod to existing save
            if (Scribe.mode == LoadSaveMode.PostLoadInit && miniMap == null)
                AddDefaultWindow();
        }

    }
}
