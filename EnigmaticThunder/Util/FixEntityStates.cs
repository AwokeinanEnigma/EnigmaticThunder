using System;
using System.Collections.Generic;
using System.Reflection;
using EntityStates;
using MonoMod.RuntimeDetour;
using RoR2;

internal static class FixEntityStates
{
    internal static List<Type> entityStates = new List<Type>();

    private static Hook set_stateTypeHook;
    private static Hook set_typeNameHook;
    private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
    private delegate void set_stateTypeDelegate(ref SerializableEntityStateType self, Type value);
    private delegate void set_typeNameDelegate(ref SerializableEntityStateType self, String value);

    internal static void RegisterStates()
    {
        //LogCore.LogI("hi");
        Type type = typeof(SerializableEntityStateType);
        HookConfig cfg = default;
        cfg.Priority = Int32.MinValue;
        set_stateTypeHook = new Hook(type.GetMethod("set_stateType", allFlags), new set_stateTypeDelegate(SetStateTypeHook), cfg);
        set_typeNameHook = new Hook(type.GetMethod("set_typeName", allFlags), new set_typeNameDelegate(SetTypeName), cfg);
    }

    private static void SetStateTypeHook(ref this SerializableEntityStateType self, Type value)
    {
        //LogCore.LogI("hi2");
        self._typeName = value.AssemblyQualifiedName;
    }

    private static void SetTypeName(ref this SerializableEntityStateType self, String value)
    {
        Type t = GetTypeFromName(value);
        //LogCore.LogI("hi3"); if (t != null)
        {
            //LogCore.LogI("hi4");
            self.SetStateTypeHook(t);
        }
    }

    private static Type GetTypeFromName(String name)
    {
        //LogCore.LogI("hi5    ");
        Type[] types = EntityStateCatalog.stateIndexToType;
        return Type.GetType(name);
    }
}
