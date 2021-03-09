using System;
using System.Collections.Generic;

namespace TheHarvest.ECS.Components.Tiles
{
    public abstract class Structure : Tile
    {
        [Flags]
        public enum StructureBuffs
        {
            AllSeasons = 1 << 0,
            Shed = 1 << 1,
            Silo = 1 << 2,
        }

        public static int NStructureBuffs = Enum.GetNames(typeof(StructureBuffs)).Length;
        public readonly StructureBuffs Buffs;
        public readonly List<StructureBuffs> BuffsList = new List<Structure.StructureBuffs>();

        public Structure(TileType tileType, StructureBuffs buffs, int x, int y, int cost=0, bool isAdvancing=false, TileType advancingType=TileType.Dirt, float cycleTime=0)
        : base(tileType, x, y, cost, isAdvancing, advancingType, cycleTime)
        {
            this.Buffs = buffs;
            for (var i = 0; i < Structure.NStructureBuffs; ++i)
            {
                var buff = (Structure.StructureBuffs) (1 << i);
                if (this.Buffs.HasFlag(buff))
                {
                    this.BuffsList.Add(buff);
                }
            }
        }

        public abstract (int X, int Y)[] GetBuffedTilePositions(bool forceUpdate=false);
    }
}