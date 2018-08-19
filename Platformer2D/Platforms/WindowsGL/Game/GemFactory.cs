using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D
{
    public sealed class GemFactory
    {
        private const int Scores = 30;
        
        private readonly ContentManager _content;
        
        public GemFactory(IServiceProvider serviceProvider)
        {
            _content = new ContentManager(serviceProvider, "Content");
        }
        
        public Gem Create(Vector2 position)
        {
            var sprite = _content.Load<Texture2D>("Sprites/Gem");
            var collectedSound = _content.Load<SoundEffect>("Sounds/GemCollected");
            
            return new Gem(sprite, collectedSound, position, Scores);
        }
    }
}