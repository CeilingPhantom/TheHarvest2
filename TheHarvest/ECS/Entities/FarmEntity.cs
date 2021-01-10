using System;
using Nez;

using TheHarvest.ECS.Components;
using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Entities
{
    public class FarmEntity : Entity
    {
        Farm farm;
        PlayerState player;

        public FarmEntity(string name) : base(name)
        {
            this.farm = this.AddComponent(SaveFileManager.GetLoadedFarm(name) ?? new Farm(name));
            this.player = this.AddComponent(PlayerState.Instance);
        }
    }
}