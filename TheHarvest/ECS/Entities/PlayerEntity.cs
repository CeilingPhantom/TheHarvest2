using Nez;

using TheHarvest.ECS.Components.Player;

namespace TheHarvest.ECS.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity() : base("Player")
        {
            this.AddComponent(PlayerState.Instance);
            this.AddComponent(PlayerCamera.Instance);
        }
    }
}