using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;
using CustomUI.Settings;
using CustomUI.Utilities;
using CustomUI.GameplaySettings;
using IPA.Loader;
using System.Collections;
using System.Linq;

namespace RainbowLighting
{
    public class Plugin : IBeatSaberPlugin
    {
        private static BS_Utils.Utilities.Config config = new BS_Utils.Utilities.Config("RainbowLighting");
        private static bool CustomSabersPresent;
        private static bool saber = true;
        private static bool trail = true;
        private static bool bg = true;
        private ColorManagerPlus mgr;
        private RandomColor randColor;
        public static LightSwitchEventEffect[] iSeeLight;
        IPALogger logger;

        public static readonly ToggleOption rbSubmenu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.PlayerSettingsRight, "Rainbow Lighting", "MainMenu", "rbUI");
        public static Sprite icon = UIUtilities.LoadSpriteFromResources("RainbowLighting.Resources.icon.png");

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if(scene.name == "MenuCore")
            {
                
            }
        }

        public void Init(object thisWillBeNull, IPALogger logger)
        {
            this.logger = logger;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (bg)
            {
                iSeeLight = Resources.FindObjectsOfTypeAll<LightSwitchEventEffect>();
                if (iSeeLight != null)
                {
                    foreach (LightSwitchEventEffect obj in iSeeLight)
                    {
                        ReflectionUtil.SetPrivateField(obj, "_lightColor0", randColor);
                        ReflectionUtil.SetPrivateField(obj, "_lightColor1", randColor);
                        ReflectionUtil.SetPrivateField(obj, "_highlightColor0", randColor);
                        ReflectionUtil.SetPrivateField(obj, "_highlightColor1", randColor);
                    }
                }
            }
        }

        public void OnApplicationQuit() => SceneManager.sceneLoaded -= OnSceneLoaded;

        public void OnApplicationStart()
        {
            CustomSabersPresent = PluginManager.AllPlugins.Any(x => x.Metadata.Id == "Custom Sabers");
            randColor = ScriptableObject.CreateInstance<RandomColor>();
            mgr = new GameObject().AddComponent<ColorManagerPlus>();
            mgr.SetLogger(logger);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        
        public static void OverrideCustomSaberColors(Color left, Color right)
        {
            OverrideSaber("LeftSaber", left);
            OverrideSaber("RightSaber", right);
        }

        public static bool OverrideSaber(string objectName, Color color)
        {
            //       Log("Attempting Override of  Saber");
            Transform saberObject = null;
            if (SceneManager.GetActiveScene().name == "MenuCore" && CustomSabersPresent)
            {
                //         Log("Finding Preview");
                saberObject = GameObject.Find("Saber Preview").transform.Find(objectName);
            }

            else
                saberObject = GameObject.Find(objectName)?.transform;
            if (saberObject == null) return false;
            var saberRenderers = saberObject.GetComponentsInChildren<Renderer>();
            if (saberRenderers == null) return false;

            foreach (var renderer in saberRenderers)
            {
                if (renderer != null)
                {
                    foreach (var renderMaterial in renderer.sharedMaterials)
                    {
                        if (renderMaterial == null)
                        {
                            continue;
                        }

                        if (renderMaterial.HasProperty("_Glow") && renderMaterial.GetFloat("_Glow") > 0 ||
                            renderMaterial.HasProperty("_Bloom") && renderMaterial.GetFloat("_Bloom") > 0)
                        {
                            renderMaterial.SetColor("_Color", color);
                        }
                    }
                }

            }
            return true;

        }

        public void OnUpdate()
        {
            if (trail)
            {
                foreach (var saberTrail in Resources.FindObjectsOfTypeAll<SaberWeaponTrail>())
                {
                    ReflectionUtil.SetPrivateField(saberTrail, "_colorManager", mgr);
                }
            }
            if (saber)
            {
                foreach (var saberColor in Resources.FindObjectsOfTypeAll<SetSaberGlowColor>())
                {
                    ReflectionUtil.SetPrivateField(saberColor, "_colorManager", mgr);
                }
            }
        }
        /* comment because doesnt work yet
        public static void SetSettingsToggles() 
        {
            //Settings Menu Setup for "enabled"
            SubMenu subMenuCC = SettingsUI.CreateSubMenu("Rainbow Lighting");
            BoolViewController disableMenu = subMenuCC.AddBool("EnabledBG");
            disableMenu.GetValue += delegate { return config.GetBool("RainbowLighting", "EnabledBG", true, true); };
            disableMenu.SetValue += delegate (bool value) { bg = value; config.SetBool("RainbowLighting", "EnabledBG", value); };
            disableMenu.EnabledText = "ON";
            disableMenu.DisabledText = "OFF";

            //Settings Menu Setup for "trail"
            BoolViewController disableMenu0 = subMenuCC.AddBool("Rainbow Trail");
            disableMenu0.GetValue += delegate { return config.GetBool("RainbowLighting", "Trail", true, true); };
            disableMenu0.SetValue += delegate (bool value) { trail = value; config.SetBool("RainbowLighting", "Trail", value); };
            disableMenu0.EnabledText = "ON";
            disableMenu0.DisabledText = "OFF";
        }
        public static void SetPlayerSettingsToggles()
        {
            //PlayerSettings Toggle for "enabled"
            ToggleOption disableOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsRight, "Rainbow Lighting", "rbUI", "Enable Rainbow Lighting", icon);
            disableOption.GetValue = config.GetBool("RainbowLighting", "EnabledBG", true, true);
            disableOption.OnToggle += (value) => { bg = value; config.SetBool("RainbowLighting", "EnabledBG", value); };

            //PlayerSettings Toggle for "trail"
            ToggleOption disableOption0 = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsRight, "Rainbow Trail", "rbUI", "Enable Rainbow Trail", icon);
            disableOption0.GetValue = config.GetBool("RainbowLighting", "Trail", true, true);
            disableOption0.OnToggle += (value) => { trail = value; config.SetBool("RainbowLighting", "Trail", value); };
        }
        */
        
        
        #region Unused voids
        public void OnFixedUpdate()
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
        #endregion
    }
}
