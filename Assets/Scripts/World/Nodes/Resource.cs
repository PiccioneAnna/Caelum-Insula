using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    #region Fields
    [Header("Resource Stats")]
    [HideInInspector] public Resource instance;
    public Stat health;
    public Item[] droppedObjs;
    [SerializeField] public ResourceType nodeType;

    // Randomized drops
    private Item drop;
    private System.Random random;
    private Vector3 position;
    private Quaternion rotation;
    private float offsetX, offsetY;
    private int multplierX, multplierY;
    public int maxDropCount, minDropCount;
    private int dropCount;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        health.SetToMax();

        random = new System.Random();
        dropCount = random.Next(maxDropCount) + minDropCount + (int)transform.localScale.x;
        position = transform.position;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // idk if i need this rn but ill keep it for later
    }

    public void Hit()
    {
        health.currVal--;

        if (health.currVal <= 0)
        {
            Debug.Log("Drop Count:" + dropCount);

            for (int i = 0; i < dropCount; i++)
            {
                drop = droppedObjs[random.Next(droppedObjs.Length)];

                // Randomized drop positoning
                offsetX = (float)random.NextDouble() / 4;
                offsetY = (float)random.NextDouble() / 8;
                multplierX = offsetX % 2 == 2 ? 1 : -1;
                multplierY = offsetY % 2 == 2 ? 1 : -1;

                // Randomized drop
                position = new Vector3(position.x + (multplierX * offsetX), position.y + (multplierY * offsetY), position.z);
                ItemSpawnManager.instance.SpawnItem(position, drop);
            }
            Destroy(gameObject);
        }
    }
}
