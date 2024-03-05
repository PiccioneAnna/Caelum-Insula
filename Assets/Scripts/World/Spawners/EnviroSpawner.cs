using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnviroSpawner : MonoBehaviour
{
    [Header("Enviro Stats")]
    public int minSpawnCount;
    public int maxSpawnCount;
    public int spawnCount;

    [HideInInspector] public EnviroSpawner enviroSpawner;
    public PolygonCollider2D spawnArea;

    [Header("Spawnable Objects")]
    public GameObject[] objects;
    public List<GameObject> spawnedObjects;
    public float minScale = .5f;
    public float maxScale = 2.5f;
    public int layer = 0;

    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;

    private System.Random random;

    Bounds colliderBounds;
    Vector3 colliderCenter;
    float[] ranges;
    [HideInInspector] public List<Vector2> takenPositions;

    // Start is called before the first frame update
    void Start()
    {
        enviroSpawner = this;

        CreateBounds();

        // Random Spawn Count
        random = new System.Random();
        spawnCount = random.Next(maxSpawnCount) + minSpawnCount;
        spawnedObjects = new List<GameObject>();

        if (objects.Length <= 0 ) { return; }

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnObject(RandomObjectPrefab());
        }

        EnumHelper.SetLayerMaskAllChildren(transform, gameObject.layer);
        EnumHelper.SetSortingOrderAllChildren(spawnedObjects, layer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnObject(GameObject go, Vector3Int pos = new Vector3Int())
    {
        RandomPosition();
        RandomScale();

        position = pos != default ? (Vector3)pos : position;

        // Doesn't spawn if already object
        if (takenPositions.Contains(position))
        {
            return null;
        }

        takenPositions.Add(position);

        GameObject temp = Instantiate(go, new Vector3(position.x + .5f, position.y + .5f, 0) + transform.position, rotation, this.transform);
        temp.transform.localScale = scale;

        if(temp.TryGetComponent(out TreeNode tre))
        {
            tre.PreExistingTreeNode();
        }

        spawnedObjects.Add(temp);

        return temp;
    }

    public GameObject NewTreeObject(GameObject go, Vector3Int pos = new Vector3Int())
    {
        position = pos != default ? (Vector3)pos : position;

        // Doesn't spawn if already object
        if (takenPositions.Contains(position))
        {
            return null;
        }

        takenPositions.Add(position);

        GameObject temp = Instantiate(go, new Vector3(position.x + .5f, position.y + .5f, 0) + transform.position, rotation, this.transform);
        temp.transform.localScale = scale;

        temp.GetComponent<TreeNode>().NewTreeNode();

        spawnedObjects.Add(temp);

        return temp;
    }

    #region Helper Methods

    private void RandomScale()
    {
        float s = UnityEngine.Random.Range(minScale, maxScale);
        scale = new Vector3(s, s, 0);
    }

    private void RandomPosition()
    {
        float randomX = Random.Range(ranges[0], ranges[1]);
        float randomY = Random.Range(ranges[2], ranges[3]);

        position = new Vector2((int)randomX + .5f, (int)randomY + .5f);
    }

    // Get a random object to spawn;
    private GameObject RandomObjectPrefab()
    {
        return objects[random.Next(objects.Length)];
    }

    private bool RandomSign()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return true;
        }
        return false;
    }

    private void CreateBounds()
    {
        colliderBounds = spawnArea.bounds;
        colliderCenter = colliderBounds.center;

        ranges = new float[]{
            colliderCenter.x - colliderBounds.extents.x,
            colliderCenter.x + colliderBounds.extents.x,
            colliderCenter.y - colliderBounds.extents.y,
            colliderCenter.y + colliderBounds.extents.y,
        };

        position = transform.position;
        rotation = transform.rotation;
    }

    #endregion


}
