using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using Qualitizer.Other;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Qualitizer
{
    public class Mod : StardewModdingAPI.Mod
    {
        public static Mod instance;
        private HarmonyInstance harmony;

        public override void Entry(IModHelper helper)
        {
            instance = this;

            GameEvents.FirstUpdateTick += firstTick;
            
            harmony = HarmonyInstance.Create("spacechase0.Qualitizer");
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

        private void firstTick(object sender, EventArgs args)
        {
            var ja = Helper.ModRegistry.GetApi<JsonAssetsApi>("spacechase0.JsonAssets");
            if ( ja == null )
            {
                Log.error("Failed to get JA API!");
            }

            ja.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets"));
        }
    }
}
