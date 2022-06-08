using Anura.ConfigurationModule.ScriptableObjects;
using Anura.Templates.MonoSingleton;
using NaughtyAttributes;
using UnityEngine;

namespace Anura.ConfigurationModule.Managers
{
    public class ConfigurationManager : MonoSingleton<ConfigurationManager>
    {
        [SerializeField, Expandable] private Config config;
        [SerializeField, Expandable] private VFXConfig visualContainer;
        [SerializeField, Expandable] private SFXConfig sfx;
        [SerializeField, Expandable] private ShapeList shapes;
        [SerializeField, Expandable] private CratesConfig crates;

        public Config Config => config;

        public VFXConfig VFXConfig => visualContainer;

        public SFXConfig SFXConfig => sfx;

        public ShapeList Shapes => shapes;

        public CratesConfig Crates => crates;
    }
}
