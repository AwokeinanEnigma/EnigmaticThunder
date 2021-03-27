using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class EffectDefs : Module
    {
        internal static ObservableCollection<EffectDef> EffectDefDefinitions = new ObservableCollection<EffectDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(EffectDef EffectDef)
        {
            //Check if the SurvivorDef has already been added.
            if (EffectDefDefinitions.Contains(EffectDef))
            {
                LogCore.LogE(EffectDef + " has already been added to the EffectDef Catalog, please do not try to add the same EffectDef twice.");
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