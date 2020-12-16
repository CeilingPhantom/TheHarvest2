namespace TheHarvest.Events
{
    public enum EventType
    {
        // Game triggered events
        AddMoney,  // add or subtract money
        ChangeDate,  // encompasses day/month/year and season change

        // UI triggered events, events triggered by user input on some part of the interface
        BuyTile,
        SellTile,
        UpgradeTile,
        Pause,
        Resume,
    }

    public abstract class Event
    {
        public EventType type;

        // different Events will have various, different (readonly) properties
        // e.g. BuyTile EventType will have info about what tile was bought, where it was placed

        // keyboard and mouse events won't go through the Event system - use Nez Input
        // but events triggered by keyboard and mouse events can go through this system
    }
}