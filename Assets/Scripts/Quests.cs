using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quests : MonoBehaviour {

    /// yeah... this is a mess. i could have organized this a lot better, and there were wayyyy more efficient
    /// ways to do what i was doing, but oh well. it's cobbled together, but it works enough. optimization is
    /// unthinkable, however.

    public Canvas can;
    public Text msg;
    public Text ending;
    public Camera mainCam;
    public Camera newCam;
    private List<string> inventory = new List<string>();
    private CanvasGroup canvas;

    private bool paused;

    private int questsComplete;
    //The four quests:
    private int mercStep = 0; //Get Mercury some ICE from Uranus.
    private int marsVenuStep = 0; //Get Venus a FLOWER from Mars.
    private int satuJupiStep = 0; //Get Jupiter a RING from Saturn.
    private int neptAnusStep = 0; //Give Neptune a hug, and press a BUTTON on his planet to flip Uranus.

    //The changed sprites:

    public Sprite mercHap;
    public Sprite venuHap;
    public Sprite jupiHap;
    public Sprite satuHap;
    public Sprite anusHap;
    public Sprite neptHap;

    public Sprite buttDown;
    public Sprite mercCold;

    void Start() {
        canvas = can.GetComponent<CanvasGroup>();
    }
    void Update () {
        if (paused) {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) {
                canvas.alpha = 0;
                Time.timeScale = 1;
                paused = false;
            }
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                CastRay();
            }

            if (questsComplete == 4) {
                questsComplete += 1;
                StartCoroutine(Ending("Earth, thank you for your kindness. 1You've made this system a happier place. \n \n ....It's a small system, after all."));
            }
        }

    }

    void CastRay() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit) {
            GameObject target = hit.collider.gameObject;
            if (target.tag == "Key") { //Quest Items.
                inventory.Add(target.name);
                Dialogue("You take the " + target.name + ".");
                Destroy(target);
            }

            if(target.name == "button") { //The button on Neptune.
                if (neptAnusStep == 2) {
                    neptAnusStep = 3;
                    questsComplete += 1;

                    Quaternion uranus  = GameObject.Find("Uranus").transform.rotation;
                    Quaternion newRot = uranus;
                    newRot.z += 90;
                    GameObject.Find("Uranus").transform.rotation = newRot;

                    target.GetComponent<SpriteRenderer>().sprite = buttDown;
                    target.name = "disabled";
                    Dialogue("You push the button, and hear a 'whirrrrr'.");
                    //Add whirrr sound effect here.
                }
                else {
                    Dialogue("You don't know what that button does!");
                }
            }

            //Quest givers: (there HAS to be something more efficient than this, but ¯\_(ツ)_/¯)

            if (target.tag == "Mercury") {

                if (mercStep == 0) {

                    if (inventory.Contains("ice cubes")) {
                        Dialogue("'Ooh, cool! Thank you!'");
                        target.GetComponent<SpriteRenderer>().sprite = mercHap;
                        GameObject.Find("Mercury").GetComponent<SpriteRenderer>().sprite = mercCold;
                        GameObject.Find("MercuryIce").GetComponent<SpriteRenderer>().enabled = true;
                        mercStep = 1;
                        questsComplete += 1;
                    }
                    else {
                        Dialogue("'I wish it were cold on both sides...'");
                    }
                }
                else {
                    Dialogue("'Thanks again for the ice!'");
                }

            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (target.tag == "Venus") {

                if (marsVenuStep == 0) {
                    Dialogue("Venus is sad, and doesn't want to talk.");
                }
                else if (marsVenuStep == 1) {
                    target.GetComponent<Animator>().SetTrigger("stopCrying");
                    target.GetComponent<SpriteRenderer>().sprite = venuHap;
                    Dialogue("'From Mars? Thank you so much!'");
                    marsVenuStep = 2;
                    questsComplete += 1;
                }
                else {
                    Dialogue("'I love it so much! Mars is the best!'");
                }

            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (target.tag == "Mars") {

                if (marsVenuStep == 0) {
                    List<string> list = new List<string>();
                    list.Add("'Can I ask you a favor?'");
                    list.Add("'Bring this flower to Venus for me, please.'");
                    list.Add("He hands you a beautiful red flower.");
                    StartCoroutine(MultiDialogue(list));
                    marsVenuStep = 1;
                }
                else if (marsVenuStep == 1) {
                    Dialogue("'Please bring that flower to Venus!'");
                }
                else if (marsVenuStep == 2) {
                    Dialogue("'Thank you so much! I'm glad she liked it.'");
                }

            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (target.tag == "Jupiter") {

                if (satuJupiStep == 0) {

                    if (inventory.Contains("ring")) {
                        GameObject ring = GameObject.Find("JupiterRing");
                        ring.GetComponent<SpriteRenderer>().enabled = true;
                        Dialogue("'So regal! I love it, thank you!'");
                        target.GetComponent<SpriteRenderer>().sprite = jupiHap;
                        satuJupiStep = 1;
                        questsComplete += 1;
                    }

                    else {
                        Dialogue("'How can I be King, with no jewelry?'");
                    }
                }
                else {
                    Dialogue("'This ring is beautiful!'");
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (target.tag == "Saturn") {

                if (satuJupiStep == 0) {

                    if (inventory.Contains("ring")) {
                        Dialogue("'Thanks for taking that off my hands, Earth.'");
                        target.GetComponent<SpriteRenderer>().sprite = satuHap;
                    }

                    else {
                        List<string> list = new List<string>();
                        list.Add("'Ugh, this RING is such an eyesore...'");
                        list.Add("'I wish somebody would take it elsewhere.'");
                        StartCoroutine(MultiDialogue(list));
                    }

                }
                else {
                    Dialogue("'Oh, Jupiter wanted it? Lovely!'");
                }

            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (target.tag == "Uranus") {

                if (neptAnusStep == 0 || neptAnusStep == 1) {
                    List<string> list = new List<string>();
                    list.Add("'Neptune flipped my planet on its side!'");
                    list.Add("'What a jerk!'");
                    StartCoroutine(MultiDialogue(list));
                    neptAnusStep = 1;
                }
                else if (neptAnusStep == 3) {
                    target.GetComponent<SpriteRenderer>().sprite = anusHap;
                    Dialogue("'Wow, it's as good as new! Thank you!'");
                }
                else {
                    Dialogue("Uranus is angry.");
                }

            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (target.tag == "Neptune") {
                if (neptAnusStep == 0) {
                    Dialogue("'Bleh! Go away!'");
                }
                else if (neptAnusStep == 1) {
                    List<string> list = new List<string>();
                    list.Add("'You want me to flip Uranus back over?'");
                    list.Add("'I'm sorry... It's just so lonely out here'");
                    list.Add("You hug Neptune.");
                    target.GetComponent<SpriteRenderer>().sprite = neptHap;
                    list.Add("'That button over there will fix it.'");
                    StartCoroutine(MultiDialogue(list));
                    neptAnusStep = 2;
                }
                else {
                    Dialogue("Sorry for being such a jerk...");
                }
            }

        }
    }

    void Dialogue(string newText) {
        canvas.alpha = 1;
        msg.text = newText;
        Time.timeScale = 0;

        paused = true;
    }

    IEnumerator MultiDialogue(List<string> messages) {
        for (int i = 0; i <= messages.Count - 1; i++) {
            Dialogue(messages[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Ending(string message) {
        yield return new WaitForSeconds(2);
        mainCam.enabled = false;
        newCam.enabled = true;
        string newText = "";
        for (int i = 0; i < message.Length; i++) {
            if (message[i] == '1') {
                newText = "";
                yield return new WaitForSeconds(1);
            }
            else {
                newText = newText + message[i];
                ending.text = newText;
                yield return new WaitForSeconds(0.15f);
            }
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
