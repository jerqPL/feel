using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public PlayerHandler playerHandler;
    public int selectedTile;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
                }
            }
            else
            {
                selectedTile = -1;
            }
        }
    }
}
