using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Vector3 mousePos;
    public bool preserveRotation = true;
    public Vector3 worldPos;
    public GameObject mag;

    //public Projectile[] activeProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.sprite == null) { return; }

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);

        worldPos = transform.position;
    }

    public void Fire(GameObject projectile, float damage)
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<Projectile>().damage = damage;
        newProjectile.transform.parent = mag.transform;
    }
}
