using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainGeneration))]
public class CityHandler : MonoBehaviour
{
    public TerrainGeneration terrainGeneration;
    public GameObject cityPrefab;
    public int cityCount = 10;
    public List<GameObject> cities = new List<GameObject>();
    public List<int> cityIndexes = new List<int>();
    public Vector3 houseRotation;

    public bool citiesBuilt = false;


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
        StartCoroutine(BuildCities());
    }

    IEnumerator BuildCities()
    {
        int attemptsPerCity = 10; // Limit the number of distance checks per city

        for (int i = 0; i < cityCount; i++)
        {
            int bestIndex = -1;
            float maxMinDistance = float.MinValue;

            for (int attempt = 0; attempt < attemptsPerCity; attempt++)
            {
                int index = Random.Range(0, terrainGeneration.tiles.Count-1);


                if (terrainGeneration.edgeIndexes.Contains(index)) continue;
                if (cityIndexes.Contains(index)) continue;
                if (terrainGeneration.tileUsed[index]) continue;

                float minDistance = float.MaxValue;

                foreach (int cityIndex in cityIndexes)
                {
                    float distance = Vector3.Distance(
                        terrainGeneration.tiles[index].transform.position,
                        terrainGeneration.tiles[cityIndex].transform.position
                    );
                    minDistance = Mathf.Min(minDistance, distance);
                }

                // Keep track of the best tile
                if (minDistance > maxMinDistance)
                {
                    maxMinDistance = minDistance;
                    bestIndex = index;
                }
            }

            if (bestIndex != -1 && maxMinDistance > terrainGeneration.tileRadius * 2 * Mathf.Sqrt(3))
            {
                cityIndexes.Add(bestIndex);
                GameObject city = Instantiate(cityPrefab, terrainGeneration.tiles[bestIndex].transform.position, Quaternion.identity);
                city.transform.parent = terrainGeneration.tiles[bestIndex].transform;
                city.transform.rotation = Quaternion.Euler(houseRotation);
                cities.Add(city);
                terrainGeneration.tiles[bestIndex].GetComponent<Tile>().isCity = true;

                //StartCoroutine(animateCity(city));
            }
            //yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < cities.Count-1; i++)
        {
            StartCoroutine(animateCity(cities[i]));
        }

        StartCoroutine(animateEndCity(cities[cities.Count-1]));
        
        yield return null;
    }

    IEnumerator animateCity(GameObject city)
    {
        float animationTime = 0.5f;
        Vector3 animationDistane = new Vector3(0, 7f, 0);
        Vector3 startPos = city.transform.position;
        city.transform.position += animationDistane;
        float t = 0;
        while (t < animationTime)
        {
            city.transform.position = Vector3.Lerp(startPos + animationDistane, startPos, Mathf.Pow(t / animationTime, 5));
            t += Time.deltaTime;
            yield return null;
        }
        city.transform.position = startPos;
    }

    IEnumerator animateEndCity(GameObject city)
    {
        yield return animateCity(city);
        citiesBuilt = true;
    }


}
