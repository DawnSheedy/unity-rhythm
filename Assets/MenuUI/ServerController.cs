using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class ServerController : MonoBehaviour
{
    private TextField _serverField;
    private UIDocument _uiDoc;

    [Tooltip("The default host for the song server")]
    public string serverHost = "localhost:8080";

    void Start()
    {
        _uiDoc = gameObject.GetComponent<UIDocument>();

        // Setup server input
        _serverField = _uiDoc.rootVisualElement.Q<TextField>("ServerSetting");

        if (PlayerPrefs.HasKey("SongServerHost"))
        {
            serverHost = PlayerPrefs.GetString("SongServerHost");
        }

        _serverField.SetValueWithoutNotify(serverHost);
        SetHost(serverHost);

        // Setup refresh button handler
        Button button = _uiDoc.rootVisualElement.Q<Button>("FetchSongButton");
        button.RegisterCallback<ClickEvent>(HandleFetchButtonClick);
    }

    private void SetHost(string newHost)
    {
        serverHost = newHost;
        PlayerPrefs.SetString("SongServerHost", serverHost);
        gameObject.BroadcastMessage("ServerChanged");
    }

    private void HandleFetchButtonClick(ClickEvent evt)
    {
        SetHost(_serverField.value);
    }
}
