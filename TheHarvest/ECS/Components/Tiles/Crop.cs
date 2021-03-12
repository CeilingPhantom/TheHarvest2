using TheHarvest.ECS.Components.Farm;
using TheHarvest.ECS.Components.Player;
using TheHarvest.Events;

namespace TheHarvest.ECS.Components.Tiles
{
    public abstract class Crop : Tile
    {
        public PlayerState.Seasons Seasons;

        public Crop(TileType tileType, int x, int y, Grid grid, int cost, PlayerState.Seasons seasons, bool isAdvancing, TileType advancingType, float cycleTime)
        : base(tileType, x, y, grid, cost, isAdvancing, advancingType, cycleTime)
        {
            this.Seasons = seasons;
        }

        public override void Update()
        {
            var cyclePassed = this.UpdateCycleTime();
            if (this.IsAdvancing && cyclePassed)
            {
                this.AdvanceTile();
                return;
            }

            if (cyclePassed)
            {
                EventManager.Instance.Publish(new AddMoneyEvent(this.GetYield()));
            }
        }

        protected abstract int GetYield();

        public override bool IsPlaceable()
        {
            return (PlayerState.Instance.Season & this.Seasons) != 0 || (this.Grid.GetTileBuffs(this.X, this.Y) & Structure.StructureBuffs.AllSeasons) != 0;
        }
    }
}