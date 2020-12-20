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
            System.Diagnostics.Debug.WriteLine(name);
            System.Diagnostics.Debug.WriteLine("hello");
            var farm = this.AddComponent(SaveFileManager.GetLoadedFarm(name) ?? new Farm(name));
            var player = this.AddComponent(PlayerState.Instance);
        }
    }
}