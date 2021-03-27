using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class EliteDefs : Module
    {
        internal static ObservableCollection<EliteDef> EliteDefDefinitions = new ObservableCollection<EliteDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(EliteDef EliteDef)
        {
            //Check if the SurvivorDef has already been added.
            if (EliteDefDefinitions.Contains(EliteDef))
            {
                LogCore.LogE(EliteDef + " has already been added to the EliteDef Catalog, please do not try to add the same EliteDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            EliteDefDefinitions.Add(EliteDef);
        }

        public override void ModifyContentPack(ContentPack pack)
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