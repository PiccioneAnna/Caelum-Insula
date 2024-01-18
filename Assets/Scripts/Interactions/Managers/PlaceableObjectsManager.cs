using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObjectsManager : MonoBehaviour
{
    [SerializeField] Inventory.PlaceableObjectsContainer placeableObjects;
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] GameObject objPrefab;

    private void Start()
    {
        GameManager.Instance.placeableObjectsManager.placeableObjectsManager = this;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            placeableObjects.placeableObjects[i].targetObject = null;
        }
    }

    // Gets list of placeable objects and places them on map
    public void VisualizeMap()
    {
        for (int i = 0; i < placeableObjects.placeableObjects.Count; i++)
        {
            VisualizeItem(placeableObjects.placeableObjects[i]);
        }
    }

    public void VisualizeItem(PlaceableObject placeableObject)
    {
        GameObject go = Instantiate(objPrefab);
        Vector3 position = targetTilemap.CellToWorld(placeableObject.positionOnGrid);
        position = position + new Vector3(0, targetTilemap.cellSize.y, -1);
        go.transform.position = position;

        placeableObject.targetObject = go.transform;
    }

    // Checks whether there is an object already in that position
    public bool Check(Vector3Int position)
    {
        return placeableObjects.Get(position) != null;
    }

    // Place creates a gameobject and spawns it on selected tile
    public void Place(Item item, Vector3Int positionOnGrid)
    {
        if (Check(positionOnGrid)) { return; }

        PlaceableObject placeableObject = new PlaceableObject(item, positionOnGrid);
        VisualizeItem(placeableObject);
        placeableObjects.placeableObjects.Add(placeableObject);
    }

    public void PickUp(Vector3Int gridPosition)
    {
        PlaceableObject placedObject = placeableObjects.Get(gridPosition);

        if (placedObject == null) { return; }

        ItemSpawnManager.instance.SpawnItem(targetTilemap.CellToWorld(gridPosition), placedObject.placedItem);

        Destroy(placedObject.targetObject.gameObject);

        placeableObjects.Remove(placedObject);
    }
}