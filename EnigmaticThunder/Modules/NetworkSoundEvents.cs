using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for registering network sound event defs to the network sound event catalog.
    /// </summary>
    public class NetworkSoundEvents : Module
    {
        internal static ObservableCollection<NetworkSoundEventDef> NetworkSoundEventDefDefinitions = new ObservableCollection<NetworkSoundEventDef>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Registers a sound def to the network sound event catalog.
        /// </summary>
        /// <param name="NetworkSoundEventDef"></param>
        public static void RegisterSoundDef(NetworkSoundEventDef NetworkSoundEventDef)
        {
            //Check if the SurvivorDef has already been registered.
            if (NetworkSoundEventDefDefinitions.Contains(NetworkSoundEventDef))
            {
                LogCore.LogE(NetworkSoundEventDef + " has already been registered, please do not register the same NetworkSoundEventDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            NetworkSoundEventDefDefinitions.Add(NetworkSoundEventDef);
        }

        internal override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);
            //Make a list of survivor defs (we'll be converting it to an array later)
            List<NetworkSoundEventDef> defs = new List<NetworkSoundEventDef>();
            //Add everything from SurvivorDefinitions to it.
            foreach (NetworkSoundEventDef def in NetworkSoundEventDefDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.networkSoundEventDefs = defs.ToArray();
        }
    }
}