using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class Artifacts : Module
    {
        internal static ObservableCollection<ArtifactDef> ArtifactDefDefinitions = new ObservableCollection<ArtifactDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(ArtifactDef ArtifactDef)
        {
            //Check if the SurvivorDef has already been added.
            if (ArtifactDefDefinitions.Contains(ArtifactDef))
            {
                LogCore.LogE(ArtifactDef + " has already been added to the ArtifactDef Catalog, please do not try to add the same ArtifactDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            ArtifactDefDefinitions.Add(ArtifactDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<ArtifactDef> defs = new List<ArtifactDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (ArtifactDef def in ArtifactDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.artifactDefs = defs.ToArray();
        }
    }
}