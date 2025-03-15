using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;

    public void EndTurn()
    {
        playerHandler.EndTurn();
    }
}
