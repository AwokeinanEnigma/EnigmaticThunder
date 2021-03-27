using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class SceneDefs : Module
    {
        internal static ObservableCollection<SceneDef> SceneDefDefinitions = new ObservableCollection<SceneDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(SceneDef SceneDef)
        {
            //Check if the SurvivorDef has already been added.
            if (SceneDefDefinitions.Contains(SceneDef))
            {
                LogCore.LogE(SceneDef + " has already been added to the SceneDef Catalog, please do not try to add the same SceneDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            SceneDefDefinitions.Add(SceneDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<SceneDef> defs = new List<SceneDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (SceneDef def in SceneDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.sceneDefs = defs.ToArray();
        }
    }
}
