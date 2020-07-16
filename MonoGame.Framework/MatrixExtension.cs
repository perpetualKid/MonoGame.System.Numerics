using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Microsoft.Xna.Framework
{
    internal static class MatrixExtension
    {
        /// <summary>
        /// Copy the values of specified <see cref="Matrix"/> to the float array.
        /// </summary>
        /// <param name="matrix">The source <see cref="Matrix"/>.</param>
        /// <returns>The array which matrix values will be stored.</returns>
        /// <remarks>
        /// Required for OpenGL 2.0 projection matrix stuff.
        /// </remarks>
        public static float[] ToFloatArray(in this Matrix4x4 matrix)
        {
            float[] matarray = {
                                    matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                                    matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                                    matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                                    matrix.M41, matrix.M42, matrix.M43, matrix.M44
                                };
            return matarray;
        }

        public static Matrix ToMatrix(in this Matrix4x4 matrix)
        {
            return new Matrix(
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
                );
        }
    }

    internal static class Vector2Extension
    {
        internal static System.Numerics.Vector2 FromVector2(in this Vector2 vector)
        {
            return new System.Numerics.Vector2(vector.X, vector.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
        {
            return new Vector2(
                MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
        {
            result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
        {
            return new Vector2(MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount), MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount));
        }

        /// <summary>
        /// Gets a <see cref="Point"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Point"/> representation for this object.</returns>
        public static Point ToPoint(in this Vector2 vector2)
        {
            return new Point((int)vector2.X, (int)vector2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="XNAPlane"/>.</param>
        /// <returns>The rounded <see cref="XNAPlane"/>.</returns>
        public static void Ceiling(ref this Vector2 value)
        {
            value.X = (float)Math.Ceiling(value.X);
            value.Y = (float)Math.Ceiling(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="result">The rounded <see cref="Vector2"/>.</param>
        public static void Ceiling(in this Vector2 value, out Vector2 result)
        {
            result.X = (float)Math.Ceiling(value.X);
            result.Y = (float)Math.Ceiling(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="XNAPlane"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <returns>The rounded <see cref="Vector2"/>.</returns>
        public static void Floor(ref this Vector2 value)
        {
            value.X = (float)Math.Floor(value.X);
            value.Y = (float)Math.Floor(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="result">The rounded <see cref="Vector2"/>.</param>
        public static void Floor(in this Vector2 value, out Vector2 result)
        {
            result.X = (float)Math.Floor(value.X);
            result.Y = (float)Math.Floor(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <returns>The rounded <see cref="Vector2"/>.</returns>
        public static void Round(ref this Vector2 value)
        {
            value.X = (float)Math.Round(value.X);
            value.Y = (float)Math.Round(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="result">The rounded <see cref="Vector2"/>.</param>
        public static void Round(in this Vector2 value, out Vector2 result)
        {
            result.X = (float)Math.Round(value.X);
            result.Y = (float)Math.Round(value.Y);
        }

    }

    internal static class Vector3Extension
    {
        internal static System.Numerics.Vector3 FromVector3(in this Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
        }

    }

    internal static class Vector4Extension
    {
        /// <summary>
        /// Creates a new <see cref="Vector4"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vector4 Hermite(Vector4 value1, Vector4 tangent1, Vector4 value2, Vector4 tangent2, float amount)
        {
            return new Vector4(MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
                               MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount),
                               MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount),
                               MathHelper.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount));
        }

        /// <summary>
        /// Round the members of this <see cref="Vector4"/> towards positive infinity.
        /// </summary>
        public static void  Ceiling(in this Vector4 vector4, out Vector4 result)
        {
            result.X = (float)Math.Ceiling(vector4.X);
            result.Y = (float)Math.Ceiling(vector4.Y);
            result.Z = (float)Math.Ceiling(vector4.Z);
            result.W = (float)Math.Ceiling(vector4.W);
        }

        /// <summary>
        /// Round the members of this <see cref="Vector4"/> towards positive infinity.
        /// </summary>
        public static void Ceiling(ref this Vector4 vector4)
        {
            vector4.X = (float)Math.Ceiling(vector4.X);
            vector4.Y = (float)Math.Ceiling(vector4.Y);
            vector4.Z = (float)Math.Ceiling(vector4.Z);
            vector4.W = (float)Math.Ceiling(vector4.W);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4"/>.</param>
        /// <returns>The rounded <see cref="Vector4"/>.</returns>
        public static void Floor(ref this Vector4 value)
        {
            value.X = (float)Math.Floor(value.X);
            value.Y = (float)Math.Floor(value.Y);
            value.Z = (float)Math.Floor(value.Z);
            value.W = (float)Math.Floor(value.W);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4"/>.</param>
        /// <param name="result">The rounded <see cref="Vector4"/>.</param>
        public static void Floor(in this Vector4 value, out Vector4 result)
        {
            result.X = (float)Math.Floor(value.X);
            result.Y = (float)Math.Floor(value.Y);
            result.Z = (float)Math.Floor(value.Z);
            result.W = (float)Math.Floor(value.W);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4"/>.</param>
        /// <returns>The rounded <see cref="Vector4"/>.</returns>
        public static void Round(ref this Vector4 value)
        {
            value.X = (float)Math.Round(value.X);
            value.Y = (float)Math.Round(value.Y);
            value.Z = (float)Math.Round(value.Z);
            value.W = (float)Math.Round(value.W);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4"/>.</param>
        /// <param name="result">The rounded <see cref="Vector4"/>.</param>
        public static void Round(in this Vector4 value, out Vector4 result)
        {
            result.X = (float)Math.Round(value.X);
            result.Y = (float)Math.Round(value.Y);
            result.Z = (float)Math.Round(value.Z);
            result.W = (float)Math.Round(value.W);
        }



    }

    internal static class PlaneExtension
    {
        internal static PlaneIntersectionType Intersects(in this Plane plane, in System.Numerics.Vector3 point)
        {
            float distance = Plane.DotCoordinate(plane, point);

            if (distance > 0)
                return PlaneIntersectionType.Front;

            if (distance < 0)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Returns a value indicating what side (positive/negative) of a plane a point is
        /// </summary>
        /// <param name="point">The point to check with</param>
        /// <param name="plane">The plane to check against</param>
        /// <returns>Greater than zero if on the positive side, less than zero if on the negative size, 0 otherwise</returns>
        public static float ClassifyPoint(in this Plane plane, in Vector3 point)
        {
            return point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D;
        }

        /// <summary>
        /// Returns the perpendicular distance from a point to a plane
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="plane">The place to check</param>
        /// <returns>The perpendicular distance from the point to the plane</returns>
        public static float PerpendicularDistance(in this Plane plane, in Vector3 point)
        {
            // dist = (ax + by + cz + d) / sqrt(a*a + b*b + c*c)
            return (float)Math.Abs((plane.Normal.X * point.X + plane.Normal.Y * point.Y + plane.Normal.Z * point.Z)
                                    / Math.Sqrt(plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z));
        }
    }



}
