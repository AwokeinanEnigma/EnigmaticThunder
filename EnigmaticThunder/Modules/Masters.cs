using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class Masters : Module
    {
        internal static ObservableCollection<GameObject> MasterDefinitions = new ObservableCollection<GameObject>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(GameObject Master)
        {
            //Check if the SurvivorDef has already been added.
            if (MasterDefinitions.Contains(Master))
            {
                LogCore.LogE(Master + " has already been added to the Master Catalog, please do not try to add the same Master twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            MasterDefinitions.Add(Master);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<GameObject> defs = new List<GameObject>();
            //Add everything from SurvivorDefinitions to it.
            foreach (GameObject def in MasterDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.masterPrefabs = defs.ToArray();
        }
    }
}
