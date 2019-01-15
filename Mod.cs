using System;
using System.IO;
using System.Reflection;
using Harmony;
using MoreMachines.Other;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace MoreMachines
{
    public class Mod : StardewModdingAPI.Mod
    {
        public static Mod instance;
        private HarmonyInstance harmony;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            instance = this;

            helper.Events.GameLoop.GameLaunched += onGameLaunched;
            
            harmony = HarmonyInstance.Create("spacechase0.MoreMachines");
            doPrefix(typeof(StardewValley.Object), "performObjectDropInAction", typeof(ObjectPerformDropInActionHook));
        }

        private void doPrefix(Type origType, string origMethod, Type newType)
        {
            doPrefix(origType.GetMethod(origMethod), newType.GetMethod("Prefix"));
        }
        private void doPrefix(MethodInfo orig, MethodInfo prefix)
        {
            try
            {
                Log.trace($"Doing prefix patch {orig}:{prefix}...");
                harmony.Patch(orig, new HarmonyMethod(prefix), null);
            }
            catch (Exception e)
            {
                Log.error($"Exception doing prefix patch {orig}:{prefix}: {e}");
            }
        }

        /// <summary>Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void onGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var ja = Helper.ModRegistry.GetApi<JsonAssetsApi>("spacechase0.JsonAssets");
            if ( ja == null )
            {
                Log.error("Failed to get JA API!");
                return;
            }

            ja.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets"));
        }
    }
}
