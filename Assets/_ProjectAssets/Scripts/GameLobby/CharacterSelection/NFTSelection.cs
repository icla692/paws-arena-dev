using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NFTSelection : MonoBehaviour
{
    public Transform playerPlatformParent;
    public GameObject playerPlatformPrefab;

    public GameObject nftButtonPrefab;
    public Transform nftButtonsParent;

    public GameObject enterArenaButton;

    private List<GameObject> nftButtons = new List<GameObject>();
    private GameObject playerPlatform;

    private async void OnEnable()
    {
        await InitPage();
    }

    private void OnDisable()
    {
        if(playerPlatform != null)
        {
            Destroy(playerPlatform);
            playerPlatform = null;
        }
    }

    public async UniTask InitPage()
    {
        await PopulateGrid();
    }

    private async UniTask PopulateGrid()
    {
        enterArenaButton.GetComponent<Button>().interactable = false;
        foreach(GameObject but in nftButtons)
        {
            Destroy(but);
        }

        nftButtons.Clear();


        List<NFT> nfts = GameState.nfts;

        //Grab all images from internet
        List<UniTask> tasks = new List<UniTask>();
        int idx = 0;
        foreach (NFT nft in nfts)
        {
            GameObject go = Instantiate(nftButtonPrefab, nftButtonsParent);
            nftButtons.Add(go);
            go.GetComponent<NFTImageButton>().SetLoadingState();
            tasks.Add(nft.GrabImage());
            idx++;
        }

        for(int i=nfts.Count; i<9; i++)
        {
            GameObject go = Instantiate(nftButtonPrefab, nftButtonsParent);
            nftButtons.Add(go);
        }
        await UniTask.WhenAll(tasks.ToArray());
        enterArenaButton.GetComponent<Button>().interactable = true;

        //Attach to images
        idx = 0;
        foreach (NFT nft in nfts)
        {
            nftButtons[idx].GetComponent<NFTImageButton>().SetTexture(nft.imageTex);
            nftButtons[idx].GetComponent<Button>().onClick.RemoveAllListeners();

            int crtIdx = idx;
            nftButtons[idx].GetComponent<Button>().onClick.AddListener(()=>
            {
                SelectNFT(crtIdx);
            });
            idx++;
        }

        SelectNFT(0);
    }

    private void SelectNFT(int idx)
    {
        if(playerPlatform != null)
        {
            Destroy(playerPlatform);
        }

        Debug.Log("Selected " + GameState.nfts[idx].imageUrl);
        GameState.SetSelectedNFT(GameState.nfts[idx]);
        nftButtons[idx].GetComponent<NFTImageButton>().Select();
        playerPlatform = GameObject.Instantiate(playerPlatformPrefab, playerPlatformParent);
        playerPlatform.transform.localPosition = Vector3.zero;
    }
}
