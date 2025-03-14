using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    public TerrainGeneration terrainGenerationInstace;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.visible = true;
        }


        if (Input.GetMouseButton(0))
        {
            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");
            transform.position += new Vector3(-inputX, 0, -inputY);
        }

        if (Input.GetMouseButton(1))
        {
            float inputY = Input.GetAxisRaw("Mouse X");
            

            

            /*transform.Rotate(new Vector3(
                terrainGenerationInstace.middleTile.transform.position.x,
                transform.position.y,
                terrainGenerationInstace.middleTile.transform.position.z
                ), inputY);*/
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float delta = -Input.GetAxisRaw("Mouse ScrollWheel");
            
            Vector3 direction = transform.position - terrainGenerationInstace.middleTile.transform.position;

            transform.position = transform.position + direction * delta;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
        }
    }

}