using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class BuffDefs : Module
    {
        internal static ObservableCollection<BuffDef> BuffDefDefinitions = new ObservableCollection<BuffDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(BuffDef BuffDef)
        {
            //Check if the SurvivorDef has already been added.
            if (BuffDefDefinitions.Contains(BuffDef))
            {
                LogCore.LogE(BuffDef + " has already been added to the BuffDef Catalog, please do not try to add the same BuffDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            BuffDefDefinitions.Add(BuffDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<BuffDef> defs = new List<BuffDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (BuffDef def in BuffDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.buffDefs = defs.ToArray();
        }
    }
}