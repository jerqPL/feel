using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;
    public CityHandler cityHandler;
    public ForestHandler forestHandler;
    public int selectedTile;

    public float clickDuration = 0f;

    void Update()
    {
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
                playerHandler.playerCameras[playerHandler.currentPlayer].GetComponent<CameraMovement>().rotaionPivot = tile.transform.position;


                if (tile.GetComponent<Tile>().isCity)
                {
                    cityHandler.SelectedCity(selectedTile);
                }
                else if (tile.GetComponent<Tile>().isForest)
                {
                    forestHandler.SelectedForest(selectedTile);
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
