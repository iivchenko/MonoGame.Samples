using Microsoft.Xna.Framework;

namespace Platformer2D
{
    public struct Circle
    {
        private readonly Vector2 _center;
        private readonly float _radius;

        public Circle(Vector2 position, float radius)
        {
            _center = position;
            _radius = radius;
        }

        public bool Intersects(Rectangle rectangle)
        {
            var v = new Vector2
            (
                MathHelper.Clamp(_center.X, rectangle.Left, rectangle.Right),
                MathHelper.Clamp(_center.Y, rectangle.Top, rectangle.Bottom)
            );

            var direction = _center - v;
            var distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < _radius * _radius));
        }
    }
}
