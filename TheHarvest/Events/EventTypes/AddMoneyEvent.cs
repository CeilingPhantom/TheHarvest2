namespace TheHarvest.Events
{
    public class AddMoneyEvent : IEvent
    {
        public float Amount { get; private set; }

        public AddMoneyEvent(float amount)
        {
            this.Amount = amount;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}