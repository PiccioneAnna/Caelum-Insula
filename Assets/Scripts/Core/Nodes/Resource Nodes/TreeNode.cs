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
    [Header("Tree Specific Components")]
    [HideInInspector] public GameObject root;
    [HideInInspector] public Collider2D col;
    public SpriteRenderer rootLeaves;
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


    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        health.SetToMax();

        root = gameObject;
        nodeType = ResourceType.Tree;
        spriteRenderer = root.GetComponent<SpriteRenderer>();
        col = root.GetComponent<Collider2D>();

        dropCount = random.Next(maxDropCount) + minDropCount + (int)transform.localScale.x;
        position = transform.position;
        rotation = transform.rotation;

        growTimer = 0;
        stageIndex = random.Next(children.Count);
        currStage = children[stageIndex];
        growTimer = currStage.time;

        PopulateRoot();
        SetSpriteLayers();

        onTimeTick += Tick;
        Init();
    }

    public void NewTreeNode()
    {
        stageIndex = 0;
        growTimer = 0;

        currStage = children[stageIndex];

        spriteRenderer.sprite = null;
        rootLeaves.sprite = null;
        rootTrunk.sprite = null;

        spriteRenderer.sortingOrder = GameManager.Instance.player.GetComponentInChildren<SpriteRenderer>().sortingOrder;

        SetSpriteLayers();
        PopulateRoot();
    }

    /// <summary>
    /// Keeps track of tree groth and subsequent behavior
    /// </summary>
    public void Tick()
    {
        growTimer++;
        health.currVal++;
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
            currStage.tree.TryGetComponent<SpriteRenderer>(out SpriteRenderer srRenderer);
            if (srRenderer != null) { spriteRenderer.sprite = srRenderer.sprite; }
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
