using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class Runs : Module
    {
        internal static ObservableCollection<Run> RunDefinitions = new ObservableCollection<Run>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(Run Run)
        {
            //Check if the SurvivorDef has already been added.
            if (RunDefinitions.Contains(Run))
            {
                LogCore.LogE(Run + " has already been added, please do not try to add the same Run twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            RunDefinitions.Add(Run);
        }

        public override void ModifyContentPack(ContentPack pack)
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
