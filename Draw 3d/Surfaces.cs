using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Draw3D
{
    public class Surfaces
    {
        public Point3D[] surface;
        public long[] pointers;
        public Pen pen;
        public Brush brush;
        public long index;
        public long next;
        public long previous;
        public Surfaces()
        {
            surface = new Point3D[0];
            pointers = new long[0];
            pen = new Pen(Color.Brown, 1);
            brush = Brushes.BlanchedAlmond;
            index = -1;
            next = -1;
            previous = -1;
        }
        public Surfaces(long i, long n, Brush b)
        {
            surface = new Point3D[0];
            pointers = new long[0];
            pen = new Pen(Color.Brown, 1);
            brush = b;
            index = i;
            next = n;
            previous = i - 1;
        }
        public Surfaces(long i, long p, long n, Brush b)
        {
            surface = new Point3D[0];
            pointers = new long[0];
            pen = new Pen(Color.Brown, 1);
            brush = b;
            index = i;
            next = n;
            previous = p;
        }
        public void AddPoint(Point3D p)
        {
            AddPoint(p, 0);
        }
        public void AddPoint(Point3D p, long pointer)
        {
            Array.Resize(ref surface, (int)(surface.LongLength + 1));
            Array.Resize(ref pointers, (int)(pointers.LongLength + 1));
            surface[surface.LongLength - 1] = new Point3D(p);
            pointers[pointers.LongLength - 1] = pointer;
        }
        public float[,] GetLimits()
        {
            try
            {
                float[,] limits = new float[3, 2];
                limits[0, 0] = surface[0].X;
                limits[0, 1] = surface[0].X;
                limits[1, 0] = surface[0].Y;
                limits[1, 1] = surface[0].Y;
                limits[2, 0] = surface[0].Z;
                limits[2, 1] = surface[0].Z;
                for (long a = 1; a < surface.LongLength; a++)
                {
                    if (surface[a].X < limits[0, 0])
                        limits[0, 0] = surface[a].X;
                    if (surface[a].X > limits[0, 1])
                        limits[0, 1] = surface[a].X;
                    if (surface[a].X < limits[1, 0])
                        limits[1, 0] = surface[a].Y;
                    if (surface[a].X > limits[1, 1])
                        limits[1, 1] = surface[a].Y;
                    if (surface[a].X < limits[2, 0])
                        limits[2, 0] = surface[a].Z;
                    if (surface[a].X > limits[2, 1])
                        limits[2, 1] = surface[a].Z;
                }
                return limits;
            }
            catch
            {
                return new float[0, 0];
            }
        }
        public void XtoY()
        {
            float temp = 0;
            for (long a = 1; a < surface.LongLength; a++)
            {
                temp = surface[a].X;
                surface[a].X = surface[a].Y;
                surface[a].Y = temp;
            }
        }
        public void YtoZ()
        {
            float temp = 0;
            for (long a = 1; a < surface.LongLength; a++)
            {
                temp = surface[a].Y;
                surface[a].Y = surface[a].Z;
                surface[a].Z = temp;
            }
        }
        public void ZtoX()
        {
            float temp = 0;
            for (long a = 1; a < surface.LongLength; a++)
            {
                temp = surface[a].Z;
                surface[a].Z = surface[a].X;
                surface[a].X = temp;
            }
        }

        public Point3D GetAverage()
        {
            try
            {
                Point3D average = new Point3D(0,0,0);
                for (long a = 0; a < surface.LongLength; a++)
                {
                    average.X += surface[a].X;
                    average.Y += surface[a].Y;
                    average.Z += surface[a].Z;
                }
                average.X = average.X / surface.LongLength;
                average.Y = average.Y / surface.LongLength;
                average.Z = average.Z / surface.LongLength;
                return average;
            }
            catch
            {
                return new Point3D(0,0,0);
            }
        }
    }
}
