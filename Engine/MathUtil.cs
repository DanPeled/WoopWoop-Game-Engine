using System.Numerics;

namespace WoopWoop
{
    public static class MathUtil
    {
        public static float CrossProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 NormalizeVector(Vector2 v)
        {
            float length = v.Length();
            return new(v.X / length, v.Y / length);
        }

        public static float VectorAngle(Vector2 v)
        {
            return MathF.Atan2(v.Y, v.X);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min == max)
            {
                return min;
            }
            else if (min > max)
            {
                throw new ArgumentOutOfRangeException("min is greater than the max");
            }
            else if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            return value;
        }
    }
}