using Nez;

using TheHarvest.ECS.Components.Farm;
using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Entities
{
    public class FarmEntity : Entity
    {
        public FarmEntity() : base()
        {
            var farm = this.AddComponent(SaveFileManager.Instance.LoadedFarm);
            this.AddComponent(new FarmDefaultTiler(farm));
            this.AddComponent<TileHighlighter>();
            this.AddComponent(new TentativeFarmGrid(farm));
        }
    }
}