using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public GameObject obj;

    public void SpawnItem(Vector3 position, Data.Item item, int count = 1)
    {
        GameObject go = Instantiate(obj, position, Quaternion.identity);
        go.GetComponent<Drop>().item = item;
        go.GetComponent<SpriteRenderer>().sprite = item.image;
    }
}
