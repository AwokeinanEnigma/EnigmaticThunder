using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class SurfaceDefs : Module
    {
        internal static ObservableCollection<SurfaceDef> SurfaceDefDefinitions = new ObservableCollection<SurfaceDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(SurfaceDef SurfaceDef)
        {
            //Check if the SurvivorDef has already been added.
            if (SurfaceDefDefinitions.Contains(SurfaceDef))
            {
                LogCore.LogE(SurfaceDef + " has already been added to the SurfaceDef Catalog, please do not try to add the same SurfaceDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            SurfaceDefDefinitions.Add(SurfaceDef);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<SurfaceDef> defs = new List<SurfaceDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (SurfaceDef def in SurfaceDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.surfaceDefs = defs.ToArray();
        }
    }
}