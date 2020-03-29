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
    class RainParticle
    {
        Texture2D texture;
        ParticleSystem particleSystem;
        Game1 game;
        ContentManager content;
        Random random = new Random();
        public RainParticle(Game1 game, ContentManager content, Texture2D texture)
        {
            this.game = game;
            this.content = content;
            this.texture = texture;
            LoadContent(game.GraphicsDevice.Viewport.Width/2,-100);
        }
        public void LoadContent(int xPosition, int yPosition)
        {
            particleSystem = new ParticleSystem(game.GraphicsDevice, 1000, texture);
            particleSystem.SpawnPerFrame = 1;

            particleSystem.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(xPosition, yPosition);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Width, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(game.GraphicsDevice.Viewport.Height, game.GraphicsDevice.Viewport.Height, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.001f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Blue;
                particle.Scale = 1f;
                particle.Life = 10f;
            };
            particleSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
        }
        public void Update(GameTime gameTime)
        {
            particleSystem.Update(gameTime);
        }
        public void Draw()
        {
            particleSystem.Draw();
        }
    }
}
