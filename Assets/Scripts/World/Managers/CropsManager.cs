using System;
using UnityEngine;

[Serializable]
public class CropTile
{
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer renderer;
    public float damage;
    public Vector2Int position;

    public bool Complete
    {
        get
        {
            if (crop == null) { return false; }
            return growTimer >= crop.timeToGrow;
        }
    }

    internal void Harvested()
    {
        growTimer = 0;
        growStage = 0;
        damage = 0;
        crop = null;
        renderer.gameObject.SetActive(false);
    }

    internal void Regrowth()
    {
        growTimer = crop.growthStageTime[crop.growthStageTime.Count - 2];
        growStage = crop.sprites.Count - 1;
    }
}

public class CropsManager : MonoBehaviour
{
    public TilemapScripts.CropsManager cropsManager;

    private void TilemapCheck()
    {
        if (cropsManager == null)
        {
            Debug.Log("No crops manager referenced, looking for one.");
            cropsManager = GameObject.Find("Crops").GetComponent<TilemapScripts.CropsManager>();

            if (cropsManager == null)
            {
                Debug.LogWarning("No crops manager found. Method returned");
                return;
            }
        }
    }

    public void PickUp(Vector3Int position)
    {
        TilemapCheck();
        cropsManager.PickUp(position);
    }

    public bool Check(Vector3Int position)
    {
        return cropsManager.Check(position);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        TilemapCheck();
        cropsManager.Seed(position, toSeed);
    }

    public void Plow(Vector3Int position)
    {
        TilemapCheck();
        cropsManager.Plow(position);
    }

    public void Till(Vector3Int position)
    {
        TilemapCheck();
        cropsManager.Till(position);
    }
}