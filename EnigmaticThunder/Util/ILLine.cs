using System;
using System.Reflection;
using System.Diagnostics;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace EnigmaticThunder.Util
{
    internal class ILLine : Module
    {
        internal override void Load()
        {
            base.Load();
            new ILHook(typeof(StackTrace).GetMethod("AddFrames", BindingFlags.Instance | BindingFlags.NonPublic), new ILContext.Manipulator(IlHook));
        }

        //replaces the call to GetFileLineNumber to a call to GetLineOrIL
        private static void IlHook(ILContext il)
        {
            var cursor = new ILCursor(il);
            
            bool found = cursor.TryGotoNext(
                x => x.MatchCallvirt(typeof(StackFrame).GetMethod("GetFileLineNumber", BindingFlags.Instance | BindingFlags.Public))
            );

            if (!found)
            {
                return;
            }

            cursor.RemoveRange(2);
            cursor.EmitDelegate<Func<StackFrame, string>>(GetLineOrIL);
        }

        //first gets the debug line number (C#) and only if that is not available returns the IL offset (jit might change it a bit)
        private static string GetLineOrIL(StackFrame instace)
        {
            var line = instace.GetFileLineNumber();
            if (line != StackFrame.OFFSET_UNKNOWN && line != 0)
            {
                return line.ToString();
            }

            return "IL_" + instace.GetILOffset().ToString("X4");
        }
    }
}
