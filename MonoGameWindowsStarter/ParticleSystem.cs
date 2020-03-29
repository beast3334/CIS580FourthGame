using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    public delegate void ParticleSpawner(ref Particle particle);
    public delegate void ParticleUpdater(float deltaT, ref Particle particle);
    class ParticleSystem
    {
        private Particle[] particles;
        private Texture2D texture;
        private SpriteBatch spritebatch;
        private Random random = new Random();
        int nextIndex = 0;
        public Vector2 Emitter { get; set; }
        public int SpawnPerFrame { get; set; }

        public ParticleSpawner SpawnParticle { get; set; }
        public ParticleUpdater UpdateParticle { get; set; }
        public ParticleSystem(GraphicsDevice graphicsDevice, int size, Texture2D texture)
        {
            this.particles = new Particle[size];
            this.spritebatch = new SpriteBatch(graphicsDevice);
            this.texture = texture;
        }
        public void Update(GameTime gameTime)
        {
            // Part 1: Spawn Particles
            // Make sure our delegate properties are set
            if (SpawnParticle == null || UpdateParticle == null) return;

            // Part 1: Spawn new particles 
            for (int i = 0; i < SpawnPerFrame; i++)
            {
                // Create the particle
                SpawnParticle(ref particles[nextIndex]);

                // Advance the index 
                nextIndex++;
                if (nextIndex > particles.Length - 1) nextIndex = 0;
            }

            // Part 2: Update Particles
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particles.Length; i++)
            {
                // Skip any "dead" particles
                if (particles[i].Life <= 0) continue;

                // Update the individual particle
                UpdateParticle(deltaT, ref particles[i]);
            }
        }
        public void Draw()
        {
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            for (int i = 0; i < particles.Length; i++)
            {
                // Skip any "dead" particles
                if (particles[i].Life <= 0) continue;

                // Draw the individual particles
                spritebatch.Draw(texture, particles[i].Position, null, particles[i].Color, 0f, Vector2.Zero, particles[i].Scale, SpriteEffects.None, 0);
            }

            spritebatch.End();
        }
    }
}
