using System;
using System.Collections.Generic;
using System.Text;

namespace Draw3D
{
    public class Shape
    {
        public Point3D[] Points = new Point3D[0];
        public Presets preset = new Presets();
        public float Mass;
        public float SpeedX;
        public float SpeedY;
        public float SpeedZ;
        public bool full;
        public Point3D MassCenter;
        public Shape()
        {
            Mass = 0;
            MassCenter = new Point3D();
            full = false;
            SpeedX = 0;
            SpeedY = 0;
            SpeedZ = 0;
        }
        public Shape(Shape s)
        {
            Mass = s.Mass;
            MassCenter = new Point3D(s.MassCenter);
            full = s.full;
            SpeedX = s.SpeedX;
            SpeedY = s.SpeedY;
            SpeedZ = s.SpeedZ;
            RecievePoints(s.Points);
        }
        public void RecievePoints(Point3D[] a, Point3D b)
        {
            MassCenter = new Point3D(b);
            RecievePoints(a);
        }
        public void RecievePoints(Point3D[] a)
        {
            //Points=a;
            Points = new Point3D[a.LongLength];
            long x = 0;
            for (x = 0; x < a.LongLength; x++)
            {
                Points[x] = new Point3D(a[x]);
            }
        }
        public Point3D[] EdgePoints()
        {
            Point3D[] p = new Point3D[2];
            p[0] = new Point3D(0, 0, 0);
            p[1] = new Point3D(0, 0, 0);
            long k = 1;
            for (k = 1; k < Points.LongLength; k++)
            {
                if (Points[k].X < p[0].X)
                {
                    p[0].X = k;
                }
                else
                {
                    if (Points[k].X > p[1].X)
                    {
                        p[1].X = k;
                    }
                }
                if (Points[k].Y < p[0].Y)
                {
                    p[0].Y = k;
                }
                else
                {
                    if (Points[k].Y > p[1].Y)
                    {
                        p[1].Y = k;
                    }
                }
                if (Points[k].Z < p[0].Z)
                {
                    p[0].Z = k;
                }
                else
                {
                    if (Points[k].Z > p[1].Z)
                    {
                        p[1].Z = k;
                    }
                }
            }
            return p;
        }
        public void MoveShape(float x, float y, float z)
        {
            for (int q = 0; q < Points.LongLength; q++)
            {
                Points[q].MovePoint(x, y, z);
            }
            MassCenter.MovePoint(x, y, z);
        }
        public void MoveShapetopoint(float x, float y, float z)
        {
            Point3D Center = new Point3D(0, 0, 0);
            for (int q = 0; q < Points.LongLength; q++)
            {
                Center.X = Center.X + Points[q].X;
                Center.Y = Center.Y + Points[q].Y;
                Center.Z = Center.Z + Points[q].Z;
            }
            Center.X = Center.X / Points.LongLength;
            Center.Y = Center.Y / Points.LongLength;
            Center.Z = Center.Z / Points.LongLength;
            MoveShape(x - Center.X, y - Center.Y, z - Center.Z);
        }
        public void MoveShapetopoint(Point3D p)
        {
            MoveShapetopoint(p.X, p.Y, p.Z);
        }
        public void MoveShapebyangle(float xz, float yz, float length)
        {
            MoveShape((float)(length * Math.Sin((double)yz)), (float)(length * Math.Cos((double)yz) * Math.Cos((double)xz)), (float)(length * Math.Cos((double)yz) * Math.Sin((double)xz)));
        }
        public void ScaleShape(float Scalex, float Scaley, float Scalez)
        {
            Point3D Center = new Point3D(0, 0, 0);
            for (int q = 0; q < Points.LongLength; q++)
            {
                Center.X = Center.X + Points[q].X;
                Center.Y = Center.Y + Points[q].Y;
                Center.Z = Center.Z + Points[q].Z;
            }
            Center.X = Center.X / Points.LongLength;
            Center.Y = Center.Y / Points.LongLength;
            Center.Z = Center.Z / Points.LongLength;
            MoveShape(-1 * Center.X, -1 * Center.Y, -1 * Center.Z);
            ScaleShapebypoint(0, 0, 0, Scalex, Scaley, Scalez);
            MoveShape(Center.X, Center.Y, Center.Z);
        }
        public void ScaleShapebypoint(Point3D p, float Scalex, float Scaley, float Scalez)
        {
            ScaleShapebypoint(p.X, p.Y, p.Z, Scalex, Scaley, Scalez);
        }
        public void ScaleShapebypoint(float x, float y, float z, float Scalex, float Scaley, float Scalez)
        {
            MoveShape(-1 * x, -1 * y, -1 * z);
            for (int q = 0; q < Points.LongLength; q++)
            {
                Points[q].X = Points[q].X * Scalex;
                Points[q].Y = Points[q].Y * Scaley;
                Points[q].Z = Points[q].Z * Scalez;
            }
            MassCenter.X = MassCenter.X * Scalex;
            MassCenter.Y = MassCenter.Y * Scaley;
            MassCenter.Z = MassCenter.Z * Scalez;
            MoveShape(x, y, z);
        }
        public void MirrorShape(float topx, float topy, float topz, float bottomx, float bottomy, float bottomz)
        {
            RotateShape((float)Math.PI, topx, topy, topz, bottomx, bottomy, bottomz);
        }
        public void MirrorShape(Point3D top, Point3D bottom)
        {
            MirrorShape(top.X, top.Y, top.Z, bottom.X, bottom.Y, bottom.Z);
        }
        public void RotateShape(float angle, Point3D top, Point3D bottom)
        {
            RotateShape(angle, top.X, top.Y, top.Z, bottom.X, bottom.Y, bottom.Z);
        }
        public void RotateShape(float angle, float topx, float topy, float topz, float bottomx, float bottomy, float bottomz)
        {
            long q = 0;
            Point3D Center = new Point3D(0, 0, 0);
            for (q = 0; q < Points.LongLength; q++)
            {
                Center.X = Center.X + Points[q].X;
                Center.Y = Center.Y + Points[q].Y;
                Center.Z = Center.Z + Points[q].Z;
            }
            Center.X = Center.X / Points.LongLength;
            Center.Y = Center.Y / Points.LongLength;
            Center.Z = Center.Z / Points.LongLength;
            MoveShape(-1 * Center.X, -1 * Center.Y, -1 * Center.Z);
            RotateShapeReal(angle, topx, topy, topz, bottomx, bottomy, bottomz);
            MoveShape(Center.X, Center.Y, Center.Z);
        }

