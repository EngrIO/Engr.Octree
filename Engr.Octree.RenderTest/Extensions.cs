using System.Drawing;
using Engr.Maths.Matrices;
using Engr.Maths.Vectors;
using OpenTK;
using OpenTK.Graphics;

namespace Engr.Octree.RenderTest
{
    public static class Extensions
    {
        public static Vector4 ToVector4(this Color4 col)
        {

            return new Vector4(col.R, col.G, col.B, col.A);
        }

        public static Vector4 ToVector4(this Color col)
        {
            return new Vector4(col.R / 255f, col.G / 255f, col.B / 255f, col.A / 255f);
        }

        public static Vector3 ToVector3(this Vect3 v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Matrix4 ToMatrix4(this Mat4 m)
        {
            var md = m.Transpose(); //http://www.opentk.com/node/2771
            return new Matrix4(
                (float)md[1, 1], (float)md[1, 2], (float)md[1, 3], (float)md[1, 4],
                (float)md[2, 1], (float)md[2, 2], (float)md[2, 3], (float)md[2, 4],
                (float)md[3, 1], (float)md[3, 2], (float)md[3, 3], (float)md[3, 4],
                (float)md[4, 1], (float)md[4, 2], (float)md[4, 3], (float)md[4, 4]);
        }

        public static Mat4 ToMat4(this Matrix4 m)
        {
            return new Mat4(new double[,]
            {
                {m.M11, m.M21, m.M31, m.M41},
                {m.M12, m.M22, m.M32, m.M42},
                {m.M13, m.M23, m.M33, m.M43},
                {m.M14, m.M24, m.M34, m.M44}
            });
        }
        public static float[] ToColumnMajorArrayFloat(this Mat4 m)
        {
            return new[]{
                (float) m[1, 1],(float) m[2, 1],(float) m[3, 1],(float) m[4, 1],
                (float) m[1, 2],(float) m[2, 2],(float) m[3, 2],(float) m[4, 2],
                (float) m[1, 3],(float) m[2, 3],(float) m[3, 3],(float) m[4, 3],
                (float) m[1, 4],(float) m[2, 4],(float) m[3, 4],(float) m[4, 4]};
        }
    }
}