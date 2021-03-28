using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for registering EliteDefs to the EliteCatalog.
    /// </summary>
    public class Elites : Module
    {
        internal static ObservableCollection<EliteDef> EliteDefDefinitions = new ObservableCollection<EliteDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Registers an EliteDef to the EliteCatalog
        /// </summary>
        /// <param name="eliteDef">The EliteDef to register.</param>
        public static void RegisterElite(EliteDef eliteDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (EliteDefDefinitions.Contains(eliteDef))
            {
                LogCore.LogE(eliteDef + " has already been registered, please do not register the same EliteDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            EliteDefDefinitions.Add(eliteDef);
        }

        internal override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<EliteDef> defs = new List<EliteDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (EliteDef def in EliteDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.eliteDefs = defs.ToArray();
        }
    }
}