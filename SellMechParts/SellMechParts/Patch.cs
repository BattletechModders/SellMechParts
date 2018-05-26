using BattleTech;
using Harmony;
using HBS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SellMechParts {

    [HarmonyPatch(typeof(Shop), "GetAllInventoryShopItems")]
    public static class Shop_GetAllInventoryShopItems_Patch {
        static void Postix(Shop __instance, ref List<ShopDefItem> __result) {
            try {
                
            }
            catch (Exception e) {
                Logger.LogError(e);

            }
        }
    }
}