        public void RotateShapeReal(float angle, float topx, float topy, float topz, float bottomx, float bottomy, float bottomz)
        {
            Point3D temp = new Point3D();
            if (topx != bottomx || topy != bottomy || topz != bottomz)
            {
                long q = 0;
                if (topx == bottomx && topy == bottomy)
                {
                    for (q = 0; q < Points.LongLength; q++)
                    {
                        temp.X = Points[q].X;
                        temp.Y = Points[q].Y;
                        Points[q].X = (float)(temp.X * Math.Cos((double)angle) - temp.Y * Math.Sin((double)angle));
                        Points[q].Y = (float)(temp.Y * Math.Cos((double)angle) + temp.X * Math.Sin((double)angle));
                    }
                    temp.X = MassCenter.X;
                    temp.Y = MassCenter.Y;
                    MassCenter.X = (float)(temp.X * Math.Cos((double)angle) - temp.Y * Math.Sin((double)angle));
                    MassCenter.Y = (float)(temp.Y * Math.Cos((double)angle) + temp.X * Math.Sin((double)angle));
                }
                else
                {
                    if (topx == bottomx && topz == bottomz)
                    {
                        for (q = 0; q < Points.LongLength; q++)
                        {
                            temp.X = Points[q].X;
                            temp.Z = Points[q].Z;
                            Points[q].X = (float)(temp.X * Math.Cos((double)angle) + temp.Z * Math.Sin((double)angle));
                            Points[q].Z = (float)(temp.Z * Math.Cos((double)angle) - temp.X * Math.Sin((double)angle));
                        }
                        temp.X = MassCenter.X;
                        temp.Z = MassCenter.Z;
                        MassCenter.X = (float)(MassCenter.X * Math.Cos((double)angle) + MassCenter.Z * Math.Sin((double)angle));
                        MassCenter.Z = (float)(MassCenter.Z * Math.Cos((double)angle) - MassCenter.X * Math.Sin((double)angle));
                    }
                    else
                    {
                        if (topy == bottomy && topz == bottomz)
                        {
                            for (q = 0; q < Points.LongLength; q++)
                            {
                                temp.Y = Points[q].Y;
                                temp.Z = Points[q].Z;
                                Points[q].Y = (float)(temp.Y * Math.Cos((double)angle) - temp.Z * Math.Sin((double)angle));
                                Points[q].Z = (float)(temp.Z * Math.Cos((double)angle) + temp.Y * Math.Sin((double)angle));
                            }
                            temp.Y = MassCenter.Y;
                            temp.Z = MassCenter.Z;
                            MassCenter.Y = (float)(temp.Y * Math.Cos((double)angle) - temp.Z * Math.Sin((double)angle));
                            MassCenter.Z = (float)(temp.Z * Math.Cos((double)angle) + temp.Y * Math.Sin((double)angle));
                        }
                        else
                        {
                            if (topx == bottomx)
                            {
                                RotateShapeReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                RotateShapeReal(angle, 0, 0, 0, 0, 0, 1);
                                RotateShapeReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                            }
                            else
                            {
                                if (topy == bottomy)
                                {
                                    RotateShapeReal((float)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                    RotateShapeReal(angle, 0, 0, 0, 1, 0, 0);
                                    RotateShapeReal((float)(Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                }
                                else
                                {
                                    if (topz == bottomz)
                                    {
                                        RotateShapeReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                        RotateShapeReal(angle, 0, 0, 0, 1, 0, 0);
                                        RotateShapeReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                    }
                                    else
                                    {
                                        RotateShapeReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                        RotateShapeReal(angle, topx, 0, topz, bottomx, 0, bottomz);
                                        RotateShapeReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        public void RotateShapebyline(float angle, Point3D top, Point3D bottom)
        {
            RotateShapebyline(angle, top.X, top.Y, top.Z, bottom.X, bottom.Y, bottom.Z);
        }
        public void RotateShapebyline(float angle, float topx, float topy, float topz, float bottomx, float bottomy, float bottomz)
        {
            if (topx != bottomx || topy != bottomy || topz != bottomz)
            {
                long q = 0;
                if (topx == bottomx && topy == bottomy)
                {
                    MoveShape(-topx, -topy, 0);
                    RotateShapeReal(angle, 0, 0, 0, 0, 0, 1);
                    MoveShape(topx, topy, 0);
                }
                else
                {
                    if (topx == bottomx && topz == bottomz)
                    {
                        MoveShape(-topx, 0, -topz);
                        RotateShapeReal(angle, 0, 0, 0, 0, 1, 0);
                        MoveShape(topx, 0, topz);
                    }
                    else
                    {
                        if (topy == bottomy && topz == bottomz)
                        {
                            MoveShape(0, -topy, -topz);
                            RotateShapeReal(angle, 0, 0, 0, 1, 0, 0);
                            MoveShape(0, topy, topz);
                        }
                        else
                        {
                            if (topx == bottomx)
                            {
                                RotateShapeReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                RotateShapebyline(angle, topx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), (float)(topz * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) + topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), bottomx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), bottomy = (float)(bottomy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - bottomz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))));
                                RotateShapeReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                            }
                            else
                            {
                                if (topy == bottomy)
                                {
                                    RotateShapeReal((float)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                    RotateShapebyline(angle, (float)(topx * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) + topz * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))), topy, (float)(topz * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) - topx * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))), bottomx = (float)(bottomx * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) + bottomz * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))), bottomy, (float)(topz * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) - topx * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))));
                                    RotateShapeReal((float)(Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                }
                                else
                                {
                                    if (topz == bottomz)
                                    {
                                        RotateShapeReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                        RotateShapebyline(angle, (float)(bottomx * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) - topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), (float)(bottomy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) + topx * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), topz, (float)(bottomx * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) - topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), (float)(bottomy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) + bottomx * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), bottomz);
                                        RotateShapeReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                    }
                                    else
                                    {
                                        RotateShapeReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                        RotateShapebyline(angle, topx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), (float)(topz * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) + topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), bottomx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), (float)(bottomz * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) + bottomy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))));
                                        RotateShapeReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        public void XtoY()
        {
            long a = 0;
            for (a = 0; a < Points.LongLength; a++)
            {
                Points[a].XtoY();
            }
            MassCenter.XtoY();
        }
        public void YtoZ()
        {
            long a = 0;
            for (a = 0; a < Points.LongLength; a++)
            {
                Points[a].YtoZ();
            }
            MassCenter.YtoZ();
        }
        public void ZtoX()
        {
            long a = 0;
            for (a = 0; a < Points.LongLength; a++)
            {
                Points[a].ZtoX();
            }
            MassCenter.ZtoX();
        }
    }
}