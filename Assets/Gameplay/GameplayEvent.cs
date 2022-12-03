public enum GameplayEventType { Note, Hold, Beat, Measure, End };

public class GameplayEvent {
    public GameplayEventType type { get; set; }
    public int eventMeta { get; set; }
    public GameplayEvent(GameplayEventType type, int eventMeta) {
        this.type = type;
        this.eventMeta = eventMeta;
    }

    public static GameplayEvent[] createFromRangeOfRawData(ref RawNoteData[] data, int start, int end) {
        GameplayEvent[] items = new GameplayEvent[end-start+1];

        int localIndex = 0;
        for (int i=start; i<=end; i++) {
            items[localIndex] = new GameplayEvent((GameplayEventType)data[i].type, data[i].detail);
            localIndex++;
        }

        return items;
    }
}