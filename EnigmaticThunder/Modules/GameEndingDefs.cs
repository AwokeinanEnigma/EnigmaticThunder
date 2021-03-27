using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class GameEndingDefs : Module
    {
        internal static ObservableCollection<GameEndingDef> GameEndingDefDefinitions = new ObservableCollection<GameEndingDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(GameEndingDef GameEndingDef)
        {
            //Check if the SurvivorDef has already been added.
            if (GameEndingDefDefinitions.Contains(GameEndingDef))
            {
                LogCore.LogE(GameEndingDef + " has already been added to the GameEndingDef Catalog, please do not try to add the same GameEndingDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            GameEndingDefDefinitions.Add(GameEndingDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<GameEndingDef> defs = new List<GameEndingDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (GameEndingDef def in GameEndingDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.gameEndingDefs = defs.ToArray();
        }
    }
}