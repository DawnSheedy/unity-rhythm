using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsMenuController : MonoBehaviour
{
    [Tooltip("Setting keys")]
    public string[] settingKeys = { };

    private class SettingController
    {
        private Label _label;
        private Button _button;
        private string _key;

        public SettingController(Label label, Button button, string settingKey)
        {
            _label = label;
            _button = button;
            _key = settingKey;

            if (!PlayerPrefs.HasKey(settingKey))
            {
                PlayerPrefs.SetInt(settingKey, 0);
            }

            _button.RegisterCallback<ClickEvent>(HandleClick);

            UpdateLabel();
        }

        void HandleClick(ClickEvent evt)
        {
            if (PlayerPrefs.GetInt(_key) == 0)
            {
                PlayerPrefs.SetInt(_key, 1);
            } else
            {
                PlayerPrefs.SetInt(_key, 0);
            }
            UpdateLabel();
        }

        void UpdateLabel()
        {
            _label.text = PlayerPrefs.GetInt(_key) == 0 ? "Off" : "On";
        }
    }

    void Start()
    {
        VisualElement root = gameObject.GetComponent<UIDocument>().rootVisualElement;

        for (int i=0; i<settingKeys.Length; i++)
        {
            string key = settingKeys[i];
            new SettingController(root.Q<Label>(key+"State"), root.Q<Button>(key + "Toggle"), key);
        }

        Button exitButton = root.Q<Button>("SongSelectButton");
        exitButton.RegisterCallback<ClickEvent>(BackToSongSelect);
    }

    void BackToSongSelect(ClickEvent evt)
    {
        SceneManager.LoadScene("SongSelectScene");
    }
}

