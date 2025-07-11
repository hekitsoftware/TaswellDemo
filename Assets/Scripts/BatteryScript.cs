using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class BatteryScript : MonoBehaviour
{
    [SerializeField] public GameObject _player;
    [SerializeField] public GameObject _battery;
    [SerializeField] public GameObject _sceneManagerObject;

    public CircleCollider2D _circleCollider;
    public SceneManager2 _sManager;
    public Animator anim;
    public AudioSource chuggington; //Generator Sound

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
        _sManager = _sceneManagerObject.GetComponent<SceneManager2>();
        anim = _battery.GetComponent<Animator>();
        chuggington = _battery.GetComponent<AudioSource>();

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
                    Charge();
                }
            }
        }
    }

    public void Charge() {
        isDead = false;
        isCharging = true;
        anim.SetBool("C_Started", true);
        chuggington.Play();
        StartCoroutine(CountdownCoroutine(countdownTime));
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

        anim.SetBool("C_Started", false);
        anim.SetBool("C_Finished", true);
        isCharging = false;
        isActive = true;
    }
}
