using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreMachines
{
    public class ObjectPerformDropInActionHook
    {
        public static bool Prefix( StardewValley.Object __instance, Item dropInItem, bool probe, Farmer who, bool __result )
        {
            if (__instance.Name == "Qualitizer")
            {
                __result = false;

                var obj = dropInItem as StardewValley.Object;
                if (__instance.heldObject.Value != null || obj == null || obj.bigCraftable.Value || obj.Quality == 4)
                {
                    return false;
                }

                if (__instance.bigCraftable.Value && !probe && (obj != null && __instance.heldObject.Value == null))
                    __instance.scale.X = 5f;

                int oldSellPrice = (dropInItem.getOne() as StardewValley.Object).sellToStorePrice();

                __instance.heldObject.Value = dropInItem.getOne() as StardewValley.Object;
                ++__instance.heldObject.Value.Quality;
                if (__instance.heldObject.Value.Quality == 3)
                    ++__instance.heldObject.Value.Quality;

                int priceDiff = __instance.heldObject.Value.sellToStorePrice() - oldSellPrice;
                if (who.money < priceDiff * 2)
                {
                    __instance.heldObject.Value = null;
                    return false;
                }

                if (!probe)
                {
                    who.currentLocation.playSound("coin");
                    __instance.MinutesUntilReady = __instance.heldObject.Value.sellToStorePrice() / 25 * 10;
                    --obj.Stack;
                    if (obj.Stack <= 0)
                        who.removeItemFromInventory(obj);
                    who.money -= priceDiff * 2;
                }

                __result = true;
                return false;
            }
            else if (__instance.Name == "Dequalitizer")
            {
                __result = false;

                var obj = dropInItem as StardewValley.Object;
                if (__instance.heldObject.Value != null || obj == null || obj.bigCraftable.Value || obj.Quality == 0)
                {
                    return false;
                }

                if (__instance.bigCraftable.Value && !probe && (obj != null && __instance.heldObject.Value == null))
                    __instance.scale.X = 5f;

                int oldSellPrice = (dropInItem.getOne() as StardewValley.Object).sellToStorePrice();

                __instance.heldObject.Value = dropInItem.getOne() as StardewValley.Object;
                --__instance.heldObject.Value.Quality;
                if (__instance.heldObject.Value.Quality == 3)
                    --__instance.heldObject.Value.Quality;

                int priceDiff = oldSellPrice - __instance.heldObject.Value.sellToStorePrice();

                if (!probe)
                {
                    who.currentLocation.playSound("coin");
                    __instance.MinutesUntilReady = __instance.heldObject.Value.sellToStorePrice() / 25 * 10;
                    --obj.Stack;
                    if (obj.Stack <= 0)
                        who.removeItemFromInventory(obj);
                    who.money += priceDiff;
                }

                __result = true;
                return false;
            }

            return true;
        }
    }
}
