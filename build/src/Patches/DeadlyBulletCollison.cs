using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;


namespace DuckGame.PortalQuack
{
    public class DeadlyBulletCollison
    {
        [HarmonyPatch(typeof(Bullet))]
        
        static class Transplier 
        {
            [HarmonyPatch("RaycastBullet")]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                Label impactJumpIn = il.DefineLabel();
                //Label impactJumpOut = il.DefineLabel();

                codes[284].labels.Add(impactJumpIn);
                //codes[290].labels.Add(impactJumpOut);

                List<CodeInstruction> deadlyCodes = new List<CodeInstruction>()
                {
                        new CodeInstruction(OpCodes.Ldloc_S, 7),
                        new CodeInstruction(OpCodes.Isinst, typeof(PortalCustom)),
                        new CodeInstruction(OpCodes.Brtrue, codes[170].operand)
                };

                List<CodeInstruction> impactCodes = new List<CodeInstruction>()
                {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(PhysicalBullet))),
                        new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(Bullet), "_physicalBullet"))
                };

                codes.InsertRange(219, impactCodes);    
                codes.InsertRange(171, deadlyCodes);

                return codes.AsEnumerable();
            }


        }

    }
}
