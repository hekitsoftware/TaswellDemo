using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera cam;
    private Rigidbody2D rb;
    public float Speed;
    public Collider2D Collider;

    public ItemManager ItemManager;
    public double dmgMulti;

    private void Start()
    {
        Collider = GetComponent<Collider2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.linearVelocity = new Vector2 (direction.x, direction.y).normalized * Speed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        //make sure the bullet is behind the gun
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);

        StartCoroutine(SelfDistruct());
    }

    private void Update()
    {
        dmgMulti = ItemManager.dmgMulti;
    }

    public int count = 0;

    IEnumerator SelfDistruct()
    {
        while (count < 1) // countdown for 1 sec
        {
            yield return new WaitForSeconds(1);
            count++;
        }
        Debug.Log("Counting finished!");

        Destroy(gameObject);
    }

    public double damage = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Drone"))
        {
            collision.gameObject.GetComponent<HealthScript>().currentHealth -= (damage += dmgMulti);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground")) { Destroy(gameObject); }
    }
}
