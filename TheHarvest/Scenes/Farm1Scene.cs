using System;

using TheHarvest.ECS.Entities;

namespace TheHarvest.Scenes
{
    public class Farm1Scene : BaseScene
    {
        static readonly Lazy<Farm1Scene> lazy = new Lazy<Farm1Scene>(() => new Farm1Scene());
        public static Farm1Scene Instance => lazy.Value;

        private Farm1Scene() : base("farm1")
        {}

        public override void Initialize()
        {
            base.Initialize();
            var farmEntity = this.AddEntity(new FarmEntity(this.Name));
        }
    }
}
