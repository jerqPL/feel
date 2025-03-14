
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject tilePrefab;
    public Texture2D biomesTexture;
    public float tileRadius = 1f;
    public int temperaturePerlinScale = 10;
    public int humidityPerlinScale = 10;
    public int temperatureSeed = 0;
    public int humiditySeed = 0;
    public int width = 10;
    public int height = 10;
    public int mapRadius = 10;
    public Vector2 startPos = new Vector2(0, 0);
    public List<GameObject> tiles = new List<GameObject>();
    public List<GameObject> forestTiles = new List<GameObject>();
    public List<Color> forestColors = new List<Color>();
    public List<GameObject> cornders = new List<GameObject>();
    public List<GameObject> edges = new List<GameObject>();
    public List<int> edgeIndexes = new List<int>();
    public GameObject middleTile;
    public bool terrainBuilt = false;
    public bool regenerate = false;
    public List<bool> tileUsed = new List<bool>();


    void Start()
    {
        StartCoroutine(buildTerrain2(mapRadius, tileRadius));
        temperatureSeed = Random.Range(0, 1000);
        humiditySeed = Random.Range(0, 1000);
    }

    void Update()
    {
        
        if (regenerate && terrainBuilt)
        {

            temperatureSeed = Random.Range(0, 1000);
            humiditySeed = Random.Range(0, 1000);
            StopAllCoroutines();
            regenerate = false;
            for (int i = 0; i < tiles.Count; i++)
            {
                Destroy(tiles[i]);
            }
            tiles.Clear();
            StartCoroutine(buildTerrain2(mapRadius, tileRadius));
        }
    }
    /*
    IEnumerator buildTerrain(int width, int height)
    {
        for (int j = 0; j < height; j++)
        {
            Vector2 rowStartPos = new Vector2(startPos.x, startPos.y + j * tileRadius * 1.5f);
            if (j % 2 == 1)
            {
                rowStartPos.x += tileRadius * Mathf.Sqrt(3) / 2;
            }
            for (int i = 0; i < width; i++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(rowStartPos.x + i * radius * Mathf.Sqrt(3), 0, rowStartPos.y), Quaternion.identity);
                newTile.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                newTile.transform.parent = transform;
                tiles.Add(newTile);
                Debug.Log((int)(1f / Time.unscaledDeltaTime));
                
            }
            yield return null;
        }
        terrainBuilt = true;
    }*/
    IEnumerator buildTerrain2(int mapRadius, float tileRadius)
    {
        int y = 0;
        terrainBuilt = false;
        Vector2 rowStartPos = new Vector2(startPos.x - mapRadius/2*tileRadius, startPos.y-mapRadius*tileRadius/2*Mathf.Sqrt(3));

        for(int j = 0; j < mapRadius/2; j++)
        {

            for (int i = 0; i < mapRadius / 2 + j; i++)
            {
                y += 1;
                
                GameObject newTile = Instantiate(tilePrefab, new Vector3(rowStartPos.x + i * tileRadius * Mathf.Sqrt(3),0, rowStartPos.y), Quaternion.identity);
                Color color = GetColor(rowStartPos.x + i * tileRadius * Mathf.Sqrt(3), rowStartPos.y);
                newTile.GetComponent<MeshRenderer>().material.color = color;
                newTile.transform.parent = transform;
                tiles.Add(newTile);
                newTile.active = false;
                if (color.g > color.r && color.g > color.b)
                {
                    forestTiles.Add(newTile);
                }
                //StartCoroutine(animateTile(newTile));
                if ((i == 0 || i == mapRadius / 2 + j - 1) && (j == 0 || j == mapRadius / 2 - 1))
                {
                    cornders.Add(newTile);
                }


                if (i == 0 || i == mapRadius/2+j-1 || j == 0 || (j == mapRadius/2-1 && (i == 0 || i == mapRadius / 2 + j - 1)))
                {
                    edges.Add(newTile);
                    edgeIndexes.Add(tiles.Count - 1);
                }

                if (j == mapRadius / 2 - 1 && i == (mapRadius / 2 + j - 1) / 2)
                {
                    middleTile = newTile;
                }
                //yield return null;

            }
            if (j % 2 == 0)
            {
                yield return null;
            }

            rowStartPos += new Vector2(-tileRadius * Mathf.Sqrt(3) / 2, 1.5f * tileRadius);
        }
        rowStartPos += 2 * (new Vector2(tileRadius * Mathf.Sqrt(3) / 2, 0));
        
        for (int j = 1; j < mapRadius / 2; j++)
        {

            for (int i = 0; i < mapRadius - j - 1; i++)
            {
                y += 1;
                GameObject newTile = Instantiate(tilePrefab, new Vector3(rowStartPos.x + i * tileRadius * Mathf.Sqrt(3), 0, rowStartPos.y), Quaternion.identity);
                Color color = GetColor(rowStartPos.x + i * tileRadius * Mathf.Sqrt(3), rowStartPos.y);
                newTile.GetComponent<MeshRenderer>().material.color = color;
                newTile.transform.parent = transform;
                tiles.Add(newTile);
                newTile.active = false;
                if (color.g > color.r && color.g > color.b)
                {
                    forestTiles.Add(newTile);
                }
                if (j == mapRadius / 2 - 1 && i == mapRadius - j - 1 - 1)
                {
                    //StartCoroutine(animateEndTile(newTile));
                }
                else
                {
                    //StartCoroutine(animateTile(newTile));
                }
                if ((i == 0 || i == mapRadius - j - 2) && (j == 0 || j == mapRadius / 2 - 1))
                {
                    cornders.Add(newTile);
                }


                if (i == 0 || i == mapRadius - j - 2 || j == 0 || (j == mapRadius / 2 - 1))
                {
                    edges.Add(newTile);
                    edgeIndexes.Add(tiles.Count - 1);
                }

                //yield return null;

            }
            if (j % 2 == 0)
            {
                yield return null;
            }
            rowStartPos += new Vector2(tileRadius * Mathf.Sqrt(3) / 2, 1.5f * tileRadius);
            
        }
        for (int i = 0; i < tiles.Count; i++)
        {
            tileUsed.Add(false);
        }
        StartCoroutine(animatetilesRandom());

        yield return new WaitForSeconds(2f);
        /*foreach (GameObject edge in edges)
        {
            edge.transform.position += new Vector3(0, 5f, 0);
        }*/


        /*foreach (GameObject corner in cornders)
        {
            corner.transform.position += new Vector3(0, 5f, 0);
        }*/



        /*
        for(int j = mapRadius/2; j >= 0; j--)
        {
            Vector2 rowStartPos = new Vector2(startPos.x, startPos.y + j * tileRadius * 1.5f);
            if (j % 2 == 1)
            {
                rowStartPos.x -= tileRadius * Mathf.Sqrt(3) / 2;
            }
            for (int i = j; i < 2 * mapRadius - j; i++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(rowStartPos.x + i * tileRadius * Mathf.Sqrt(3), 0, rowStartPos.y), Quaternion.identity);
                newTile.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                newTile.transform.parent = transform;
                tiles.Add(newTile);
                yield return null;
            }
            
        }
        for (int j = 0; j < mapRadius / 2; j++)
        {
            Vector2 rowStartPos = new Vector2(startPos.x, startPos.y + (j + mapRadius / 2) * tileRadius * 1.5f);
            if (j % 2 == 1)
            {
                rowStartPos.x -= tileRadius * Mathf.Sqrt(3) / 2;
            }
            for (int i = j; i < 2 * mapRadius - j; i++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(rowStartPos.x + i * tileRadius * Mathf.Sqrt(3), 0, rowStartPos.y), Quaternion.identity);
                newTile.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                newTile.transform.parent = transform;
                tiles.Add(newTile);
                yield return null;
            }
            
        }*/

        /*
        for (int j = 0; j < height; j++)
        {
            Vector2 rowStartPos = new Vector2(startPos.x, startPos.y + j * radius * 1.5f);
            if (j % 2 == 1)
            {
                rowStartPos.x += radius * Mathf.Sqrt(3) / 2;
            }
            for (int i = 0; i < width; i++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(rowStartPos.x + i * radius * Mathf.Sqrt(3), 0, rowStartPos.y), Quaternion.identity);
                newTile.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                newTile.transform.parent = transform;
                tiles.Add(newTile);
                Debug.Log((int)(1f / Time.unscaledDeltaTime));

            }
            yield return null;
        }
        */

    }


    Color GetColor(float x, float z)
    {
        float temperature = Mathf.PerlinNoise((x + temperatureSeed) / temperaturePerlinScale, (z + temperatureSeed) / temperaturePerlinScale);
        float humidity = Mathf.PerlinNoise((x + humiditySeed) / humidityPerlinScale, (z + humiditySeed) / humidityPerlinScale);
        Color color = biomesTexture.GetPixel((int)(temperature * biomesTexture.width), (int)(humidity * biomesTexture.height));
        Debug.Log(ColorToIndex(color));
        return color;
    }


    IEnumerator animatetilesRandom()
    {
        int ileNaRaz = 50;

        List<int> tileShuffledIndexes = new List<int>();

        for (int i = 0; i < tiles.Count; i++)
        {
            tileShuffledIndexes.Add(i);
        }

        var count = tileShuffledIndexes.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = tileShuffledIndexes[i];
            tileShuffledIndexes[i] = tileShuffledIndexes[r];
            tileShuffledIndexes[r] = tmp;
        }

        int ileAnimated = 0;
        while (ileAnimated < count-ileNaRaz)
        {
            for (int i = 0; i < ileNaRaz; i++)
            {
                StartCoroutine(animateTile(tiles[tileShuffledIndexes[ileAnimated + i]]));
            }
            ileAnimated += ileNaRaz;
            ileNaRaz++;
            yield return null;
        }

        for (int i = ileAnimated; i < count - 1; i++)
        {
            StartCoroutine(animateTile(tiles[tileShuffledIndexes[i]]));
        }

        StartCoroutine(animateEndTile(tiles[tileShuffledIndexes[count - 1]]));



    }

    int ColorToIndex(Color color)
    {
        int red = (int)(color.r * 255);
        int green = (int)(color.g * 255);
        int blue = (int)(color.b * 255);

        int[,] colorMap = {
            {63, 167, 53}, {9, 150, 211}, {109, 166, 22}, {12, 89, 45},
            {15, 197, 206}, {224, 199, 32}, {30, 116, 53}, {24, 210, 105},
            {174, 25, 25}, {255, 255, 255}, {39, 237, 21}, {210, 117, 15}
        };

        for (int i = 0; i < colorMap.GetLength(0); i++)
        {
            if (colorMap[i, 0] == red && colorMap[i, 1] == green && colorMap[i, 2] == blue)
                return i;
        }

        return -1;
    }

    IEnumerator animateTile(GameObject tile)
    {
        tile.active = true;
        float animationTime = .7f;
        Vector3 animationDistane = new Vector3(0, 7f, 0);
        Vector3 startPos = tile.transform.position;
        tile.transform.position += animationDistane;
        float t = 0;
        while (t < animationTime)
        {
            tile.transform.position = Vector3.Lerp(startPos + animationDistane, startPos, Mathf.Pow(t/animationTime, 3));
            t += Time.deltaTime;
            yield return null;
        }
        tile.transform.position = startPos;
    }

    IEnumerator animateEndTile(GameObject tile)
    {
        yield return animateTile(tile);
        terrainBuilt = true;
    }
}
