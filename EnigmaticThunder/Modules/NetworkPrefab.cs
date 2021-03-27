using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class NetworkPrefabs : Module
    {
        internal static ObservableCollection<GameObject> NetworkPrefabDefinitions = new ObservableCollection<GameObject>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(GameObject NetworkPrefab)
        {
            //Check if the SurvivorDef has already been added.
            if (NetworkPrefabDefinitions.Contains(NetworkPrefab))
            {
                LogCore.LogE(NetworkPrefab + " has already been added to the Network Prefab Catalog, please do not try to add the same NetworkPrefab twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            NetworkPrefabDefinitions.Add(NetworkPrefab);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<GameObject> defs = new List<GameObject>();
            //Add everything from SurvivorDefinitions to it.
            foreach (GameObject def in NetworkPrefabDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.networkedObjectPrefabs = defs.ToArray();
        }
    }
}
