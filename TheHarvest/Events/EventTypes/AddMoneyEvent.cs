namespace TheHarvest.Events
{
    public class AddMoneyEvent : IEvent
    {
        private float amount;

        public AddMoneyEvent(float amount)
        {
            this.amount = amount;
        }

        public void Accept(IEventSubscriber member)
        {
            member.ProcessEvent(this);
        }
    }
}