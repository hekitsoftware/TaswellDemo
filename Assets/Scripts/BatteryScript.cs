using UnityEngine;
using System.Collections;

public class BatteryScript : MonoBehaviour
{
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _battery;
    [SerializeField] public GameObject _sceneManagerObject;

    public CircleCollider2D _circleCollider;
    public SceneManager _sManager;
    public Animator anim;

    //Timer Stuff
    public float countdownTime = 60f; // seconds
    private float currentTime;

    public bool isDead;
    public bool isCharging;
    public bool isActive;

    private void Awake()
    {
        //Init Variables
        _circleCollider = _battery.GetComponent<CircleCollider2D>();
        _sManager = _sceneManagerObject.GetComponent<SceneManager>();

        isDead = true;
        isCharging = false;
        isActive = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_circleCollider != null)
        {
            if(collision.rigidbody.tag == "Player")
            {
                if ((isDead && !isCharging && !isActive) && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Attepted to Activate BATTERYS");
                    Charge();
                }
            }
        }
    }

    public void Charge() {
        isDead = false;
        isCharging = true;
        anim.SetBool("C_Started", true);
        StartCoroutine(CountdownCoroutine(60f));
    }
    //Timer

    IEnumerator CountdownCoroutine(float duration)
    {
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            Debug.Log("Time left: " + Mathf.CeilToInt(timeLeft));

            yield return null; // wait one frame
            timeLeft -= Time.deltaTime;
        }

        Debug.Log("Time's up!");

        anim.SetBool("C_Started", false);
        anim.SetBool("C_Finished", true);
        isCharging = false;
        isActive = true;
    }
}
