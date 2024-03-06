using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GrowthStage
{
    public GameObject tree;
    public GameObject trunk;
    public GameObject leaves;
    public int time;
}

public class TreeNode : Resource
{
    #region Fields

    [HideInInspector] public TreeNode instance;

    [Header("Tree Specific Components")]
    [HideInInspector] public GameObject root;
    [HideInInspector] public Collider2D col;
    public SpriteRenderer rootLeaves;
    public SpriteRenderer rootTree;
    public SpriteRenderer rootTrunk;
    public List<GrowthStage> children;
    private GrowthStage currStage;

    [Header("Growth Tracking")]
    public int stageIndex;
    public int growTimer;

    [Header("Tree States/Behaviors")]
    public bool fullGrown = false;
    public bool canFruit = false;

    #endregion

    private void Awake()
    {
        if (instance == null) // If there is no instance already
        {
            instance = this;
        }
        else if (instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }
    }

    // Start is called before the first frame update
    private void GetComponents()
    {
        random = new System.Random();

        health.SetToMax();

        root = gameObject;
        nodeType = ResourceType.Tree;
        col = root.GetComponent<Collider2D>();

        dropCount = random.Next(maxDropCount) + minDropCount + (int)transform.localScale.x;
        position = transform.position;
        rotation = transform.rotation;
    }

    public void PreExistingTreeNode()
    {
        GetComponents();

        growTimer = 0;
        stageIndex = random.Next(children.Count);
        currStage = children[stageIndex];
        growTimer = currStage.time;

        if(GameManager.Instance.enviroSpawner == null) { GameManager.Instance.FindEnviroSpawner(); }

        spriteRenderer.sortingOrder = GameManager.Instance.enviroSpawner.layer;

        PopulateRoot();
        SetSpriteLayers();

        onTimeTick += Tick;
        Init();
    }

    public void NewTreeNode()
    {
        GetComponents();

        stageIndex = 0;
        growTimer = 0;

        fullGrown = false;

        currStage = children[stageIndex];

        spriteRenderer.sprite = null;
        rootLeaves.sprite = null;
        rootTrunk.sprite = null;

        spriteRenderer.sortingOrder = GameManager.Instance.player.GetComponentInChildren<SpriteRenderer>().sortingOrder;

        PopulateRoot();
        SetSpriteLayers();

        onTimeTick += Tick;
        Init();
    }

    /// <summary>
    /// Keeps track of tree groth and subsequent behavior
    /// </summary>
    public void Tick()
    {
        growTimer++;

        if(health.currVal < health.maxVal)
        {
            health.currVal++;
        }

        UpdateHealthBar();

        if(!fullGrown && growTimer >= children[stageIndex].time)
        {
            if(stageIndex < children.Count)
            {
                currStage = children[stageIndex];
                PopulateRoot();
                stageIndex++;

                health.SetToMax();
                UpdateHealthBar();

                if (stageIndex >= children.Count)
                {
                    fullGrown = true;
                }
            }
        }
    }

    public void SetSpriteLayers()
    {
        rootLeaves.sortingOrder = spriteRenderer.sortingOrder + 1;
        rootTrunk.sortingOrder = spriteRenderer.sortingOrder - 1;

        healthBar.gameObject.GetComponentInParent<Canvas>().sortingOrder = spriteRenderer.sortingOrder + 1;
    }

    private void PopulateRoot()
    {
        if(currStage.trunk != null)
        {
            rootTrunk.gameObject.SetActive(true);
            currStage.trunk.TryGetComponent<SpriteRenderer>(out SpriteRenderer srTrunk);
            if (srTrunk != null) 
            { 
                rootTrunk.sprite = srTrunk.sprite;
                rootTrunk.transform.position = currStage.trunk.transform.position;
            }
        }
        else
        {
            rootTrunk.sprite = null;
            rootTrunk.gameObject.SetActive(false);
        }

        if (currStage.tree != null)
        {
            currStage.tree.TryGetComponent<SpriteRenderer>(out SpriteRenderer srTree);
            if (srTree != null) 
            {
                rootTree.sprite = srTree.sprite;
                rootTree.transform.position = currStage.tree.transform.position;
            }
        }

        if (currStage.leaves != null)
        {
            currStage.leaves.TryGetComponent<SpriteRenderer>(out SpriteRenderer srLeaves);
            if (srLeaves != null) 
            { 
                rootLeaves.sprite = srLeaves.sprite;
                rootLeaves.transform.position = currStage.leaves.transform.position;
            }
        }
    }
}
