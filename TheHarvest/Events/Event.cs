namespace TheHarvest.Events
{
    public enum EventType
    {}

    public abstract class Event
    {
        public EventType type;

        // different Events will have various, different (readonly) properties
        // e.g. KEYDOWN Event will have X, Y
    }
}