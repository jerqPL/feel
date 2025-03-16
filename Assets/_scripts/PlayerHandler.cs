using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    public int numberOfPlayers = 1;
    public CityHandler cityHandler;
    public TerrainGeneration terrainGeneration;
    public InGameUIHandler inGameUIHandler;
    public ForestHandler forestHandler;

    public List<Player> players = new List<Player>();

    public int currentPlayer = 0;   

    public List<bool> takenTiles = new List<bool>();

    public GameObject mainCamera;
    public List<GameObject> playerCameras = new List<GameObject>();

    public float measureError = 0.5f;

    public List<int> playerGold = new List<int>();

    void Start()
    {
        playerCameras.Add(mainCamera);
        if (numberOfPlayers > cityHandler.cityCount)
        {
            Debug.LogError(" cities are not enough for " + numberOfPlayers + " players");
        }


        for (int i = 0; i < numberOfPlayers; i++)
        {
            players.Add(new Player());
            playerGold.Add(5);
        }

        for(int i = 0; i < numberOfPlayers - 1; i++)
        {
            playerCameras.Add(Instantiate(mainCamera));
            playerCameras[i+1].active = false;
        }

        

        StartCoroutine(checkForCities());
    }

    IEnumerator checkForCities()
    {
        while (!cityHandler.citiesBuilt)
        {
            yield return null;
        }
        for (int i = 0; i < terrainGeneration.tiles.Count; i++)
        {
            takenTiles.Add(false);
        }
        AssignCities();
    }

    void AssignCities()
    {
        List<int> cityIndexes = new List<int>();
        for (int i = 0; i < cityHandler.cityCount; i++)
        {
            cityIndexes.Add(i);
        }

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Color playerColor = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
            playerColor = Color.magenta;
            players[i].name = "Player " + i;
            players[i].playerColor = playerColor;
            players[i].capitalCity = new PlayerCity();
            players[i].capitalCity.cityIndex = cityIndexes[Random.Range(0, cityIndexes.Count - 1)];
            players[i].capitalCity.GetCityTiles(takenTiles, terrainGeneration, cityHandler, measureError, playerColor);
            //players[i].capitalCity.IncreaseCitySize(takenTiles, terrainGeneration, cityHandler, measureError, playerColor);

            players[i].playerCities.Add(players[i].capitalCity);
            
            cityIndexes.Remove(players[i].capitalCity.cityIndex);
            cityHandler.cities[players[i].capitalCity.cityIndex].GetComponent<MeshRenderer>().material.color = playerColor;

            Vector3 cameraToMiddle = terrainGeneration.middleTile.transform.position - playerCameras[i].transform.position;
            // newCamPos + cameraToMiddle = playerCapitalCityPos => newCamPos = playerCapitalCityPos - cameraToMiddle
            playerCameras[i].transform.position = cityHandler.cities[players[i].capitalCity.cityIndex].transform.position - cameraToMiddle;
            playerCameras[i].GetComponent<CameraMovement>().rotaionPivot = cityHandler.cities[players[i].capitalCity.cityIndex].transform.position;

        }

        inGameUIHandler.SetGold(playerGold[currentPlayer], players[currentPlayer].goldGain);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndTurn()
    {
        if (currentPlayer == numberOfPlayers - 1)
        {
            currentPlayer = 0;
        }
        else
        {
            currentPlayer++;
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerCameras[i].active = false;
        }
        playerCameras[currentPlayer].active = true;

        for (int i = 0;  i < players[currentPlayer].playerCities.Count; i++)
        {
            playerGold[currentPlayer] += players[currentPlayer].playerCities[i].goldGain;
        }

        inGameUIHandler.SetGold(playerGold[currentPlayer], players[currentPlayer].goldGain);

    }

    public void SelectedCity(int index)
    {
        Debug.Log("City " + index + " selected");
        for (int i = 0; i < numberOfPlayers; i++)
        {
            for (int j = 0; j < players[i].playerCities.Count; j++)
            {
                if (players[i].playerCities[j].cityIndex == index)
                {
                    //players[i].playerCities[j].IncreaseCitySize(takenTiles, terrainGeneration, cityHandler, measureError, players[i].playerColor);
                }
            }

        }
    }

    public void SelectedForest(int index)
    {
        int goldCost = 2;
        Debug.Log("Forest " + index + " selected");
        int forestTileIndex = forestHandler.forestIndexes[index];
        for (int i = 0; i < players[currentPlayer].playerCities.Count; i++)
        {
            if (players[currentPlayer].playerCities[i].cityTiles.Contains(forestTileIndex) && playerGold[currentPlayer] > goldCost)
            {
                playerGold[currentPlayer] -= goldCost;
                Debug.Log("found city");
                players[currentPlayer].playerCities[i].requiredUpgrades -= 1;
                if (players[currentPlayer].playerCities[i].requiredUpgrades <= 0)
                {
                    players[currentPlayer].playerCities[i].tier += 1;
                    players[currentPlayer].goldGain += 1;
                    players[currentPlayer].playerCities[i].goldGain += 1;
                    players[currentPlayer].playerCities[i].requiredUpgrades = ((int)Mathf.Pow(2, players[currentPlayer].playerCities[i].tier - 1));
                    players[currentPlayer].playerCities[i].IncreaseCitySize(takenTiles, terrainGeneration, cityHandler, measureError, players[currentPlayer].playerColor);
                    
                }

                inGameUIHandler.SetGold(playerGold[currentPlayer], players[currentPlayer].goldGain);
                forestHandler.DeleteForest(index);
            }
        }
    }
}

