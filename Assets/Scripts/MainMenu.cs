using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button startButton;
    public Button exitButton;
    public Camera mainCam;
    public Camera newCam;
    public Text prologue;

    void Start () {
        startButton.GetComponent<Button>().onClick.AddListener(StartBtn);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitBtn);
    }
	
	void StartBtn () {
        StartCoroutine(Narration());
    }

    void ExitBtn () {
        Application.Quit();
    }

    IEnumerator Narration() {
        mainCam.enabled = false;
        newCam.enabled = true;
        string msg = "Everyone seems so sad, lately. \n \n ....Earth, can you help?";
        string newText = "";
        for (int i = 0; i < msg.Length; i++) {
            newText = newText + msg[i];
            prologue.text = newText;
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);

    }
}
