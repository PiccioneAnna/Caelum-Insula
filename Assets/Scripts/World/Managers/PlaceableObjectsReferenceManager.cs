using Data;
using UnityEngine;

public class PlaceableObjectsReferenceManager : MonoBehaviour
{
    public PlaceableObjectsManager placeableObjectsManager;

    public void Place(Item item, Vector3Int positionOnGrid)
    {
        if (placeableObjectsManager == null)
        {
            Debug.LogWarning("No placeableObjectsManager reference detected");
            return;
        }

        placeableObjectsManager.Place(item, positionOnGrid);
    }

    public void PickUp(Vector3Int gridPosition)
    {
        if (placeableObjectsManager == null)
        {
            Debug.LogWarning("No placeableObjectsManager reference detected");
            return;
        }

        placeableObjectsManager.PickUp(gridPosition);
    }

    public bool Check(Vector3Int pos)
    {
        if (placeableObjectsManager == null)
        {
            Debug.LogWarning("No placeableObjectsManager reference detected");
            return false;
        }
        return placeableObjectsManager.Check(pos);
    }
}