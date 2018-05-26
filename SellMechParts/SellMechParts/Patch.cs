using BattleTech;
using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SellMechParts {

 [HarmonyPatch(typeof(Shop), "GetAllInventoryShopItems")]
    public static class Shop_GetAllInventoryShopItems_Patch {
        static void Postfix(Shop __instance, ref List<ShopDefItem> __result) {
            try {
                SimGameState Sim = (SimGameState)ReflectionHelper.GetPrivateField(__instance, "Sim");
                List<ChassisDef> allInventoryMechDefs = (List<ChassisDef>)ReflectionHelper.InvokePrivateMethode(Sim, "GetAllInventoryMechParts", null);
                foreach (ChassisDef chassisDef in allInventoryMechDefs) {
                    ShopDefItem shopDefItem = new ShopDefItem();
                    shopDefItem.ID = chassisDef.Description.Id.Replace("chassisdef", "mechdef");
                    shopDefItem.Count = chassisDef.MechPartCount;
                    float num = (float)chassisDef.Description.Cost;
                    shopDefItem.SellCost = Mathf.FloorToInt(num * Sim.Constants.Finances.ShopSellModifier);
                    shopDefItem.Type = ShopItemType.MechPart;
                    __result.Add(shopDefItem);
                }
            }
            catch (Exception e) {
                Logger.LogError(e);

            }
        }
    }

    public static class Shop_SellInventoryItem_Patch {
        static bool Prefix(Shop __instance, ref bool __result, string id, ShopItemType type, bool isDamaged, int cost) {
            try {

                
                SimGameState Sim = (SimGameState)ReflectionHelper.GetPrivateField(__instance, "Sim");
                Type type2;
                if (type == ShopItemType.MechPart) {
                    ReflectionHelper.InvokePrivateMethode(Sim, "RemoveItemStat", new object[] { id, "MECHPART", false});
                    Sim.AddFunds(cost, "Store", true);
                    __result = true;
                    return false;
                }
                if (type == ShopItemType.Mech || type == ShopItemType.Chassis_DEPRECATED) {
                    type2 = typeof(MechDef);
                }
                else {
                    ComponentType componentType = Shop.ShopItemTypeToComponentType(type);
                    type2 = SimGameState.GetTypeFromComponent(componentType);
                }
                int itemCount = Sim.GetItemCount(id, type2, (!isDamaged) ? SimGameState.ItemCountType.UNDAMAGED_ONLY : SimGameState.ItemCountType.DAMAGED_ONLY);
                if (itemCount < 1) {
                    Logger.LogLine("Count 0");
                    __result = false;
                    return false;
                }
                Sim.AddFunds(cost, "Store", true);
                Sim.RemoveItemStat(id, type2, isDamaged);
                __result = true;

                return false;
            }
            catch (Exception e) {
                Logger.LogError(e);
                return false;
            }
        }
    }
    
}