using UnityEngine.UIElements;

public class SongListItemController
{
    Label _titleLabel;
    Label _artistLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        _titleLabel = visualElement.Q<Label>("SongTitle");
        _artistLabel = visualElement.Q<Label>("SongArtist");
    }

    public void SetCharacterData(SongMeta songMeta)
    {
        _titleLabel.text = songMeta.title;
        _artistLabel.text = songMeta.artist;
    }
}
