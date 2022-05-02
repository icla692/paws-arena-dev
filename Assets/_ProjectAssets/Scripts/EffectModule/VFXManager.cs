using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{
    public void InstantiateExplosion(Vector3 position)
    {

        var explosion = PaintingManager.Instance.GetCurrentShape().visualFX;

        if (explosion == null)
        {
            explosion = ConfigurationManager.Instance.VFXConfig.GetExplosion();
            Instantiate(explosion, position, Quaternion.identity);
        }
        else
        {
            var instance = Instantiate(explosion, position, Quaternion.identity);
            instance.transform.localScale = Vector3.one * (PaintingManager.Instance.GetCurrentShape().GetSize() / 16);
        }
    }
}
