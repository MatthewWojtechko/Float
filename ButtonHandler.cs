using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public Button playButton;
    public Button creditsButton;
    public GameObject logo;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(playClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playClick()
    {
        // start the game!
        Messenger.Broadcast(GameEvent.OPENING);
        Messenger.Broadcast(GameEvent.START_LEVEL_ONE);
        logo.SetActive(false);
        playButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
    }
}
