using UnityEngine;

namespace Tamamon.Ingame.Adventure
{
    public class EncountField : MonoBehaviour
    {
        [SerializeField]
        private Tamamon.Data.EncountFieldData m_data;

        public int GetEncountRate()
        {
            return m_data.m_encounterRate;
        }
    }
}