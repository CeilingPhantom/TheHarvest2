using System.Collections.Generic;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Greenhouse1Tile : Structure
    {
        static int aoe = 1;

        List<(int x, int y)> buffedTilePositions = null;

        public Greenhouse1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime)
        : base(TileType.Greenhouse1, StructureBuffs.AllSeasons, x, y, 50, isAdvancing, advancingType, cycleTime)
        {}

        public override (int X, int Y)[] GetBuffedTilePositions(bool forceUpdate=false)
        {
            if (this.buffedTilePositions == null || forceUpdate)
            {
                this.buffedTilePositions = new List<(int x, int y)>();
                for (var i = this.X - Greenhouse1Tile.aoe; i <= this.X + Greenhouse1Tile.aoe; ++i)
                {
                    for (var j = this.Y - Greenhouse1Tile.aoe; j <= this.Y + Greenhouse1Tile.aoe; ++j)
                    {
                        this.buffedTilePositions.Add((i, j));
                    }
                }
                this.buffedTilePositions.Remove((this.X, this.Y));
            }
            return this.buffedTilePositions.ToArray();
        }
    }
}