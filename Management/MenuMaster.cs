using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMaster : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public InputField inputField;

    void Start()
    {
        PlayerPrefs.SetFloat("poopy", 1);
        Debug.Log(PlayerPrefs.GetFloat("poopy"));
        button.onClick.AddListener(Clicked);

    }

    // Update is called once per frame
    void Update()
    {
        buttonText.text = inputField.text;
    }

    void Clicked()
    {
        button.GetComponent<Image>().color = Color.blue;
        buttonText.text = "clicked";
        PlayerPrefs.SetFloat("poopy", 2);
        Debug.Log(PlayerPrefs.GetFloat("poopy"));
    }
}
