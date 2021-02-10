namespace TheHarvest.Events
{
    public class AddMoneyEvent : IEvent
    {
        public int Amount { get; }

        public AddMoneyEvent(int amount)
        {
            this.Amount = amount;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}