using System.Numerics;

namespace WoopWoop
{
    public static class Util
    {
        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }
        public static Vector3 ToVector3(this Vector3 v)
        {
            return new Vector3(v.X, v.Y, 0f);
        }
    }
}