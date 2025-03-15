using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;
    public CityHandler cityHandler;
    public int selectedTile;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(isClick());
            
        }
    }

    IEnumerator isClick()
    {
        yield return new WaitForSeconds(0.1f);
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            AfterClick();
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

            }
        }
        else
        {
            selectedTile = -1;
            playerHandler.playerCameras[playerHandler.currentPlayer].GetComponent<CameraMovement>().rotaionPivot = playerHandler.terrainGeneration.middleTile.transform.position;
        }
    }
}
