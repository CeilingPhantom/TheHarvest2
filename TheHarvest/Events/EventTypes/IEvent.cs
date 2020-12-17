namespace TheHarvest.Events
{
    /*
    Game triggered events

    AddMoney  // add or subtract money
    ChangeDate  // encompasses day/month/year and season change


    UI triggered events, events triggered by user input on some part of the interface

    BuyTile
    SellTile
    UpgradeTile
    TogglePause
    */

    // different Events will have various, different (readonly) properties
    // e.g. BuyTile Event will have info about what tile was bought, where it was placed

    // keyboard and mouse events won't go through the Event system - use Nez Input
    // but events triggered by keyboard and mouse events can go through this system

    // Events can trigger other Events
    // e.g. BuyTile (from UI to game) triggers AddMoney (UI adjusts money counter, sound makes appropriate affirmative/negative sound)
    public interface IEvent
    {
        void Accept(EventSubscriber subscriber);
    }
}