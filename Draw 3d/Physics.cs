using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Draw3D
{
    class Physics
    {
        float Gap, speedlimit;
        public Physics(float g, float s)
        {
            Gap = g;
            speedlimit = s;
        }
        public Boolean PointOTriangleCollision(PointF P, PointF[] T)
        {
            if (T[2].X == 0 && T[2].Y == 0 && T[1].Y == 0)
            {
                PointF temp1 = T[2];
                PointF temp2 = T[1];
                PointF temp3 = T[0];
                float m = 0;
                if (T[2].X > T[1].X)
                {
                    T[1] = T[2];
                    T[2] = temp2;
                    P.X = P.X - T[2].X;
                    T[0].X = T[0].X - T[2].X;
                    T[1].X = T[1].X - T[2].X;
                    T[2].X = T[2].X - T[2].X;
                }
                if (T[0].Y < 0)
                {
                    T[0].Y = -1 * T[0].Y;
                    P.Y = -1 * P.Y;
                }
                m = (P.Y - T[0].Y) / (P.X - T[0].X);

                if (P.Y < 0 || P.Y > T[0].Y || P.X < T[2].X && P.X < T[0].X || P.X > T[0].X && P.X > T[1].X)
                {
                    return false;
                }
                else
                {
                    if (T[0].X > T[1].X || T[0].X < T[2].X)
                    {
                        if (P.X == T[0].X)
                        {
                            return false;
                        }
                        else
                        {
                            if (m < T[0].Y / T[0].X || m > T[0].Y / (T[0].X - T[1].X))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (P.X == T[0].X)
                        {
                            return true;
                        }
                        if (T[0].Y == T[2].Y)
                        {
                            if (m > T[0].Y / (T[0].X - T[1].X))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (T[0].Y == T[1].Y)
                            {
                                if (m < T[0].Y / T[0].X)
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (P.X < T[0].X && m < T[0].Y / T[0].X || P.X > T[0].X && m > T[0].Y / (T[0].X - T[1].X))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
        public Boolean LineSurfaceCollision(Point3D l1, Point3D l2, Point3D P1, Point3D P2, Point3D P3, bool infiniteline, bool infinitesurface)
        {
            Point3D Step4 = new Point3D(P3);
            PointF P0 = new PointF();
            PointF[] Triangle0 = new PointF[3];
            float Step3 = (float)(-1 * Math.PI / 2), Step2 = (float)(-1 * Math.PI / 2), Step1 = (float)(-1 * Math.PI / 2);

            if (P1.EqualTo(P2) || P1.EqualTo(P3) || P2.EqualTo(P3) || P1.X == P2.X && P1.X == P3.X && (P1.Z == P2.Z && P1.Z == P3.Z || (P2.Y - P3.Y) / (P2.Z - P3.Z) == (P1.Y - P3.Y) / (P1.Z - P3.Z)) || (P2.Y - P3.Y) / (P2.X - P3.X) == (P1.Y - P3.Y) / (P1.X - P3.X) && (P2.Z - P3.Z) / (P2.X - P3.X) == (P1.Z - P3.Z) / (P1.X - P3.X))
            {
                return false;
            }
            else
            {
                l1.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                l2.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P1.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P2.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P3.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);

                if (P2.Z == 0)
                {
                    Step3 = 0;
                }
                else
                {
                    if (P2.Y != 0)
                    {
                        Step3 = (float)(-1 * Math.Atan((double)(P2.Z / P2.Y)));
                    }
                    else
                    {
                        Step3 = Step1;
                    }
                }
                l1.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                l2.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P1.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P2.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P3.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);

                if (P2.Y == 0)
                {
                    Step2 = 0;
                }
                else
                {
                    if (P2.X != 0)
                    {
                        Step2 = (float)(-1 * Math.Atan((double)(P2.Y / P2.X)));
                    }
                    else
                    {
                        Step2 = Step1;
                    }
                }
                l1.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                l2.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P1.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P2.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P3.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);

                if (P1.Z == 0)
                {
                    Step1 = 0;
                }
                else
                {
                    if (P1.Y != 0)
                    {
                        Step1 = (float)(-1 * Math.Atan((double)(P1.Z / P1.Y)));
                    }
                    else
                    {
                        Step1 = Step1;
                    }
                }
                l1.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                l2.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P1.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P2.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P3.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);

                Triangle0[2] = new PointF(0F, 0F);
                Triangle0[1] = new PointF(P2.X, 0F);
                Triangle0[0] = new PointF(P1.X, P1.Y);

                if (l1.EqualTo(l2))
                {
                    if (l1.Z == 0)
                    {
                        P0.X = l1.X;
                        P0.Y = l1.Y;
                        return PointOTriangleCollision(P0, Triangle0);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (infinitesurface)
                    {
                        if (l1.Z == l2.Z)
                        {
                            if (l1.Z == 0)
                            {
                                return false;//
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (infiniteline || l1.Z * l2.Z <= 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (l1.Z == l2.Z)
                        {
                            if (l1.Z == 0)
                            {
                                return false;//
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (infiniteline || l1.Z * l2.Z <= 0)
                            {
                                P0.X = l2.X - l2.Z * (l1.X - l1.X) / (l1.Z - l2.Z);
                                P0.Y = l2.Y - l2.Z * (l1.Y - l1.Y) / (l1.Z - l2.Z);
                                return PointOTriangleCollision(P0, Triangle0);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        public Shape ShapeSurfaceCollision(Shape Shape, Point3D s1, Point3D s2, Point3D s3, bool infinitesurface)
        {
            Shape s = new Shape(Shape);
            Point3D speed = new Point3D(Shape.SpeedX, Shape.SpeedY, Shape.SpeedZ);
            Point3D P1 = new Point3D(s1);
            Point3D P2 = new Point3D(s2);
            Point3D P3 = new Point3D(s3);
            Point Closest = new Point(0, 0);
            Point3D Step4 = new Point3D(s3);
            long counter = 0;
            float Step3 = (float)(-1 * Math.PI / 2), Step2 = (float)(-1 * Math.PI / 2), Step1 = (float)(-1 * Math.PI / 2);
            if (P1.EqualTo(P2) || P1.EqualTo(P3) || P2.EqualTo(P3) || P1.X == P2.X && P1.X == P3.X && (P1.Z == P2.Z && P1.Z == P3.Z || (P2.Y - P3.Y) / (P2.Z - P3.Z) == (P1.Y - P3.Y) / (P1.Z - P3.Z)) || (P2.Y - P3.Y) / (P2.X - P3.X) == (P1.Y - P3.Y) / (P1.X - P3.X) && (P2.Z - P3.Z) / (P2.X - P3.X) == (P1.Z - P3.Z) / (P1.X - P3.X))
            {
                return Shape;
            }
            else
            {
                s.MoveShape(-Step4.X, -Step4.Y, -Step4.Z);
                P1.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P2.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P3.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);

                if (P2.Z == 0)
                {
                    Step3 = 0;
                }
                else
                {
                    if (P2.Y != 0)
                    {
                        Step3 = (float)(-1 * Math.Atan((double)(P2.Z / P2.Y)));
                    }
                    else
                    {
                        Step3 = Step1;
                    }
                }
                s.RotateShapebyline(Step3, 1, 0, 0, 0, 0, 0);
                speed.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P1.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P2.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P3.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);

                if (P2.Y == 0)
                {
                    Step2 = 0;
                }
                else
                {
                    if (P2.X != 0)
                    {
                        Step2 = (float)(-1 * Math.Atan((double)(P2.Y / P2.X)));
                    }
                    else
                    {
                        Step2 = Step1;
                    }
                }
                s.RotateShapebyline(Step2, 0, 0, 0, 0, 0, 1);
                speed.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P1.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P2.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P3.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);

                if (P1.Z == 0)
                {
                    Step1 = 0;
                }
                else
                {
                    if (P1.Y != 0)
                    {
                        Step1 = (float)(-1 * Math.Atan((double)(P1.Z / P1.Y)));
                    }
                    else
                    {
                        Step1 = Step1;
                    }
                }
                s.RotateShapebyline(Step1, 1, 0, 0, 0, 0, 0);
                speed.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P1.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P2.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P3.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                PointF[] Tri = new PointF[3];
                Tri[0] = new PointF(P1.X, P1.Y);
                Tri[1] = new PointF(P2.X, 0);
                Tri[2] = new PointF(0, 0);

                Point3D Max = new Point3D(s.EdgePoints()[1]);
                Point3D Min = new Point3D(s.EdgePoints()[0]);
                for (counter = 0; counter < s.Points.LongLength; counter++)
                {
                    if (s.Points[counter].Z >= 0 && s.Points[counter].Z < s.Points[Closest.X].Z)
                    {
                        Closest.X = (int)(counter);
                    }
                    else
                    {
                        if (s.Points[counter].Z < 0 && s.Points[counter].Z > s.Points[Closest.Y].Z)
                        {
                            Closest.Y = (int)(counter);
                        }
                    }
                }

                if (s.Points[(int)(Min.Z)].Z > Gap || s.Points[(int)(Max.Z)].Z < -1 * Gap)
                {
                    return Shape;
                }
                else
                {
                    if (infinitesurface)
                    {

                        if (speed.Z < speedlimit && speed.Z > -1 * speedlimit)
                            speed.Z = 0;
                        else
                        {
                            if (s.Points[(int)(Min.Z)].Z > -1 * Gap)
                            {
                                speed.Z = Math.Abs(speed.Z);
                            }
                            else
                            {
                                if (s.Points[(int)(Max.Z)].Z < Gap)
                                {
                                    speed.Z = -1 * Math.Abs(speed.Z);
                                }
                                else
                                {
                                    if (s.Points[(int)(Max.Z)].Z > Math.Abs(s.Points[(int)(Min.Z)].Z))
                                    {
                                        speed.Z = Math.Abs(speed.Z);
                                    }
                                    else
                                    {
                                        speed.Z = -1 * Math.Abs(speed.Z);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (s.Points[(int)(Min.Z)].Z > -1 * Gap)
                        {
                            if (PointOTriangleCollision(new PointF(s.Points[(int)(Min.Z)].X, s.Points[(int)(Min.Z)].Y), Tri))
                            {
                                if (speed.Z < speedlimit && speed.Z > -1 * speedlimit)
                                    speed.Z = 0;
                                else
                                    speed.Z = Math.Abs(speed.Z);
                            }
                            else
                            {
                                return Shape;
                            }
                        }
                        else
                        {
                            if (s.Points[(int)(Max.Z)].Z < Gap)
                            {
                                if (PointOTriangleCollision(new PointF(s.Points[(int)(Max.Z)].X, s.Points[(int)(Max.Z)].Y), Tri))
                                {
                                    if (speed.Z < speedlimit && speed.Z > -1 * speedlimit)
                                        speed.Z = 0;
                                    else
                                        speed.Z = -1 * Math.Abs(speed.Z);
                                }
                                else
                                {
                                    return Shape;
                                }
                            }
                            else
                            {
                                PointF Middle = new PointF(s.Points[Closest.X].Z - s.Points[Closest.X].X * (s.Points[Closest.X].Z - s.Points[Closest.Y].Z) / (s.Points[Closest.X].X - s.Points[Closest.Y].X), s.Points[Closest.X].Z - s.Points[Closest.X].Y * (s.Points[Closest.X].Z - s.Points[Closest.Y].Z) / (s.Points[Closest.X].Y - s.Points[Closest.Y].Y));
                                if (s.Points[Closest.X].X == s.Points[Closest.Y].X)
                                {
                                    Middle.X = s.Points[Closest.X].X;
                                }
                                if (s.Points[Closest.X].Y == s.Points[Closest.Y].Y)
                                {
                                    Middle.Y = s.Points[Closest.X].Y;
                                }
                                if (PointOTriangleCollision(Middle, Tri))
                                {
                                    if (speed.Z < speedlimit && speed.Z > -1 * speedlimit)
                                        speed.Z = 0;
                                    else
                                    {
                                        if (s.Points[(int)(Max.Z)].Z > Math.Abs(s.Points[(int)(Min.Z)].Z))
                                        {
                                            speed.Z = Math.Abs(speed.Z);
                                        }
                                        else
                                        {
                                            speed.Z = -1 * Math.Abs(speed.Z);
                                        }
                                    }
                                }
                                else
                                {
                                    return Shape;
                                }
                            }
                        }
                    }
                    speed.RotatePoint(-Step1, 1, 0, 0, 0, 0, 0);
                    speed.RotatePoint(-Step2, 0, 0, 0, 0, 0, 1);
                    speed.RotatePoint(-Step3, 1, 0, 0, 0, 0, 0);
                    Shape.SpeedX = speed.X;
                    Shape.SpeedY = speed.Y;
                    Shape.SpeedZ = speed.Z;
                    return Shape;
                }
            }
        }

        public Shape SphereSurfaceCollision(Shape Sphere, float Radius, Point3D s1, Point3D s2, Point3D s3, bool infinitesurface)
        {
            Point3D Center = new Point3D(Sphere.MassCenter);
            Point3D speed = new Point3D(Sphere.SpeedX, Sphere.SpeedY, Sphere.SpeedZ);
            Point3D P1 = new Point3D(s1);
            Point3D P2 = new Point3D(s2);
            Point3D P3 = new Point3D(s3);
            Point3D Step4 = new Point3D(s3);
            float Step3 = (float)(-1 * Math.PI / 2), Step2 = (float)(-1 * Math.PI / 2), Step1 = (float)(-1 * Math.PI / 2);
            if (P1.EqualTo(P2) || P1.EqualTo(P3) || P2.EqualTo(P3) || P1.X == P2.X && P1.X == P3.X && (P1.Z == P2.Z && P1.Z == P3.Z || (P2.Y - P3.Y) / (P2.Z - P3.Z) == (P1.Y - P3.Y) / (P1.Z - P3.Z)) || (P2.Y - P3.Y) / (P2.X - P3.X) == (P1.Y - P3.Y) / (P1.X - P3.X) && (P2.Z - P3.Z) / (P2.X - P3.X) == (P1.Z - P3.Z) / (P1.X - P3.X))
            {
                return Sphere;
            }
            else
            {
                Center.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P1.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P2.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);
                P3.MovePoint(-Step4.X, -Step4.Y, -Step4.Z);

                if (P2.Z == 0)
                {
                    Step3 = 0;
                }
                else
                {
                    if (P2.Y != 0)
                    {
                        Step3 = (float)(-1 * Math.Atan((double)(P2.Z / P2.Y)));
                    }
                    else
                    {
                        Step3 = Step1;
                    }
                }
                Center.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                speed.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P1.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P2.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);
                P3.RotatePoint(Step3, 1, 0, 0, 0, 0, 0);

                if (P2.Y == 0)
                {
                    Step2 = 0;
                }
                else
                {
                    if (P2.X != 0)
                    {
                        Step2 = (float)(-1 * Math.Atan((double)(P2.Y / P2.X)));
                    }
                    else
                    {
                        Step2 = Step1;
                    }
                }
                Center.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                speed.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P1.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P2.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);
                P3.RotatePoint(Step2, 0, 0, 0, 0, 0, 1);

                if (P1.Z == 0)
                {
                    Step1 = 0;
                }
                else
                {
                    if (P1.Y != 0)
                    {
                        Step1 = (float)(-1 * Math.Atan((double)(P1.Z / P1.Y)));
                    }
                    else
                    {
                        Step1 = Step1;
                    }
                }
                Center.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                speed.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P1.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P2.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                P3.RotatePoint(Step1, 1, 0, 0, 0, 0, 0);
                PointF[] Tri = new PointF[3];
                Tri[0] = new PointF(P1.X, P1.Y);
                Tri[1] = new PointF(P2.X, 0);
                Tri[2] = new PointF(0, 0);

                if (infinitesurface || PointOTriangleCollision(new PointF(Center.X, Center.Y), Tri))
                {
                    if (Center.Z == 0)
                    {
                        speed.Z = -1 * speed.Z;
                        speed.RotatePoint(-Step1, 1, 0, 0, 0, 0, 0);
                        speed.RotatePoint(-Step2, 0, 0, 0, 0, 0, 1);
                        speed.RotatePoint(-Step3, 1, 0, 0, 0, 0, 0);
                        Sphere.SpeedX = speed.X;
                        Sphere.SpeedY = speed.Y;
                        Sphere.SpeedZ = speed.Z;
                    }
                    else
                    {
                        if (Center.Z > 0 && Center.Z - Radius < Gap)
                        {
                            speed.Z = Math.Abs(speed.Z);
                            speed.RotatePoint(-Step1, 1, 0, 0, 0, 0, 0);
                            speed.RotatePoint(-Step2, 0, 0, 0, 0, 0, 1);
                            speed.RotatePoint(-Step3, 1, 0, 0, 0, 0, 0);
                            Sphere.SpeedX = speed.X;
                            Sphere.SpeedY = speed.Y;
                            Sphere.SpeedZ = speed.Z;
                        }
                        else
                        {
                            if (Center.Z < 0 && Center.Z + Radius > -1 * Gap)
                            {
                                speed.Z = -1 * Math.Abs(speed.Z);
                                speed.RotatePoint(-Step1, 1, 0, 0, 0, 0, 0);
                                speed.RotatePoint(-Step2, 0, 0, 0, 0, 0, 1);
                                speed.RotatePoint(-Step3, 1, 0, 0, 0, 0, 0);
                                Sphere.SpeedX = speed.X;
                                Sphere.SpeedY = speed.Y;
                                Sphere.SpeedZ = speed.Z;
                            }
                        }
                    }
                }
                return Sphere;
            }
        }

        public Boolean PointOTriangleCollision(Point3D P, Point3D[] T)
        {
            PointF[] T2 = new PointF[3];
            T2[0] = new PointF(T[0].X, T[0].Y);
            T2[1] = new PointF(T[1].X, 0);
            T2[0] = new PointF(0, 0);
            return PointOTriangleCollision(new PointF(P.X, P.Y), T2);
        }
        public Boolean PointOTriangleCollision(Point3D P, Point3D t1, Point3D t2, Point3D t3)
        {
            PointF[] T2 = new PointF[3];
            T2[0] = new PointF(t1.X, t1.Y);
            T2[1] = new PointF(t2.X, 0);
            T2[0] = new PointF(0, 0);
            return PointOTriangleCollision(new PointF(P.X, P.Y), T2);
        }
        public Boolean PointOTriangleCollision(float Px, float Py, Point3D[] T)
        {
            PointF[] T2 = new PointF[3];
            T2[0] = new PointF(T[0].X, T[0].Y);
            T2[1] = new PointF(T[1].X, 0);
            T2[0] = new PointF(0, 0);
            return PointOTriangleCollision(new PointF(Px, Py), T2);
        }
        public Boolean PointOTriangleCollision(float Px, float Py, float P1x, float P1y, float P2x)
        {
            PointF[] T2 = new PointF[3];
            T2[0] = new PointF(P1x, P1y);
            T2[1] = new PointF(P2x, 0);
            T2[0] = new PointF(0, 0);
            return PointOTriangleCollision(new PointF(Px, Py), T2);
        }

        public Boolean LineSurfaceCollision(Point3D l1, Point3D l2, Point3D[] s, bool infiniteline, bool infinitesurface)
        {
            return LineSurfaceCollision(l1, l2, s[0], s[1], s[2], infiniteline, infinitesurface);
        }
        public Boolean LineSurfaceCollision(Point3D l1, Point3D l2, Point3D[] s, bool infiniteline)
        {
            return LineSurfaceCollision(l1, l2, s[0], s[1], s[2], infiniteline, false);
        }
        public Boolean LineSurfaceCollision(Point3D l1, Point3D l2, Point3D[] s)
        {
            return LineSurfaceCollision(l1, l2, s[0], s[1], s[2]);
        }
        public Boolean LineSurfaceCollision(Point3D l1, Point3D l2, Point3D s1, Point3D s2, Point3D s3, bool infiniteline)
        {
            return LineSurfaceCollision(l1, l2, s1, s2, s3, infiniteline, false);
        }
        public Boolean LineSurfaceCollision(Point3D l1, Point3D l2, Point3D s1, Point3D s2, Point3D s3)
        {
            return LineSurfaceCollision(l1, l2, s1, s2, s3, false, false);
        }

        public Shape ShapeSurfaceCollision(Shape Shape, Point3D[] s)
        {
            return ShapeSurfaceCollision(Shape, s[0], s[1], s[2], false);
        }
        public Shape ShapeSurfaceCollision(Shape Shape, Point3D[] s, bool infinitesurface)
        {
            return ShapeSurfaceCollision(Shape, s[0], s[1], s[2], infinitesurface);
        }
        public Shape ShapeSurfaceCollision(Shape Shape, Point3D s1, Point3D s2, Point3D s3)
        {
            return ShapeSurfaceCollision(Shape, s1, s2, s3, false);
        }
    }
}
