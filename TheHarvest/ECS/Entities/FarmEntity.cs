using System;
using Nez;

using TheHarvest.ECS.Components;
using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Entities
{
    public class FarmEntity : Entity
    {
        public FarmEntity(string name) : base(name)
        {
            this.AddComponent(SaveFileManager.GetLoadedFarm(name) ?? new Farm(name));
            this.AddComponent(PlayerState.Instance);
        }
    }
}