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
    class FireParticles
    {
        Texture2D texture;
        ParticleSystem particleSystem;
        Game1 game;
        ContentManager content;
        Random random = new Random();
        public FireParticles(Game1 game, ContentManager content, Texture2D texture, int xPosition)
        {
            this.game = game;
            this.content = content;
            this.texture = texture;
            LoadContent(xPosition, random.Next(game.GraphicsDevice.Viewport.Height - 25, game.GraphicsDevice.Viewport.Height + 25));
        }
        public void LoadContent(int xPosition, int yPosition)
        {
            particleSystem = new ParticleSystem(game.GraphicsDevice, 1000, texture);
            particleSystem.SpawnPerFrame = 4;

            particleSystem.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(xPosition, yPosition);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-75, 75, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(-100, 20, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.OrangeRed;
                particle.Scale = 1.5f;
                particle.Life = 0.5f;
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
