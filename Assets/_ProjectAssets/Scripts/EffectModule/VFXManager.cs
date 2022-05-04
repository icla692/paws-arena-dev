using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Photon.Pun;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{
    public void PUN_InstantiateExplosion(Vector3 position)
    {
        var explosion = PaintingManager.Instance.GetCurrentShape().visualFX;

        if (explosion == null)
        {
            explosion = ConfigurationManager.Instance.VFXConfig.GetExplosion();
            PhotonNetwork.Instantiate(explosion.name, position, Quaternion.identity);
        }
        else
        {
            var instance = PhotonNetwork.Instantiate(explosion.name, position, Quaternion.identity);
            instance.transform.localScale = Vector3.one * (PaintingManager.Instance.GetCurrentShape().GetSize() / 16);
        }
    }
}
