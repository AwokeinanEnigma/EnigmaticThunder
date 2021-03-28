using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for registering EffectDefs to the EffectCatalog
    /// </summary>
    public class Effects : Module
    {
        internal static ObservableCollection<EffectDef> EffectDefDefinitions = new ObservableCollection<EffectDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Creates an EffectDef from a prefab and adds it to the EffectCatalog.
        /// The prefab must have an the following components: EffectComponent, VFXAttributes
        /// For more control over the EffectDef, use RegisterEffect(EffectDef)
        /// </summary>
        /// <param name="effect">The prefab of the effect to be added</param>
        public static void RegisterGenericEffect(GameObject effect)
        {
            if (!effect)
            {
                LogCore.LogE(string.Format("Effect prefab: \"{0}\" is null", effect.name));

                
            }

            var effectComp = effect.GetComponent<EffectComponent>();
            if (effectComp == null)
            {
                LogCore.LogE(string.Format("Effect prefab: \"{0}\" does not have an EffectComponent.", effect.name));
                
            }

            var vfxAttrib = effect.GetComponent<VFXAttributes>();
            if (vfxAttrib == null)
            {
                LogCore.LogE(string.Format("Effect prefab: \"{0}\" does not have a VFXAttributes component.", effect.name));
                
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
            RegisterEffect(def);
        }

        /// <summary>
        /// Creates an EffectDef from a prefab.
        /// The prefab must have an the following components: EffectComponent, VFXAttributes
        /// </summary>
        /// <param name="effect">The prefab of the effect to be added</param>
        /// <returns>The newly created EffectDef</returns>
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


        /// <summary>
        /// Adds an EffectDef to the EffectCatalog.
        /// </summary>
        /// <param name="effectDef">The EffectDef to add</param>
        public static void RegisterEffect(EffectDef effectDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (EffectDefDefinitions.Contains(effectDef) || !effectDef.prefab)
            {
                LogCore.LogE(effectDef + " has already been registered to the EffectDef Catalog, please do not register the same EffectDef twice. Or, the EffectDef does not have a prefab.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            EffectDefDefinitions.Add(effectDef);
        }

        internal override void ModifyContentPack(ContentPack pack)
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