public class Player
{
    public PlayerCity capitalCity;
    public List<PlayerCity> playerCities = new List<PlayerCity>();
    public string name;
    public Color playerColor;
    public int goldGain = 1;

}

public class PlayerCity
{
    public int cityIndex;
    public List<int> cityTiles = new List<int>();
    public int goldGain = 1;
    public int tier = 1;
    public int requiredUpgrades = 1;

    public void GetCityTiles(List<bool> takenTiles, TerrainGeneration terrainGeneration, CityHandler cityHandler, float measureError, Color color)
    {
     
        Debug.Log("City Index: " + cityIndex);
        int cityTileIndex = cityHandler.cityIndexes[cityIndex];
        int leftTileIndex = cityTileIndex - 1;
        int rightTileIndex = cityTileIndex + 1;
        cityTiles.Add(cityTileIndex);
        takenTiles[cityTileIndex] = true;
        
        if (cityTileIndex - 1 >= 0)
        {
            if (takenTiles[cityTileIndex - 1] == false && terrainGeneration.tiles[cityTileIndex - 1].transform.position.z == terrainGeneration.tiles[cityTileIndex].transform.position.z)
            {
                cityTiles.Add(cityTileIndex - 1);
                takenTiles[cityTileIndex - 1] = true;
            }
        }
        
        
        if (takenTiles[cityTileIndex + 1] == false && terrainGeneration.tiles[cityTileIndex + 1].transform.position.z == terrainGeneration.tiles[cityTileIndex].transform.position.z)
        {
            cityTiles.Add(cityTileIndex + 1);
            takenTiles[cityTileIndex + 1] = true;
        }
        for (int i = cityTileIndex+2; i < terrainGeneration.tiles.Count; i++)
        {
            Debug.Log("i: " + i);
            if (Vector3.Distance(terrainGeneration.tiles[i].transform.position, terrainGeneration.tiles[cityTileIndex].transform.position) < (Mathf.Sqrt(3) + measureError) * terrainGeneration.tileRadius)
            {

                cityTiles.Add(i);
                takenTiles[i] = true;
            }
        }
        for (int i = cityTileIndex - 2; i >= 0; i--)
        {
            Debug.Log("i: " + i);
            if (Vector3.Distance(terrainGeneration.tiles[i].transform.position, terrainGeneration.tiles[cityTileIndex].transform.position) < (Mathf.Sqrt(3) + measureError) * terrainGeneration.tileRadius)
            {

                cityTiles.Add(i);
                takenTiles[i] = true;
            }
        }
        foreach (int tileIndex in cityTiles)
        {
            terrainGeneration.tiles[tileIndex].GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void IncreaseCitySize(List<bool> takenTiles, TerrainGeneration terrainGeneration, CityHandler cityHandler, float measureError, Color color)
    {
        List<int> oldCityTiles = new List<int>();
        foreach (int tileIndex in cityTiles)
        {
            oldCityTiles.Add(tileIndex);
        }
        foreach (int tileIndex in oldCityTiles)
        {
            for (int i = 0; i < terrainGeneration.tiles.Count; i++)
            {
                if (Vector3.Distance(terrainGeneration.tiles[i].transform.position, terrainGeneration.tiles[tileIndex].transform.position) < (Mathf.Sqrt(3) + measureError) * terrainGeneration.tileRadius)
                {
                    cityTiles.Add(i);
                    takenTiles[i] = true;
                    terrainGeneration.tiles[i].GetComponent<MeshRenderer>().material.color = color;
                }
            }
        }
    }

    
}