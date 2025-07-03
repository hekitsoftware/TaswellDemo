using UnityEngine;


public class WeaponParent : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject weapon;
    public Transform bulletTransform;
    public AudioSource adSrc;
    public AudioClip shotSnd;

    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    public Vector2 PointerPosition { get; set; }
    public bool IsFacingRight { get; set; }

    [SerializeField] float offset;

    private void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.z = 0;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (!IsFacingRight)
        {
            rotation_z += 180f; // flip direction
        }

        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + offset);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canFire)
        {
            Fire();
        }
    }

    public void Fire()
    {
        adSrc.PlayOneShot(shotSnd);
        canFire = false;
        Instantiate(bulletPrefab, bulletTransform.position, Quaternion.identity);
    }
}
