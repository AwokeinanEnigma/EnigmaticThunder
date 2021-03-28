using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class GameEndings : Module
    {
        internal static ObservableCollection<GameEndingDef> GameEndingDefDefinitions = new ObservableCollection<GameEndingDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void RegisterGameEndingDef(GameEndingDef gameEndingDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (GameEndingDefDefinitions.Contains(gameEndingDef))
            {
                LogCore.LogE(gameEndingDef + " has already been registered, please do not register the same GameEndingDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            GameEndingDefDefinitions.Add(gameEndingDef);
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