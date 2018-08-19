using System;
using Microsoft.Xna.Framework;

namespace Platformer2D.Graphics
{
    public sealed class Animation
    {
        private readonly Frame[] _frames;
        private readonly float _frameTime;

        private Vector2 _position;
        private int _index = 0;
        
        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float _time;

        public Animation(Frame[] frames, float frameTime)
        {
            _frames = frames ?? throw new ArgumentNullException(nameof(frames));
            _frameTime = frameTime;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            _position = position;
            
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time >_frameTime)
            {
                _time -= _frameTime;
                _index++;

                if (_index > _frames.Length - 1)
                {
                    _index = 0;
                }
            }
        }

        public void Draw()
        {
            _frames[_index].Draw(_position);
        }
    }
}