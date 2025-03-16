using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    public TerrainGeneration terrainGenerationInstace;
    public Vector3 rotaionPivot;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(1))
        {
            //Cursor.visible = true;
        }


        if (Input.GetMouseButton(0))
        {
            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");

            Vector3 moveDirection = (-inputX * transform.right) + (-inputY * transform.forward);
            transform.position += new Vector3(moveDirection.x, 0, moveDirection.z);
        }


        if (Input.GetMouseButton(1))
        {
            float speed = 2;
            float inputY = Input.GetAxisRaw("Mouse X");

            if (rotaionPivot == null)
            {
                rotaionPivot = terrainGenerationInstace.middleTile.transform.position;
            }

            Vector3 pivot = new Vector3(
                rotaionPivot.x,
                transform.position.y,
                rotaionPivot.z);

            transform.RotateAround(pivot, Vector3.up, inputY * speed);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float delta = -Input.GetAxisRaw("Mouse ScrollWheel");
            
            Vector3 direction = transform.position - rotaionPivot;

            transform.position = transform.position + direction * delta;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 pivot = new Vector3(
                terrainGenerationInstace.middleTile.transform.position.x,
                transform.position.y,
                terrainGenerationInstace.middleTile.transform.position.z);

            transform.RotateAround(pivot, Vector3.up, 60);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 pivot = new Vector3(
                terrainGenerationInstace.middleTile.transform.position.x,
                transform.position.y,
                terrainGenerationInstace.middleTile.transform.position.z);

            transform.RotateAround(pivot, Vector3.up, -60);
        }
    }

}