using System;

using TheHarvest.ECS.Entities;

namespace TheHarvest.Scenes
{
    public class Farm1Scene : BaseScene
    {
        public static readonly string Name = "farm1";
        static readonly Lazy<Farm1Scene> lazy = new Lazy<Farm1Scene>(() => new Farm1Scene());
        public static Farm1Scene Instance => lazy.Value;

        private Farm1Scene() : base()
        {}

        public override void Initialize()
        {
            base.Initialize();
            this.AddEntity(new FarmEntity(Farm1Scene.Name));
        }
    }
}
