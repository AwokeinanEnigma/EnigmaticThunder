using EnigmaticThunder.Util;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EnigmaticThunder.Modules
{
    class Pickups : Module
    {
        internal static ObservableCollection<EquipmentDef> EquipmentDefDefinitions = new ObservableCollection<EquipmentDef>();
        internal static ObservableCollection<ItemDef> ItemDefDefinitions = new ObservableCollection<ItemDef>();
        public override void Load()
        {
            base.Load();
            //Meow (Waiting for something to happen?)
        }

        public static void AddItem(ItemDef itemDef)
        {
            //Check if the SurvivorDef has already been added.
            if (ItemDefDefinitions.Contains(itemDef))
            {
                LogCore.LogE(itemDef + " has already been added, please do not try to add the same ItemDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            ItemDefDefinitions.Add(itemDef);
        }
        public static void AddEquipment(EquipmentDef equipmentDef)
        {
            //Check if the SurvivorDef has already been added.
            if (EquipmentDefDefinitions.Contains(equipmentDef))
            {
                LogCore.LogE(equipmentDef + " has already been added, please do not try to add the same EquipmentDef twice.");
                return;
            }
            //If not, add it to our SurvivorDefinitions
            EquipmentDefDefinitions.Add(equipmentDef);
        }
        public override void ModifyContentPack(ContentPack pack)
        {
            base.ModifyContentPack(pack);

            List<ItemDef> itemDefs = new List<ItemDef>();
            //Add everything from ItemDefDefinitions to it.
            foreach (ItemDef def in ItemDefDefinitions)
            {
                itemDefs.Add(def);
            }

            List<EquipmentDef> equipmentDefs = new List<EquipmentDef>();
            //Add everything from EquipmentDefDefinitions to it.
            foreach (EquipmentDef def in EquipmentDefDefinitions)
            {
                equipmentDefs.Add(def);
            }

            //Convert our lists into arrays and give it to the ContentPack.
            pack.itemDefs = itemDefs.ToArray();
            pack.equipmentDefs = equipmentDefs.ToArray();
        }
    }
}