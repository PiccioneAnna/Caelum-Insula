using UnityEngine;

public class CorrectRotation : MonoBehaviour
{
    Transform parentTransform;
    SpriteRenderer spriteRenderer;
    WeaponPoint weaponPoint;

    public bool q1,q2,q3,q4;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponPoint = GetComponentInParent<WeaponPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponPoint.preserveRotation) { PreserveRotation(); }
    }

    void PreserveRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, parentTransform.rotation.z * -1);

        float z = transform.localRotation.eulerAngles.z;
        //Debug.Log(z);

        //local euler angles relative to parent, inspector is bs

        q4 = (z > 0 && z < 90);
        q3 = (z > 90 && z < 180);
        q2 = (z > 270 && z < 360);
        q1 = (z > 180 && z < 270);

        // Left
        if (q3 || q1)
        {
            spriteRenderer.flipX = false;
        }
        // Right
        else if (q2 || q4)
        {
            spriteRenderer.flipX = true;
        }
    }
}
