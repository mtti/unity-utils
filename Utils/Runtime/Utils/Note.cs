using UnityEngine;

namespace mtti.Funcs
{
    /// <summary>
    /// A text note which can be added to a GameObject. The contents will be
    /// removed from builds, but an empty Note component will be left in.
    /// </summary>
    public class Note : MonoBehaviour
    {
#if UNITY_EDITOR
        public UnityEditor.MessageType MessageType
            = UnityEditor.MessageType.None;

        public string Text;
#endif
    }
}
