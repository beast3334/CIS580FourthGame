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
    enum Direction
    { 
        RIGHT,
        LEFT,
        DOWN
    }
    class Enemy
    {
        Game1 game;
        public Texture2D texture;
        ContentManager content;
        public BoundingRectangle bounds;
        Direction direction = Direction.RIGHT;
        Direction oldDirection;
        int downCounter = 0;
        public bool isVisible = true;
        public Rectangle RectBounds()
        {
            return bounds;
        }
        public Enemy(Game1 game, EnemyModel model, ContentManager content, int startLocation)
        {
            this.game = game;
            texture = model.texture;
            this.content = content;
            bounds.X = startLocation;
            bounds.Y = game.GraphicsDevice.Viewport.Height - 75;
            LoadContent();
        }
        public void LoadContent()
        {
            bounds.Width = 25;
            bounds.Height = 25;
        }
        public void Update(GameTime gameTime)
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    if (bounds.X <= game.GraphicsDevice.Viewport.Width - bounds.Width)
                    {
                        bounds.X += game.level * 1.5f;
                    }
                    else
                    {
                        oldDirection = Direction.RIGHT;
                        direction = Direction.DOWN;
                    }
                    break;
                case Direction.LEFT:
                    if (bounds.X >= 0)
                    {
                        bounds.X-= game.level * 1.5f;
                    }
                    else
                    {
                        oldDirection = Direction.LEFT;
                        direction = Direction.DOWN;
                    }
                    break;
                case Direction.DOWN:
                    if(downCounter < 5)
                    {
                        bounds.Y -= game.level * 1.5f;
                        downCounter++;
                    }
                    else
                    {
                        if(oldDirection == Direction.RIGHT)
                        {
                            direction = Direction.LEFT;
                        }
                        else
                        {
                            direction = Direction.RIGHT;
                        }
                        downCounter = 0;
                    }
                    break;
                default:
                    break;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
