using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
namespace MonoGameWindowsStarter
{
    class Background
    {
        Texture2D texture;
        BoundingRectangle bounds;
        public Background(Game1 game, ContentManager content)
        {
            texture = content.Load<Texture2D>("background");
            bounds.Height = game.GraphicsDevice.Viewport.Height;
            bounds.Width = game.GraphicsDevice.Viewport.Width;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
