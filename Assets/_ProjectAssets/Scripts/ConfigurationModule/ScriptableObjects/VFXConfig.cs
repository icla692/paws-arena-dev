using UnityEngine;

namespace Anura.ConfigurationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "VFXConfig", menuName = "Configurations/VFXConfig", order = 2)]
    public class VFXConfig : ScriptableObject
    {
        [SerializeField] private GameObject explosion;

        public GameObject GetExplosion()
        {
            return explosion;
        }
    }
}
