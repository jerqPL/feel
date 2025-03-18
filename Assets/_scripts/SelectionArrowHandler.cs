using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionArrowHandler : MonoBehaviour
{
    public TerrainGeneration terrainGeneration;
    bool animate = false;

    private void Start()
    {
        if (animate)
        {
            StartCoroutine(animateArrow());
        }
    }

    public void MoveArrow(int index)
    {
        Vector3 position = terrainGeneration.tiles[index].transform.position;
        transform.position = new Vector3(
            position.x,
            transform.position.y,
            position.z
            );
    }

    IEnumerator animateArrow()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Sin(time * 5f) * 0.5f + 2.5f,
                transform.position.z
                );
            yield return null;
        }    
    }
}
