using EnigmaticThunder.Util;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    public class Projectiles : Module
    {
        internal static ObservableCollection<GameObject> ProjectileDefinitions = new ObservableCollection<GameObject>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void Add(GameObject Projectile)
        {
            //Check if the SurvivorDef has already been added.
            if (ProjectileDefinitions.Contains(Projectile))
            {
                LogCore.LogE(Projectile + " has already been added to the Projectile Catalog, please do not try to add the same Projectile twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            ProjectileDefinitions.Add(Projectile);
        }

        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            //Make a list of survivor defs (we'll be converting it to an array later)
            List<GameObject> defs = new List<GameObject>();
            //Add everything from SurvivorDefinitions to it.
            foreach (GameObject def in ProjectileDefinitions)
            {
                defs.Add(def);
            }
            //Convert the list into an array and give it to the ContentPack.
            pack.projectilePrefabs = defs.ToArray();
        }
    }
}
