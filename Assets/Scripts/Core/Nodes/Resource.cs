using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class Resource : MonoBehaviour
{
    #region Fields
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer shadowRenderer;
    public Sprite[] sprites;
    public StatusBar healthBar;
    public TMP_Text healthValueText;

    [Header("Resource Stats")]
    [HideInInspector] public Resource instance;
    public Stat health;
    public Item[] droppedObjs;
    public bool matchLayerNum = true;
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

    [Header("Behaviors")]
    public bool multiSprite = false;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        health.SetToMax();
        random = new System.Random();

        if (multiSprite)
        {
            HandleMultipleSprites();
        }

        dropCount = random.Next(maxDropCount) + minDropCount + (int)transform.localScale.x;
        position = transform.position;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // idk if i need this rn but ill keep it for later
    }

    public void HandleMultipleSprites()
    {
        if (spriteRenderer != null && sprites.Length > 0)
        {
            int i = random.Next(sprites.Length);

            spriteRenderer.sprite = sprites[i];
            //shadowRenderer.sprite = spriteRenderer.sprite;
        }
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
            Destroy(gameObject);
        }
    }
}
