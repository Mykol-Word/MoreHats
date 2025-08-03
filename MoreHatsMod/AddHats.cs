using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace MoreHatsMod
{ 
    [HarmonyPatch(typeof(CharacterCustomizerManager))]
    internal class AddHats
    {
        public static bool included_the_hats = false;
        public static string[] registered_hats = ["Hat_Cube", "Hat_Cylinder", "Hat_Troll", "Hat_Sphere", "Hat_Thug"];
        public static List<GameObject> hat_list;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void Postfix()
        {
            if (!included_the_hats)
            {
                included_the_hats = true;

                hat_list = new List<GameObject>();
                LoadHats();
                foreach (GameObject hat in hat_list)
                {
                    MenuToGameBridger.Singleton.hat_Prefabs.Add(hat);
                }
            }
        }

        static void LoadHats()
        {
            foreach (string registered_hat in registered_hats)
            {
                GameObject hat = Plugin.Load<GameObject>(registered_hat + ".prefab");
                if (hat == null)
                {
                    continue;
                }

                hat.AddComponent<HumanCustomizerAccessory>();
                HumanCustomizerAccessory customizer_accesory = hat.GetComponent<HumanCustomizerAccessory>();
                customizer_accesory.displayName = registered_hat;
                customizer_accesory.allowInFriendsPass = true;
                customizer_accesory.alwaysUnlocked = true;

                Sprite hat_sprite = Plugin.Load<Sprite>(registered_hat + ".png");
                if (hat_sprite != null)
                {
                    customizer_accesory.thumbnailImage = hat_sprite;
                }
                else
                {
                    customizer_accesory.thumbnailImage = Plugin.Load<Sprite>("Hat_Empty.png");
                }

                    Material hat_mat = GetHatMat();

                foreach (Renderer hat_renderer in hat.GetComponentsInChildren<MeshRenderer>())
                {
                    int num_of_materials = hat_renderer.materials.Length;
                    Material[] new_hat_materials = new Material[num_of_materials];
                    for (int i = 0; i < num_of_materials; i++)
                    {
                        new_hat_materials[i] = hat_mat;
                    }
                    hat_renderer.SetMaterialArray(new_hat_materials);
                }

                hat_list.Add(hat);
            }
        }

        static Material GetHatMat()
        {
            Material hat_mat = null;
            foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
            {
                if (material.name == "Clay_Accessories") // || material.name == "Clay_Accessories (Instance) (Instance)")
                {
                    hat_mat = material;
                    break;
                }
            }
            return hat_mat;
        }
    }
}
