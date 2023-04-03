using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace Microsoft.Xna.Framework
{
    public static class MatrixExtension
    {
        /// <summary>
        /// Copy the values of specified <see cref="Matrix4x4"/> to the float array.
        /// </summary>
        /// <param name="matrix">The source <see cref="Matrix4x4"/>.</param>
        /// <returns>The array which matrix values will be stored.</returns>
        /// <remarks>
        /// Required for OpenGL 2.0 projection matrix stuff.
        /// </remarks>
        public static float[] ToFloatArray(this ref Matrix4x4 matrix)
        {
            float[] matarray = {
                                    matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                                    matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                                    matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                                    matrix.M41, matrix.M42, matrix.M43, matrix.M44
                                };
            return matarray;
        }

        /// <summary>
        /// The backward vector formed from the third row M31, M32, M33 elements.
        /// </summary>
        public static Vector3 Backward(this ref Matrix4x4 value)
        {
            return new Vector3(value.M31, value.M32, value.M33);
        }

        /// <summary>
        /// The down vector formed from the second row -M21, -M22, -M23 elements.
        /// </summary>
        public static Vector3 Down(this ref Matrix4x4 value)
        {
            return new Vector3(-value.M21, -value.M22, -value.M23);
        }

        /// <summary>
        /// The forward vector formed from the third row -M31, -M32, -M33 elements.
        /// </summary>
        public static Vector3 Forward(this ref Matrix4x4 value)
        {
            return new Vector3(-value.M31, -value.M32, -value.M33);
        }

        /// <summary>
        /// The left vector formed from the first row -M11, -M12, -M13 elements.
        /// </summary>
        public static Vector3 Left(this ref Matrix4x4 value)
        {
            return new Vector3(-value.M11, -value.M12, -value.M13);
        }

        /// <summary>
        /// The right vector formed from the first row M11, M12, M13 elements.
        /// </summary>
        public static Vector3 Right(this ref Matrix4x4 value)
        {
            return new Vector3(value.M11, value.M12, value.M13);
        }

        /// <summary>
        /// The upper vector formed from the second row M21, M22, M23 elements.
        /// </summary>
        public static Vector3 Up(this ref Matrix4x4 value)
        {
            return new Vector3(value.M21, value.M22, value.M23);
        }
    }

    public static class Vector2Extension
    {
        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed <see cref="Vector2"/>.</returns>
        public static Vector2 Transform(Vector2 position, Matrix4x4 matrix)
        {
            return new Vector2((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed <see cref="Vector2"/> as an output parameter.</param>
        public static void Transform(ref Vector2 position, ref Matrix4x4 matrix, out Vector2 result)
        {
            var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vector2"/>.</returns>
        public static Vector2 Transform(Vector2 value, Quaternion rotation)
        {
            Transform(ref value, ref rotation, out value);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vector2"/> as an output parameter.</param>
        public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector2 result)
        {
            var rot1 = new Vector3(rotation.X + rotation.X, rotation.Y + rotation.Y, rotation.Z + rotation.Z);
            var rot2 = new Vector3(rotation.X, rotation.X, rotation.W);
            var rot3 = new Vector3(1, rotation.Y, rotation.Z);
            var rot4 = rot1 * rot2;
            var rot5 = rot1 * rot3;

            var v = new Vector2();
            v.X = (float)((double)value.X * (1.0 - (double)rot5.Y - (double)rot5.Z) + (double)value.Y * ((double)rot4.Y - (double)rot4.Z));
            v.Y = (float)((double)value.X * ((double)rot4.Y + (double)rot4.Z) + (double)value.Y * (1.0 - (double)rot4.X - (double)rot5.Z));
            result.X = v.X;
            result.Y = v.Y;
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix4x4 matrix, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];
                destination.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
                destination.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];

                Vector2 v;
                Transform(ref position, ref rotation, out v);

                destination.X = v.X;
                destination.Y = v.Y;

                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(Vector2[] sourceArray, ref Matrix4x4 matrix, Vector2[] destinationArray)
        {
            Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(Vector2[] sourceArray, ref Quaternion rotation, Vector2[] destinationArray)
        {
            Transform(sourceArray, 0, ref rotation, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix)
        {
            return new Vector2((normal.X * matrix.M11) + (normal.Y * matrix.M21), (normal.X * matrix.M12) + (normal.Y * matrix.M22));
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(ref Vector2 normal, ref Matrix4x4 matrix, out Vector2 result)
        {
            var x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            var y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Vector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector2"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal(Vector2[] sourceArray, int sourceIndex, ref Matrix4x4 matrix, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int i = 0; i < length; i++)
            {
                var normal = sourceArray[sourceIndex + i];

                destinationArray[destinationIndex + i] = new Vector2((normal.X * matrix.M11) + (normal.Y * matrix.M21),
                                                                     (normal.X * matrix.M12) + (normal.Y * matrix.M22));
            }
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Vector2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal(Vector2[] sourceArray, ref Matrix4x4 matrix, Vector2[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination array length is lesser than source array length");

            for (int i = 0; i < sourceArray.Length; i++)
            {
                var normal = sourceArray[i];

                destinationArray[i] = new Vector2((normal.X * matrix.M11) + (normal.Y * matrix.M21),
                                                  (normal.X * matrix.M12) + (normal.Y * matrix.M22));
            }
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
        internal static string DebugDisplayString(this in Vector3 value)
        {
            return string.Concat(
                value.X.ToString(), "  ",
                value.Y.ToString(), "  ",
                value.Z.ToString()
            );
        }

        internal static Vector3 Forward => new Vector3(0f, 0f, -1f);
        internal static Vector3 Backward => Vector3.UnitZ;

        /// <summary>
        /// Creates a new <see cref="Vector3"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <returns>The rounded <see cref="Vector3"/>.</returns>
        public static void Ceiling(ref this Vector3 value)
        {
            value.X = (float)Math.Ceiling(value.X);
            value.Y = (float)Math.Ceiling(value.Y);
            value.Z = (float)Math.Ceiling(value.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="result">The rounded <see cref="Vector3"/>.</param>
        public static void Ceiling(in this Vector3 value, out Vector3 result)
        {
            result.X = (float)Math.Ceiling(value.X);
            result.Y = (float)Math.Ceiling(value.Y);
            result.Z = (float)Math.Ceiling(value.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <returns>The rounded <see cref="Vector3"/>.</returns>
        public static void Round(ref this Vector3 value)
        {
            value.X = (float)Math.Round(value.X);
            value.Y = (float)Math.Round(value.Y);
            value.Z = (float)Math.Round(value.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="result">The rounded <see cref="Vector3"/>.</param>
        public static void Round(in this Vector3 value, out Vector3 result)
        {
            result.X = (float)Math.Round(value.X);
            result.Y = (float)Math.Round(value.Y);
            result.Z = (float)Math.Round(value.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <returns>The rounded <see cref="Vector3"/>.</returns>
        public static void Floor(ref this Vector3 value)
        {
            value.X = (float)Math.Floor(value.X);
            value.Y = (float)Math.Floor(value.Y);
            value.Z = (float)Math.Floor(value.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="result">The rounded <see cref="Vector3"/>.</param>
        public static void Floor(in this Vector3 value, out Vector3 result)
        {
            result.X = (float)Math.Floor(value.X);
            result.Y = (float)Math.Floor(value.Y);
            result.Z = (float)Math.Floor(value.Z);
        }
    }

    public static class Vector4Extension
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
        public static void Ceiling(in this Vector4 vector4, out Vector4 result)
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

    public static class PlaneExtension
    {
        internal static string DebugDisplayString(this in Plane value)
        {
            return string.Concat(
                                value.Normal.DebugDisplayString(), "  ",
                                value.D.ToString()
                                );
        }

        public static PlaneIntersectionType Intersects(in this Plane plane, BoundingBox box)
        {
            return box.Intersects(plane);
        }

        public static PlaneIntersectionType Intersects(in this Plane plane, BoundingFrustum frustum)
        {
            return frustum.Intersects(plane);
        }

        public static PlaneIntersectionType Intersects(in this Plane plane, BoundingSphere sphere)
        {
            return sphere.Intersects(plane);
        }

        public static PlaneIntersectionType Intersects(in this Plane plane, in Vector3 point)
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
