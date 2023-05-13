using UnityEngine;

namespace mtti.Funcs.Components
{
    /// <summary>
    /// Prevents the GameObject it's attached to from being destroyed on scene
    /// load.
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
