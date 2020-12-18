using Nez;
using Nez.Sprites;

using TheHarvest.ECS.Components;

namespace TheHarvest.ECS.Entities
{
    public class TileEntity : Entity
    {
        public TileEntity(Tile tile) : base()
        {
            this.AddComponent(tile);
            this.AddComponent<SpriteRenderer>();
        }
    }
}