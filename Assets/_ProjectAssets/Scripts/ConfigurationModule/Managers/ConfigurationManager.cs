using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Templates.MonoSingleton;
using UnityEngine;

namespace Anura.ConfigurationModule.Managers
{
    public class ConfigurationManager : MonoSingleton<ConfigurationManager>
    {
        [SerializeField] private Config config;
        [SerializeField] private VFXConfig visualContainer;

        public Config Config => config;

        public VFXConfig VFXConfig => visualContainer;
    }
}
