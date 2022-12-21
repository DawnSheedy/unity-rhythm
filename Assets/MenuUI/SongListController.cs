using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class SongListController : MonoBehaviour
{
    [Tooltip("Template visual tree item to use as template.")]
    public VisualTreeAsset listItemTemplate;

    private VisualElement _root;
    private ListView _listView;

    private SongMeta[] songMeta;

    // Start is called before the first frame update
    void Start()
    {
        _root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        _listView = _root.Q<ListView>("SongList");
    }

    void ServerChanged()
    {
        GetSongList();
    }

    public void GetSongList()
    {
        string serverHost = PlayerPrefs.GetString("SongServerHost");
        StartCoroutine(getSongListRequest("http://" + serverHost + "/api/songs/"));
    }

    IEnumerator getSongListRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (!string.IsNullOrEmpty(uwr.error))
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            loadNewSongsFromJson(uwr.downloadHandler.text);
        }
    }

    private void loadNewSongsFromJson(string json)
    {
        SongMetaApiResponse apiRes = SongMetaApiResponse.createFromJSON(json);
        songMeta = apiRes.songs;
        InitializeCharacterList();
    }

    public void InitializeCharacterList()
    {
        FillCharacterList();

        // Register to get a callback when an item is selected
        _listView.onSelectionChange += OnCharacterSelected;
    }

    void FillCharacterList()
    {
        // Set up a make item function for a list entry
        _listView.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = listItemTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new SongListItemController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        _listView.bindItem = (item, index) =>
        {
            (item.userData as SongListItemController).SetCharacterData(songMeta[index]);
        };


        // Set the actual item's source list/array
        _listView.itemsSource = songMeta;
    }

    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        SongMeta selectedSong = _listView.selectedItem as SongMeta;

        if (selectedSong == null)
        {
            PlayerPrefs.DeleteKey("SelectedSongUuid");
        }
        else
        {
            PlayerPrefs.SetString("SelectedSongUuid", selectedSong.uuid);
        }

        gameObject.BroadcastMessage("NewSongSelected");
    }
}
