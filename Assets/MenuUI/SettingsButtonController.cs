using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsButtonController : MonoBehaviour
{
    void Start()
    {
        VisualElement root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        Button exitButton = root.Q<Button>("SettingsButton");
        exitButton.RegisterCallback<ClickEvent>(GoToSettings);
    }

    void GoToSettings(ClickEvent evt)
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
