using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame.PortalQuack
{
    [HarmonyPatch(typeof(AdultSwimLogo))]
    [HarmonyPatch("Update")]
    public class LogoPatch
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> LevelPatch(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            codes[29].operand = AccessTools.Constructor(typeof(DuckTeamLogo));
            return codes.AsEnumerable();
        }

    }
}
