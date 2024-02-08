using Data;
using Player;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Item item;
    private Controller player;
    public float distance;

    [SerializeField] float speed = 2f;
    [SerializeField] float pickUpDistance = 3.5f;
    //[SerializeField] float ttl = 10f;

    private void Awake()
    {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > pickUpDistance)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.transform.position,
            speed * Time.deltaTime
            );

        if (distance < .1f)
        {
            if (player.inventoryManager.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}