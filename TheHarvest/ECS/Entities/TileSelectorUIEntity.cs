using Nez;

using TheHarvest.ECS.Components.UI;

namespace TheHarvest.ECS.Entities
{
    public class TileSelectorUIEntity : Entity
    {
        public TileSelectorUIEntity() : base("ui")
        {
            this.AddComponent<TileSelectorUI>();
        }
    }
}