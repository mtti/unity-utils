using System;
using UnityEngine;

namespace mtti.Funcs.Gameplay
{
    public class Health : MonoBehaviour
    {
        public static int Get(GameObject obj)
        {
            Health health = obj.GetComponent<Health>();
            if (health != null)
            {
                return health.Value;
            }
            return 0;
        }
        
        /// <summary>
        /// Sort game objects by health from smallest to greatest with <c>List.Sort()</c>.
        /// </summary>
        public static int SortAsc(GameObject a, GameObject b)
        {
            int aHealth = Health.Get(a);
            int bHealth = Health.Get(b);
            return aHealth.CompareTo(bHealth);
        }
        
        public event Action<int> Changed; 

        [SerializeField]
        private int _value = 100;

        public int Value
        {
            get { return _value; }

            set
            {
                if (value != _value)
                {
                    _value = value;
                    if (Changed != null) Changed(value);
                }
            }
        }

        public void Adjust(int delta)
        {
            Value = _value + delta;
        }
    }
}
