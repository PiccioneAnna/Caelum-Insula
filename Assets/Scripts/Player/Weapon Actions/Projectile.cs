using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private Collider2D coli;

    public float force = 10f;
    public float ttl = 25f;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        coli = GetComponent<Collider2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        rb.velocity = (Vector2)direction.normalized * force;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTime();
    }

    private void CheckTime()
    {
        if (ttl > 0) { ttl -= Time.deltaTime; }

        if (ttl <= 0) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("Collision detected on projectile");

        Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, coli.bounds.size, 0f);

        foreach (Collider2D c in targets)
        {
            if (c.TryGetComponent<Damageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
                break;
            }

            Damageable damageable1 = c.gameObject.GetComponentInParent<Damageable>();

            if (damageable1 != null)
            {
                damageable1.TakeDamage(damage);
                break;
            }
        }
    }
}
