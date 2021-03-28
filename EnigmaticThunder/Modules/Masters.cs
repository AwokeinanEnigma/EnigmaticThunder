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

        /// <summary>
        /// Adds a GameObject to the master catalog.
        /// GameObject cannot be null and have a CharacterMaster component.
        /// </summary>
        /// <param name="master">The master GameObject to register to the master catalog.</param>
        /// <returns></returns>
        public static void RegisterMaster(GameObject master)
        {
            //Check if the SurvivorDef has already been registered.
            if (MasterDefinitions.Contains(master) || !master.GetComponent<CharacterMaster>())
            {
                string error = master + " has already been registered, please do not register the same master twice.";
                if (!master.GetComponent<CharacterMaster>())
                {
                    error += " And/Or, the master does not have a character master component.";
                }
                LogCore.LogE(error);
            }
            //If not, add it to our SurvivorDefinitions
            MasterDefinitions.Add(master);
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
