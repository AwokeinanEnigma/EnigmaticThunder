using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnigmaticThunder.Util;
using RoR2;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace EnigmaticThunder.Modules
{/*
    public class DoT : Module
    {
        internal static ReadOnlyCollection<DotController.DotDef> BaseDotDefs;

        public delegate void CustomDotAddAction(DotController self, DotController.DotStack dotStack);
        public delegate void CustomDotUpdateAction(DotController self);
        private struct DotActionContainer
        {
            public CustomDotAddAction Add;
            public CustomDotUpdateAction Update;
        }
        private static Dictionary<DotController.DotDef, DotActionContainer> CustomDotDefs = new Dictionary<DotController.DotDef, DotActionContainer>();

        internal override void Load()
        {
            base.Load();

            On.RoR2.DotController.Awake += DotConAwake;
            On.RoR2.DotController.InitDotCatalog += DotCatalogInit;
            On.RoR2.DotController.GetDotDef += GetDotDef;
            IL.RoR2.DotController.AddDot += AddDotIL;
            On.RoR2.DotController.FixedUpdate += OnDotControllerUpdate;
        }

        public static DotController.DotIndex RegisterDot(DotController.DotDef def, CustomDotAddAction addAction, CustomDotUpdateAction updateAction)
        {
            if (CustomDotDefs.ContainsKey(def))
            {
                LogCore.LogE(def + " has already been registered, please do not register the same DotDef twice.");
                return DotController.DotIndex.None;
            }

            CustomDotDefs.Add(def, new DotActionContainer() { Add = addAction, Update = updateAction });

            return (DotController.DotIndex)((int)DotController.DotIndex.Count + CustomDotDefs.Count);
        }

        public static DotController.DotIndex RegisterDot(float interval, float damageCoef, DamageColorIndex colorIndex, BuffDef associatedBuff, CustomDotAddAction addAction, CustomDotUpdateAction updateAction)
        {
            return RegisterDot(new DotController.DotDef
            {
                interval = interval,
                damageCoefficient = damageCoef,
                damageColorIndex = colorIndex,
                associatedBuff = associatedBuff
            }, addAction, updateAction);
        }

        private static void DotConAwake(On.RoR2.DotController.orig_Awake orig, DotController self)
        {
            orig(self);
            self.dotTimers = new float[DotController.dotDefs.Length];
        }

        private static void DotCatalogInit(On.RoR2.DotController.orig_InitDotCatalog orig)
        {
            orig();

            BaseDotDefs = DotController.dotDefs.ToList().AsReadOnly();

            DotController.dotDefs = BaseDotDefs.Concat(CustomDotDefs.Keys.ToList()).ToArray();
        }

        private static DotController.DotDef GetDotDef(On.RoR2.DotController.orig_GetDotDef orig, DotController self, DotController.DotIndex dotIndex)
        {
            return DotController.dotDefs[(int)dotIndex];
        }

        private static void AddDotIL(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            int dotStackLoc = 5;

            bool found = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdloc(out dotStackLoc),
                x => x.MatchLdcI4((int)DamageType.Generic),
                x => x.MatchStfld(HarmonyLib.AccessTools.Field(typeof(DotController.DotStack), nameof(DotController.DotStack.damageType))),
                x => x.MatchLdarg(out _),
                x => x.MatchSwitch(out _)
            );
            if (!found)
            {
                LogCore.LogE("Add DoT IL hook failed, aborting...");
                return;
            }

            c.Index += 3;
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldloc, dotStackLoc);
            c.EmitDelegate<Action<DotController, DotController.DotStack>>((self, stack) =>
            {
                if (CustomDotDefs.TryGetValue(stack.dotDef, out DotActionContainer container))
                {
                    container.Add?.Invoke(self, stack);
                }
            });
        }

        private static void OnDotControllerUpdate(On.RoR2.DotController.orig_FixedUpdate orig, DotController self)
        {
            for (int i = (int)DotController.DotIndex.Count; i < DotController.dotDefs.Length; i++)
            {
                if (self.HasDotActive((DotController.DotIndex)i) && CustomDotDefs.TryGetValue(DotController.dotDefs[i], out DotActionContainer container))
                {
                    container.Update?.Invoke(self);
                }
            }
            orig(self);
            //where's muh orig(self)?
        }
    }*/
}
