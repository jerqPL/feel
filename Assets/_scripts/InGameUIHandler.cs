using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;
    public SelectionHandler selectionHandler;
    public TMP_Text goldText;
    public GameObject cutButton;

    private void Start()
    {
        cutButton.SetActive(false);
    }

    public void EndTurn()
    {
        playerHandler.EndTurn();
    }

    public void SetGold(int gold, int goldGain)
    {
        goldText.text = "Gold: " + gold + " / +" + goldGain ;
    }

    public void ShowCutButton()
    {
        cutButton.SetActive(true);
    }
    public void HideCutButton()
    {
        cutButton.SetActive(false);
        StartCoroutine(notOnUI());
    }

    IEnumerator notOnUI()
    {
        yield return null;
        selectionHandler.onUI = false;
    }

    public void CutForest()
    {
        selectionHandler.forestHandler.CutForest(selectionHandler.selectedTile);
        HideCutButton();
    }
}
