﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engr.Maths.Vectors;

namespace Engr.Octree.RenderTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var root = OctreeNode<object>.Filled(Vect3.Zero, 5, 0, new object());
            

            var t = new OctreeNode<object>(Vect3.Zero, 5, 0, root.Split());

            var tree = new Octree<object>(t);


            using (var win = new Window<object>(tree, node => Color.Blue))
            {
                win.Run();
                Console.ReadLine();
            }
        }
    }
}
