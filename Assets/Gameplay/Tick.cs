public class Tick {
    public int tick { get; set; }
    public GameplayEvent[] events { get; set; }

    public Tick(int tickNumber, GameplayEvent[] events) {
        tick = tickNumber;
        this.events = events;
    }

    public override string ToString() {
        return "Tick: "+tick+" EventCount: "+events.Length;
    }
}