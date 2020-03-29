
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
    class Player
    {
        BoundingRectangle bounds;
        Game1 game;
        Texture2D texture;
        public List<Bullet> bulletList = new List<Bullet>();
        BulletModel bulletModel;
        ContentManager content;
        double shootingLag = 0;
        SoundEffect shootSound;
        public Player(Game1 game)
        {
            this.game = game;
        }
        public void LoadContent(ContentManager content)
        {
            this.content = content;
            shootSound = content.Load<SoundEffect>("Shoot");
            texture = content.Load<Texture2D>("player");
            bounds.Width = 50;
            bounds.Height = 50;
            bounds.X = game.GraphicsDevice.Viewport.Width / 2 - 50;
            bounds.Y = 0;
            bulletModel = new BulletModel(content);
        }
        public void Update(GameTime gameTime)
        {
            shootingLag += gameTime.ElapsedGameTime.TotalMilliseconds;
            var keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.Right))
            {
                if(bounds.X <= game.GraphicsDevice.Viewport.Width - 50)
                {
                    bounds.X += 5;
                }
            }
            if(keyboardState.IsKeyDown(Keys.Left))
            {
                if(bounds.X >= 0)
                {
                    bounds.X -= 5;
                }
            }
            if(keyboardState.IsKeyDown(Keys.Space) && shootingLag >= 250)
            {
                bulletList.Add(new Bullet(game, content, bulletModel, (int)bounds.X + 25));
                shootSound.Play();
                shootingLag = 0;
            }
            foreach(Bullet bullet in bulletList)
            {
                bullet.Update(gameTime);
            }
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList.ElementAt(i).isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
            foreach(Bullet bullet in bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
