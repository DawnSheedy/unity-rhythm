public class Tick {
    public int tick { get; set; }
    private GameplayEvent[] _events;

    public Tick(int tickNumber, GameplayEvent[] events) {
        tick = tickNumber;
        _events = events;
    }

    public GameplayEvent[] getEvents() {
        return _events;
    }

    public override string ToString() {
        return "Tick: "+tick+" EventCount: "+_events.Length;
    }
}