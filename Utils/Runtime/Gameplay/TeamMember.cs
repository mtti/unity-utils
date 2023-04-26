using System.Collections.Generic;
using mtti.Funcs.Collections;
using UnityEngine;

namespace mtti.Funcs.Gameplay
{
    /// <summary>
    /// Mark a GameObject as a member of a team identified by an integer ID.
    /// </summary>
    public class TeamMember : MonoBehaviour
    {
        private static MultiValueDictionary<int, GameObject> s_index = new MultiValueDictionary<int, GameObject>();

        /// <summary>
        /// Get the team of a game object.
        /// </summary>
        public static NullableValue<int> Get(GameObject obj)
        {
            var tm = obj.GetComponent<TeamMember>();
            if (tm == null)
            {
                return new NullableValue<int>();
            }
            return new NullableValue<int>(tm.TeamId);
        }

        public static int FindAllInTeam(int teamId, List<GameObject> result)
        {
            return s_index.Get(teamId, result);
        }
        
        [SerializeField]
        private int _teamId = 0;

        public int TeamId
        {
            get { return _teamId; }

            set
            {
                if (value == _teamId) return;
                RemoveFromIndex();
                _teamId = value;
                AddToIndex();
            }
        }

        private void AddToIndex()
        {
            s_index.Add(_teamId, this.gameObject);
        }

        private void RemoveFromIndex()
        {
            s_index.Remove(_teamId, this.gameObject);
        }

        private void OnEnable()
        {
            AddToIndex();
        }

        private void OnDisable()
        {
            RemoveFromIndex();
        }
    }
}
