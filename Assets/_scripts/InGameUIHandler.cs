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
    public GameObject recruitSwordmanButton;

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

    public void ShowRecruitSwordmanButton()
    {
        recruitSwordmanButton.SetActive(true);
    }

    public void HideRecruitSwordmanButton()
    {
        recruitSwordmanButton.SetActive(false);
        StartCoroutine(notOnUI());
    }

    IEnumerator notOnUI()
    {
        yield return null;
        selectionHandler.onUI = false;
    }

    public void CutForest()
    {
        if (selectionHandler.playerHandler.CanCutForestAndHaveGold(selectionHandler.selectedTile))
        {
            selectionHandler.forestHandler.CutForest(selectionHandler.selectedTile);
            HideCutButton();
        }
        
        
    }

    public void RecruitSwordman()
    {
        Debug.Log("RecruitSwordman");
        if (playerHandler.CanRecruitSwordman(selectionHandler.selectedTile))
            playerHandler.RecruitSwordman(selectionHandler.selectedTile);
    }
}
