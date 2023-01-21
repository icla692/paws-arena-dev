using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NFTSelection : MonoBehaviour
{
    public PagesBehaviour pages;

    public Transform playerPlatformParent;
    public GameObject playerPlatformPrefab;

    public GameObject nftButtonPrefab;
    public Transform nftButtonsParent;

    public GameObject enterArenaButton;

    public LoadingScreen loadingScreen;

    private List<GameObject> nftButtons = new List<GameObject>();
    private GameObject playerPlatform;

    private int currentPage = 0;
    private int pageSize = 9;

    private List<NFT> currentNFTs = new List<NFT>();

    private void OnEnable()
    {
        InitNFTScreen();
        pages.OnClick += OnPageSelected;
    }

    private void OnDisable()
    {
        foreach (NFT nfts in currentNFTs)
        {
            Destroy(nfts.imageTex);
            nfts.imageTex = null;
        }
        currentNFTs.Clear();

        if (playerPlatform != null)
        {
            Destroy(playerPlatform);
            playerPlatform = null;
        }


        pages.OnClick -= OnPageSelected;
    }

    public async void InitNFTScreen()
    {
        currentPage = 0;
        int maxPages = (int)Math.Floor((GameState.nfts.Count - 1) * 1.0 / pageSize);
        pages.SetNumberOfPages(maxPages + 1);
        await PopulateGridAsync();
        SelectNFT(0);
    }

    private async void OnPageSelected(int idx)
    {
        currentPage = idx;
        await PopulateGridAsync();
    }

    private List<NFT> GetNFTs(int pageNr, int pageSize)
    {
        return GameState.nfts.Skip(pageNr * pageSize).Take(pageSize).ToList();
    }
    private async UniTask PopulateGridAsync()
    {
        enterArenaButton.GetComponent<Button>().interactable = false;

        loadingScreen.Activate("Loading NFTs...");
        foreach(GameObject but in nftButtons)
        {
            Destroy(but);
        }

        nftButtons.Clear();

        foreach(NFT nfts in currentNFTs)
        {
            Destroy(nfts.imageTex);
            nfts.imageTex = null;
        }


        currentNFTs = GetNFTs(currentPage, pageSize);

        //Grab all images from internet
        List<UniTask> tasks = new List<UniTask>();
        int idx = 0;
        foreach (NFT nft in currentNFTs)
        {
            GameObject go = Instantiate(nftButtonPrefab, nftButtonsParent);
            nftButtons.Add(go);
            go.GetComponent<NFTImageButton>().SetLoadingState();
            tasks.Add(nft.GrabImage());
            idx++;
        }

        for(int i= currentNFTs.Count; i<9; i++)
        {
            GameObject go = Instantiate(nftButtonPrefab, nftButtonsParent);
            nftButtons.Add(go);
        }
        await UniTask.WhenAll(tasks.ToArray());

        loadingScreen.Deactivate();

        enterArenaButton.GetComponent<Button>().interactable = true;

        //Attach to images
        idx = 0;
        foreach (NFT nft in currentNFTs)
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
    }

    private void SelectNFT(int idx)
    {
        if(playerPlatform != null)
        {
            Destroy(playerPlatform);
        }

        Debug.Log("Selected " + currentNFTs[idx].imageUrl);

        GameState.SetSelectedNFT(currentNFTs[idx]);
        nftButtons[idx].GetComponent<NFTImageButton>().Select();
        playerPlatform = GameObject.Instantiate(playerPlatformPrefab, playerPlatformParent);
        playerPlatform.transform.localPosition = Vector3.zero;
    }
}
