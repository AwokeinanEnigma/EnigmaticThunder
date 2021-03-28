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

        public static void RegisterNetworkPrefab(GameObject networkPrefab)
        {
            //Check if the SurvivorDef has already been registered.
            if (NetworkPrefabDefinitions.Contains(networkPrefab))
            {
                LogCore.LogE(networkPrefab + " has already been registered, please do not register the same NetworkPrefab twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            NetworkPrefabDefinitions.Add(networkPrefab);
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
