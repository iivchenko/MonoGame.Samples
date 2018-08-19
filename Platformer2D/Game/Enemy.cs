// TODO: Make enemy idle when player dies or winss
// TODO: Replace LEVEL with Collider

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer2D.Graphics;

namespace Platformer2D
{
    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    public class Enemy
    {
        private readonly IState _idleState;
        private readonly IState _moveLeftState;
        private readonly IState _moveRightState;
        private IState _state;

        private readonly Vector2 _origin;
        private readonly int _width;
        private readonly int _height;

        private Vector2 _position;
        private FaceDirection _direction = FaceDirection.Left;

        /// <summary>
        /// Constructs a new Enemy.
        /// </summary>
        public Enemy(
            Level level, 
            Vector2 position, 
            Texture2D idleTexture, 
            Texture2D runTexture,
            SpriteBatch spriteBatch)
        {
            _position = position;

            _idleState = new IdleState(this, CreateAnimation(spriteBatch, idleTexture, SpriteEffects.None));
            _moveLeftState = new MoveState(this, CreateAnimation(spriteBatch, runTexture, SpriteEffects.None), level);
            _moveRightState = new MoveState(this,  CreateAnimation(spriteBatch, runTexture, SpriteEffects.FlipHorizontally), level);

            _state = _idleState;

            _width = (int) (idleTexture.Height * 0.35);
            _height = (int) (idleTexture.Height * 0.7);

            _origin = new Vector2(_width / 2.0f, _height);
        }
        
        public Rectangle BoundingRectangle =>
            new Rectangle
            (
                (int) Math.Round(_position.X - _origin.X),
                (int) Math.Round(_position.Y - _origin.Y),
                _width,
                _height
            );

        public void Update(GameTime gameTime)
        {
            _state.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _state.Draw();
        }

        private static Animation CreateAnimation(SpriteBatch spriteBatch, Texture2D texture, SpriteEffects effects)
        {
            var frames = new List<Frame>();
            for (var i = 0; i < texture.Width / texture.Height; i++)
            {
                frames.Add(new Frame(spriteBatch, texture,
                    new Rectangle(i * texture.Height, 0, texture.Height, texture.Height), effects));
            }

            return new Animation(frames.ToArray(), 0.15f);
        }

        private enum FaceDirection
        {
            Left = -1,
            Right = 1,
        }
        
        private interface IState
        {
            void Update(GameTime gameTime);

            void Draw();
        }

        private class IdleState : IState
        {
            private const float MaxWaitTime = 0.5f;
            
            private readonly Enemy _context;
            private readonly Animation _animation;
            
            private float _waitTime;

            public IdleState(Enemy context, Animation animation)
            {
                _context = context;
                _animation = animation;
            }

            public void Update(GameTime gameTime)
            {
                _animation.Update(gameTime, _context._position);
                
                _waitTime = Math.Max(0.0f, _waitTime - (float) gameTime.ElapsedGameTime.TotalSeconds);
                
                if (_waitTime <= 0.0f)
                {
                    _waitTime = MaxWaitTime;
                    
                    _context._direction = (FaceDirection) (-(int) _context._direction);

                    _context._state =
                        _context._direction == FaceDirection.Left
                            ? _context._moveLeftState
                            : _context._moveRightState;
                    
                    _context._state.Update(gameTime);
                }
            }

            public void Draw()
            {
                _animation.Draw();
            }
        }

        private class MoveState : IState
        {
            private const float MoveSpeed = 64.0f;
            
            private readonly Enemy _context;
            private readonly Animation _animation;
            private readonly Level _level;

            public MoveState(Enemy context, Animation animation, Level level)
            {
                _context = context;
                _animation = animation;
                _level = level;
            }

            public void Update(GameTime gameTime)
            {
                var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
                var posX = _context._position.X + _context._origin.X * (int) _context._direction;

                var tileX = (int) Math.Floor(posX / Tile.Width) - (int) _context._direction;
                var tileY = (int) Math.Floor(_context._position.Y / Tile.Height);

                if (_level.GetCollision(tileX + (int) _context._direction, tileY - 1) == TileCollision.Impassable ||
                    _level.GetCollision(tileX + (int) _context._direction, tileY) == TileCollision.Passable)
                {
                    _context._state = _context._idleState;
                    
                    _context._state.Update(gameTime);
                }
                else
                {
                    var velocity = new Vector2((int) _context._direction * MoveSpeed * elapsed, 0.0f);
                    _context._position += velocity;

                    _animation.Update(gameTime, _context._position);
                }
            }

            public void Draw()
            {
                _animation.Draw();
            }
        }
    }
}