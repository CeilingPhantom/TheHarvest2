using Nez;

using TheHarvest.ECS.Components;

namespace TheHarvest.ECS.Entities
{
    public class PlayerCameraEntity : Entity
    {
        public PlayerCameraEntity() : base("PlayerCamera")
        {
            this.AddComponent<PlayerCamera>();
        }
    }
}