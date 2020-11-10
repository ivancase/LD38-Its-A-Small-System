using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject planet;
    public Camera cam;
    public float speed = 1;
    private bool facingRight = false;

    private Rigidbody2D me;
    private Animator anim;
    private SpriteRenderer sprite;

    private Vector3 newPos;
    private float newScale;

    private bool moveCam = false;
    private float startTime;
    private float journeyLength;

    void Start () {
        me = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	void FixedUpdate () {

        float gravity = planet.GetComponent<Orbit>().gravityScale;

        //Gravity!
        me.AddForce((planet.transform.position - transform.position) * gravity);

        //Movement!
        float hori = Input.GetAxis("Horizontal");

        if (hori > 0 && !facingRight || hori < 0 && facingRight) {
            sprite.flipX = !sprite.flipX;
            facingRight = !facingRight;
            anim.SetBool("isWalking", true);
        }
        else if (hori == 0) {
            anim.SetBool("isWalking", false);
        }

        transform.RotateAround(planet.transform.position, -Vector3.forward, hori * speed / gravity);


    }

    void Update() {
        //Move that camera!
        if (moveCam == true) {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;

            cam.transform.position = Vector3.Lerp(cam.transform.position, newPos, fracJourney);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newScale, fracJourney);
            cam.transform.SetParent(planet.transform);

            if (cam.transform.position == newPos) {
                moveCam = false;
            }
        }

        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }

       if (Input.GetKeyDown("r")) { // hot patch for falling over bug
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(transform.localPosition.y, transform.localPosition.x) * (180 / Mathf.PI) - 90);
            Debug.Log(Mathf.Atan2(transform.localPosition.y, transform.localPosition.x) * 180/Mathf.PI - 90);
        } 
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Planet" && other.gameObject != planet) {
            sprite.flipY = !sprite.flipY;
            sprite.flipX = !sprite.flipX;

            planet.GetComponent<Orbit>().playerOn = false;

            planet = other.gameObject;
            planet.GetComponent<Orbit>().playerOn = true;
            transform.SetParent(planet.transform);

            startTime = Time.time;
            newPos = new Vector3(planet.transform.position.x, planet.transform.position.y, -10);
            newScale = planet.GetComponent<Orbit>().camSize;
            journeyLength = Vector3.Distance(cam.transform.position, newPos);
            moveCam = true;
        }
    }
}
