using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// honestly just save yourself the headache and use the Prefabs class 
    /// </summary>
    public class NetworkPrefabs : Module
    {
        internal static ObservableCollection<GameObject> NetworkPrefabDefinitions = new ObservableCollection<GameObject>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// registers a network prefab just use the Prefabs class
        /// </summary>
        /// <param name="networkPrefab">-.-</param>
        public static void RegisterNetworkPrefab(GameObject networkPrefab)
        {
            //Check if the SurvivorDef has already been registered.
            if (NetworkPrefabDefinitions.Contains(networkPrefab) || !networkPrefab)
            {
                LogCore.LogE(networkPrefab + " has already been registered, please do not register the same NetworkPrefab twice. Or is null.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            NetworkPrefabDefinitions.Add(networkPrefab);
        }

        internal static GameObject[] DumpContent()
        {


            //Make a list of survivor defs (we'll be converting it to an array later)
            List<GameObject> defs = new List<GameObject>();
            //Add everything from SurvivorDefinitions to it.
            foreach (GameObject def in NetworkPrefabDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            return defs.ToArray();
        }
    }
}
