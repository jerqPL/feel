using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TerrainGeneration))]
public class ForestHandler : MonoBehaviour
{
    public TerrainGeneration terrainGeneration;
    public GameObject forestPrefab;
    public float forestProcentage;
    public List<GameObject> forests = new List<GameObject>();
    public List<int> forestIndexes = new List<int>();
    public Vector3 forestRotation;


    void Start()
    {
        StartCoroutine(checkForTerrain());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator checkForTerrain()
    {
        while (!terrainGeneration.terrainBuilt)
        {
            yield return null;
        }
        StartCoroutine(BuildForests());
    }

    IEnumerator BuildForests()
    {
        for (int i = 0; i < terrainGeneration.tiles.Count; i++)
        {
            GameObject forestTile = terrainGeneration.tiles[i];
            if (terrainGeneration.tileUsed[i]) continue;
            if (Random.RandomRange(0, 100) < forestProcentage * 100)
            {
                
                GameObject forest = Instantiate(forestPrefab, forestTile.transform.position, Quaternion.identity);
                forest.transform.parent = forestTile.transform;
                forest.transform.rotation = Quaternion.Euler(forestRotation);
                forests.Add(forest);
                StartCoroutine(animateForest(forest));
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    IEnumerator animateForest(GameObject forest)
    {
        float animationTime = 0.5f;
        Vector3 animationDistane = new Vector3(0, 7f, 0);
        Vector3 startPos = forest.transform.position;
        forest.transform.position += animationDistane;
        float t = 0;
        while (t < animationTime)
        {
            forest.transform.position = Vector3.Lerp(startPos + animationDistane, startPos, Mathf.Pow(t / animationTime, 5));
            t += Time.deltaTime;
            yield return null;
        }
        forest.transform.position = startPos;
    }


}
