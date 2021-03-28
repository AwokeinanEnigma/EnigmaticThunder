using EnigmaticThunder.Util;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    /// <summary>
    /// Helper class for registering projectiles to the projectile catalog.
    /// </summary>
    public class Projectiles : Module
    {
        internal static ObservableCollection<GameObject> ProjectileDefinitions = new ObservableCollection<GameObject>();
        internal override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        /// <summary>
        /// Adds a GameObject to the projectile catalog.
        /// GameObject cannot be null and must have a ProjectileController component.
        /// </summary>
        /// <param name="projectile">The projectile to register to the projectile catalog.</param>
        /// <returns></returns>
        public static void RegisterProjectile(GameObject projectile)
        {
            //Check if the SurvivorDef has already been registered.
            if (ProjectileDefinitions.Contains(projectile) || !projectile || !projectile.GetComponent<ProjectileController>())
            {
                string error = projectile + " has already been registered, please do not register the same projectile twice.";
                if (!projectile.GetComponent<ProjectileController>())
                {
                    error += " And/Or, the projectile does not have a projectile controller component.";
                }
                LogCore.LogE(error);
            }
            //If not, add it to our SurvivorDefinitions
            ProjectileDefinitions.Add(projectile);
        }

        internal override void ModifyContentPack(ContentPack pack)
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
