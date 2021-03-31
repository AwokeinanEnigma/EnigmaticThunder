using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for registering UnlockableDef to the UnlockableCatalog.
    /// </summary>
    public class Unlockables : Module
    {
        internal static ObservableCollection<UnlockableDef> UnlockableDefDefinitions = new ObservableCollection<UnlockableDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Registers a UnlockableDef to the UnlockableCatalog
        /// </summary>
        /// <param name="unlockableDef">The UnlockableDef to register.</param>
        public static void RegisterUnlockableDef(UnlockableDef unlockableDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (UnlockableDefDefinitions.Contains(unlockableDef))
            {
                LogCore.LogE(unlockableDef + " has already been registered to the UnlockableDef Catalog, please do not register the same  UnlockableDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            UnlockableDefDefinitions.Add(unlockableDef);
        }

        internal void ModifyContentPack()
        {

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