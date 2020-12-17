namespace TheHarvest.Events
{
    public class AddMoneyEvent : IEvent
    {
        private float amount;

        public AddMoneyEvent(float amount)
        {
            this.amount = amount;
        }

        public void Accept(EventSubscriber member)
        {
            member.ProcessEvent(this);
        }
    }
}