using Microsoft.Xna.Framework;
using Nez.Textures;
using Microsoft.Xna.Framework.Graphics;

namespace Nez.UI
{
	/// <summary>
	/// TiledDrawable where source sprite can be scaled
	/// </summary>
	public class ScaledTiledDrawable : TiledDrawable
	{
        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;

		public ScaledTiledDrawable(Sprite sprite) : base(sprite)
		{ }


		public ScaledTiledDrawable(Texture2D texture) : base(texture)
		{ }

        public void SetScale(float scale) {
            ScaleX = ScaleY = scale;
        }

		public override void Draw(Batcher batcher, float x, float y, float width, float height, Color color)
		{
			float regionWidth = Sprite.SourceRect.Width * ScaleX, regionHeight = Sprite.SourceRect.Height * ScaleY;
			int fullX = (int) (width / regionWidth), fullY = (int) (height / regionHeight);
			float remainingSourceX = (width - regionWidth * fullX) / ScaleX, remainingSourceY = (height - regionHeight * fullY) / ScaleY;
			float startX = x, startY = y;

			// draw all full, unclipped first
			for (var i = 0; i < fullX; i++)
			{
				y = startY;
				for (var j = 0; j < fullY; j++)
				{
					batcher.Draw(Sprite, new Rectangle((int) x, (int) y, 
                                 (int) regionWidth, (int) regionHeight), 
                                 Sprite.SourceRect, color);
					y += regionHeight;
				}

				x += regionWidth;
			}

			var tempSourceRect = Sprite.SourceRect;
			if (remainingSourceX > 0)
			{
				// right edge

				tempSourceRect.Width = (int) remainingSourceX + 1;
				y = startY;
				for (var ii = 0; ii < fullY; ii++)
				{
					batcher.Draw(Sprite, new Rectangle((int) x, (int) y, 
                                 (int) (tempSourceRect.Width * ScaleX), (int) regionHeight), 
                                 tempSourceRect, color);
					y += regionHeight;
				}

				// lower right corner.
				tempSourceRect.Height = (int) remainingSourceY + 1;
				if (remainingSourceY > 0)
					batcher.Draw(Sprite, new Rectangle((int) x, (int) y, 
                                 (int) (tempSourceRect.Width * ScaleX), (int) (tempSourceRect.Height * ScaleY)), 
                                 tempSourceRect, color);
			}

			if (remainingSourceY > 0)
			{
				// bottom edge
				tempSourceRect.Height = (int) remainingSourceY + 1;
                tempSourceRect.Width = Sprite.SourceRect.Width;
				x = startX;
				for (var i = 0; i < fullX; i++)
				{
					batcher.Draw(Sprite, new Rectangle((int) x, (int) y, 
                                 (int) regionWidth, (int) (tempSourceRect.Height * ScaleY)), 
                                 tempSourceRect, color);
					x += regionWidth;
				}
			}
		}
	}
}