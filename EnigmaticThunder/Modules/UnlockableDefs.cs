using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class UnlockableDefs : Module
    {
        internal static ObservableCollection<UnlockableDef> UnlockableDefDefinitions = new ObservableCollection<UnlockableDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(UnlockableDef UnlockableDef)
        {
            //Check if the SurvivorDef has already been added.
            if (UnlockableDefDefinitions.Contains(UnlockableDef))
            {
                LogCore.LogE(UnlockableDef + " has already been added to the UnlockableDef Catalog, please do not try to add the same UnlockableDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            UnlockableDefDefinitions.Add(UnlockableDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<UnlockableDef> defs = new List<UnlockableDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (UnlockableDef def in UnlockableDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.unlockableDefs = defs.ToArray();
        }
    }
}