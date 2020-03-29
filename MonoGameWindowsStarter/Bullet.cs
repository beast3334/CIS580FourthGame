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
    class Bullet
    {
        BoundingRectangle bounds;
        Game1 game;
        Texture2D texture;
        ContentManager content;
        public bool isVisible = true;
        public Rectangle RectBounds()
        {
            return bounds;
        }
        public Bullet(Game1 game, ContentManager content, BulletModel model, int xPosition)
        {
            this.game = game;
            this.content = content;
            texture = model.texture;
            LoadContent(xPosition);
        }
        public void LoadContent(int xPosition)
        {
            bounds.Height = 10;
            bounds.Width = 10;
            bounds.X = xPosition;
            bounds.Y = 0;
        }
        public void Update(GameTime gameTime)
        {
            if(bounds.Y >= game.GraphicsDevice.Viewport.Height)
            {
                isVisible = false;
            }
            bounds.Y += 10;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
