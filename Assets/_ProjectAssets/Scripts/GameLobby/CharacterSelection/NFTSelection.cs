using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NFTSelection : MonoBehaviour
{
    public Transform nftGridParent;
    public TMPro.TextMeshProUGUI furType;

    public GameObject leftArrow;
    public GameObject rightArrow;

    public List<NFTImageButton> nftButtons;

    private int crtPage = 0;
    private int pageSize = 6;
    private int nrOfPages;

    private async void OnEnable()
    {
        nrOfPages = GetNrOfPages();
        await PopulateGrid(crtPage);
    }

    private async UniTask PopulateGrid(int page)
    {
        HandleArrows(page);

        foreach(NFTImageButton but in nftButtons)
        {
            but.ResetState();
        }


        List<NFT> nfts = GetNFTs(page, pageSize);

        //Grab all images from internet
        List<UniTask> tasks = new List<UniTask>();
        int idx = 0;
        foreach (NFT nft in nfts)
        {
            nftButtons[idx].SetLoadingState();
            tasks.Add(nft.GrabImage());
            idx++;
        }
        await UniTask.WhenAll(tasks.ToArray());

        //Attach to images
        idx = 0;
        foreach (NFT nft in nfts)
        {
            nftButtons[idx].SetTexture(nft.imageTex);
            nftButtons[idx].GetComponent<Button>().onClick.RemoveAllListeners();

            int crtIdx = idx;
            nftButtons[idx].GetComponent<Button>().onClick.AddListener(()=>
            {
                GameState.SetSelectedNFT(GameState.nfts[crtPage * pageSize + crtIdx]);
                nftButtons[crtIdx].Select();
            });
            furType.text = nft.furType;
            idx++;
        }

        GameState.SetSelectedNFT(GameState.nfts[crtPage * pageSize + 0]);
        nftButtons[0].Select();
    }

    private void HandleArrows(int page)
    {
        if (page == 0)
        {
            leftArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
        }

        if (page == nrOfPages)
        {
            rightArrow.SetActive(false);
        }
        else
        {
            rightArrow.SetActive(true);
        }
    }

    public async void NextPage()
    {
        crtPage++;
        await PopulateGrid(crtPage);
    }

    public async void PreviousPage()
    {
        crtPage--;
        await PopulateGrid(crtPage);
    }

    private int GetNrOfPages()
    {
        return GameState.nfts.Count / pageSize;
    }

    private List<NFT> GetNFTs(int page, int pageSize)
    {
        List<NFT> nfts = GameState.nfts;
        return nfts.Skip(pageSize * page).Take(pageSize).ToList();
    }
}
