using System;

namespace TheHarvest.Scenes
{
    public class FarmScene : BaseScene
    {
        static readonly Lazy<FarmScene> lazy = new Lazy<FarmScene>(() => new FarmScene());
        public static FarmScene Instance => lazy.Value;

        private FarmScene() : base() { }
    }
}
