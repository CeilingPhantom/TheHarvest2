using Nez;

using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Entities
{
    public class FarmEntity : Entity
    {
        public FarmEntity() : base()
        {
            this.AddComponent(SaveFileManager.LoadedFarm);
        }
    }
}