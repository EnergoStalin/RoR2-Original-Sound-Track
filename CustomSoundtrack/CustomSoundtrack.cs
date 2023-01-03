using BepInEx;
using R2API.Utils;

using System;
using System.Reflection;
using UnityEngine.SceneManagement;

using Path = System.IO.Path;

namespace CustomSoundtrack
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.energostalin.customsoundtrack", "CustomSoundtrack", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class CustomSoundtrack : BaseUnityPlugin {

        private bool _inited = false;
        private Settings _settings;
        private MusicManager _musicManager;

        public void Awake() {
            if(_inited) return;

            _inited = true;

            var _pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _settings = Settings.TryLoad(_pluginPath);

            _musicManager = new MusicManager(_settings);

            _insertHooks();
        }

        private void _insertHooks()
        {
            On.RoR2.TeleporterInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                _musicManager.PlayNext();
                orig(self, activator);
            };

            On.RoR2.TeleporterInteraction.RpcClientOnActivated += (orig, self, activator) =>
            {
                _musicManager.PlayNext();
                orig(self, activator);
            };

            On.RoR2.UI.PauseScreenController.OnEnable += (orig, self) =>
            {
                _musicManager.Player.Pause();
                orig(self);
            };

            On.RoR2.UI.PauseScreenController.OnDisable += (orig, self) =>
            {
                _musicManager.Player.Play();
                orig(self);
            };

            On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter += (orig, self) =>
            {
                _musicManager.PlayNext();
                orig(self);
            };

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                _musicManager.PlayNext(false);
            };
        }
    }
}
