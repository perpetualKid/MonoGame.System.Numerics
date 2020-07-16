using Microsoft.Xna.Framework;
using NUnit.Framework;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace MonoGame.Tests.Framework
{
    class Vector2Test
    {
        [Test]
        public void CatmullRom()
        {
            var expectedResult = new Vector2(5.1944f, 6.1944f);
            var v1 = new Vector2(1, 2); var v2 = new Vector2(3, 4); var v3 = new Vector2(5, 6); var v4 = new Vector2(7, 8); var value = 1.0972f;

            Vector2 result;
            Vector2Extension.CatmullRom(ref v1, ref v2, ref v3, ref v4, value, out result);

            Assert.That(expectedResult, Is.EqualTo(Vector2Extension.CatmullRom(v1, v2, v3, v4, value)).Using(Vector2Comparer.Epsilon));
            Assert.That(expectedResult, Is.EqualTo(result).Using(Vector2Comparer.Epsilon));
        }

        [Test]
        public void Multiply()
        {
            var vector = new Vector2(1, 2);

            // Test 0.0 scale.
            Assert.AreEqual(Vector2.Zero, 0 * vector);
            Assert.AreEqual(Vector2.Zero, vector * 0);
            Assert.AreEqual(Vector2.Zero, Vector2.Multiply(vector, 0));
            Assert.AreEqual(Vector2.Multiply(vector, 0), vector * 0.0f);

            // Test 1.0 scale.
            Assert.AreEqual(vector, 1 * vector);
            Assert.AreEqual(vector, vector * 1);
            Assert.AreEqual(vector, Vector2.Multiply(vector, 1));
            Assert.AreEqual(Vector2.Multiply(vector, 1), vector * 1.0f);

            var scaledVec = vector * 2;

            // Test 2.0 scale.
            Assert.AreEqual(scaledVec, 2 * vector);
            Assert.AreEqual(scaledVec, vector * 2);
            Assert.AreEqual(scaledVec, Vector2.Multiply(vector, 2));
            Assert.AreEqual(vector * 2.0f, scaledVec);
            Assert.AreEqual(2 * vector, Vector2.Multiply(vector, 2));

            scaledVec = vector * 0.999f;

            // Test 0.999 scale.
            Assert.AreEqual(scaledVec, 0.999f * vector);
            Assert.AreEqual(scaledVec, vector * 0.999f);
            Assert.AreEqual(scaledVec, Vector2.Multiply(vector, 0.999f));
            Assert.AreEqual(vector * 0.999f, scaledVec);
            Assert.AreEqual(0.999f * vector, Vector2.Multiply(vector, 0.999f));

            var vector2 = new Vector2(2, 2);

            // Test two vectors multiplication.
            Assert.AreEqual(new Vector2(vector.X * vector2.X, vector.Y * vector2.Y), vector * vector2);
            Assert.AreEqual(vector2 * vector, new Vector2(vector.X * vector2.X, vector.Y * vector2.Y));
            Assert.AreEqual(vector * vector2, Vector2.Multiply(vector, vector2));
            Assert.AreEqual(Vector2.Multiply(vector, vector2), vector * vector2);

            Vector2 refVec;
        }

        [Test]
        public void Hermite()
        {
            var t1 = new Vector2(1.40625f, 1.40625f);
            var t2 = new Vector2(2.662375f, 2.26537514f);

            var v1 = new Vector2(1, 1); var v2 = new Vector2(2, 2); var v3 = new Vector2(3, 3); var v4 = new Vector2(4, 4);
            var v5 = new Vector2(4, 3); var v6 = new Vector2(2, 1); var v7 = new Vector2(1, 2); var v8 = new Vector2(3, 4);

            Assert.That(t1, Is.EqualTo(Vector2Extension.Hermite(v1, v2, v3, v4, 0.25f)).Using(Vector2Comparer.Epsilon));
            Assert.That(t2, Is.EqualTo(Vector2Extension.Hermite(v5, v6, v7, v8, 0.45f)).Using(Vector2Comparer.Epsilon));
        }

        [Test]
        public void Transform()
        {
            // STANDART OVERLOADS TEST

            var expectedResult1 = new System.Numerics.Vector2(24, 28);
            var expectedResult2 = new System.Numerics.Vector2(-0.0168301091f, 2.30964f);

            var v1 = new System.Numerics.Vector2(1, 2);
            var m1 = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            var v2 = new System.Numerics.Vector2(1.1f, 2.45f);
            var q2 = new System.Numerics.Quaternion(0.11f, 0.22f, 0.33f, 0.55f);

            var q3 = new System.Numerics.Quaternion(1, 2, 3, 4);

            Assert.That(expectedResult1, Is.EqualTo(System.Numerics.Vector2.Transform(v1, m1)).Using(Vector2Comparer.Epsilon));
            Assert.That(expectedResult2, Is.EqualTo(System.Numerics.Vector2.Transform(v2, q2)).Using(Vector2Comparer.Epsilon));

        }

        [Test]
        public void TransformNormal()
        {
            var normal = new Vector2(1.5f, 2.5f);
            var matrix = new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            var expectedResult1 = new Vector2(14, 18);
            var expectedResult2 = expectedResult1;

            Assert.That(expectedResult1, Is.EqualTo(Vector2.TransformNormal(normal, matrix)).Using(Vector2Comparer.Epsilon));

            Vector2 result = Vector2.TransformNormal(normal, matrix);

            Assert.That(expectedResult2, Is.EqualTo(result).Using(Vector2Comparer.Epsilon));

        }

        [Test]
        [Ignore("Type Converter not applicable for System.Numerics.Vector2")]
        public void TypeConverter()
        {
            var converter = TypeDescriptor.GetConverter(typeof(Vector2));
            var invariantCulture = CultureInfo.InvariantCulture;

            Assert.AreEqual(new Vector2(32, 64), converter.ConvertFromString(null, invariantCulture, "32, 64"));
            Assert.AreEqual(new Vector2(0.5f, 2.75f), converter.ConvertFromString(null, invariantCulture, "0.5, 2.75"));
            Assert.AreEqual(new Vector2(1024.5f, 2048.75f), converter.ConvertFromString(null, invariantCulture, "1024.5, 2048.75"));
            Assert.AreEqual("32, 64", converter.ConvertToString(null, invariantCulture, new Vector2(32, 64)));
            Assert.AreEqual("0.5, 2.75", converter.ConvertToString(null, invariantCulture, new Vector2(0.5f, 2.75f)));
            Assert.AreEqual("1024.5, 2048.75", converter.ConvertToString(null, invariantCulture, new Vector2(1024.5f, 2048.75f)));

            var otherCulture = new CultureInfo("el-GR");
            var vectorStr = (1024.5f).ToString(otherCulture) + otherCulture.TextInfo.ListSeparator + " " +
                            (2048.75f).ToString(otherCulture);
            Assert.AreEqual(new Vector2(1024.5f, 2048.75f), converter.ConvertFromString(null, otherCulture, vectorStr));
            Assert.AreEqual(vectorStr, converter.ConvertToString(null, otherCulture, new Vector2(1024.5f, 2048.75f)));
        }

        [Test]
        public void HashCode()
        {
            // Checking for overflows in hash calculation.
            var max = new Vector2(float.MaxValue, float.MaxValue);
            var min = new Vector2(float.MinValue, float.MinValue);
            Assert.AreNotEqual(max.GetHashCode(), Vector2.Zero.GetHashCode());
            Assert.AreNotEqual(min.GetHashCode(), Vector2.Zero.GetHashCode());

            // Common values
            var a = new Vector2(0f, 0f);
            Assert.AreEqual(a.GetHashCode(), Vector2.Zero.GetHashCode());
            Assert.AreNotEqual(a.GetHashCode(), Vector2.One.GetHashCode());

            // Individual properties alter hash
            var xa = new Vector2(2f, 1f);
            var xb = new Vector2(3f, 1f);
            var ya = new Vector2(1f, 2f);
            var yb = new Vector2(1f, 3f);
            Assert.AreNotEqual(xa.GetHashCode(), xb.GetHashCode(), "Different properties should change hash.");
            Assert.AreNotEqual(ya.GetHashCode(), yb.GetHashCode(), "Different properties should change hash.");
#if !XNA
            Assert.AreNotEqual(xa.GetHashCode(), ya.GetHashCode(), "Identical values on different properties should have different hashes.");
            Assert.AreNotEqual(xb.GetHashCode(), yb.GetHashCode(), "Identical values on different properties should have different hashes.");
#endif
            Assert.AreNotEqual(xa.GetHashCode(), yb.GetHashCode());
            Assert.AreNotEqual(ya.GetHashCode(), xb.GetHashCode());
        }

#if !XNA
        [Test]
        public void ToPoint()
        {
            Assert.AreEqual(new Point(0, 0), new Vector2(0.1f, 0.1f).ToPoint());
            Assert.AreEqual(new Point(0, 0), new Vector2(0.5f, 0.5f).ToPoint());
            Assert.AreEqual(new Point(0, 0), new Vector2(0.55f, 0.55f).ToPoint());
            Assert.AreEqual(new Point(0, 0), new Vector2(1.0f - 0.1f, 1.0f - 0.1f).ToPoint());
            Assert.AreEqual(new Point(1, 1), new Vector2(1.0f - float.Epsilon, 1.0f - float.Epsilon).ToPoint());
            Assert.AreEqual(new Point(1, 1), new Vector2(1.0f, 1.0f).ToPoint());
            Assert.AreEqual(new Point(19, 27), new Vector2(19.033f, 27.1f).ToPoint());
        }

        [Test]
        public void Round()
        {
            Vector2 vector2 = new Vector2(0.4f, 0.6f);

            // CEILING

            Vector2 ceilMember = vector2;
            ceilMember.Ceiling();

            vector2.Ceiling(out Vector2 ceilResult);

            Assert.AreEqual(new Vector2(1.0f, 1.0f), ceilMember);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), ceilResult);

            // FLOOR

            Vector2 floorMember = vector2;
            floorMember.Floor();

            vector2.Floor(out Vector2 floorResult);

            Assert.AreEqual(new Vector2(0.0f, 0.0f), floorMember);
            Assert.AreEqual(new Vector2(0.0f, 0.0f), floorResult);

            // ROUND

            Vector2 roundMember = vector2;
            roundMember.Round();

            vector2.Round(out Vector2 roundResult);

            Assert.AreEqual(new Vector2(0.0f, 1.0f), roundMember);
            Assert.AreEqual(new Vector2(0.0f, 1.0f), roundResult);
        }
#endif
    }
}
