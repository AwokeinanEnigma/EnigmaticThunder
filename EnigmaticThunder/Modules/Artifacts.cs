using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for adding custom artifacts to the game. 
    /// </summary>
    public class Artifacts : Module
    {
        internal static ObservableCollection<ArtifactDef> ArtifactDefDefinitions = new ObservableCollection<ArtifactDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Registers an ArtifactDef to the ArtifactCatalog.
        /// </summary>
        /// <param name="ArtifactDef">The ArtifactDef you want to register.</param>
        public static void RegisterArtifact(ArtifactDef ArtifactDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (ArtifactDefDefinitions.Contains(ArtifactDef))
            {
                LogCore.LogE(ArtifactDef + " has already been registered, please do not register the same ArtifactDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            ArtifactDefDefinitions.Add(ArtifactDef);
        }

        internal override void ModifyContentPack(ContentPack pack)
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