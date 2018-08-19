using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D.Graphics
{
    public sealed class Frame
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _texture;
        private readonly Rectangle _rectangle;
        private readonly SpriteEffects _effects;
        private Vector2 _origin;

        public Frame(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, SpriteEffects effects)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            _texture = texture ?? throw new ArgumentNullException(nameof(texture));
            _rectangle = rectangle;
            _effects = effects;
            _origin = new Vector2(_rectangle.Width / 2.0f, _rectangle.Height);
        }
        
        public void Draw(Vector2 position)
        {
            _spriteBatch.Draw(_texture, position, _rectangle, Color.White, 0.0f, _origin, 1.0f, _effects, 0.0f);
        }
    }
}