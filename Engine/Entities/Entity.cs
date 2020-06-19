using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong.Engine.Entities {
    public class Entity {

        Texture2D texture;
        public Vector2 position;
        public Vector2 dimensions;

        // Constructor
        public Entity(Texture2D texture, Vector2 position, Vector2 dimensions) {
            this.texture = texture;
            this.position = position;
            this.dimensions = dimensions;
        }

        // Draw Method
        public virtual void Draw(SpriteBatch spriteBatch) {

            var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);

            spriteBatch.Draw(texture, rectangle, Color.White);

        }

        // Position Getter and Setter
        public Vector2 getPosition() {
            return this.position;
        }
        public void setPosition(Vector2 newPosition) {
            this.position = newPosition;
        }
    }
}
