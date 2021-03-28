using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class SurfaceDefinitions : Module
    {
        internal static ObservableCollection<SurfaceDef> SurfaceDefDefinitions = new ObservableCollection<SurfaceDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void RegisterSurfaceDef(SurfaceDef surfaceDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (SurfaceDefDefinitions.Contains(surfaceDef))
            {
                LogCore.LogE(surfaceDef + " has already been registered, please do not register the same SurfaceDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            SurfaceDefDefinitions.Add(surfaceDef);
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