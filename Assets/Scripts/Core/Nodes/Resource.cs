using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class Resource : TimeAgent
{
    #region Fields
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer shadowRenderer;
    public StatusBar healthBar;
    public TMP_Text healthValueText;

    [Header("Resource Stats")]
    public Stat health;
    [HideInInspector] public bool matchLayerNum = true;
    [HideInInspector] public ResourceType nodeType;

    // Randomized drops
    [Header("Drops")]
    public Item[] droppedObjs;
    protected Item drop;
    protected System.Random random;
    protected Vector3 position;
    protected Quaternion rotation;
    protected float offsetX, offsetY;
    protected int multplierX, multplierY;
    public int maxDropCount, minDropCount;
    protected int dropCount;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
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

    public void UpdateHealthBar()
    {
        if(healthBar == null || healthValueText == null) { return; }

        bool vis = health.currVal < health.maxVal;

        healthBar.gameObject.SetActive(vis);
        healthValueText.gameObject.SetActive(vis);

        healthValueText.text = health.currVal.ToString() + "/" + health.maxVal.ToString();

        healthBar.Set(health.currVal, health.maxVal);
    }

    public void Shake()
    {
        Debug.Log("Shake object");
        animator.SetTrigger("Shake");
    }

    public void Hit()
    {
        health.currVal--;
        UpdateHealthBar();
        Shake();

        if (health.currVal <= 0)
        {
            Debug.Log("Drop Count:" + dropCount);

            for (int i = 0; i < dropCount; i++)
            {
                drop = droppedObjs[random.Next(droppedObjs.Length)];

                // Randomized drop positoning
                offsetX = (float)random.NextDouble() / 2;
                offsetY = (float)random.NextDouble() / 4;
                multplierX = offsetX % 2 == 2 ? 1 : -1;
                multplierY = offsetY % 2 == 2 ? 1 : -1;

                // Randomized drop
                position = new Vector3(position.x + (multplierX * offsetX), position.y + (multplierY * offsetY), position.z);
                ItemSpawnManager.instance.SpawnItem(position, drop);
            }

            GameManager.Instance.FindEnviroSpawner();
            if (GameManager.Instance.enviroSpawner.spawnedObjects.Contains(gameObject))
            {
                GameManager.Instance.enviroSpawner.spawnedObjects.Remove(gameObject);
            }

            Destroy(gameObject);
        }
    }
}
