using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;

using TheHarvest.ECS.Components;

namespace TheHarvest.FileManagers
{
    public class TilesetSpriteManager
    {
        static readonly Lazy<TilesetSpriteManager> lazy = new Lazy<TilesetSpriteManager>(() => new TilesetSpriteManager());
        public static TilesetSpriteManager Instance => lazy.Value;

        private TilesetSpriteManager()
        {}

        public int TileSpriteSize { get; private set; } = 0;
        Dictionary<int, TmxTilesetTile> tilesetTiles = new Dictionary<int, TmxTilesetTile>();
        Dictionary<TileType, int> tiletypeToTileId = new Dictionary<TileType, int>();
        Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>();

        public void Load()
        {
            using (var stream = TitleContainer.OpenStream("Content/Tiled/tiles.tsx"))
            {
                var tileset = new TmxTileset().LoadTmxTileset(null, XDocument.Load(stream).Element("tileset"), 0, "Content/Tiled");
                this.TileSpriteSize = tileset.TileWidth;
                this.tilesetTiles = tileset.Tiles;
                foreach (var entry in tileset.Tiles)
                {
                    var id = entry.Key;
                    var tilesetTile = entry.Value;
                    // where tile type in tsx is set to the tile name if it is the one with its animation set
                    if (!String.IsNullOrWhiteSpace(tilesetTile.Type))
                    {
                        var tiletype = (TileType) Enum.Parse(typeof(TileType), tilesetTile.Type, true);
                        this.tiletypeToTileId[tiletype] = id;
                    }
                    // path of tile's xnb
                    var src = Regex.Match(tilesetTile.Image.Source, @"imgs/tiles/.*[^(.png)]").Value;
                    this.sprites[id] = new Sprite(Core.Content.LoadTexture(src));
                }
            }
        }

        TmxTilesetTile GetTmxTilesetTile(TileType tileType)
        {
            return this.tilesetTiles[this.tiletypeToTileId[tileType]];
        }

        public bool HasAnimation(TileType tileType)
        {
            var tilesetTile = GetTmxTilesetTile(tileType);
            return tilesetTile.AnimationFrames.Count != 0;
        }

        public SpriteAnimation GetAnimation(TileType tileType)
        {
            var tilesetTile = GetTmxTilesetTile(tileType);
            // for now, assume duration of each frame in an animation is the same
            var frameRate = 1 / tilesetTile.AnimationFrames[0].Duration;
            List<Sprite> animationFrames = new List<Sprite>();
            foreach (var frame in tilesetTile.AnimationFrames)
            {
                animationFrames.Add(sprites[frame.Gid]);
            }
            return new SpriteAnimation(animationFrames.ToArray(), frameRate);
        }

        public Sprite GetSprite(TileType tileType)
        {
            return this.sprites[this.tiletypeToTileId[tileType]];
        }
    }
}