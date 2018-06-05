using BattleTech;
using Harmony;
using System;
using System.Reflection;

namespace SellMechParts
{
    public class SellMechParts
    {
        internal static string ModDirectory;
        public static void Init(string directory, string settingsJSON) {
            var harmony = HarmonyInstance.Create("de.morphyum.SellMechParts");        
            var original = typeof(Shop).GetMethod("SellInventoryItem", new Type[] { typeof(string), typeof(ShopItemType), typeof(bool), typeof(int)});
            var prefix = AccessTools.Method(typeof(Shop_SellInventoryItem_Patch), "Prefix");
            harmony.Patch(original, new HarmonyMethod(prefix), null, null);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModDirectory = directory;
        }
    }
}
