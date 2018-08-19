using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D
{
    public sealed class EnemyFactory
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;
        
        public EnemyFactory(IServiceProvider serviceProvider, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _content = new ContentManager(serviceProvider, "Content");
        }
        
        public Enemy Create(Level level, Vector2 position, string spriteSet)
        {
            spriteSet = "Sprites/" + spriteSet + "/";
            var idleTexture = _content.Load<Texture2D>(spriteSet + "Idle");
            var runTexture = _content.Load<Texture2D>(spriteSet + "Run");
            
            return new Enemy(level, position, idleTexture, runTexture, _spriteBatch);
        }
    }
}