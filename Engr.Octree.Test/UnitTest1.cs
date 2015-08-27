using System;
using Engr.Geometry.Datums;
using Engr.Geometry.Primatives;
using Engr.Geometry.Shapes;
using Engr.Maths.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engr.Octree.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var oct = Octree<object>.Empty(20.0);
            Assert.IsTrue(oct.Root.IsEmpty());

            var s1 = new Sphere(Vect3.Zero, 4);


            var oct2 = oct.Intersect(node => s1.Intersects(node), new object());
            //var oct2 = oct.Interset(node => s1.Intersects(node),5);



        }
    }

    public static class temp
    {
        public static bool Intersects(this Sphere s, IAABB b)
        {
            return b.SqDistPointAABB(s.Center) <= Math.Pow(s.Radius, 2);
        }
        private static double SqDistPointAABB(this IAABB b, Vect3 p)
        {
            var sqDist = 0.0;
            if (p.X < b.Min.X) sqDist += (b.Min.X - p.X) * (b.Min.X - p.X);
            if (p.X > b.Max.X) sqDist += (p.X - b.Max.X) * (p.X - b.Max.X);

            if (p.Y < b.Min.Y) sqDist += (b.Min.Y - p.Y) * (b.Min.Y - p.Y);
            if (p.Y > b.Max.Y) sqDist += (p.Y - b.Max.Y) * (p.Y - b.Max.Y);

            if (p.Z < b.Min.Z) sqDist += (b.Min.Z - p.Z) * (b.Min.Z - p.Z);
            if (p.Z > b.Max.Z) sqDist += (p.Z - b.Max.Z) * (p.Z - b.Max.Z);

            return sqDist;
        }
    }
}
