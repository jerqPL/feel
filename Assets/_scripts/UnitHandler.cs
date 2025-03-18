using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public TerrainGeneration terrainGeneration;

    public List<GameObject> unitPrefabs;
    public List<Unit> units = new List<Unit>();

    public Unit CreateUnit(int position, int type)
    {
        Unit unit = new Unit();
        unit.position = position;
        unit.type = type;
        unit.gameObject = Instantiate(unitPrefabs[type], terrainGeneration.tiles[position].transform.position, Quaternion.identity);
        units.Add(unit);
        return unit;
    }

    public void MoveUnit(Unit unit, int pos2)
    {
        unit.position = pos2;
        unit.gameObject.transform.position = terrainGeneration.tiles[pos2].transform.position;
    }
}

public class Unit
{
    public int position;
    public int type; // 0 = swordman, 1 = archer
    public GameObject gameObject;
}
