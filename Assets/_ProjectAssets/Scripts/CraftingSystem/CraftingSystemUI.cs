using UnityEngine;

public class CraftingSystemUI : MonoBehaviour
{
    [SerializeField] CraftingUI craftingUI;
    [SerializeField] DisenchantUI disenchantUI;

    public void Setup()
    {
        craftingUI.Setup();
        disenchantUI.Close();
    }

    //called from UI buttons
    public void ShowCraftiongUI()
    {
        craftingUI.Setup();
        disenchantUI.Close();
    }


    //called from UI buttons
    public void ShowDisenchtUI()
    {
        craftingUI.Close();
        disenchantUI.Setup();
    }
}
