using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong.Engine.Entities {
    public class EntityManager {

        // Create entities
        Entity player;
        Entity enemy;
        Entity ball;
        Random rand;

        // List of entities
        List<Entity> entities = new List<Entity>();

        // Entities texture
        Texture2D texture;
        Color pixelColor = Color.White;

        // Game properties
        private bool multiplayerMode = false;
        private int distanceFromBorder = 20;
        private int maxScore = 5;
        private int playerScore = 0;
        private int enemyScore = 0;

        // Entity properties
        private const int entityHeight = 80;
        private const int entityWidth = 10;
        private const float playersSpeed = 7f;
        private const float enemySpeed = 4.5f;

        // Ball properties
        private int ballCollisionCounter = 0;
        private const int ballCollisionTime = 10;
        private const int ballSize = 10;

        private float defaultBallHorizontalSpeed = 4f;
        private float ballHorizontalSpeed = 4f;
        private float ballVerticalSpeed = 0f;

        private const float ballMaxHorizontalSpeed = 12f;
        private const float ballMaxVerticalSpeed = 5f;

        public EntityManager(GraphicsDevice graphicsDevice) {

            // Set pixel graphic and color
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { pixelColor });

            // Initialize entities
            player = new Entity(texture, new Vector2(distanceFromBorder, 150),
                                         new Vector2(entityWidth, entityHeight));

            enemy = new Entity(texture, new Vector2(Game1.screenWidth - distanceFromBorder - entityWidth, 150),
                                         new Vector2(entityWidth, entityHeight));

            ball = new Entity(texture, new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2),
                                       new Vector2(ballSize, ballSize));

            // Set entities in entity list
            entities.Add(player);
            entities.Add(enemy);
            entities.Add(ball);

            // Create Random
            rand = new Random();
        }

        public void Update() {
            // Get input
            KeyboardState keyboard = Keyboard.GetState();

            // Update player 1 and ball
            Player1Movement(keyboard);
            BallUpdate();
            ManageScore();

            // Update player 2 or bot
            if (multiplayerMode == true)
                Player2Movement(keyboard);
            else
                EnemyMovement();
        }

        public void Draw(SpriteBatch spriteBatch) {

            foreach (Entity e in entities) 
                e.Draw(spriteBatch);

        }

        public void DrawUI(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(Game1.defaultFont, playerScore.ToString() + "        " + enemyScore.ToString(),
                                   new Vector2((Game1.screenWidth / 2) - 115, 20), Color.White);
        }
        
        public void Player1Movement(KeyboardState keyboard) {

            if (keyboard.IsKeyDown(Keys.W) && player.position.Y > 0) {
                player.position.Y -= playersSpeed;
            }
            else
            if (keyboard.IsKeyDown(Keys.S) && player.position.Y + playersSpeed < Game1.screenHeight - entityHeight) {
                player.position.Y += playersSpeed;
            }
        }

        public void Player2Movement(KeyboardState keyboard) {
            if (keyboard.IsKeyDown(Keys.Up) && enemy.position.Y > 0) {
                enemy.position.Y -= playersSpeed;
            }
            else
            if (keyboard.IsKeyDown(Keys.Down) && enemy.position.Y + playersSpeed < Game1.screenHeight - entityHeight) {
                enemy.position.Y += playersSpeed;
            }
        }

        public void EnemyMovement() {
            float enemyDir = ball.position.Y - (enemy.position.Y + entityHeight / 2);

            if (ballHorizontalSpeed < 0) {
                if (enemyDir > 0) {
                    enemy.position.Y += enemySpeed;
                }
                else {
                    enemy.position.Y -= enemySpeed;
                }
            }
        }

        public void BallUpdate() {

            ball.position.X -= ballHorizontalSpeed;
            ball.position.Y += ballVerticalSpeed;

            // Collision with walls
            if ((ball.position.Y + ballSize >= Game1.screenHeight) || (ball.position.Y <= 0)) {
                ballVerticalSpeed = -ballVerticalSpeed;
            }

            // Collisision with entities
            ballCollisionCounter--;
            foreach (Entity e in entities) {

                if (e == ball)
                    continue;

                if (EntityCollision(ball, e) && ballCollisionCounter <= 0) {

                    Game1.pongSound.Play(0.5f, 0.0f, 0.0f);

                    ballCollisionCounter = ballCollisionTime;

                    ballHorizontalSpeed = -ballHorizontalSpeed;

                    // Increase ball horizontal speed
                    if (ballHorizontalSpeed < ballMaxHorizontalSpeed) {
                        float addHSpeed = ballHorizontalSpeed > 0 ? 0.5f : -0.5f;
                        ballHorizontalSpeed += addHSpeed;
                    }

                    ballVerticalSpeed = rand.Next((int)-ballMaxVerticalSpeed, (int)ballMaxVerticalSpeed);
                }
            }
        }

        public bool EntityCollision(Entity e1, Entity e2) {
            Rectangle rect1 = new Rectangle((int)e1.position.X, (int)e1.position.Y, (int)e1.dimensions.X, (int)e1.dimensions.Y);
            Rectangle rect2 = new Rectangle((int)e2.position.X, (int)e2.position.Y, (int)e2.dimensions.X, (int)e2.dimensions.Y);

            return rect1.Intersects(rect2);
        }

        public void ManageScore() {
            // Add player score
            if (ball.position.X > Game1.screenWidth) {
                playerScore++;
                RestartRound();
            }
            // Add enemy Score
            if (ball.position.X + ballSize < 0) {
                enemyScore++;
                RestartRound();
            }
            // Check the winner
            if (enemyScore >= maxScore || playerScore >= maxScore) {
                RestartGame();
            }
        }

        public void RestartGame() {
            enemyScore = 0;
            playerScore = 0;
            RestartRound();
        }

        public void RestartRound() {
            // Set default ball position
            ball.position = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
            // Set default ball speed
            ballHorizontalSpeed = defaultBallHorizontalSpeed;
            ballVerticalSpeed = 0;
        }

    }
}
