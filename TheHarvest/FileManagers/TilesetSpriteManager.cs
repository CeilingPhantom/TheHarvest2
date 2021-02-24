using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;

using TheHarvest.ECS.Components.Tiles;

namespace TheHarvest.FileManagers
{
    public class TilesetSpriteManager
    {
        static readonly Lazy<TilesetSpriteManager> lazy = new Lazy<TilesetSpriteManager>(() => new TilesetSpriteManager());
        public static TilesetSpriteManager Instance => lazy.Value;

        Dictionary<string, Dictionary<int, TmxTilesetTile>> tilesetTiles = new Dictionary<string, Dictionary<int, TmxTilesetTile>>();
        Dictionary<string, (string Tsx, int Id)> tileNameToTileKey = new Dictionary<string, (string Tsx, int Id)>();
        Dictionary<(string Tsx, int Id), Sprite> sprites = new Dictionary<(string Tsx, int Id), Sprite>();

        static readonly string[] tsxes = { "tiles", "other" };

        private TilesetSpriteManager()
        {}

        public void Load()
        {
            foreach (var tsx in TilesetSpriteManager.tsxes)
            {
                using (var stream = TitleContainer.OpenStream($"Content/Tiled/{tsx}.tsx"))
                {
                    var tileset = new TmxTileset().LoadTmxTileset(null, XDocument.Load(stream).Element("tileset"), 0, "Content/Tiled");
                    this.tilesetTiles[tsx] = tileset.Tiles;
                    foreach (var entry in tileset.Tiles)
                    {
                        var id = entry.Key;
                        var tilesetTile = entry.Value;
                        var key = (tsx, id);
                        // where tile type in tsx is set to the tile name if it is the one with its animation set
                        if (!String.IsNullOrWhiteSpace(tilesetTile.Type))
                        {
                            this.tileNameToTileKey[tilesetTile.Type] = key;
                        }
                        // path of tile's xnb
                        var src = Regex.Match(tilesetTile.Image.Source, @"imgs/.*(?=.png)").Value;
                        this.sprites[key] = new Sprite(Core.Content.LoadTexture(src));
                    }
                }
            }
        }

        TmxTilesetTile GetTmxTilesetTile(string tileName)
        {
            var key = this.tileNameToTileKey[tileName];
            return this.tilesetTiles[key.Tsx][key.Id];
        }

        TmxTilesetTile GetTmxTilesetTile(TileType tileType)
        {
            return GetTmxTilesetTile(Enum.GetName(typeof(TileType), tileType).ToLower());
        }

        public bool HasAnimation(string tileName)
        {
            var tilesetTile = GetTmxTilesetTile(tileName);
            return tilesetTile.AnimationFrames.Count != 0;
        }

        public bool HasAnimation(TileType tileType)
        {
            return HasAnimation(Enum.GetName(typeof(TileType), tileType).ToLower());
        }

        public SpriteAnimation GetAnimation(string tileName)
        {
            var tilesetTile = GetTmxTilesetTile(tileName);
            var key = this.tileNameToTileKey[tileName];
            // for now, assume duration of each frame in an animation is the same
            var frameRate = 1 / tilesetTile.AnimationFrames[0].Duration;
            List<Sprite> animationFrames = new List<Sprite>();
            foreach (var frame in tilesetTile.AnimationFrames)
            {
                animationFrames.Add(sprites[(key.Tsx, frame.Gid)]);
            }
            return new SpriteAnimation(animationFrames.ToArray(), frameRate);
        }

        public SpriteAnimation GetAnimation(TileType tileType)
        {
            return this.GetAnimation(Enum.GetName(typeof(TileType), tileType).ToLower());
        }

        public Sprite GetSprite(string tileName)
        {
            return this.sprites[this.tileNameToTileKey[tileName]];
        }

        public Sprite GetSprite(TileType tileType)
        {
            return GetSprite(Enum.GetName(typeof(TileType), tileType).ToLower());
        }
    }
}