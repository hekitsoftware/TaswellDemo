using System.Threading;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2 (direction.x, direction.y).normalized * force;

        Vector3 rotation = transform.position - player.transform.position;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90f);
    }

    private void Update()
    {
        float timer = 0;

        timer += Time.deltaTime;

        if(timer > 5) { Destroy(gameObject); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else { Destroy(gameObject); }
    }
}
