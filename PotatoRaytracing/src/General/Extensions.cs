using System;
using System.DoubleNumerics;
using System.Drawing;

namespace PotatoRaytracing
{
    public static class Extensions
    {
        public static Color Add(this Color color, Color colorToAdd)
        {
            return Color.FromArgb((color.R + colorToAdd.R) / 2,
                                  (color.G + colorToAdd.G) / 2,
                                  (color.B + colorToAdd.B) / 2);
        }

        public static Color Multiply(this Color color, float x)
        {
            return Color.FromArgb((int)Math.Round(color.R * x),
                                  (int)Math.Round(color.G * x),
                                  (int)Math.Round(color.B * x));
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static void Swap<T>(ref T first, ref T second) //TODO: Mettre ca dans un utilitaire.
        {
            T temp = first;
            first = second;
            second = temp;
        }

        public static double Angle(this Vector3 vector3, Vector3 other)
        {
            double dot = Vector3.Dot(vector3, other);
            double lenA = vector3.Length();
            double lenB = other.Length();

            return Math.Acos(dot / (lenA - lenB));      
        }

        public static double Get(this Vector3 vector3, int index)
        {
            switch(index)
            {
                case 0: return vector3.X;
                case 1: return vector3.Y;
                case 2: return vector3.Z;
                default: throw new System.ArgumentException("index must be >= 0 <= 2");
            }
        }

        public static void SetByIndex(this ref Vector3 vector3, int index, double value)
        {
            switch (index)
            {
                case 0: vector3.X = value; break;
                case 1: vector3.Y = value; break;
                case 2: vector3.Z = value; break;
                default: throw new ArgumentException("index must be >= 0 <= 2");
            }
        }

        public static void Set(this ref Vector3 vector3, double value)
        {
            vector3.X = value;
            vector3.Y = value;
            vector3.Z = value;
        }

        public static void Set(this ref Vector3 vector3, double x, double y, double z)
        {
            vector3.X = x;
            vector3.Y = y;
            vector3.Z = z;
        }

        public static void Set(this ref Vector2 vector2, double value)
        {
            vector2.X = value;
            vector2.Y = value;
        }

        public static void Set(this ref Vector2 vector2, double x, double y)
        {
            vector2.X = x;
            vector2.Y = y;
        }

        public static Color ParseColor(string str)
        {
            string[] values = str.Split(' ');
            return Color.FromArgb(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
        }
    }
}
