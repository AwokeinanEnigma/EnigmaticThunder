using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class Bodies : Module
    {
        internal static ObservableCollection<GameObject> BodyDefinitions = new ObservableCollection<GameObject>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(GameObject body)
        {
            //Check if the SurvivorDef has already been added.
            if (BodyDefinitions.Contains(body))
            {
                LogCore.LogE(body + " has already been added to the Body Catalog, please do not try to add the same body twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            BodyDefinitions.Add(body);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<GameObject> defs = new List<GameObject>();
            //Add everything from SurvivorDefinitions to it.
            foreach (GameObject def in BodyDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.bodyPrefabs = defs.ToArray();
        }
    }
}
