using Nez;

using TheHarvest.ECS.Components;

namespace TheHarvest.ECS.Entities
{
    public class UIEntity : Entity
    {
        public UIEntity() : base("ui")
        {
            this.AddComponent<TileUI>();
        }
    }
}