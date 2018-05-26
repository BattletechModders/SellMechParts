using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellMechParts
{
    public class SellMechParts
    {
        public static void Init() {
            var harmony = HarmonyInstance.Create("de.morphyum.SellMechParts");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
