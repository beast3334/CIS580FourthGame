using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<FireParticles> fireParticles = new List<FireParticles>();
        Texture2D fireTexture;
        Random random = new Random();
        List<Enemy> enemyList = new List<Enemy>();
        EnemyModel enemyModel;
        Background background;
        List<ExplosionParticle> explosionParticles = new List<ExplosionParticle>();
        Texture2D explosionTexture;
        Texture2D rainTexture;
        RainParticle rainParticle;
        Player player;
        double explosionTime = 0;
        bool LevelComplete = false;
        bool gameOver = false;
        SpriteFont gameOverFont;
        public int level = 1;
        int snakeCount = 10;
        SoundEffect explosionSound;
        SoundEffect nextLevelEffect;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            player = new Player(this);
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            background = new Background(this, Content);
            fireTexture = Content.Load<Texture2D>("fire");
            explosionTexture = Content.Load<Texture2D>("Explosion");
            rainTexture = Content.Load<Texture2D>("rain");
            gameOverFont = Content.Load<SpriteFont>("GameOver");
            explosionSound = Content.Load<SoundEffect>("ExplosionSound");
            nextLevelEffect = Content.Load<SoundEffect>("NextLevel");
            rainParticle = new RainParticle(this,Content, rainTexture);
            for (int i = 0; i < 15; i++)
            {
                fireParticles.Add(new FireParticles(this, Content, fireTexture, i * 75));
            }
            player.LoadContent(Content);
            enemyModel = new EnemyModel(this, Content);

            for (int i = 0; i < snakeCount; i++)
            {
                enemyList.Add(new Enemy(this, enemyModel, Content, i * 25));
            }
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!LevelComplete && !gameOver)
            {
                explosionTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                foreach (Enemy enemy in enemyList)
                {
                    enemy.Update(gameTime);
                    if(enemy.bounds.Y <= 50)
                    {
                        gameOver = true;
                    }
                }
                player.Update(gameTime);

                //check bullet collision with enemy
                for (int i = 0; i < player.bulletList.Count; i++)
                {
                    for (int j = 0; j < enemyList.Count; j++)
                    {
                        if (player.bulletList.ElementAt(i).RectBounds().Intersects(enemyList.ElementAt(j).RectBounds()))
                        {
                            player.bulletList.ElementAt(i).isVisible = false;
                            enemyList.ElementAt(j).isVisible = false;
                            explosionParticles.Add(new ExplosionParticle(this, Content, explosionTexture, (int)enemyList.ElementAt(j).bounds.X, (int)enemyList.ElementAt(j).bounds.Y));
                            explosionSound.Play();
                        }
                    }
                }
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (!enemyList.ElementAt(i).isVisible)
                    {
                        enemyList.RemoveAt(i);
                        i--;
                    }
                }
                foreach (FireParticles fire in fireParticles)
                {
                    fire.Update(gameTime);
                }
                foreach (ExplosionParticle explosion in explosionParticles)
                {
                    explosion.Update(gameTime);
                }
                for (int i = 0; i < explosionParticles.Count; i++)
                {
                    if (explosionParticles.ElementAt(i).aliveTime >= 1000)
                    {
                        explosionParticles.RemoveAt(i);
                        i--;
                    }
                }
                if(enemyList.Count == 0)
                {
                    LevelComplete = true;
                    nextLevelEffect.Play();
                }
                
            }
            else if(LevelComplete)
            {
                rainParticle.Update(gameTime);
                var keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    level++;
                    snakeCount += 5;
                    LevelComplete = false;
                    resetGame();
                }
            }
            else
            {
                var keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    level = 1;
                    snakeCount = 10;
                    gameOver = false;
                    resetGame();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            background.Draw(spriteBatch);
            if (!LevelComplete && !gameOver)
            {
                foreach (Enemy enemy in enemyList)
                {
                    enemy.Draw(spriteBatch);
                }
                player.Draw(spriteBatch);

            }

            else if (LevelComplete)
            {
                spriteBatch.DrawString(gameOverFont, "Press Space for Next Level!", new Vector2(this.graphics.GraphicsDevice.Viewport.Width / 2 - 200, this.GraphicsDevice.Viewport.Height / 2), Color.White);
            }
            else
            {
                spriteBatch.DrawString(gameOverFont, "Game Over, press space to try again!", new Vector2(this.graphics.GraphicsDevice.Viewport.Width / 2 - 200, this.GraphicsDevice.Viewport.Height / 2), Color.White);
            }
            spriteBatch.DrawString(gameOverFont, "Level " + level, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here
            if (!LevelComplete && !gameOver)
            {
                foreach (FireParticles fire in fireParticles)
                {
                    fire.Draw();
                }
            }
            else
            {
               
                
                rainParticle.Draw();
            }
            
            foreach (ExplosionParticle explosion in explosionParticles)
            {
                explosion.Draw();
            }
            
            base.Draw(gameTime);
        }

        private void spawnExplosion(int xLocation, int yLocation, ParticleSystem particleSystem)
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.

            particleSystem.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(xLocation, yLocation);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-50, 50, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(0, 100, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Red;
                particle.Scale = 1f;
                particle.Life = 1f;
            };
            particleSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
        }
        private void resetGame()
        {
            enemyList.Clear();
            player.bulletList.Clear();

            for (int i = 0; i < snakeCount; i++)
            {
                enemyList.Add(new Enemy(this, enemyModel, Content, i * 25));
            }
        }
    }
}
