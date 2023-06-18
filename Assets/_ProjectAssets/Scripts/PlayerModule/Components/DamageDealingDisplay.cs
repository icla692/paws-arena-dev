using UnityEngine;
using Anura.ConfigurationModule.Managers;
using Photon.Pun;

public class DamageDealingDisplay : MonoBehaviour
{
    public GameObject damageDealPrefab;
    public BasePlayerComponent basePlayerComponent;
    [SerializeField] GameObject experiencePrefab;
    bool isBotPlayer;
    PhotonView photonView;
    Vector3 damageOffset = new Vector3(0, 1, 0);
    int amountOfShowingDamageTexts = 0;
    static int totallDamageDealth;

    private void OnEnable()
    {
        isBotPlayer = GetComponentInParent<BotPlayerComponent>();
        if (ConfigurationManager.Instance.Config.GetIsMultiplayer())
        {
            photonView = GetComponent<PhotonView>();
        }

        basePlayerComponent.onDamageTaken += OnDamageTaken;
        DamageDealingText.Finished += DeduceAmountOfTexts;
    }

    private void OnDisable()
    {
        basePlayerComponent.onDamageTaken -= OnDamageTaken;
        DamageDealingText.Finished -= DeduceAmountOfTexts;
    }

    void DeduceAmountOfTexts()
    {
        amountOfShowingDamageTexts--;
    }

    private void OnDamageTaken(int damage)
    {
        amountOfShowingDamageTexts++;
        var go = GameObject.Instantiate(damageDealPrefab, transform.position, Quaternion.identity, null);
        go.transform.localPosition += (amountOfShowingDamageTexts * damageOffset);
        go.transform.GetChild(0).position = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), 0);
        go.transform.GetChild(0).GetComponent<DamageDealingText>().Init(damage);
        SpawnExperience(damage);
    }

    void SpawnExperience(int _damageTaken)
    {
        if (DataManager.Instance.GameData.HasSeasonEnded)
        {
            return;
        }

        if (photonView!=null)
        {
            if (photonView.IsMine)
            {
                return;
            }
        }
        else
        {
            if (!isBotPlayer)
            {
                return;
            }
        }

        totallDamageDealth += _damageTaken;
        for (int i = 0; i < _damageTaken; i += 5)
        {
            GameObject _experience = Instantiate(experiencePrefab);
            _experience.transform.position = transform.position;
        }
    }

    private void OnDestroy()
    {
        if (DataManager.Instance.GameData.HasSeasonEnded)
        {
            return;
        }

        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                return;
            }
        }
        else
        {
            if (!isBotPlayer)
            {
                return;
            }
        }

        DataManager.Instance.PlayerData.Experience += totallDamageDealth;
    }
}
