using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;
    public TMP_Text goldText;

    public void EndTurn()
    {
        playerHandler.EndTurn();
    }

    public void SetGold(int gold, int goldGain)
    {
        goldText.text = "Gold: " + gold + " / +" + goldGain ;
    }
}
