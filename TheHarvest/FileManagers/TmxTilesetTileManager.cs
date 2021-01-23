using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Nez.Tiled;

using TheHarvest.ECS.Components;

namespace TheHarvest.FileManagers
{
    public static class TmxTilesetTileManager
    {
        public static int TileSize;
        static Dictionary<int, TmxTilesetTile> tiles = new Dictionary<int, TmxTilesetTile>();
        public static Dictionary<TileType, int> tiletypeToTileId = new Dictionary<TileType, int>();

        public static void Load()
        {
            using (var stream = TitleContainer.OpenStream("Content/Tiled/tiles.tsx"))
            {
                var tileset = new TmxTileset().LoadTmxTileset(null, XDocument.Load(stream).Element("tileset"), 1, "Content/Tiled");
                TmxTilesetTileManager.TileSize = tileset.TileWidth;
                TmxTilesetTileManager.tiles = tileset.Tiles;
                foreach (var entry in tileset.Tiles)
                {
                    var id = entry.Key;
                    var tile = entry.Value;
                    // where tile type in tsx is set to the tile name if it is the one with its animation set
                    if (!String.IsNullOrWhiteSpace(tile.Type))
                    {
                        var tiletype = (TileType) Enum.Parse(typeof(TileType), tile.Type, true);
                        TmxTilesetTileManager.tiletypeToTileId[tiletype] = id;
                    }
                }
            }
        }
    }
}