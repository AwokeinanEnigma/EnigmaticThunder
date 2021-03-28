using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class Effects : Module
    {
        internal static ObservableCollection<EffectDef> EffectDefDefinitions = new ObservableCollection<EffectDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static EffectDef CreateGenericEffectDef(GameObject effect)
        {
            if (!effect)
            {
                LogCore.LogE(string.Format("Effect prefab: \"{0}\" is null", effect.name));

                return null;
            }

            var effectComp = effect.GetComponent<EffectComponent>();
            if (effectComp == null)
            {
                LogCore.LogE(string.Format("Effect prefab: \"{0}\" does not have an EffectComponent.", effect.name));
                return null;
            }

            var vfxAttrib = effect.GetComponent<VFXAttributes>();
            if (vfxAttrib == null)
            {
                LogCore.LogE(string.Format("Effect prefab: \"{0}\" does not have a VFXAttributes component.", effect.name));
                return null;
            }

            var def = new EffectDef
            {
                prefab = effect,
                prefabEffectComponent = effectComp,
                prefabVfxAttributes = vfxAttrib,
                prefabName = effect.name,
                spawnSoundEventName = effectComp.soundName,
                //cullMethod = new Func<EffectData, bool>()
            };
            return def;
        }

        public static void RegisterEffect(EffectDef EffectDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (EffectDefDefinitions.Contains(EffectDef))
            {
                LogCore.LogE(EffectDef + " has already been registered to the EffectDef Catalog, please do not register the same EffectDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            EffectDefDefinitions.Add(EffectDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<EffectDef> defs = new List<EffectDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (EffectDef def in EffectDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.effectDefs = defs.ToArray();
        }
    }
}