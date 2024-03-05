using UnityEngine;

public class MineralNode : Resource
{
    #region Fields
    [Header("Components")]
    public Sprite[] sprites;

    [Header("Behaviors")]
    public bool multiSprite = true;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        health.SetToMax();
        random = new System.Random();
        nodeType = ResourceType.Mineral;

        dropCount = random.Next(maxDropCount) + minDropCount + (int)transform.localScale.x;
        position = transform.position;
        rotation = transform.rotation;

        if (multiSprite)
        {
            HandleMultipleSprites();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
