using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace MoreHatsMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle bundle;
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            bundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("MoreHatsMod.dll", "morehats"));
            harmony.PatchAll(typeof(AddHats));
        }
        public static T Load<T>(string key) where T : UnityEngine.Object
        {
            return bundle.LoadAsset<T>(key);
        }
    }
}
