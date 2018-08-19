using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Platformer2D
{
    public sealed class Gem
    {
        // Bounce control constants
        private const float BounceHeight = 0.18f;
        private const float BounceRate = 3.0f;
        private const float BounceSync = -0.75f;
        
        private readonly Texture2D _sprite;
        private readonly Vector2 _origin;
        private readonly SoundEffect _collectedSound;
        private readonly Color _color = Color.Yellow;

        // The gem is animated from a base position along the Y axis.
        private readonly Vector2 _basePosition;
        private Vector2 _position;
       
        public Gem(Texture2D sprite, SoundEffect collectSound, Vector2 position, int scores)
        {
            _basePosition = position;
            _sprite = sprite;
            _collectedSound = collectSound;
            
            _origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);

            Scores = scores;
        }
        
        public Circle BoundingCircle => new Circle(_position, Tile.Width / 3.0f);
        
        public int Scores { get; }
        
        public void Update(GameTime gameTime)
        {
            var t = gameTime.TotalGameTime.TotalSeconds * BounceRate + _position.X * BounceSync;
            var bounce = (float)Math.Sin(t) * BounceHeight * _sprite.Height;
            _position = _basePosition + new Vector2(0.0f, bounce);
        }
        
        public void OnCollected(Player collectedBy)
        {
            _collectedSound.Play();
        }
     
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, _position, null, _color, 0.0f, _origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
