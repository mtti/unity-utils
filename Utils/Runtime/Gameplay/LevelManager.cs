using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mtti.Funcs.Gameplay
{
    public delegate void LevelEvent(Level level);

    /// <summary>
    /// Adds convenience functionality on top of Unity's core scene management
    /// for managing levels. A level is a scene with a Level GameObject at
    /// its root which can contain components that get initialized in a clear
    /// order when the level has been loaded. Checks are also in place to
    /// ensure only one Level scene is loaded at a time regardless of what
    /// other scenes are loaded.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Called when a level has been loaded.
        /// </summary>
        public event LevelEvent OnLevelLoad;

        /// <summary>
        /// Called when a level is about to be unloaded.
        /// </summary>
        public event LevelEvent OnLevelWillUnload;

        private bool _busy = false;

        private Level _currentLevel;

        public bool Busy
        {
            get
            {
                return _busy;
            }

            private set
            {
                _busy = value;
            }
        }

        /// <summary>
        /// The currently loaded level.
        /// </summary>
        public Level Current { get { return _currentLevel; } }

        /// <summary>
        /// Load a level.
        /// </summary>
        public Coroutine Load(string scenePath)
        {
            if (Busy)
            {
                Debug.LogError("Can not load level in current state");
                return null;
            }

            Busy = true;

            return StartCoroutine(LoadLevelCoroutine(scenePath));
        }

        /// <summary>
        /// Reload the currently loaded level.
        /// </summary>
        public Coroutine Reload()
        {
            if (Busy)
            {
                Debug.LogError("Can not reload level in current state");
                return null;
            }

            if (_currentLevel == null)
            {
                Debug.LogError("No level currently loaded");
                return null;
            }

            return Load(_currentLevel.gameObject.scene.path);
        }

        /// <summary>
        /// Unload the currently loaded level.
        /// </summary>
        public Coroutine Unload()
        {
            if (Busy)
            {
                Debug.LogError("Can not unload level in current state");
                return null;
            }

            Busy = true;
            return StartCoroutine(UnloadLevelCoroutine());
        }

        private IEnumerator LoadLevelCoroutine(string scenePath)
        {
            // Unload currently running level, if any
            yield return UnloadCurrentLevel();

            // Load the scene containing the new level
            yield return UnityUtils.LoadScene(scenePath);
            Scene scene;
            if (!UnityUtils.GetLoadedScene(scenePath, out scene))
            {
                Debug.LogErrorFormat(
                    "Could not find loaded scene {0}",
                    scenePath
                );
                Busy = false;
                yield break;
            }

            var level = scene.FindRootObject<Level>();
            if (level == null)
            {
                Debug.LogErrorFormat(
                    "Scene {0} is not a level",
                    scene.path
                );
                Busy = false;
                yield break;
            }

            TriggerLevelLoaded(level);

            Busy = false;
        }

        private IEnumerator UnloadLevelCoroutine()
        {
            yield return UnloadCurrentLevel();
            Busy = false;
        }

        private IEnumerator UnloadCurrentLevel()
        {
            if (_currentLevel == null)
            {
                yield break;
            }

            Debug.LogFormat(
                "Unloading level: {0}",
                _currentLevel.gameObject.scene.path
            );

            TriggerLevelWillUnload(_currentLevel);
            yield return SceneManager.UnloadSceneAsync(
                _currentLevel.gameObject.scene
            );
            _currentLevel = null;
        }

        private void TriggerLevelLoaded(Level level)
        {
            _currentLevel = level;
            SceneManager.SetActiveScene(_currentLevel.gameObject.scene);
            _currentLevel.TriggerLevelLoad(this);
            if (OnLevelLoad != null) OnLevelLoad(level);
        }

        private void TriggerLevelWillUnload(Level level)
        {
            level.TriggerLevelWillUnload();
            if (OnLevelWillUnload != null) OnLevelWillUnload(level);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Discover an already loaded level when run in the editor.
        /// </summary>
        private void Start()
        {
            var loadedLevels = new List<Level>();
            UnityUtils.FindRootObjects<Level>(loadedLevels);
            if (loadedLevels.Count == 1)
            {
                Debug.LogFormat(
                    "Found an already loaded level: {0}",
                    loadedLevels[0].gameObject.scene.path
                );
                TriggerLevelLoaded(loadedLevels[0]);
            }
            else if (loadedLevels.Count > 1)
            {
                Debug.LogWarning("More than one level loaded, doing nothing");
            }
        }
#endif
    }
}
