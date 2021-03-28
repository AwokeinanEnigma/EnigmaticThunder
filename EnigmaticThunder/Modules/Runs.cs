using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for adding game modes (runs). Not to be confused with game endings.
    /// </summary>
    public class Runs : Module
    {
        internal static ObservableCollection<Run> RunDefinitions = new ObservableCollection<Run>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Registers a run to the game mode catalog
        /// </summary>
        /// <param name="run">The run to register</param>
        public static void RegisterRun(Run run)
        {
            //Check if the SurvivorDef has already been registered.
            if (RunDefinitions.Contains(run))
            {
                LogCore.LogE(run + " has already been registered, please do not register the same Run twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            RunDefinitions.Add(run);
        }

        internal override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<Run> defs = new List<Run>();
            //Add everything from SurvivorDefinitions to it.
            foreach (Run def in RunDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.gameModePrefabs = defs.ToArray();
        }
    }
}
