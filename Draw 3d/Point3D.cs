using System;
using System.Collections.Generic;
using System.Text;

namespace Draw3D
{
    public class Point3D
    {
        public float X;
        public float Y;
        public float Z;
        public float Q;
        public int[] lconnected;
        public int[] bconnected;
        public Point3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Q=0;
            lconnected = new int[0];
            bconnected = new int[0];
        }
        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            Q = 0;
            lconnected = new int[0];
            bconnected = new int[0];
        }
        public Point3D(float x, float y, float z, int[] Lconnected)
        {
            X = x;
            Y = y;
            Z = z;
            Q = 0;
            lconnected = Lconnected;
            bconnected = new int[0];
        }
        public Point3D(float x, float y, float z, int[] Lconnected, int[] Bconnected)
        {
            X = x;
            Y = y;
            Z = z;
            Q = 0;
            lconnected = Lconnected;
            bconnected = Bconnected;
        }
        public Point3D(float x, float y, float z, float Q, int[] Lconnected, int[] Bconnected)
        {
            X = x;
            Y = y;
            Z = z;
            Q = Q;
            lconnected = Lconnected;
            bconnected = Bconnected;
        }
        public Point3D(Point3D P)
        {
            X = P.X;
            Y = P.Y;
            Z = P.Z;
            Q = P.Q;
            lconnected = P.lconnected;
            bconnected = P.bconnected;
        }
        public void XtoY()
        {
            float t = X;
            X = Y;
            Y = t;
        }
        public void YtoZ()
        {
            float t = Y;
            Y = -Z;
            Z = -t;
        }
        public void ZtoX()
        {
            float t = Z;
            Z = -X;
            X = -t;
        }

        public void MovePoint(float x, float y, float z)
        {
            X += x;
            Y += y;
            Z += z;
        }

        public void RotatePointReal(float angle, float topx, float topy, float topz, float bottomx, float bottomy, float bottomz)
        {
            Point3D temp = new Point3D();
            if (topx != bottomx || topy != bottomy || topz != bottomz)
            {
                long q = 0;
                if (topx == bottomx && topy == bottomy)
                {
                    temp.X = X;
                    temp.Y = Y;
                    X = (float)(temp.X * Math.Cos((double)angle) - temp.Y * Math.Sin((double)angle));
                    Y = (float)(temp.Y * Math.Cos((double)angle) + temp.X * Math.Sin((double)angle));
                }
                else
                {
                    if (topx == bottomx && topz == bottomz)
                    {
                        temp.X = X;
                        temp.Z = Z;
                        X = (float)(temp.X * Math.Cos((double)angle) + temp.Z * Math.Sin((double)angle));
                        Z = (float)(temp.Z * Math.Cos((double)angle) - temp.X * Math.Sin((double)angle));
                    }
                    else
                    {
                        if (topy == bottomy && topz == bottomz)
                        {
                            temp.Y = Y;
                            temp.Z = Z;
                            Y = (float)(temp.Y * Math.Cos((double)angle) - temp.Z * Math.Sin((double)angle));
                            Z = (float)(temp.Z * Math.Cos((double)angle) + temp.Y * Math.Sin((double)angle));
                        }
                        else
                        {
                            if (topx == bottomx)
                            {
                                RotatePointReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                RotatePointReal(angle, 0, 0, 0, 0, 0, 1);
                                RotatePointReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                            }
                            else
                            {
                                if (topy == bottomy)
                                {
                                    RotatePointReal((float)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                    RotatePointReal(angle, 0, 0, 0, 1, 0, 0);
                                    RotatePointReal((float)(Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                }
                                else
                                {
                                    if (topz == bottomz)
                                    {
                                        RotatePointReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                        RotatePointReal(angle, 0, 0, 0, 1, 0, 0);
                                        RotatePointReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                    }
                                    else
                                    {
                                        RotatePointReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                        RotatePointReal(angle, topx, 0, topz, bottomx, 0, bottomz);
                                        RotatePointReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        public void RotatePoint(float angle, float topx, float topy, float topz, float bottomx, float bottomy, float bottomz)
        {
            if (topx != bottomx || topy != bottomy || topz != bottomz)
            {
                if (topx == bottomx && topy == bottomy)
                {
                    MovePoint(-topx, -topy, 0);
                    RotatePointReal(angle, 0, 0, 0, 0, 0, 1);
                    MovePoint(topx, topy, 0);
                }
                else
                {
                    if (topx == bottomx && topz == bottomz)
                    {
                        MovePoint(-topx, 0, -topz);
                        RotatePointReal(angle, 0, 0, 0, 0, 1, 0);
                        MovePoint(topx, 0, topz);
                    }
                    else
                    {
                        if (topy == bottomy && topz == bottomz)
                        {
                            MovePoint(0, -topy, -topz);
                            RotatePointReal(angle, 0, 0, 0, 1, 0, 0);
                            MovePoint(0, topy, topz);
                        }
                        else
                        {
                            if (topx == bottomx)
                            {
                                RotatePointReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                RotatePoint(angle, topx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), (float)(topz * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) + topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), bottomx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), bottomy = (float)(bottomy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - bottomz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))));
                                RotatePointReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                            }
                            else
                            {
                                if (topy == bottomy)
                                {
                                    RotatePointReal((float)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                    RotatePoint(angle, (float)(topx * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) + topz * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))), topy, (float)(topz * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) - topx * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))), bottomx = (float)(bottomx * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) + bottomz * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))), bottomy, (float)(topz * Math.Cos((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx)))) - topx * Math.Sin((double)(-Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))))));
                                    RotatePointReal((float)(Math.Atan((float)(topz - bottomz) / (float)(topx - bottomx))), 0, 0, 0, 0, 1, 0);
                                }
                                else
                                {
                                    if (topz == bottomz)
                                    {
                                        RotatePointReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                        RotatePoint(angle, (float)(bottomx * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) - topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), (float)(bottomy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) + topx * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), topz, (float)(bottomx * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) - topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), (float)(bottomy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx)))) + bottomx * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))))), bottomz);
                                        RotatePointReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topx - bottomx))), 0, 0, 0, 0, 0, 1);
                                    }
                                    else
                                    {
                                        RotatePointReal((float)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                        RotatePoint(angle, topx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), (float)(topz * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) + topy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), bottomx, (float)(topy * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) - topz * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))), (float)(bottomz * Math.Cos((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz)))) + bottomy * Math.Sin((double)(-Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))))));
                                        RotatePointReal((float)(Math.Atan((float)(topy - bottomy) / (float)(topz - bottomz))), 0, 0, 0, 1, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        public bool EqualTo(Point3D p)
        {
            return EqualTo(p.X, p.Y, p.Z);
        }
        public bool EqualTo(float x, float y, float z)
        {
            if (X == x && Y == y && Z == z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
