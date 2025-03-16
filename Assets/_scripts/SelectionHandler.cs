using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;
    public CityHandler cityHandler;
    public ForestHandler forestHandler;
    public SelectionArrowHandler selectionArrowHandler;
    public InGameUIHandler inGameUIHandler;
    public int selectedTile;

    public float clickDuration = 0f;
    public bool onUI = false;

    void Update()
    {
        if (onUI)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            clickDuration += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (clickDuration < 0.2f)
            {
                AfterClick();
            }
            clickDuration = 0f;

        }
    }
    void AfterClick()
    {
        Debug.Log("Mouse0");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Tile")
            {

                GameObject tile = hit.collider.gameObject;
                selectedTile = tile.GetComponent<Tile>().index;
                selectionArrowHandler.MoveArrow(selectedTile);
                playerHandler.playerCameras[playerHandler.currentPlayer].GetComponent<CameraMovement>().rotaionPivot = tile.transform.position;


                if (tile.GetComponent<Tile>().isCity)
                {
                    cityHandler.SelectedCity(selectedTile);
                    inGameUIHandler.HideCutButton();
                }
                else if (tile.GetComponent<Tile>().isForest)
                {
                    if (playerHandler.CanCutForest(selectedTile))
                    {
                        inGameUIHandler.ShowCutButton();
                    }
                    else
                    {
                        inGameUIHandler.HideCutButton();
                        //forestHandler.SelectedForest(selectedTile);
                    }
                }
                else
                {
                    inGameUIHandler.HideCutButton();
                }


            }
        }
        else
        {
            selectedTile = -1;
            playerHandler.playerCameras[playerHandler.currentPlayer].GetComponent<CameraMovement>().rotaionPivot = playerHandler.terrainGeneration.middleTile.transform.position;
        }
    }
}
