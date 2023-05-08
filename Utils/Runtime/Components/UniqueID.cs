using mtti.Funcs.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace mtti.Funcs.Components
{
    /// <summary>
    /// A component which assigns the GameObject a globally unique ID.
    /// </summary>
    public class UniqueID : MonoBehaviour
    {
        #region Statics

        private static Dictionary<Uuid, UniqueID> s_index = new();

        /// <summary>
        /// Get a GameObject by its unique ID.
        /// </summary>
        public static GameObject Get(Uuid id)
        {
            if (!s_index.ContainsKey(id))
            {
                return null;
            }
            return s_index[id].gameObject;
        }
        
        #endregion
        
        [SerializeField]
        private Uuid _value = Uuid.NewV4();

        [Tooltip("If enabled, the ID will be randomized when the object is first spawned. This is useful with prefabs where each spawned instance should have its own unique ID.")]
        [SerializeField]
        private bool _randomizeOnStart = false;

        private bool _wasDuplicate = false;

        public Uuid Value
        {
            get { return _value; }

            set
            {
                if (isActiveAndEnabled)
                {
                    throw new InvalidOperationException(
                        "Can't change the unique ID of an active GameObject"
                    );
                }
                
                if (value == Uuid.Empty)
                {
                    throw new ArgumentException(
                        "Can't be the null UUID",
                        nameof(value)
                    );
                }

                _value = value;
            }
        }
        
        private void Start()
        {
            if (_randomizeOnStart) _value = new Uuid();
        }

        private void OnEnable()
        {
            if (s_index.ContainsKey(_value))
            {
                Debug.LogWarning("A GameObject with the same unique ID is already active", this);
                _wasDuplicate = true;
                gameObject.SetActive(false);
            }

            s_index[_value] = this;
        }

        private void OnDisable()
        {
            // Don't touch the index if the object is being disabled due to
            // being a duplicate
            if (_wasDuplicate)
            {
                _wasDuplicate = false;
                return;
            }

            s_index.Remove(_value);
        }
    }
}
