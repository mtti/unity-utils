using System.Collections.Generic;
using UnityEngine;

namespace mtti.Funcs.Gameplay
{
    public class Level : MonoBehaviour
    {
        /// <summary>
        /// Temporarily stores references to MonoBehaviours.
        /// </sumamry>
        private static List<MonoBehaviour> s_components
            = new List<MonoBehaviour>();

        private LevelManager _manager;

        public LevelManager Manager { get { return _manager; } }

        /// <summary>
        /// Called from <see cref="mtti.Funcs.Gameplay.LevelManager" /> after
        /// the scene containing this GameObject has been loaded.
        /// </summary>
        internal void TriggerLevelLoad(LevelManager manager)
        {
            _manager = manager;

            try
            {
                GetComponents<MonoBehaviour>(s_components);
                for (int i = 0, count = s_components.Count; i < count; i++)
                {
                    var listener = s_components[i] as ILevelLoadListener;
                    if (listener != null) listener.OnLevelLoad();
                }
            }
            finally
            {
                s_components.Clear();
            }
        }

        /// <summary>
        /// Called from <see cref="mtti.Funcs.Gameplay.LevelManager" /> when
        /// the scene containing this GameObject is about to be unloaded.
        /// </summary>
        internal void TriggerLevelWillUnload()
        {
            try
            {
                GetComponents<MonoBehaviour>(s_components);
                for (int i = 0, count = s_components.Count; i < count; i++)
                {
                    var listener = s_components[i] as ILevelLoadListener;
                    if (listener != null) listener.OnLevelWillUnload();
                }
            }
            finally
            {
                s_components.Clear();
            }
        }
    }
}
