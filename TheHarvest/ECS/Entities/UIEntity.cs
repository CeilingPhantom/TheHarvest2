using Nez;

using TheHarvest.ECS.Components.UI;

namespace TheHarvest.ECS.Entities
{
    public class UIEntity : Entity
    {
        public UIEntity() : base("ui")
        {
            this.AddComponent<PlayerStateUI>();
            this.AddComponent<TileSelectorUI>();
        }
    }
}