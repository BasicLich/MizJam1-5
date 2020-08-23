using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace MizJam1.Utilities
{
    /// <summary>
    /// Contains useful math-related extension methods.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Multiplies all the values inside <paramref name="rectangle"/> by the given <paramref name="number"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="number"></param>
        /// <returns>The new rectangle</returns>
        public static Rectangle Multiply(this Rectangle rectangle, int number) => new Rectangle(rectangle.X * number, rectangle.Y * number, rectangle.Width * number, rectangle.Height * number);
        /// <summary>
        /// Divides all the values inside <paramref name="rectangle"/> by the given <paramref name="number"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Rectangle Divide(this Rectangle rectangle, int number) => new Rectangle(rectangle.X / number, rectangle.Y / number, rectangle.Width / number, rectangle.Height / number);
        /// <summary>
        /// Divides the <paramref name="rectangle"/> by the <paramref name="value"/>, round down the location, and round up the size.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="value"></param>
        /// <returns>The new rectangle</returns>
        public static Rectangle DivideRounding(this Rectangle rectangle, int value) => new Rectangle((int)Math.Floor(rectangle.X / (float)value),
                                                                                                     (int)Math.Floor(rectangle.Y / (float)value),
                                                                                                     (int)Math.Ceiling(rectangle.Width / (float)value),
                                                                                                     (int)Math.Ceiling(rectangle.Height / (float)value));
        /// <summary>
        /// Increases the size of the <paramref name="rectangle"/> by the given <paramref name="amount"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="amount"></param>
        /// <returns>The new <see cref="Rectangle"/></returns>
        public static Rectangle IncreaseSize(this Rectangle rectangle, Point amount) => new Rectangle(rectangle.Location, rectangle.Size + amount);
        public static Rectangle IncreaseSize(this Rectangle rect, int nb) => rect.IncreaseSize(new Point(nb));
        /// <summary>
        /// Creates the biggest possible rectangle from the corners of the two rectangles.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="other"></param>
        /// <returns>The new rectangle</returns>
        public static Rectangle JoinedBounds(this Rectangle rect, Rectangle other)
        {
            Point pos = new Point(Min(rect.X, other.X), Min(rect.Y, other.Y));
            Point otherCorner = new Point(Max(rect.Right, other.Right), Max(rect.Bottom, other.Bottom));

            Point size = otherCorner - pos;
            return new Rectangle(pos, size);
        }
        /// <summary>
        /// Translates the rectangle using the X & Y values of the given Point.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="p"></param>
        /// <returns>The new rectangle</returns>
        public static Rectangle Translate(this Rectangle rect, Point p) => new Rectangle(rect.Location + p, rect.Size);

        /// <summary>
        /// Adds the given number to the point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public static Point Add(this Point p, int nb) => new Point(p.X + nb, p.Y + nb);
        /// <summary>
        /// Adds the x and y values given to the Point's X and Y values.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The new Point</returns>
        public static Point Add(this Point p, int x, int y) => new Point(p.X + x, p.Y + y);

        /// <summary>
        /// Subtracts the given value from the Point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public static Point Sub(this Point p, int nb) => p - new Point(nb);

        /// <summary>
        /// Subtracts the given value from the Point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public static Point Sub(this Point p, int x, int y) => p - new Point(x, y);
        /// <summary>
        /// Multiplies the Point by the given value.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public static Point Multiply(this Point p, int nb) => new Point(p.X * nb, p.Y * nb);
        /// <summary>
        /// Divides the Point by the given number.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public static Point Divide(this Point p, int nb) => new Point(p.X / nb, p.Y / nb);

        /// <summary>
        /// Returns whether the two points are neighbors (if their distance is exactly equal to 1)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool IsNeighbor(this Point p, Point p2) => p.X == p2.X && Abs(p.Y - p2.Y) == 1 || p.Y == p2.Y && Abs(p.X - p2.X) == 1;

        /// <summary>
        /// Rounds down the vector's attributes.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Floor(this Vector2 v) => new Vector2((float)Math.Floor(v.X), (float)Math.Floor(v.Y));
        /// <summary>
        /// Rounds up the vector's attributes
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Ceiling(this Vector2 v) => new Vector2((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y));
        /// <summary>
        /// Rounds the vector's attributes
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Round(this Vector2 v) => new Vector2((float)Math.Round(v.X), (float)Math.Round(v.Y));

        /// <summary>
        /// Returns a random point between the two given points, inclusively.
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="upLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        public static Point RandomPoint(this Random rand, Point upLeft, Point bottomRight) =>
            new Point(rand.Next(upLeft.X, bottomRight.X + 1), rand.Next(upLeft.Y, bottomRight.Y + 1));

        /// <summary>
        /// Returns a random element from the given list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rand"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T RandomFromList<T>(this Random rand, List<T> list) => list[rand.Next(list.Count)];

        /// <summary>
        /// Returns a random KeyValuePair from the given dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="rand"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static KeyValuePair<T, T2> RandomFromDictionary<T, T2>(this Random rand, Dictionary<T, T2> dict) => dict.Skip(rand.Next(dict.Count)).First();

        public static int CellDistance(Point p1, Point p2) => Abs(p1.X - p2.X) + Abs(p1.Y - p2.Y);
    }
}
