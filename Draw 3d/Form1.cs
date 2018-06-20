using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Draw3D
{
    public partial class Form1 : Form
    {
        //Rotate is Clockwise
        //Parameters
        float gravity = 5, windlimit = 0.01F, windspeed = 10, gridunits = 7.5f, fuse = 1.5F, speedzerolimit = 0.005F;
        double interval = 100;
        long targetdistance = 15, minimumtargetdistance = 3;
        bool windspeedlimit = true;
        //Parameters

        Graphics g;
        Presets Presets = new Presets();
        Physics Phys = new Physics(0.001F, 0.00001F);
        double time = 0;
        bool hit = false, clrscrn = true;
        long CurrentSurface = 0, HighSurfaceIndex = 2, cursor = 65555, helpcursor = 65579;
        long hits = 0, misses = 0, score = 0, level = 0;
        float cannonforce = 0, cannonspeed = 0, WindAccelaration = 0, WindForce = 0, WindDirection = 0;
        Double convert = 50, angle = 0, angleYZ = 0, ballangleXY = 0, ballangleXZ = 0, ballangleXY2 = 0, ballangleXZ2 = 180;
        Shape[] shape = new Shape[1];
        Point3D[] trailpoints = new Point3D[0];
        PointF[] drawtrailpoints = new PointF[0];
        Point3D roomorigin = new Point3D(-10, -10, -10), roomend = new Point3D(10, 10, 10), perspective = new Point3D(0, 0, -100), cannonorigin = new Point3D(0, 0, 0), targetloc = new Point3D(0, 0, 0);
        Point3D[][] surface = new Point3D[1][];
        Shape CannonBall = new Shape();
        Point3D[] room = new Point3D[8];
        Point3D[] target = new Point3D[32];
        Point3D[][] DrawCannon = new Point3D[3][];
        Point3D MouseDownXY = new Point3D(0, 0, 0), MouseDownXZ = new Point3D(0, 0, 0), MouseDownAngleXY = new Point3D(0, 0, 0), MouseDownAngleXZ = new Point3D(0, 0, 0);

        //game 2 (pong)
        Point3D TunnelSize = new Point3D(3F, 3, 10);
        Point3D PaddleLocation = new Point3D(0, 0, 0);
        Point3D PaddleLocation2 = new Point3D(0, 0, 11);
        float paddlesize = 0.5F;
        float paddlespeed = 0.0001F;

        long KeyPressed = -1;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }
        public PointF Transform3D(Point3D p)
        {
            return Transform3D(p.X, p.Y, p.Z);
        }
        public PointF Transform3D(float x, float y, float z)
        {
            PointF p = new PointF();
            float tx = 0, ty = 0;
            if (Perspective.Checked)
            {
                y = perspective.Z * y / (perspective.Z - z);
                x = perspective.Z * x / (perspective.Z - z);
            }
            tx = (float)(z * Math.Sin((float)(angleYZ)) * Math.Cos(Math.PI - angle) + x);
            ty = (float)(z * Math.Sin((float)(angleYZ)) * Math.Sin(Math.PI - angle) + y);
            tx = (float)(tx * convert + panel1.Width / 2);
            ty = (float)(panel1.Height / 2 - ty * convert);

            p = new PointF(tx, ty);
            return p;
        }
        public void DrawShape(Shape s)
        {
            DrawShape(s, Pens.Brown);
        }
        public void DrawShape(Shape s, Pen pen)
        {
            long x = 0, y = 0;
            PointF[] d = new PointF[s.Points.LongLength];
            Shape t = new Shape();
            t.RecievePoints(s.Points, s.MassCenter);
            if (this.Views.SelectedIndex == 1)
            {
                t.YtoZ();
            }
            else
                if (this.Views.SelectedIndex == 2)
                {
                    t.ZtoX();
                }
            for (x = 0; x < t.Points.Length; x++)
            {
                d[x] = new PointF(Transform3D(t.Points[x]).X, Transform3D(t.Points[x]).Y);
            }
            for (x = 0; x < t.Points.Length; x++)
            {
                for (y = 0; y < t.Points[x].lconnected.Length; y++)
                {
                    if (t.Points[x].lconnected[y] > x + 1)
                    {
                        if (d[x].X < 0 && d[t.Points[x].lconnected[y] - 1].X < 0) ;
                        else if (d[x].X > panel1.Width && d[t.Points[x].lconnected[y] - 1].X > panel1.Width) ;
                        else if (d[x].Y < 0 && d[t.Points[x].lconnected[y] - 1].Y < 0) ;
                        else if (d[x].Y > panel1.Height && d[t.Points[x].lconnected[y] - 1].Y > panel1.Height) ;
                        else
                        {
                            g.DrawLine(pen, d[x].X, d[x].Y, d[t.Points[x].lconnected[y] - 1].X, d[t.Points[x].lconnected[y] - 1].Y);
                        }
                    }
                }
            }
            if (this.Views.SelectedIndex == 1)
            {
                for (x = 0; x < t.Points.Length; x++)
                {
                    t.Points[x].YtoZ();
                }
            }
            else
                if (this.Views.SelectedIndex == 2)
                {
                    for (x = 0; x < t.Points.Length; x++)
                    {
                        t.Points[x].ZtoX();
                    }
                }
        }
        public void DrawSurfacesList(Surfaces[] s)
        {
            long a = 0, b = 0;
            PointF[] drawfront = new PointF[0];
            if (this.Views.SelectedIndex == 1)
            {
                for (a = 0; a < s.LongLength; a++)
                    s[a].YtoZ();
            }
            else
            {
                if (this.Views.SelectedIndex == 2)
                {
                    for (a = 0; a < s.LongLength; a++)
                        s[a].ZtoX();
                }
            }
            for (a = 0; a < -1; )
            {
                for (b = s[a].next; b > -1; )
                {
                    if (s[b].GetAverage().Z > s[a].GetAverage().Z)
                    {
                        s[s[b].next].previous = s[b].previous;
                        s[s[b].previous].next = s[b].next;
                        s[s[a].previous].next = b;
                        s[b].previous = s[a].previous;
                        s[a].previous = b;
                        s[b].next = a;
                        a = b;
                    }
                    b = s[b].next;
                }
                a = s[a].next;
            }
            for (a = 0; a < s.LongLength && a > -1; )
            {
                b = a;
                a = s[a].previous;
            }
            bool XY = true;
            for (; b > -1; )
            {
                drawfront = new PointF[s[b].surface.Length];
                for (a = 0; a < drawfront.Length; a++)
                {
                    drawfront[(int)a] = Transform3D(s[b].surface[(int)a]);
                    if (s[b].surface[(int)a].Z <= 0)
                    {
                        XY = false;
                    }
                }
                try
                {
                    g.FillPolygon(s[b].brush, drawfront);
                    g.DrawPolygon(s[b].pen, drawfront);
                    if (XY)
                    {
                        g.DrawLine(Pens.Black, drawfront[0].X, panel1.Height / 2, drawfront[1].X, panel1.Height / 2);
                        g.DrawLine(Pens.Black, drawfront[2].X, panel1.Height / 2, drawfront[1].X, panel1.Height / 2);
                        g.DrawLine(Pens.Black, panel1.Width / 2, drawfront[0].Y, panel1.Width / 2, drawfront[1].Y);
                        g.DrawLine(Pens.Black, panel1.Width / 2, drawfront[2].Y, panel1.Width / 2, drawfront[1].Y);
                    }
                }
                catch { }
                b = s[b].next;
            }
        }
        public Shape SortShapeSurfaces(Shape shape)
        {
            if (this.Views.SelectedIndex == 1)
            {
                shape.YtoZ();
            }
            else
                if (this.Views.SelectedIndex == 2)
                {
                    shape.ZtoX();
                }
            long a = 0, b = 0;
            Surfaces[] surfaces = new Surfaces[0];
            Point3D temp = new Point3D(0, 0, 0);
            for (a = 0; a < shape.Points.LongLength; a++)
            {
                if (shape.Points[a].bconnected.LongLength > 2)
                {
                    Array.Resize(ref surfaces, (int)(surfaces.Length + 1));
                    surfaces[a] = new Surfaces();
                    for (b = 0; b < shape.Points[a].bconnected.LongLength; b++)
                    {
                        surfaces[a].AddPoint(shape.Points[shape.Points[a].bconnected[b] - 1], shape.Points[a].bconnected[b]);
                    }
                }
                else
                {
                    a = shape.Points.LongLength;
                }
            }
            Point3D[] Averages = new Point3D[surfaces.LongLength];
            for (a = 0; a < surfaces.LongLength; a++)
            {
                Averages[a] = new Point3D(surfaces[a].GetAverage());
                Averages[a].Q = a;
            }
            for (a = 0; a < Averages.LongLength - 1; a++)
            {
                for (b = a + 1; b < Averages.LongLength; b++)
                {
                    if (Averages[b].Z > Averages[a].Z)
                    {
                        temp = new Point3D(Averages[a]);
                        Averages[a] = new Point3D(Averages[b]);
                        Averages[b] = new Point3D(temp);
                    }
                }
            }

            for (a = 0; a < surfaces.LongLength; a++)
            {
                Array.Resize(ref shape.Points[a].bconnected, (int)(surfaces[(int)(Averages[a].Q)].pointers.LongLength));
                for (b = 0; b < shape.Points[a].bconnected.LongLength; b++)
                {
                    shape.Points[a].bconnected[b] = (int)(surfaces[(int)(Averages[a].Q)].pointers[b]);
                }
            }
            if (this.Views.SelectedIndex == 1)
            {
                for (a = 0; a < shape.Points.Length; a++)
                {
                    shape.Points[a].YtoZ();
                }
            }
            else
                if (this.Views.SelectedIndex == 2)
                {
                    for (a = 0; a < shape.Points.Length; a++)
                    {
                        shape.Points[a].ZtoX();
                    }
                }
            return shape;
        }
        public void FillShapeBit(Shape shape, long x)
        {
            FillShapeBit(shape, x, Brushes.Brown, Pens.Brown);
        }
        public void FillShapeBit(Shape shape, long x, Brush brush)
        {
            FillShapeBit(shape, x, brush, Pens.Brown);
        }
        public void FillShapeBit(Shape shape, long x, Brush brush, Pen p)
        {
            Shape s = new Shape();
            bool XY = true;
            s.RecievePoints(shape.Points);
            if (this.Views.SelectedIndex == 1)
            {
                s.YtoZ();
            }
            else
                if (this.Views.SelectedIndex == 2)
                {
                    s.ZtoX();
                }
            PointF[] drawfront = new PointF[s.Points[x].bconnected.Length];
            int a = 0;
            for (a = 0; a < s.Points[x].bconnected.Length; a++)
            {
                drawfront[(int)a] = Transform3D(s.Points[s.Points[x].bconnected[(int)a] - 1]);
                if (s.Points[s.Points[x].bconnected[(int)a] - 1].Z <= 0)
                {
                    XY = false;
                }
            }
            try
            {
                g.FillPolygon(brush, drawfront);
                g.DrawPolygon(p, drawfront);
                if (XY)
                {
                    g.DrawLine(Pens.Black, drawfront[0].X, panel1.Height / 2, drawfront[1].X, panel1.Height / 2);
                    g.DrawLine(Pens.Black, drawfront[2].X, panel1.Height / 2, drawfront[1].X, panel1.Height / 2);
                    g.DrawLine(Pens.Black, panel1.Width / 2, drawfront[0].Y, panel1.Width / 2, drawfront[1].Y);
                    g.DrawLine(Pens.Black, panel1.Width / 2, drawfront[2].Y, panel1.Width / 2, drawfront[1].Y);
                }
            }
            catch { }
            if (this.Views.SelectedIndex == 1)
            {
                s.YtoZ();
            }
            else
                if (this.Views.SelectedIndex == 2)
                {
                    s.ZtoX();
                }
        }
        public Point3D[] SortbyXYorZ(Point3D[] p, int type)
        {
            return p;
        }

        public void panel1_Paint(object sender, PaintEventArgs e)
        {
            int x, y, z;
            float a;
            Shape temps = new Shape();
            Surfaces[] SurfaceList = new Surfaces[0];
            g = panel1.CreateGraphics();
            Point3D drawperspective = new Point3D();
            Point3D temppoint = new Point3D(0, 0, 0);
            Array.Resize(ref perspective.lconnected, 1);
            try
            {
                if (CannonBall.Mass != float.Parse(Ballmass.Text))
                {
                    CannonBall.Mass = float.Parse(Ballmass.Text);
                }
            }
            catch
            {
            }
            //cannon time
            {
                int[] temparray;
                Point3D[] pfront;
                PointF[] drawfront;
                ballangleXY = -(ballangleXY2 * 2) - 53.5;
                ballangleXZ = ballangleXZ2;
                {
                    if (gridcheck.Checked)
                    {
                        temps.RecievePoints(Presets.GridPoints(gridunits, targetdistance));
                        DrawShape(temps, Pens.PaleGoldenrod);
                    }
                    g.DrawLine(Pens.Black, 0, panel1.Height / 2, panel1.Width, panel1.Height / 2);
                    for (a = panel1.Width / 2; a <= panel1.Width; a += (float)convert)
                        g.DrawLine(Pens.Black, a, panel1.Height / 2, a, panel1.Height / 2 + 5);
                    for (a = panel1.Width / 2; a >= 0; a -= (float)convert)
                        g.DrawLine(Pens.Black, a, panel1.Height / 2, a, panel1.Height / 2 + 5);
                    g.DrawLine(Pens.Black, panel1.Width / 2, 0, panel1.Width / 2, panel1.Height);
                    for (a = panel1.Height / 2; a <= panel1.Height; a += (float)convert)
                        g.DrawLine(Pens.Black, panel1.Width / 2, a, panel1.Width / 2 - 5, a);
                    for (a = panel1.Height / 2; a >= 0; a -= (float)convert)
                        g.DrawLine(Pens.Black, panel1.Width / 2, a, panel1.Width / 2 - 5, a);
                    if (Perspective.Checked)
                    {
                        perspective.lconnected[0] = 1;
                    }
                    else
                    {
                        perspective.lconnected[0] = 0;
                    }
                    if (Perspective.Checked)
                    {
                        Point3D tempperspective = new Point3D();
                        tempperspective.X = (float)(-1 * perspective.Z * Math.Sin((float)(angleYZ)) * Math.Cos(Math.PI - angle) + perspective.X);
                        tempperspective.Y = (float)(-1 * perspective.Z * Math.Sin((float)(angleYZ)) * Math.Sin(Math.PI - angle) + perspective.Y);
                        if (tempperspective.X != 0)
                        {
                            tempperspective.Z = (float)((tempperspective.Y) / (tempperspective.X));
                            tempperspective.Q = (float)(0);
                            if (tempperspective.Z > 0.001 || tempperspective.Z < -0.001)
                            {
                                if (tempperspective.X < 0)
                                {
                                    if (((panel1.Width / 2) / convert) * tempperspective.Z + tempperspective.Q > (panel1.Height / -2) / convert || ((panel1.Width / 2) / convert) * tempperspective.Z + tempperspective.Q < (panel1.Height / 2) / convert)
                                    {
                                        if (tempperspective.Y < 0)
                                        {
                                            g.DrawLine(Pens.Black, (float)((((panel1.Height / 2) / convert - tempperspective.Q) / tempperspective.Z) * convert + panel1.Width / 2), 0, (float)(tempperspective.X * convert + panel1.Width / 2), (float)(panel1.Height / 2 - tempperspective.Y * convert));
                                        }
                                        else
                                        {
                                            g.DrawLine(Pens.Black, (float)((((panel1.Height / -2) / convert - tempperspective.Q) / tempperspective.Z) * convert + panel1.Width / 2), panel1.Height, (float)(tempperspective.X * convert + panel1.Width / 2), (float)(panel1.Height / 2 - tempperspective.Y * convert));
                                        }
                                    }
                                    else
                                    {
                                        g.DrawLine(Pens.Black, panel1.Width, (float)(panel1.Height / 2 - convert * (((panel1.Width / 2) / convert) * tempperspective.Z + tempperspective.Q)), (float)(tempperspective.X * convert + panel1.Width / 2), (float)(panel1.Height / 2 - tempperspective.Y * convert));
                                    }
                                }
                                else
                                {
                                    if (((panel1.Width / 2) / convert) * tempperspective.Z + tempperspective.Q > (panel1.Height / -2) / convert || ((panel1.Width / 2) / convert) * tempperspective.Z + tempperspective.Q < (panel1.Height / 2) / convert)
                                    {
                                        if (tempperspective.Y < 0)
                                        {
                                            g.DrawLine(Pens.Black, (float)((((panel1.Height / 2) / convert - tempperspective.Q) / tempperspective.Z) * convert + panel1.Width / 2), 0, (float)(tempperspective.X * convert + panel1.Width / 2), (float)(panel1.Height / 2 - tempperspective.Y * convert));
                                        }
                                        else
                                        {
                                            g.DrawLine(Pens.Black, (float)((((panel1.Height / -2) / convert - tempperspective.Q) / tempperspective.Z) * convert + panel1.Width / 2), panel1.Height, (float)(tempperspective.X * convert + panel1.Width / 2), (float)(panel1.Height / 2 - tempperspective.Y * convert));
                                        }
                                    }
                                    else
                                    {
                                        g.DrawLine(Pens.Black, 0, (float)(panel1.Height / 2 - convert * (((panel1.Width / -2) / convert) * tempperspective.Z + tempperspective.Q)), (float)(tempperspective.X * convert + panel1.Width / 2), (float)(panel1.Height / 2 - tempperspective.Y * convert));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((panel1.Width / (-2 * convert)) * Math.Tan(Math.PI - angle) >= panel1.Width / (-2 * convert) && (panel1.Width / (-2 * convert)) * Math.Tan(Math.PI - angle) <= panel1.Width / (2 * convert))
                        {
                            g.DrawLine(Pens.Black, 0, (float)(panel1.Height / 2 - (panel1.Width / (-2 * convert)) * Math.Tan(Math.PI - angle) * convert), panel1.Width, (float)(panel1.Height / 2 - (float)(panel1.Width / (2 * convert)) * Math.Tan(Math.PI - angle) * convert));
                        }
                        else
                        {
                            g.DrawLine(Pens.Black, (float)(panel1.Width / 2 + (panel1.Height / (-2 * convert)) * (Math.Cos(Math.PI - angle) / Math.Sin(Math.PI - angle)) * convert), panel1.Height, (float)(panel1.Width / 2 + (panel1.Height / (+2 * convert)) * (Math.Cos(Math.PI - angle) / Math.Sin(Math.PI - angle)) * convert), 0);
                        }
                    }



                    if (Room.Checked)
                    {
                        room[0] = roomorigin;
                        room[1] = new Point3D(roomend.X, roomorigin.Y, roomorigin.Z);
                        room[2] = new Point3D(roomend.X, roomend.Y, roomorigin.Z);
                        room[3] = new Point3D(roomorigin.X, roomend.Y, roomorigin.Z);
                        room[4] = new Point3D(roomorigin.X, roomorigin.Y, roomend.Z);
                        room[5] = new Point3D(roomend.X, roomorigin.Y, roomend.Z);
                        room[6] = roomend;
                        room[7] = new Point3D(roomorigin.X, roomend.Y, roomend.Z);

                        Array.Resize(ref room[0].lconnected, 3);
                        Array.Resize(ref room[1].lconnected, 3);
                        Array.Resize(ref room[2].lconnected, 3);
                        Array.Resize(ref room[3].lconnected, 3);
                        Array.Resize(ref room[4].lconnected, 3);
                        Array.Resize(ref room[5].lconnected, 3);
                        Array.Resize(ref room[6].lconnected, 3);
                        Array.Resize(ref room[7].lconnected, 3);

                        room[0].lconnected[0] = 2;
                        room[0].lconnected[1] = 4;
                        room[0].lconnected[2] = 5;
                        room[1].lconnected[0] = 1;
                        room[1].lconnected[1] = 3;
                        room[1].lconnected[2] = 6;
                        room[2].lconnected[0] = 2;
                        room[2].lconnected[1] = 4;
                        room[2].lconnected[2] = 7;
                        room[3].lconnected[0] = 1;
                        room[3].lconnected[1] = 3;
                        room[3].lconnected[2] = 8;
                        room[4].lconnected[0] = 6;
                        room[4].lconnected[1] = 8;
                        room[4].lconnected[2] = 1;
                        room[5].lconnected[0] = 5;
                        room[5].lconnected[1] = 7;
                        room[5].lconnected[2] = 2;
                        room[6].lconnected[0] = 6;
                        room[6].lconnected[1] = 8;
                        room[6].lconnected[2] = 3;
                        room[7].lconnected[0] = 5;
                        room[7].lconnected[1] = 7;
                        room[7].lconnected[2] = 4;

                        temps.RecievePoints(room);
                        DrawShape(temps, Pens.OrangeRed);
                    }
                    if (SurfaceCheck.Checked)
                    {
                        for (long surfacek = 0; surfacek < surface.LongLength; surfacek++)
                        {
                            Array.Resize(ref surface[surfacek][0].lconnected, 2);
                            Array.Resize(ref surface[surfacek][1].lconnected, 2);
                            Array.Resize(ref surface[surfacek][2].lconnected, 2);
                            surface[surfacek][0].lconnected[0] = 2;
                            surface[surfacek][0].lconnected[1] = 3;
                            surface[surfacek][1].lconnected[0] = 1;
                            surface[surfacek][1].lconnected[1] = 3;
                            surface[surfacek][2].lconnected[0] = 1;
                            surface[surfacek][2].lconnected[1] = 2;
                            Array.Resize(ref surface[surfacek][0].bconnected, 3);
                            surface[surfacek][0].bconnected[0] = 2;
                            surface[surfacek][0].bconnected[1] = 3;
                            surface[surfacek][0].bconnected[2] = 1;

                            temps.RecievePoints(surface[surfacek]);
                            if (Oblique.Checked)
                            {
                                FillShapeBit(temps, 0, Brushes.Crimson);
                            }
                            DrawShape(temps, Pens.Red);
                        }
                    }
                    if (Game2.Checked) //paddle here
                    {
                        temps.Points = new Point3D[4];
                        temps.Points[0] = new Point3D(PaddleLocation2.X - paddlesize, PaddleLocation2.Y - paddlesize, PaddleLocation2.Z);
                        temps.Points[1] = new Point3D(PaddleLocation2.X - paddlesize, PaddleLocation2.Y + paddlesize, PaddleLocation2.Z);
                        temps.Points[2] = new Point3D(PaddleLocation2.X + paddlesize, PaddleLocation2.Y - paddlesize, PaddleLocation2.Z);
                        temps.Points[3] = new Point3D(PaddleLocation2.X + paddlesize, PaddleLocation2.Y + paddlesize, PaddleLocation2.Z);
                        for (temps.Mass = 1; temps.Mass < 4; temps.Mass++)
                        {
                            g.DrawLine(Pens.Green, Transform3D(temps.Points[0]), Transform3D(temps.Points[(int)(temps.Mass)]));
                            g.DrawLine(Pens.Green, Transform3D(temps.Points[3]), Transform3D(temps.Points[(int)(temps.Mass - 1)]));
                        }
                    }
                }
                if (StartTimeS.Text == "Start" || time < fuse)
                {
                    Array.Resize(ref CannonBall.Points, 4 * 5 * 4 + 2);
                    CannonBall.RecievePoints(Presets.SpherePoints());

                    DrawCannon[0] = Presets.CannonBasePoints();
                    temps.Points = DrawCannon[0];
                    temps.MoveShape(cannonorigin.X, cannonorigin.Y, cannonorigin.Z);
                    DrawCannon[0] = temps.Points;

                    DrawCannon[1] = Presets.CannonBasePoints();
                    temps.Points = DrawCannon[1];
                    temps.MoveShape(0, 0, 2.25F);
                    temps.MoveShape(cannonorigin.X, cannonorigin.Y, cannonorigin.Z);
                    DrawCannon[1] = temps.Points;

                    DrawCannon[2] = Presets.CannonPoints();
                    temps.Points = DrawCannon[2];
                    temps.MoveShape(cannonorigin.X, cannonorigin.Y, cannonorigin.Z);
                    DrawCannon[2] = temps.Points;

                    float tempangle = (float)(ballangleXY / 360);
                    tempangle *= (float)(Math.PI);
                    temps.Points = DrawCannon[2];
                    temps.RotateShapebyline(tempangle, cannonorigin.X, cannonorigin.Y, cannonorigin.Z, cannonorigin.X, cannonorigin.Y, cannonorigin.Z + 1);
                    DrawCannon[2] = temps.Points;

                    tempangle = (float)(ballangleXZ / 180);
                    tempangle *= (float)(Math.PI);
                    temps.Points = DrawCannon[0];
                    temps.RotateShapebyline(tempangle, cannonorigin.X, cannonorigin.Y, cannonorigin.Z, cannonorigin.X, cannonorigin.Y + 1, cannonorigin.Z);
                    DrawCannon[0] = temps.Points;
                    temps.Points = DrawCannon[1];
                    temps.RotateShapebyline(tempangle, cannonorigin.X, cannonorigin.Y, cannonorigin.Z, cannonorigin.X, cannonorigin.Y + 1, cannonorigin.Z);
                    DrawCannon[1] = temps.Points;
                    temps.Points = DrawCannon[2];
                    temps.RotateShapebyline(tempangle, cannonorigin.X, cannonorigin.Y, cannonorigin.Z, cannonorigin.X, cannonorigin.Y + 1, cannonorigin.Z);
                    DrawCannon[2] = temps.Points;

                    ballangleXY = -1 * ballangleXY2 * Math.PI / 180;
                    ballangleXZ = (ballangleXZ2 - 180) * Math.PI / 180;
                    CannonBall.SpeedY = (float)(-1 * cannonspeed * Math.Sin((double)(ballangleXY)));
                    CannonBall.SpeedX = (float)(cannonspeed * Math.Cos((double)(ballangleXY)));
                    CannonBall.SpeedZ = (float)(-1 * CannonBall.SpeedX * Math.Sin((double)(ballangleXZ)));
                    CannonBall.SpeedX = (float)(CannonBall.SpeedX * Math.Cos((double)(ballangleXZ)));

                    ballangleXY = -(ballangleXY2 * 2) - 53.5;
                    ballangleXZ = ballangleXZ2;

                    temps.RecievePoints(CannonBall.Points, CannonBall.MassCenter);
                    temps.MoveShape((float)((DrawCannon[2][16].X + DrawCannon[2][24].X) / 2), (float)((DrawCannon[2][16].Y + DrawCannon[2][24].Y) / 2), (float)((DrawCannon[2][16].Z + DrawCannon[2][24].Z) / 2));
                    CannonBall.RecievePoints(temps.Points, temps.MassCenter);
                    CannonBall.MassCenter = new Point3D((float)((DrawCannon[2][16].X + DrawCannon[2][24].X) / 2), (float)((DrawCannon[2][16].Y + DrawCannon[2][24].Y) / 2), (float)((DrawCannon[2][16].Z + DrawCannon[2][24].Z) / 2));

                    //draw

                    Shape tempcannon = new Shape();
                    if (Oblique.Checked)
                    {
                        if (this.Views.SelectedIndex == 1)
                        {
                            if (DrawCannon[0][0].Y > DrawCannon[1][0].Y)
                            {
                                x = 1;
                            }
                            else
                            {
                                x = 0;
                            }
                            if (AngleScroll.Value > 50 && AngleScroll.Value < 100)
                            {
                                x = 1 - x;
                            }
                            if (AngleYZScroll.Value > 50 && AngleYZScroll.Value < 100)
                            {
                                x = 1 - x;
                            }
                        }
                        else
                        {
                            if (this.Views.SelectedIndex == 2)
                            {
                                if (DrawCannon[0][0].X > DrawCannon[1][0].X)
                                {
                                    x = 1;
                                }
                                else
                                {
                                    x = 0;
                                }
                                if (AngleScroll.Value > 25 && AngleScroll.Value < 75)
                                {
                                    x = 1 - x;
                                }
                                if (AngleYZScroll.Value > 50 && AngleYZScroll.Value < 100)
                                {
                                    x = 1 - x;
                                }
                            }
                            else
                            {
                                if (DrawCannon[0][0].Z > DrawCannon[1][0].Z)
                                {
                                    x = 0;
                                }
                                else
                                {
                                    x = 1;
                                }
                            }
                        }
                        tempcannon.RecievePoints(DrawCannon[x]);
                        x = 1 - x;
                        SortShapeSurfaces(tempcannon);
                        for (a = 0; a < 13; a++)
                        {
                            FillShapeBit(tempcannon, (long)a, Brushes.BurlyWood, Pens.Brown);
                        }

                        tempcannon.RecievePoints(DrawCannon[2]);
                        SortShapeSurfaces(tempcannon);
                        for (a = 0; a < 18; a++)
                        {
                            if (tempcannon.Points[(int)(a)].bconnected.LongLength == 16 && tempcannon.Points[(int)(tempcannon.Points[(int)(a)].bconnected[0])].lconnected.Length <= 3)
                            {
                                FillShapeBit(tempcannon, (long)a, Brushes.Black, Pens.Brown);
                            }
                            else
                            {
                                FillShapeBit(tempcannon, (long)a, Brushes.Gray, Pens.Brown);
                            }
                        }
                        tempcannon.RecievePoints(DrawCannon[x]);
                        SortShapeSurfaces(tempcannon);
                        for (a = 0; a < 13; a++)
                        {
                            FillShapeBit(tempcannon, (long)a, Brushes.BurlyWood, Pens.Brown);
                        }
                    }
                    else
                    {
                        for (int counter = 0; counter < 3; counter++)
                        {
                            tempcannon.RecievePoints(DrawCannon[counter]);
                            DrawShape(tempcannon, Pens.Brown);
                        }
                    }
                }

                //Target Time
                if (GameStart.Checked)
                {
                    Shape tempcannon = new Shape();
                    tempcannon.RecievePoints(target);
                    if (hit)
                    {
                        if (Oblique.Checked)
                        {
                            FillShapeBit(tempcannon, 7, Brushes.Green);
                            for (z = 1; z < 6; z++)
                            {
                                FillShapeBit(tempcannon, z, Brushes.Blue);
                            }
                            FillShapeBit(tempcannon, 6, Brushes.Green);
                        }
                        DrawShape(tempcannon, Pens.Green);
                    }
                    else
                    {
                        if (Oblique.Checked)
                        {
                            FillShapeBit(tempcannon, 7, Brushes.Red);
                            for (z = 1; z < 6; z++)
                            {
                                FillShapeBit(tempcannon, z, Brushes.Blue);
                            }
                            FillShapeBit(tempcannon, 6, Brushes.Red);
                        }
                        DrawShape(tempcannon, Pens.Brown);
                    }
                }
                //ball time

                if (ForceTimerS.Enabled || StartTimeS.Text == "Reset" || PauseButton.Text == "Unpause")
                {
                    temps.RecievePoints(CannonBall.Points, CannonBall.MassCenter);
                    if (Oblique.Checked)
                    {
                        if (this.Views.SelectedIndex == 1)
                        {
                            CannonBall.MassCenter.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                CannonBall.MassCenter.ZtoX();
                            }
                        try
                        {
                            g.FillEllipse(Brushes.BlueViolet, Transform3D(CannonBall.MassCenter.X - 1, CannonBall.MassCenter.Y + 1, CannonBall.MassCenter.Z).X, Transform3D(CannonBall.MassCenter.X - 1, CannonBall.MassCenter.Y + 1, CannonBall.MassCenter.Z).Y, Transform3D(CannonBall.MassCenter.X + 1, CannonBall.MassCenter.Y - 1, CannonBall.MassCenter.Z).X - Transform3D(CannonBall.MassCenter.X - 1, CannonBall.MassCenter.Y + 1, CannonBall.MassCenter.Z).X, Transform3D(CannonBall.MassCenter.X + 1, CannonBall.MassCenter.Y - 1, CannonBall.MassCenter.Z).Y - Transform3D(CannonBall.MassCenter.X - 1, CannonBall.MassCenter.Y + 1, CannonBall.MassCenter.Z).Y);
                        }
                        catch { }
                        if (this.Views.SelectedIndex == 1)
                        {
                            CannonBall.MassCenter.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                CannonBall.MassCenter.ZtoX();
                            }
                    }
                    if (Game2.Checked)
                    {
                        if (CannonBall.MassCenter.Z < PaddleLocation.Z + 2 && CannonBall.SpeedZ < 0)
                            DrawShape(temps, Pens.Blue);
                        else
                            if (CannonBall.MassCenter.Z > PaddleLocation2.Z - 2 && CannonBall.SpeedZ > 0)
                                DrawShape(temps, Pens.Green);
                            else
                                DrawShape(temps, Pens.OrangeRed);
                    }
                    else
                    {
                        DrawShape(temps, Pens.OrangeRed);
                    }
                    if (Trail.Checked && trailpoints.LongLength > 0)
                    {
                        temps.RecievePoints(trailpoints);
                        if (this.Views.SelectedIndex == 1)
                        {
                            for (x = 0; x < temps.Points.Length; x++)
                            {
                                temps.Points[x].YtoZ();
                            }
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                for (x = 0; x < temps.Points.Length; x++)
                                {
                                    temps.Points[x].ZtoX();
                                }
                            }
                        pfront = temps.Points;
                        for (x = 0; x < drawtrailpoints.Length; x++)
                        {
                            if (perspective.lconnected[0] == 1)
                            {
                                pfront[x].Y = perspective.Z * temps.Points[x].Y / (perspective.Z - temps.Points[x].Z);
                                pfront[x].X = perspective.Z * temps.Points[x].X / (perspective.Z - temps.Points[x].Z);
                            }
                            drawtrailpoints[x].X = (float)(pfront[x].Z * Math.Sin((float)(angleYZ)) * Math.Cos(Math.PI - angle) + pfront[x].X);
                            drawtrailpoints[x].Y = (float)(pfront[x].Z * Math.Sin((float)(angleYZ)) * Math.Sin(Math.PI - angle) + pfront[x].Y);

                            drawtrailpoints[x].X = (float)(drawtrailpoints[x].X * convert + panel1.Width / 2);
                            drawtrailpoints[x].Y = (float)(panel1.Height / 2 - drawtrailpoints[x].Y * convert);

                        }
                        //real draw
                        for (x = 0; x < temps.Points.Length; x++)
                        {
                            if (Oblique.Checked)
                            {
                                g.FillEllipse(Brushes.Gold, drawtrailpoints[x].X - 5, drawtrailpoints[x].Y - 5, 10, 10);
                            }
                            else
                            {
                                g.DrawEllipse(Pens.Gold, drawtrailpoints[x].X - 5, drawtrailpoints[x].Y - 5, 10, 10);
                            }
                        }
                        if (this.Views.SelectedIndex == 1)
                        {
                            for (x = 0; x < temps.Points.Length; x++)
                            {
                                temps.Points[x].YtoZ();
                            }
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                for (x = 0; x < temps.Points.Length; x++)
                                {
                                    temps.Points[x].ZtoX();
                                }
                            }
                    }
                    if (Vectors.Checked)
                    {
                        temppoint = new Point3D(CannonBall.SpeedX + CannonBall.MassCenter.X, CannonBall.SpeedY + CannonBall.MassCenter.Y, CannonBall.SpeedZ + CannonBall.MassCenter.Z);
                        if (this.Views.SelectedIndex == 1)
                        {
                            temppoint.YtoZ();
                            CannonBall.MassCenter.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                temppoint.ZtoX();
                                CannonBall.MassCenter.ZtoX();
                            }
                        g.DrawLine(Pens.Orange, Transform3D(CannonBall.MassCenter), Transform3D(temppoint));
                        g.DrawEllipse(Pens.Orange, Transform3D(temppoint).X - 5, Transform3D(temppoint).Y - 5, 10, 10);
                        if (this.Views.SelectedIndex == 1)
                        {
                            CannonBall.MassCenter.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                CannonBall.MassCenter.ZtoX();
                            }
                        temppoint = new Point3D(CannonBall.MassCenter.X, CannonBall.MassCenter.Y, CannonBall.MassCenter.Z);
                        if (Gravity.Checked)
                        {
                            temppoint.Y -= gravity;
                        }
                        if (Wind.Checked)
                        {
                            temppoint.X += (float)(WindAccelaration * Math.Cos((double)(WindDirection)));
                            temppoint.Z += (float)(WindAccelaration * Math.Sin((double)(WindDirection)));
                        }
                        if (this.Views.SelectedIndex == 1)
                        {
                            temppoint.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                temppoint.ZtoX();
                            }
                        if (this.Views.SelectedIndex == 1)
                        {
                            CannonBall.MassCenter.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                CannonBall.MassCenter.ZtoX();
                            }
                        g.DrawLine(Pens.Red, Transform3D(CannonBall.MassCenter), Transform3D(temppoint));
                        g.DrawEllipse(Pens.Red, Transform3D(temppoint).X - 5, Transform3D(temppoint).Y - 5, 10, 10);
                        if (this.Views.SelectedIndex == 1)
                        {
                            CannonBall.MassCenter.YtoZ();
                        }
                        else
                            if (this.Views.SelectedIndex == 2)
                            {
                                CannonBall.MassCenter.ZtoX();
                            }
                    }
                }

                if (Game2.Checked) //paddle here
                {
                    temps.Points = new Point3D[4];
                    temps.Points[0] = new Point3D(PaddleLocation.X - paddlesize, PaddleLocation.Y - paddlesize, PaddleLocation.Z);
                    temps.Points[1] = new Point3D(PaddleLocation.X - paddlesize, PaddleLocation.Y + paddlesize, PaddleLocation.Z);
                    temps.Points[2] = new Point3D(PaddleLocation.X + paddlesize, PaddleLocation.Y - paddlesize, PaddleLocation.Z);
                    temps.Points[3] = new Point3D(PaddleLocation.X + paddlesize, PaddleLocation.Y + paddlesize, PaddleLocation.Z);
                    for (temps.Mass = 1; temps.Mass < 4; temps.Mass++)
                    {
                        g.DrawLine(Pens.Blue, Transform3D(temps.Points[0]), Transform3D(temps.Points[(int)(temps.Mass)]));
                        g.DrawLine(Pens.Blue, Transform3D(temps.Points[3]), Transform3D(temps.Points[(int)(temps.Mass - 1)]));
                    }
                }
            }
        }


        public long[] sorttemp(long[] temp, Point3D[] fillercompare)
        {
            long x, y, z;
            for (x = 0; fillercompare.LongLength > x; x++)
            {
                for (y = 0; y < x && (x == 0 || y != temp[x - 1]); y++) ;
                temp[x] = y;
                for (y = x + 1; fillercompare.LongLength > y; y++)
                {
                    if (fillercompare[y].Z > fillercompare[temp[x]].Z)
                    {
                        for (z = 0; z < x + 1 && z > -1; z++)
                        {
                            if (temp[z] == y)
                            {
                                z = -2;
                            }
                        }
                        if (z > -1 && (x == 0 || y != temp[x - 1]))
                            temp[x] = y;
                    }
                }
            }
            return temp;
        }

        public void AngleScroll_Scroll(object sender, ScrollEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Change the angle from which you see the simulation here");
            }
            else
            { }
            angle = AngleScroll.Value * Math.PI / 50;
            panel1.Invalidate();
        }

        public void AngleYZScroll_Scroll(object sender, ScrollEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Change the angle from which you see the simulation here");
            }
            else
            { }
            angleYZ = AngleYZScroll.Value * Math.PI / 50;
            panel1.Invalidate();
        }

        public void Perspective_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        public void Views_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            cursor = (Cursor.Handle).ToInt64();
        }

        public void Oblique_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Here, the simulation is drawn. You can move the cannon with the mouse and double click on trails, targets or vectors for more information");
            }
            else
            {
            }
        }

        public void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownXY = new Point3D(e.X, e.Y, 1);

        }
        public void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDownXY.Z = 0;
            MouseDownXZ.Z = 0;
            MouseDownAngleXY.Z = 0;
            MouseDownAngleXZ.Z = 0;
        }

        public void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            float x, y;
            if (Game2.Checked)
            {
                x = (float)((e.X - panel1.Width / 2) / convert);
                y = (float)((panel1.Height / 2 - e.Y) / convert);
                if (x < paddlesize - TunnelSize.X)
                {
                    PaddleLocation.X = paddlesize - TunnelSize.X;
                }
                else
                {
                    if (x > TunnelSize.X - paddlesize)
                    {
                        PaddleLocation.X = TunnelSize.X - paddlesize;
                    }
                    else
                    {
                        PaddleLocation.X = x;
                    }
                }
                if (y < paddlesize - TunnelSize.Y)
                {
                    PaddleLocation.Y = paddlesize - TunnelSize.Y;
                }
                else
                {
                    if (y > TunnelSize.Y - paddlesize)
                    {
                        PaddleLocation.Y = TunnelSize.Y - paddlesize;
                    }
                    else
                    {
                        PaddleLocation.Y = y;
                    }
                }
            }
            else
            {
                if (MouseDownXY.Z == 1)
                {
                    x = (float)((e.X - MouseDownXY.X) / convert);
                    y = (float)((e.Y - MouseDownXY.Y) / convert);
                    if (Views.SelectedIndex == 2)
                    {
                        y = -1 * y;
                        x = -1 * x;
                        CannonZ.Text = x.ToString();
                        CannonY.Text = y.ToString();
                    }
                    else
                    {
                        if (Views.SelectedIndex == 1)
                        {
                            CannonX.Text = x.ToString();
                            CannonZ.Text = y.ToString();
                        }
                        else
                        {
                            y = -1 * y;
                            CannonX.Text = x.ToString();
                            CannonY.Text = y.ToString();
                        }
                    }
                }
                else
                {
                    if (MouseDownXZ.Z == 1)
                    {
                        x = (float)((e.X - MouseDownXY.X) / convert);
                        y = (float)((e.Y - MouseDownXY.Y) / convert);
                        if (Views.SelectedIndex == 2)
                        {
                            CannonZ.Text = x.ToString();
                            CannonX.Text = y.ToString();
                        }
                        else
                        {
                            if (Views.SelectedIndex == 1)
                            {
                                y = -1 * y;
                                CannonX.Text = x.ToString();
                                CannonY.Text = y.ToString();
                            }
                            else
                            {
                                CannonX.Text = x.ToString();
                                CannonZ.Text = y.ToString();
                            }
                        }
                    }
                }
            }
        }

        public void Game2_CheckedChanged(object sender, EventArgs e)
        {
            if (Game2.Checked)
            {
                GameStart.Enabled = false;
                Room.Checked = true;
                RoomOX.Text = TunnelSize.X.ToString();
                RoomOY.Text = TunnelSize.Y.ToString();
                RoomOZ.Text = "-1";
                TunnelSize.X = -1 * TunnelSize.X;
                TunnelSize.Y = -1 * TunnelSize.Y;
                TunnelSize.Z = TunnelSize.Z + 2;
                RoomX.Text = (TunnelSize.X).ToString();
                RoomY.Text = TunnelSize.Y.ToString();
                RoomZ.Text = TunnelSize.Z.ToString();
                TunnelSize.X = -1 * TunnelSize.X;
                TunnelSize.Y = -1 * TunnelSize.Y;
                TunnelSize.Z = TunnelSize.Z - 2;
                PaddleLocation2 = new Point3D(0, 0, TunnelSize.Z + 1);
                PaddleLocation = new Point3D(0, 0, 0);

                panel6.Visible = true;
                levelcheck.Visible = true;
                HitsLabel.Visible = true;
                HitsLabelLabel.Visible = true;
                Misseslabel.Visible = true;
                MissesLabelLabel.Visible = true;
                Ballmass.ReadOnly = true;
                CannonX.ReadOnly = true;
                CannonY.ReadOnly = true;
                CannonZ.ReadOnly = true;
                Ballmass.Text = "100";
                CannonX.Text = "0";
                CannonY.Text = "0";
                TunnelSize.Z = (float)(TunnelSize.Z / 2);
                CannonZ.Text = TunnelSize.Z.ToString();
                TunnelSize.Z = (float)(TunnelSize.Z * 2);
                BallSpeed.Text = "2";
                Views.SelectedIndex = 0;
                Views.Enabled = false;
                Perspective.Checked = true;
                PerspectiveX.Text = "0";
                PerspectiveY.ReadOnly = true;
                PerspectiveY.Text = "0";
                PerspectiveZ.ReadOnly = true;
                PerspectiveZ.Text = "-50";
                CannonAngleXY.Text = "45";
                CannonAngleXZ.Text = "45";
                Room.Enabled = false;
                SurfaceCheck.Checked = false;
                SurfaceCheck.Enabled = false;
                Gravity.Checked = false;
                Gravity.Enabled = false;
                Wind.Checked = false;
                Wind.Enabled = false;
                levelcheck.Enabled = true;
                levelcheck.Checked = false;
                windaccelaration1.Text = "0";
                winddirection1.Text = "0";
                windforce1.Text = "0";

                levellabel.Text = "0";
                level = 0;
                HitsLabel.Text = "0";
                Misseslabel.Text = "0";

                FuseParameter.Text = "1.5";
                IntervalParameter.Text = "0.1";
                GridUnitsParameter.Text = "7.5";

                Properties_Click(sender, e);
                Properties_Click(sender, e);
                Cannon_Click(sender, e);
                Cannon_Click(sender, e);
            }
            else
            {
                GameStart.Enabled = true;
                panel6.Visible = false;
                levelcheck.Visible = false;
                HitsLabel.Visible = false;
                HitsLabelLabel.Visible = false;
                Misseslabel.Visible = false;
                MissesLabelLabel.Visible = false;
                Ballmass.ReadOnly = false;
                CannonX.ReadOnly = false;
                CannonY.ReadOnly = false;
                CannonZ.ReadOnly = false;
                Views.Enabled = true;
                PerspectiveY.ReadOnly = false;
                PerspectiveZ.ReadOnly = false;
                Room.Enabled = true;
                SurfaceCheck.Enabled = true;
                Gravity.Enabled = true;
                Wind.Enabled = true;
                levelcheck.Enabled = false;
                levelcheck.Checked = false;
                Properties_Click(sender, e);
                Properties_Click(sender, e);
                Cannon_Click(sender, e);
                Cannon_Click(sender, e);

                HitsLabel.Text = "0";
                Misseslabel.Text = "0";
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Click here to pause the cannon firing simulation");
            }
            else
            {
                if (ForceTimerS.Enabled)
                {
                    ForceTimerS.Enabled = false;
                    PauseButton.Text = "Unpause";
                    StartTimeS.Enabled = false;
                }
                else
                {
                    if (PauseButton.Text == "Unpause")
                    {
                        ForceTimerS.Enabled = true;
                        PauseButton.Text = "Pause";
                        StartTimeS.Enabled = true;
                    }
                }
            }
        }

        public void Properties_Click(object sender, EventArgs e)
        {

            if (Game2.Checked)
            {
                if (ZoomLabel.Visible)
                {
                    ZoomLabel.Visible = false;
                    ZoomBox.Visible = false;
                    ZoomBox.ReadOnly = true;
                    panel2.Visible = false;
                    ParametersCheck.Visible = false;
                    ParametersCheck.Enabled = false;
                    PerspectiveXlabel.Visible = false;
                    PerspectiveYlabel.Visible = false;
                    PerspectiveZlabel.Visible = false;
                    PerspectiveX.Visible = false;
                    PerspectiveX.ReadOnly = false;
                    PerspectiveY.Visible = false;
                    PerspectiveY.ReadOnly = false;
                    PerspectiveZ.Visible = false;
                    PerspectiveZ.ReadOnly = false;
                    Room.Visible = false;
                    Room.Enabled = false;
                    SurfaceCheck.Visible = false;
                    SurfaceCheck.Enabled = false;
                    Gravity.Visible = false;
                    Gravity.Enabled = false;
                    panel3.Visible = false;
                    Roomxlabel.Visible = false;
                    Roomylabel.Visible = false;
                    Roomzlabel.Visible = false;
                    RoomXlabel2.Visible = false;
                    RoomYlabel2.Visible = false;
                    RoomZlabel2.Visible = false;
                    RoomX.Visible = false;
                    RoomX.ReadOnly = true;
                    RoomY.Visible = false;
                    RoomY.ReadOnly = true;
                    RoomZ.Visible = false;
                    RoomZ.ReadOnly = true;
                    RoomOX.Visible = false;
                    RoomOX.ReadOnly = true;
                    RoomOY.Visible = false;
                    RoomOY.ReadOnly = true;
                    RoomOZ.Visible = false;
                    RoomOZ.ReadOnly = true;
                    Wind.Visible = false;
                    Wind.Enabled = false;
                    panel5.Visible = false;
                    winddirectionlabel.Visible = false;
                    windaccelarationlabel.Visible = false;
                    windforcelabel.Visible = false;
                    winddirection1.Visible = false;
                    windaccelaration1.Visible = false;
                    windforce1.Visible = false;
                    winddirection1.ReadOnly = true;
                    windaccelaration1.ReadOnly = true;
                    windforce1.ReadOnly = true;
                }
                else
                {
                    ZoomLabel.Visible = true;
                    ZoomBox.Visible = true;
                    ZoomBox.ReadOnly = false;
                    ParametersCheck.Visible = true;
                    ParametersCheck.Enabled = true;
                    panel2.Visible = true;
                    PerspectiveXlabel.Visible = true;
                    PerspectiveYlabel.Visible = true;
                    PerspectiveZlabel.Visible = true;
                    PerspectiveX.Visible = true;
                    PerspectiveY.Visible = true;
                    PerspectiveZ.Visible = true;
                    Room.Visible = true;
                    SurfaceCheck.Visible = true;
                    Gravity.Visible = true;
                    Wind.Visible = true;
                    panel3.Visible = true;
                    Roomxlabel.Visible = true;
                    Roomylabel.Visible = true;
                    Roomzlabel.Visible = true;
                    RoomXlabel2.Visible = true;
                    RoomYlabel2.Visible = true;
                    RoomZlabel2.Visible = true;
                    RoomX.Visible = true;
                    RoomY.Visible = true;
                    RoomZ.Visible = true;
                    RoomOX.Visible = true;
                    RoomOY.Visible = true;
                    RoomOZ.Visible = true;
                    if (levelcheck.Checked == false)
                    {
                        RoomOX.ReadOnly = false;
                        RoomOY.ReadOnly = false;
                        RoomZ.ReadOnly = false;
                    }
                }
            }
            else
            {
                if (GameStart.Checked)
                {
                    if (ZoomLabel.Visible)
                    {
                        ZoomLabel.Visible = false;
                        ZoomBox.Visible = false;
                        ZoomBox.ReadOnly = true;
                        panel2.Visible = false;
                        ParametersCheck.Visible = false;
                        ParametersCheck.Enabled = false;
                        PerspectiveXlabel.Visible = false;
                        PerspectiveYlabel.Visible = false;
                        PerspectiveZlabel.Visible = false;
                        PerspectiveX.Visible = false;
                        PerspectiveX.ReadOnly = true;
                        PerspectiveY.Visible = false;
                        PerspectiveY.ReadOnly = true;
                        PerspectiveZ.Visible = false;
                        PerspectiveZ.ReadOnly = true;
                        Room.Visible = false;
                        Room.Enabled = false;
                        SurfaceCheck.Visible = false;
                        SurfaceCheck.Enabled = false;
                        Gravity.Visible = false;
                        Gravity.Enabled = false;
                        panel3.Visible = false;
                        Roomxlabel.Visible = false;
                        Roomylabel.Visible = false;
                        Roomzlabel.Visible = false;
                        RoomXlabel2.Visible = false;
                        RoomYlabel2.Visible = false;
                        RoomZlabel2.Visible = false;
                        RoomX.Visible = false;
                        RoomX.ReadOnly = true;
                        RoomY.Visible = false;
                        RoomY.ReadOnly = true;
                        RoomZ.Visible = false;
                        RoomZ.ReadOnly = true;
                        RoomOX.Visible = false;
                        RoomOX.ReadOnly = true;
                        RoomOY.Visible = false;
                        RoomOY.ReadOnly = true;
                        RoomOZ.Visible = false;
                        RoomOZ.ReadOnly = true;
                        Wind.Visible = false;
                        Wind.Enabled = false;
                        panel5.Visible = false;
                        winddirectionlabel.Visible = false;
                        windaccelarationlabel.Visible = false;
                        windforcelabel.Visible = false;
                        winddirection1.Visible = false;
                        windaccelaration1.Visible = false;
                        windforce1.Visible = false;
                        winddirection1.ReadOnly = true;
                        windaccelaration1.ReadOnly = true;
                        windforce1.ReadOnly = true;
                    }
                    else
                    {
                        ZoomLabel.Visible = true;
                        ZoomBox.Visible = true;
                        ZoomBox.ReadOnly = false;
                        ParametersCheck.Visible = true;
                        ParametersCheck.Enabled = true;
                        panel2.Visible = true;
                        PerspectiveXlabel.Visible = true;
                        PerspectiveYlabel.Visible = true;
                        PerspectiveZlabel.Visible = true;
                        PerspectiveX.Visible = true;
                        PerspectiveY.Visible = true;
                        PerspectiveZ.Visible = true;
                        Room.Visible = true;
                        SurfaceCheck.Visible = true;
                        Gravity.Visible = true;
                        Wind.Visible = true;
                        panel5.Visible = true;
                        winddirectionlabel.Visible = true;
                        windaccelarationlabel.Visible = true;
                        windforcelabel.Visible = true;
                        winddirection1.Visible = true;
                        windaccelaration1.Visible = true;
                        windforce1.Visible = true;


                        if (StartTimeS.Text == "Reset")
                        {
                            Cannon.Enabled = false;
                            Room.Enabled = false;
                            SurfaceCheck.Enabled = false;
                            Gravity.Enabled = false;
                            GameStart.Enabled = false;
                            RoomX.ReadOnly = true;
                            RoomY.ReadOnly = true;
                            RoomZ.ReadOnly = true;
                            RoomOX.ReadOnly = true;
                            RoomOY.ReadOnly = true;
                            RoomOZ.ReadOnly = true;
                            SurfaceP1X.ReadOnly = true;
                            SurfaceP1Y.ReadOnly = true;
                            SurfaceP1Z.ReadOnly = true;
                            SurfaceP2X.ReadOnly = true;
                            SurfaceP2Y.ReadOnly = true;
                            SurfaceP2Z.ReadOnly = true;
                            SurfaceP3X.ReadOnly = true;
                            SurfaceP3Y.ReadOnly = true;
                            SurfaceP3Z.ReadOnly = true;
                            surfaceComboBox.Enabled = false;
                            Ballmass.ReadOnly = true;
                            CannonX.ReadOnly = true;
                            CannonY.ReadOnly = true;
                            CannonZ.ReadOnly = true;
                            CannonAngleXY.ReadOnly = true;
                            CannonAngleXZ.ReadOnly = true;
                            BallForce.ReadOnly = true;
                            BallSpeed.ReadOnly = true;
                            winddirection1.ReadOnly = true;
                            windaccelaration1.ReadOnly = true;
                            windforce1.ReadOnly = true;
                            PerspectiveX.ReadOnly = true;
                            PerspectiveY.ReadOnly = true;
                            PerspectiveZ.ReadOnly = true;
                        }
                    }
                }
                else
                {
                    if (ZoomLabel.Visible)
                    {
                        ZoomLabel.Visible = false;
                        ZoomBox.Visible = false;
                        ZoomBox.ReadOnly = true;
                        panel2.Visible = false;
                        PerspectiveXlabel.Visible = false;
                        PerspectiveYlabel.Visible = false;
                        PerspectiveZlabel.Visible = false;
                        PerspectiveX.Visible = false;
                        PerspectiveX.ReadOnly = true;
                        PerspectiveY.Visible = false;
                        PerspectiveY.ReadOnly = true;
                        PerspectiveZ.Visible = false;
                        PerspectiveZ.ReadOnly = true;
                        Room.Visible = false;
                        Room.Enabled = false;
                        SurfaceCheck.Visible = false;
                        SurfaceCheck.Enabled = false;
                        Gravity.Visible = false;
                        Gravity.Enabled = false;
                        panel3.Visible = false;
                        Roomxlabel.Visible = false;
                        Roomylabel.Visible = false;
                        Roomzlabel.Visible = false;
                        RoomXlabel2.Visible = false;
                        RoomYlabel2.Visible = false;
                        RoomZlabel2.Visible = false;
                        RoomX.Visible = false;
                        RoomX.ReadOnly = true;
                        RoomY.Visible = false;
                        RoomY.ReadOnly = true;
                        RoomZ.Visible = false;
                        RoomZ.ReadOnly = true;
                        RoomOX.Visible = false;
                        RoomOX.ReadOnly = true;
                        RoomOY.Visible = false;
                        RoomOY.ReadOnly = true;
                        RoomOZ.Visible = false;
                        RoomOZ.ReadOnly = true;

                        panel8.Visible = false;

                        SurfacePoint1label.Visible = false;
                        SurfacePoint2label.Visible = false;
                        SurfacePoint3label.Visible = false;
                        SurfaceP1Xlabel.Visible = false;
                        SurfaceP1Ylabel.Visible = false;
                        SurfaceP1Zlabel.Visible = false;
                        SurfaceP2Xlabel.Visible = false;
                        SurfaceP2Ylabel.Visible = false;
                        SurfaceP2Zlabel.Visible = false;
                        SurfaceP3Xlabel.Visible = false;
                        SurfaceP3Ylabel.Visible = false;
                        SurfaceP3Zlabel.Visible = false;

                        SurfaceP1X.Visible = false;
                        SurfaceP1X.ReadOnly = true;
                        SurfaceP1Y.Visible = false;
                        SurfaceP1Y.ReadOnly = true;
                        SurfaceP1Z.Visible = false;
                        SurfaceP1Z.ReadOnly = true;
                        SurfaceP2X.Visible = false;
                        SurfaceP2X.ReadOnly = true;
                        SurfaceP2Y.Visible = false;
                        SurfaceP2Y.ReadOnly = true;
                        SurfaceP2Z.Visible = false;
                        SurfaceP2Z.ReadOnly = true;
                        SurfaceP3X.Visible = false;
                        SurfaceP3X.ReadOnly = true;
                        SurfaceP3Y.Visible = false;
                        SurfaceP3Y.ReadOnly = true;
                        SurfaceP3Z.Visible = false;
                        SurfaceP3Z.ReadOnly = true;
                        surfaceComboBox.Visible = false;
                        surfaceComboBox.Enabled = false;

                        Wind.Visible = false;
                        Wind.Enabled = false;
                        panel5.Visible = false;
                        winddirectionlabel.Visible = false;
                        windaccelarationlabel.Visible = false;
                        windforcelabel.Visible = false;
                        winddirection1.Visible = false;
                        windaccelaration1.Visible = false;
                        windforce1.Visible = false;
                        winddirection1.ReadOnly = true;
                        windaccelaration1.ReadOnly = true;
                        windforce1.ReadOnly = true;
                    }
                    else
                    {
                        ZoomLabel.Visible = true;
                        ZoomBox.Visible = true;
                        ZoomBox.ReadOnly = false;
                        ParametersCheck.Visible = true;
                        ParametersCheck.Enabled = true;
                        panel2.Visible = true;
                        PerspectiveXlabel.Visible = true;
                        PerspectiveYlabel.Visible = true;
                        PerspectiveZlabel.Visible = true;
                        PerspectiveX.Visible = true;
                        PerspectiveX.ReadOnly = false;
                        PerspectiveY.Visible = true;
                        PerspectiveY.ReadOnly = false;
                        PerspectiveZ.Visible = true;
                        PerspectiveZ.ReadOnly = false;
                        Room.Visible = true;
                        Room.Enabled = true;
                        SurfaceCheck.Visible = true;
                        SurfaceCheck.Enabled = true;
                        Gravity.Visible = true;
                        Gravity.Enabled = true;
                        if (Room.Checked)
                        {
                            panel3.Visible = true;
                            Roomxlabel.Visible = true;
                            Roomylabel.Visible = true;
                            Roomzlabel.Visible = true;
                            RoomXlabel2.Visible = true;
                            RoomYlabel2.Visible = true;
                            RoomZlabel2.Visible = true;
                            RoomX.Visible = true;
                            RoomX.ReadOnly = false;
                            RoomY.Visible = true;
                            RoomY.ReadOnly = false;
                            RoomZ.Visible = true;
                            RoomZ.ReadOnly = false;
                            RoomOX.Visible = true;
                            RoomOX.ReadOnly = false;
                            RoomOY.Visible = true;
                            RoomOY.ReadOnly = false;
                            RoomOZ.Visible = true;
                            RoomOZ.ReadOnly = false;
                        }
                        if (SurfaceCheck.Checked)
                        {
                            panel8.Visible = true;

                            SurfacePoint1label.Visible = true;
                            SurfacePoint2label.Visible = true;
                            SurfacePoint3label.Visible = true;
                            SurfaceP1Xlabel.Visible = true;
                            SurfaceP1Ylabel.Visible = true;
                            SurfaceP1Zlabel.Visible = true;
                            SurfaceP2Xlabel.Visible = true;
                            SurfaceP2Ylabel.Visible = true;
                            SurfaceP2Zlabel.Visible = true;
                            SurfaceP3Xlabel.Visible = true;
                            SurfaceP3Ylabel.Visible = true;
                            SurfaceP3Zlabel.Visible = true;

                            SurfaceP1X.Visible = true;
                            SurfaceP1X.ReadOnly = false;
                            SurfaceP1Y.Visible = true;
                            SurfaceP1Y.ReadOnly = false;
                            SurfaceP1Z.Visible = true;
                            SurfaceP1Z.ReadOnly = false;
                            SurfaceP2X.Visible = true;
                            SurfaceP2X.ReadOnly = false;
                            SurfaceP2Y.Visible = true;
                            SurfaceP2Y.ReadOnly = false;
                            SurfaceP2Z.Visible = true;
                            SurfaceP2Z.ReadOnly = false;
                            SurfaceP3X.Visible = true;
                            SurfaceP3X.ReadOnly = false;
                            SurfaceP3Y.Visible = true;
                            SurfaceP3Y.ReadOnly = false;
                            SurfaceP3Z.Visible = true;
                            SurfaceP3Z.ReadOnly = false;
                            surfaceComboBox.Visible = true;
                            surfaceComboBox.Enabled = true;
                        }
                        Wind.Visible = true;
                        Wind.Enabled = true;
                        if (Wind.Checked)
                        {
                            panel5.Visible = true;
                            winddirectionlabel.Visible = true;
                            windaccelarationlabel.Visible = true;
                            windforcelabel.Visible = true;
                            winddirection1.Visible = true;
                            windaccelaration1.Visible = true;
                            windforce1.Visible = true;
                            winddirection1.ReadOnly = false;
                            windaccelaration1.ReadOnly = false;
                            windforce1.ReadOnly = false;
                        }

                        if (StartTimeS.Text == "Reset")
                        {
                            Cannon.Enabled = false;
                            Room.Enabled = false;
                            SurfaceCheck.Enabled = false;
                            Gravity.Enabled = false;
                            GameStart.Enabled = false;
                            RoomX.ReadOnly = true;
                            RoomY.ReadOnly = true;
                            RoomZ.ReadOnly = true;
                            RoomOX.ReadOnly = true;
                            RoomOY.ReadOnly = true;
                            RoomOZ.ReadOnly = true;
                            SurfaceP1X.ReadOnly = true;
                            SurfaceP1Y.ReadOnly = true;
                            SurfaceP1Z.ReadOnly = true;
                            SurfaceP2X.ReadOnly = true;
                            SurfaceP2Y.ReadOnly = true;
                            SurfaceP2Z.ReadOnly = true;
                            SurfaceP3X.ReadOnly = true;
                            SurfaceP3Y.ReadOnly = true;
                            SurfaceP3Z.ReadOnly = true;
                            surfaceComboBox.Enabled = false;
                            Ballmass.ReadOnly = true;
                            CannonX.ReadOnly = true;
                            CannonY.ReadOnly = true;
                            CannonZ.ReadOnly = true;
                            CannonAngleXY.ReadOnly = true;
                            CannonAngleXZ.ReadOnly = true;
                            BallForce.ReadOnly = true;
                            BallSpeed.ReadOnly = true;
                            winddirection1.ReadOnly = true;
                            windaccelaration1.ReadOnly = true;
                            windforce1.ReadOnly = true;

                            Properties.Enabled = true;
                            ZoomBox.ReadOnly = false;
                            PerspectiveX.ReadOnly = false;
                            PerspectiveY.ReadOnly = false;
                            PerspectiveZ.ReadOnly = false;
                            Perspective.Enabled = true;
                            Oblique.Enabled = true;
                            Views.Enabled = true;
                        }
                    }
                }
            }
        }

        public void StartTime_Click(object sender, EventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Click here to start, stop or reset the cannon firing simulation");
            }
            else
            {
                Shape temp = new Shape();
                if (StartTimeS.Text == "Stop")
                {
                    ForceTimerS.Enabled = false;
                    StartTimeS.Text = "Reset";

                    Helpbutton.Enabled = true;
                    gridcheck.Enabled = true;
                    Properties.Enabled = true;
                    ZoomBox.ReadOnly = false;
                    PerspectiveX.ReadOnly = false;
                    PerspectiveY.ReadOnly = false;
                    PerspectiveZ.ReadOnly = false;
                    Perspective.Enabled = true;
                    Oblique.Enabled = true;
                    Views.Enabled = true;
                    if (GameStart.Checked)
                    {
                        PerspectiveX.ReadOnly = true;
                        PerspectiveY.ReadOnly = true;
                        PerspectiveZ.ReadOnly = true;

                        if (hit)
                        {
                            hits++;
                            score++;
                            HitsLabel.Text = hits.ToString();
                            ScoreLabel.Text = score.ToString();
                            if (levelcheck.Checked && hits / level == 5)
                            {
                                levellabel.Text = ((long)(level + 1)).ToString();
                            }
                        }
                        else
                        {
                            misses++;
                            score--;
                            Misseslabel.Text = misses.ToString();
                            ScoreLabel.Text = score.ToString();
                        }
                    }
                    panel1.Invalidate();
                }
                else
                {
                    if (StartTimeS.Text == "Start")
                    {
                        ForceTimerS.Interval = (int)interval;

                        StartTimeS.Text = "Stop";

                        Cannon.Enabled = false;
                        Properties.Enabled = false;
                        Room.Enabled = false;
                        ZoomBox.ReadOnly = true;
                        PerspectiveX.ReadOnly = true;
                        PerspectiveY.ReadOnly = true;
                        PerspectiveZ.ReadOnly = true;
                        Gravity.Enabled = false;
                        GameStart.Enabled = false;
                        RoomX.ReadOnly = true;
                        RoomY.ReadOnly = true;
                        RoomZ.ReadOnly = true;
                        RoomOX.ReadOnly = true;
                        RoomOY.ReadOnly = true;
                        RoomOZ.ReadOnly = true;
                        Perspective.Enabled = false;
                        Oblique.Enabled = false;
                        Views.Enabled = false;
                        Ballmass.ReadOnly = true;
                        CannonX.ReadOnly = true;
                        CannonY.ReadOnly = true;
                        CannonZ.ReadOnly = true;
                        CannonAngleXY.ReadOnly = true;
                        CannonAngleXZ.ReadOnly = true;
                        BallForce.ReadOnly = true;
                        BallSpeed.ReadOnly = true;
                        winddirection1.ReadOnly = true;
                        windaccelaration1.ReadOnly = true;
                        windforce1.ReadOnly = true;
                        gridcheck.Enabled = false;
                        Helpbutton.Enabled = false;
                        Trail.Enabled = false;
                        Vectors.Enabled = false;

                        panel1.Invalidate();
                        time = 0;
                        ForceTimerS.Enabled = true;
                    }
                    else
                    {
                        if (StartTimeS.Text == "Reset")
                        {

                            StartTimeS.Text = "Start";

                            Cannon.Enabled = true;
                            Properties.Enabled = true;
                            Room.Enabled = true;
                            GameStart.Enabled = true;
                            Game2.Enabled = true;
                            Gravity.Enabled = true;
                            ZoomBox.ReadOnly = false;
                            PerspectiveX.ReadOnly = false;
                            PerspectiveY.ReadOnly = false;
                            PerspectiveZ.ReadOnly = false;
                            RoomX.ReadOnly = false;
                            RoomY.ReadOnly = false;
                            RoomZ.ReadOnly = false;
                            RoomOX.ReadOnly = false;
                            RoomOY.ReadOnly = false;
                            RoomOZ.ReadOnly = false;
                            Perspective.Enabled = true;
                            Oblique.Enabled = true;
                            Views.Enabled = true;
                            Ballmass.ReadOnly = false;
                            CannonX.ReadOnly = false;
                            CannonY.ReadOnly = false;
                            CannonZ.ReadOnly = false;
                            CannonAngleXY.ReadOnly = false;
                            CannonAngleXZ.ReadOnly = false;
                            BallForce.ReadOnly = false;
                            BallSpeed.ReadOnly = false;
                            winddirection1.ReadOnly = false;
                            windaccelaration1.ReadOnly = false;
                            windforce1.ReadOnly = false;
                            trailpoints = new Point3D[0];
                            drawtrailpoints = new PointF[0];

                            if (GameStart.Checked)
                            {
                                Gravity.Enabled = false;
                                Room.Enabled = false;
                                PerspectiveX.ReadOnly = true;
                                PerspectiveY.ReadOnly = true;
                                PerspectiveZ.ReadOnly = true;
                                RoomX.ReadOnly = true;
                                RoomY.ReadOnly = true;
                                RoomZ.ReadOnly = true;
                                RoomOX.ReadOnly = true;
                                RoomOY.ReadOnly = true;
                                RoomOZ.ReadOnly = true;
                                Ballmass.ReadOnly = true;
                                CannonX.ReadOnly = true;
                                CannonY.ReadOnly = true;
                                CannonZ.ReadOnly = true;
                                BallSpeed.ReadOnly = true;
                                winddirection1.ReadOnly = true;
                                windaccelaration1.ReadOnly = true;
                                windforce1.ReadOnly = true;
                                Game2.Enabled = false;

                                if (hit)
                                {
                                    Random rand = new Random(); targetloc = new Point3D((float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F, (float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F, (float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F);
                                    target = Presets.TargetPoints(targetloc);
                                    windaccelaration1.Text = ((float)((float)(rand.Next(0, (int)(windlimit * 1000))) / 1000F)).ToString();
                                    winddirection1.Text = ((float)(rand.Next(0, 360))).ToString();
                                    hit = false;
                                }
                            } 
                            if (Game2.Checked)
                            {
                                GameStart.Enabled = false;
                            }
                            panel1.Invalidate();
                        }
                    }
                }
            }
        }

        public void ForceTimer_Tick(object sender, EventArgs e)
        {
            time = time + interval / (double)(1000);
            int a = 0, b = 0;
            if (time < fuse)
            {
            }
            else
            {
                if (Room.Checked)
                {
                    if (roomend.X > roomorigin.X)
                    {
                        if ((CannonBall.MassCenter.X + 1 > roomend.X || CannonBall.MassCenter.X - 1 < roomorigin.X))
                        {
                            CannonBall.SpeedX = -1 * CannonBall.SpeedX;
                        }
                    }
                    else
                    {
                        if ((CannonBall.MassCenter.X - 1 < roomend.X || CannonBall.MassCenter.X + 1 > roomorigin.X))
                        {
                            CannonBall.SpeedX = -1 * CannonBall.SpeedX;
                        }
                    }
                    if (roomend.Y > roomorigin.Y)
                    {
                        if ((CannonBall.MassCenter.Y + 1 > roomend.Y || CannonBall.MassCenter.Y - 1 < roomorigin.Y))
                        {
                            if (CannonBall.SpeedY < speedzerolimit && CannonBall.SpeedY > -1 * speedzerolimit)
                                CannonBall.SpeedY = 0;
                            else
                                CannonBall.SpeedY = -1 * CannonBall.SpeedY;
                        }
                    }
                    else
                    {
                        if ((CannonBall.MassCenter.Y - 1 < roomend.Y || CannonBall.MassCenter.Y + 1 > roomorigin.Y))
                        {
                            if (CannonBall.SpeedY < speedzerolimit && CannonBall.SpeedY > -1 * speedzerolimit)
                                CannonBall.SpeedY = 0;
                            else
                                CannonBall.SpeedY = -1 * CannonBall.SpeedY;
                        }
                    }
                    if (roomend.Z > roomorigin.Z)
                    {
                        if ((CannonBall.MassCenter.Z + 1 > roomend.Z || CannonBall.MassCenter.Z - 1 < roomorigin.Z))
                        {
                            CannonBall.SpeedZ = -1 * CannonBall.SpeedZ;
                            if (Game2.Checked)
                            {
                                if (CannonBall.MassCenter.Z + 1 > roomend.Z)
                                {
                                    hits++;
                                    HitsLabel.Text = hits.ToString();
                                    if (hits >= misses + 2 * level && levelcheck.Checked)
                                    {
                                        level++;
                                        levellabel.Text = level.ToString();
                                    }
                                }
                                else
                                {
                                    misses++;
                                    Misseslabel.Text = misses.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((CannonBall.MassCenter.Z - 1 < roomend.Z || CannonBall.MassCenter.Z + 1 > roomorigin.Z))
                        {
                            CannonBall.SpeedZ = -1 * CannonBall.SpeedZ;
                        }
                    }
                }
                if (SurfaceCheck.Checked)
                {
                    for (long surfacek = 0; surfacek < surface.LongLength; surfacek++)
                    {
                        CannonBall = Phys.SphereSurfaceCollision(CannonBall, 1, surface[surfacek][0], surface[surfacek][1], surface[surfacek][2], false);
                    }
                }
                if (Game2.Checked) //scorecheck
                {
                    if (PaddleLocation2.X > CannonBall.MassCenter.X + paddlespeed * interval)
                        PaddleLocation2.X = (float)(PaddleLocation2.X - paddlespeed * interval);
                    else
                    {
                        if (PaddleLocation2.X < CannonBall.MassCenter.X - paddlespeed * interval)
                            PaddleLocation2.X = (float)(PaddleLocation2.X + paddlespeed * interval);
                        else
                            PaddleLocation2.X = CannonBall.MassCenter.X;
                    }
                    if (PaddleLocation2.Y > CannonBall.MassCenter.Y + paddlespeed * interval)
                        PaddleLocation2.Y = (float)(PaddleLocation2.Y - paddlespeed * interval);
                    else
                    {
                        if (PaddleLocation2.Y < CannonBall.MassCenter.Y - paddlespeed * interval)
                            PaddleLocation2.Y = (float)(PaddleLocation2.Y + paddlespeed * interval);
                        else
                            PaddleLocation2.Y = CannonBall.MassCenter.Y;
                    }
                    if (CannonBall.MassCenter.Z + 1 > PaddleLocation2.Z && CannonBall.MassCenter.X > PaddleLocation2.X - paddlesize && CannonBall.MassCenter.X < PaddleLocation2.X + paddlesize && CannonBall.MassCenter.Y > PaddleLocation2.Y - paddlesize && CannonBall.MassCenter.Y < PaddleLocation2.Y + paddlesize)
                    {
                        CannonBall.SpeedZ = -1 * Math.Abs(CannonBall.SpeedZ);
                    }
                    else
                    {
                        if (CannonBall.MassCenter.Z - 1 < PaddleLocation.Z && CannonBall.MassCenter.X > PaddleLocation.X - paddlesize && CannonBall.MassCenter.X < PaddleLocation.X + paddlesize && CannonBall.MassCenter.Y > PaddleLocation.Y - paddlesize && CannonBall.MassCenter.Y < PaddleLocation.Y + paddlesize)
                        {
                            CannonBall.SpeedZ = Math.Abs(CannonBall.SpeedZ);
                        }
                    }
                }
                if (WindAccelaration * time <= windspeed || windspeedlimit == false)
                {
                    CannonBall.SpeedX = (float)(CannonBall.SpeedX + WindAccelaration * interval * Math.Cos((double)(WindDirection)) / 1000F);
                    CannonBall.SpeedZ = (float)(CannonBall.SpeedZ + WindAccelaration * interval * Math.Sin((double)(WindDirection)) / 1000F);
                }
                if (Gravity.Checked)
                    CannonBall.SpeedY = (float)(CannonBall.SpeedY - gravity * interval / 1000F);
                CannonBall.MoveShape((float)(CannonBall.SpeedX * interval / 1000F), (float)(CannonBall.SpeedY * interval / 1000F), (float)(CannonBall.SpeedZ * interval / 1000F));

                if (Trail.Checked)
                {
                    Array.Resize(ref trailpoints, (int)(trailpoints.LongLength + 1));
                    Array.Resize(ref drawtrailpoints, (int)(drawtrailpoints.LongLength + 1));
                    trailpoints[trailpoints.LongLength - 1] = new Point3D(CannonBall.MassCenter.X, CannonBall.MassCenter.Y, CannonBall.MassCenter.Z);
                }
                if (GameStart.Checked && hit == false)
                {
                    if (CannonBall.MassCenter.X < targetloc.X + 0.25 && CannonBall.MassCenter.X > targetloc.X - 0.25 && CannonBall.MassCenter.Y < targetloc.Y + 0.25 && CannonBall.MassCenter.Y > targetloc.Y - 0.25 && CannonBall.MassCenter.Z < targetloc.Z + 0.25 && CannonBall.MassCenter.Z > targetloc.Z - 0.25)
                    {
                        hit = true;
                    }
                    else
                    {
                        for (a = 0; a < 360; a++)
                        {
                            for (b = 0; b < 360; b++)
                            {
                                if (CannonBall.MassCenter.X + Math.Cos((double)(Math.PI * a / 180)) * Math.Cos((double)(Math.PI * b / 180)) < targetloc.X + 0.25 && CannonBall.MassCenter.X + Math.Cos((double)(Math.PI * a / 180)) * Math.Cos((double)(Math.PI * b / 180)) > targetloc.X - 0.25 && CannonBall.MassCenter.Y + Math.Sin((double)(Math.PI * a / 180)) < targetloc.Y + 0.25 && CannonBall.MassCenter.Y + Math.Sin((double)(Math.PI * a / 180)) > targetloc.Y - 0.25 && CannonBall.MassCenter.Z + Math.Cos((double)(Math.PI * a / 180)) * Math.Sin((double)(Math.PI * b / 180)) < targetloc.Z + 0.25 && CannonBall.MassCenter.Z + Math.Cos((double)(Math.PI * a / 180)) * Math.Sin((double)(Math.PI * b / 180)) > targetloc.Z - 0.25)
                                {
                                    hit = true;
                                    b = 360;
                                    a = 360;
                                }
                            }
                        }
                    }
                }
            }
            balllocX.Text = CannonBall.MassCenter.X.ToString();
            balllocY.Text = CannonBall.MassCenter.Y.ToString();
            balllocZ.Text = CannonBall.MassCenter.Z.ToString();
            clrscrn = false;
            panel1.Invalidate();
        }

        public void Help_Click(object sender, EventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                Cursor = new Cursor((IntPtr)(cursor));
            }
            else
            {
                MessageBox.Show("Click on what you want to learn how to use (click on button again to stop)");
                Cursor = new Cursor(Helpbutton.Cursor.Handle);
                helpcursor = Cursor.Handle.ToInt64();
            }
        }

        public void Cannon_Click(object sender, EventArgs e)
        {
            if (Game2.Checked)
            {
                if (panel4.Visible == false)
                {
                    panel4.Visible = true;
                    Ballmasslabel.Visible = true;
                    Cannonlocxlabel.Visible = true;
                    Cannonlocylabel.Visible = true;
                    Cannonloczlabel.Visible = true;
                    Cannonangle1label.Visible = true;
                    Cannonangle2label.Visible = true;
                    Cannonforcelabel.Visible = true;
                    Ballspeedlabel.Visible = true;

                    Ballmass.Visible = true;
                    CannonX.Visible = true;
                    CannonY.Visible = true;
                    CannonZ.Visible = true;
                    CannonAngleXY.Visible = true;
                    CannonAngleXY.ReadOnly = false;
                    CannonAngleXZ.Visible = true;
                    CannonAngleXZ.ReadOnly = false;
                    BallForce.Visible = true;
                    BallForce.ReadOnly = false;
                    BallSpeed.Visible = true;
                    BallSpeed.ReadOnly = false;
                    Trail.Visible = true;
                    Trail.Enabled = true;
                    Vectors.Visible = true;
                    Vectors.Enabled = true;
                    if (levelcheck.Checked)
                    {
                        BallForce.ReadOnly = true;
                        CannonAngleXY.ReadOnly = true;
                        CannonAngleXZ.ReadOnly = true;
                        BallSpeed.ReadOnly = true;
                    }
                }
                else
                {
                    panel4.Visible = false;
                    Ballmasslabel.Visible = false;
                    Cannonlocxlabel.Visible = false;
                    Cannonlocylabel.Visible = false;
                    Cannonloczlabel.Visible = false;
                    Cannonangle1label.Visible = false;
                    Cannonangle2label.Visible = false;
                    Cannonforcelabel.Visible = false;
                    Ballspeedlabel.Visible = false;

                    Ballmass.Visible = false;
                    Ballmass.ReadOnly = true;
                    CannonX.Visible = false;
                    CannonX.ReadOnly = true;
                    CannonY.Visible = false;
                    CannonY.ReadOnly = true;
                    CannonZ.Visible = false;
                    CannonZ.ReadOnly = true;
                    CannonAngleXY.Visible = false;
                    CannonAngleXY.ReadOnly = true;
                    CannonAngleXZ.Visible = false;
                    CannonAngleXZ.ReadOnly = true;
                    BallForce.Visible = false;
                    BallForce.ReadOnly = true;
                    BallSpeed.Visible = false;
                    BallSpeed.ReadOnly = true;
                    Trail.Visible = false;
                    Trail.Enabled = false;
                    Vectors.Visible = false;
                    Vectors.Enabled = false;
                }
            }
            else
            {
                if (GameStart.Checked)
                {
                    if (panel4.Visible == false)
                    {
                        panel4.Visible = true;
                        Ballmasslabel.Visible = true;
                        Cannonlocxlabel.Visible = true;
                        Cannonlocylabel.Visible = true;
                        Cannonloczlabel.Visible = true;
                        Cannonangle1label.Visible = true;
                        Cannonangle2label.Visible = true;
                        Cannonforcelabel.Visible = true;
                        Ballspeedlabel.Visible = true;

                        Ballmass.Visible = true;
                        CannonX.Visible = true;
                        CannonY.Visible = true;
                        CannonZ.Visible = true;
                        CannonAngleXY.Visible = true;
                        CannonAngleXY.ReadOnly = false;
                        CannonAngleXZ.Visible = true;
                        CannonAngleXZ.ReadOnly = false;
                        BallForce.Visible = true;
                        BallForce.ReadOnly = false;
                        BallSpeed.Visible = true;
                        Trail.Visible = true;
                        Trail.Enabled = true;
                        Vectors.Visible = true;
                        Vectors.Enabled = true;
                    }
                    else
                    {
                        panel4.Visible = false;
                        Ballmasslabel.Visible = false;
                        Cannonlocxlabel.Visible = false;
                        Cannonlocylabel.Visible = false;
                        Cannonloczlabel.Visible = false;
                        Cannonangle1label.Visible = false;
                        Cannonangle2label.Visible = false;
                        Cannonforcelabel.Visible = false;
                        Ballspeedlabel.Visible = false;

                        Ballmass.Visible = false;
                        Ballmass.ReadOnly = true;
                        CannonX.Visible = false;
                        CannonX.ReadOnly = true;
                        CannonY.Visible = false;
                        CannonY.ReadOnly = true;
                        CannonZ.Visible = false;
                        CannonZ.ReadOnly = true;
                        CannonAngleXY.Visible = false;
                        CannonAngleXY.ReadOnly = true;
                        CannonAngleXZ.Visible = false;
                        CannonAngleXZ.ReadOnly = true;
                        BallForce.Visible = false;
                        BallForce.ReadOnly = true;
                        BallSpeed.Visible = false;
                        BallSpeed.ReadOnly = true;
                        Trail.Visible = false;
                        Trail.Enabled = false;
                        Vectors.Visible = false;
                        Vectors.Enabled = false;
                    }
                }
                else
                {
                    if (panel4.Visible == false)
                    {
                        panel4.Visible = true;
                        Ballmasslabel.Visible = true;
                        Cannonlocxlabel.Visible = true;
                        Cannonlocylabel.Visible = true;
                        Cannonloczlabel.Visible = true;
                        Cannonangle1label.Visible = true;
                        Cannonangle2label.Visible = true;
                        Cannonforcelabel.Visible = true;
                        Ballspeedlabel.Visible = true;

                        Ballmass.Visible = true;
                        Ballmass.ReadOnly = false;
                        CannonX.Visible = true;
                        CannonX.ReadOnly = false;
                        CannonY.Visible = true;
                        CannonY.ReadOnly = false;
                        CannonZ.Visible = true;
                        CannonZ.ReadOnly = false;
                        CannonAngleXY.Visible = true;
                        CannonAngleXY.ReadOnly = false;
                        CannonAngleXZ.Visible = true;
                        CannonAngleXZ.ReadOnly = false;
                        BallForce.Visible = true;
                        BallForce.ReadOnly = false;
                        BallSpeed.Visible = true;
                        BallSpeed.ReadOnly = false;
                        Trail.Visible = true;
                        Trail.Enabled = true;
                        Vectors.Visible = true;
                        Vectors.Enabled = true;
                    }
                    else
                    {
                        panel4.Visible = false;
                        Ballmasslabel.Visible = false;
                        Cannonlocxlabel.Visible = false;
                        Cannonlocylabel.Visible = false;
                        Cannonloczlabel.Visible = false;
                        Cannonangle1label.Visible = false;
                        Cannonangle2label.Visible = false;
                        Cannonforcelabel.Visible = false;
                        Ballspeedlabel.Visible = false;

                        Ballmass.Visible = false;
                        Ballmass.ReadOnly = true;
                        CannonX.Visible = false;
                        CannonX.ReadOnly = true;
                        CannonY.Visible = false;
                        CannonY.ReadOnly = true;
                        CannonZ.Visible = false;
                        CannonZ.ReadOnly = true;
                        CannonAngleXY.Visible = false;
                        CannonAngleXY.ReadOnly = true;
                        CannonAngleXZ.Visible = false;
                        CannonAngleXZ.ReadOnly = true;
                        BallForce.Visible = false;
                        BallForce.ReadOnly = true;
                        BallSpeed.Visible = false;
                        BallSpeed.ReadOnly = true;
                        Trail.Visible = false;
                        Trail.Enabled = false;
                        Vectors.Visible = false;
                        Vectors.Enabled = false;
                    }
                }
            }
        }

        public void GameStart_CheckedChanged(object sender, EventArgs e)
        {
            if (GameStart.Checked)
            {
                Game2.Enabled = false;
                panel6.Visible = true;
                levelcheck.Visible = true;
                HitsLabel.Visible = true;
                HitsLabelLabel.Visible = true;
                Misseslabel.Visible = true;
                MissesLabelLabel.Visible = true;
                ScoreLabel.Visible = true;
                ScoreLabelLabel.Visible = true;
                Ballmass.ReadOnly = true;
                CannonX.ReadOnly = true;
                CannonY.ReadOnly = true;
                CannonZ.ReadOnly = true;
                BallSpeed.ReadOnly = true;
                Ballmass.Text = "100";
                CannonX.Text = "0";
                CannonY.Text = "0";
                CannonZ.Text = "0";
                BallSpeed.Text = "0";
                BallForce.Text = "0";
                Views.SelectedIndex = 0;
                Perspective.Checked = true;
                PerspectiveX.ReadOnly = true;
                PerspectiveX.Text = "0";
                PerspectiveY.ReadOnly = true;
                PerspectiveY.Text = "0";
                PerspectiveZ.ReadOnly = true;
                PerspectiveZ.Text = "-50";
                Room.Checked = false;
                Room.Enabled = false;
                SurfaceCheck.Checked = false;
                SurfaceCheck.Enabled = false;
                Gravity.Checked = true;
                Gravity.Enabled = false;
                Wind.Checked = true;
                Wind.Enabled = false;
                levelcheck.Enabled = true;
                levelcheck.Checked = false;
                windforce1.ReadOnly = true;
                windaccelaration1.ReadOnly = true;
                winddirection1.ReadOnly = true;

                HitsLabel.Text = "0";
                Misseslabel.Text = "0";
                ScoreLabel.Text = "0";

                MinTargetDistanceParameter.Text = "3";
                FuseParameter.Text = "1.5";
                IntervalParameter.Text = "0.1";
                GravityParameter.Text = "5";
                TargetDistanceParameter.Text = "15";
                WindLimitParameter.Text = "0.01";
                GridUnitsParameter.Text = "7.5";
                WindSpeedLimitParameter.Text = "10";
                WindSpeedLimitCheck.Checked = true;

                Random rand = new Random();
                targetloc = new Point3D((float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F, (float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F, (float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F);
                target = Presets.TargetPoints(targetloc);
                windaccelaration1.Text = ((float)((float)(rand.Next(0, (int)(windlimit * 1000))) / 1000F)).ToString();
                winddirection1.Text = ((float)(rand.Next(0, 360))).ToString();
            }
            else
            {
                Game2.Enabled = true;
                panel6.Visible = false;
                levelcheck.Visible = false;
                HitsLabel.Visible = false;
                HitsLabelLabel.Visible = false;
                Misseslabel.Visible = false;
                MissesLabelLabel.Visible = false;
                ScoreLabel.Visible = false;
                ScoreLabelLabel.Visible = false;
                Ballmass.ReadOnly = false;
                CannonX.ReadOnly = false;
                CannonY.ReadOnly = false;
                CannonZ.ReadOnly = false;
                BallSpeed.ReadOnly = false;
                Views.Enabled = true;
                Perspective.Enabled = true;
                PerspectiveX.ReadOnly = false;
                PerspectiveY.ReadOnly = false;
                PerspectiveZ.ReadOnly = false;
                Room.Enabled = true;
                SurfaceCheck.Enabled = true;
                Gravity.Enabled = true;
                Wind.Enabled = true;
                levelcheck.Enabled = false;
                levelcheck.Checked = false;
                windforce1.ReadOnly = false;
                windaccelaration1.ReadOnly = false;
                winddirection1.ReadOnly = false;

                hits = 0;
                misses = 0;
                score = 0;
            }
        }

        public void ParametersCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ParametersCheck.Checked)
            {
                panel7.Visible = true;
                Gaplabel.Visible = true;
                sizepaddlelabel.Visible = true;
                speedpaddlelabel.Visible = true;
                Gap.ReadOnly = false;
                Gap.Visible = true;
                sizepaddle.Visible = true;
                speedpaddle.Visible = true;
                sizepaddle.ReadOnly = false;
                speedpaddle.ReadOnly = false;
                MinimumTargetDistancelabel.Visible = true;
                MinimumTargetDistancelabel.Visible = true;
                MinTargetDistanceParameter.Visible = true;
                MinTargetDistanceParameter.ReadOnly = false;
                TargetDistancelabel.Visible = true;
                TargetDistanceParameter.Visible = true;
                TargetDistanceParameter.ReadOnly = false;
                GridUnitslabel.Visible = true;
                GridUnitsParameter.Visible = true;
                GridUnitsParameter.ReadOnly = false;
                GravityParlabel.Visible = true;
                GravityParameter.Visible = true;
                GravityParameter.ReadOnly = false;
                Intervallabel.Visible = true;
                IntervalParameter.Visible = true;
                IntervalParameter.ReadOnly = false;
                Fuselabel.Visible = true;
                FuseParameter.Visible = true;
                FuseParameter.ReadOnly = false;
                WindLimitlabel.Visible = true;
                WindLimitParameter.Visible = true;
                WindLimitParameter.ReadOnly = false;
                WindSpeedLimitlabel.Visible = true;
                WindSpeedLimitParameter.Visible = true;
                WindSpeedLimitParameter.ReadOnly = false;
                WindSpeedLimitCheck.Visible = true;
                WindSpeedLimitCheck.Enabled = true;

                StartTimeS.Enabled = false;
                Cannon.Enabled = false;
                Properties.Enabled = false;
                Room.Enabled = false;
                SurfaceCheck.Enabled = false;
                ZoomBox.ReadOnly = true;
                PerspectiveX.ReadOnly = true;
                PerspectiveY.ReadOnly = true;
                PerspectiveZ.ReadOnly = true;
                Gravity.Enabled = false;
                GameStart.Enabled = false;
                Game2.Enabled = false;
                RoomX.ReadOnly = true;
                RoomY.ReadOnly = true;
                RoomZ.ReadOnly = true;
                RoomOX.ReadOnly = true;
                RoomOY.ReadOnly = true;
                RoomOZ.ReadOnly = true;
                SurfaceP1X.ReadOnly = true;
                SurfaceP1Y.ReadOnly = true;
                SurfaceP1Z.ReadOnly = true;
                SurfaceP2X.ReadOnly = true;
                SurfaceP2Y.ReadOnly = true;
                SurfaceP2Z.ReadOnly = true;
                SurfaceP3X.ReadOnly = true;
                SurfaceP3Y.ReadOnly = true;
                SurfaceP3Z.ReadOnly = true;
                surfaceComboBox.Enabled = false;
                Perspective.Enabled = false;
                Oblique.Enabled = false;
                Views.Enabled = false;
                Ballmass.ReadOnly = true;
                CannonX.ReadOnly = true;
                CannonY.ReadOnly = true;
                CannonZ.ReadOnly = true;
                CannonAngleXY.ReadOnly = true;
                CannonAngleXZ.ReadOnly = true;
                BallForce.ReadOnly = true;
                BallSpeed.ReadOnly = true;
                winddirection1.ReadOnly = true;
                windaccelaration1.ReadOnly = true;
                windforce1.ReadOnly = true;
                gridcheck.Enabled = false;
                Helpbutton.Enabled = false;
                Trail.Enabled = false;
                Vectors.Enabled = false;
                Wind.Enabled = false;
                Views.Enabled = false;
                levelcheck.Enabled = false;

                if (StartTimeS.Text == "Stop" || PauseButton.Text == "Unpause" || levelcheck.Checked == true || GameStart.Checked)
                {
                    StartTimeS.Enabled = true;
                    MinTargetDistanceParameter.ReadOnly = true;
                    TargetDistanceParameter.ReadOnly = true;
                    GridUnitsParameter.ReadOnly = true;
                    GravityParameter.ReadOnly = true;
                    IntervalParameter.ReadOnly = true;
                    FuseParameter.ReadOnly = true;
                    WindLimitParameter.ReadOnly = true;
                    WindSpeedLimitParameter.ReadOnly = true;
                    WindSpeedLimitCheck.Enabled = false;
                    Gap.ReadOnly = true;
                    sizepaddle.ReadOnly = true;
                    speedpaddle.ReadOnly = true;
                }
            }
            else
            {
                panel7.Visible = false;
                Gaplabel.Visible = false;
                sizepaddlelabel.Visible = false;
                speedpaddlelabel.Visible = false;
                Gap.Visible = false;
                sizepaddle.Visible = false;
                speedpaddle.Visible = false;
                Gap.ReadOnly = true;
                sizepaddle.ReadOnly = true;
                speedpaddle.ReadOnly = true;
                MinimumTargetDistancelabel.Visible = false;
                MinTargetDistanceParameter.Visible = false;
                MinTargetDistanceParameter.ReadOnly = true;
                TargetDistancelabel.Visible = false;
                TargetDistanceParameter.Visible = false;
                TargetDistanceParameter.ReadOnly = true;
                GridUnitslabel.Visible = false;
                GridUnitsParameter.Visible = false;
                GridUnitsParameter.ReadOnly = true;
                GravityParlabel.Visible = false;
                GravityParameter.Visible = false;
                GravityParameter.ReadOnly = true;
                Intervallabel.Visible = false;
                IntervalParameter.Visible = false;
                IntervalParameter.ReadOnly = true;
                Fuselabel.Visible = false;
                FuseParameter.Visible = false;
                FuseParameter.ReadOnly = true;
                WindLimitlabel.Visible = false;
                WindLimitParameter.Visible = false;
                WindLimitParameter.ReadOnly = true;
                WindSpeedLimitlabel.Visible = false;
                WindSpeedLimitParameter.Visible = false;
                WindSpeedLimitParameter.ReadOnly = true;
                WindSpeedLimitCheck.Visible = false;
                WindSpeedLimitCheck.Enabled = false;

                StartTimeS.Enabled = true;
                Cannon.Enabled = true;
                Properties.Enabled = true;
                Oblique.Enabled = true;
                Views.Enabled = true;
                GameStart.Enabled = true;
                Game2.Enabled = true;
                gridcheck.Enabled = true;
                Helpbutton.Enabled = true;
                Views.Enabled = true;
                Wind.Enabled = true;
                Perspective.Enabled = true;
                ZoomBox.ReadOnly = false;
                if (GameStart.Checked == false)
                {
                    Room.Enabled = true;
                    PerspectiveX.ReadOnly = false;
                    PerspectiveY.ReadOnly = false;
                    PerspectiveZ.ReadOnly = false;
                    Gravity.Enabled = true;
                    if (panel4.Visible)
                    {
                        Ballmass.ReadOnly = false;
                        CannonX.ReadOnly = false;
                        CannonY.ReadOnly = false;
                        CannonZ.ReadOnly = false;
                        CannonAngleXY.ReadOnly = false;
                        CannonAngleXZ.ReadOnly = false;
                        BallForce.ReadOnly = false;
                        BallSpeed.ReadOnly = false;
                        Trail.Enabled = true;
                        Vectors.Enabled = true;
                    }
                    if (panel8.Visible)
                    {
                        SurfaceP1X.ReadOnly = false;
                        SurfaceP1Y.ReadOnly = false;
                        SurfaceP1Z.ReadOnly = false;
                        SurfaceP2X.ReadOnly = false;
                        SurfaceP2Y.ReadOnly = false;
                        SurfaceP2Z.ReadOnly = false;
                        SurfaceP3X.ReadOnly = false;
                        SurfaceP3Y.ReadOnly = false;
                        SurfaceP3Z.ReadOnly = false;
                        surfaceComboBox.Enabled = true;
                    }
                    if (panel3.Visible)
                    {
                        RoomX.ReadOnly = false;
                        RoomY.ReadOnly = false;
                        RoomZ.ReadOnly = false;
                        RoomOX.ReadOnly = false;
                        RoomOY.ReadOnly = false;
                        RoomOZ.ReadOnly = false;
                    }
                    if (panel5.Visible)
                    {
                        winddirection1.ReadOnly = false;
                        windaccelaration1.ReadOnly = false;
                        windforce1.ReadOnly = false;
                    }
                }
                else
                {
                    levelcheck.Enabled = true;
                    if (panel4.Visible)
                    {
                        CannonAngleXY.ReadOnly = false;
                        CannonAngleXZ.ReadOnly = false;
                        BallForce.ReadOnly = false;
                        Trail.Enabled = true;
                        Vectors.Enabled = true;
                    }
                }
            }
        }

        public void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point3D Temp3d = new Point3D(0, 0, 0);
            PointF TempPoint = new PointF();
            if (Trail.Checked && (StartTimeS.Text == "Reset" || PauseButton.Text == "Unpause") && (e.Button).ToString() == "Left")
            {
                long x = 0;
                for (x = 0; x < drawtrailpoints.LongLength; x++)
                {
                    if (e.X > drawtrailpoints[x].X - 5 && e.X < drawtrailpoints[x].X + 5 && e.Y > drawtrailpoints[x].Y - 5 && e.Y < drawtrailpoints[x].Y + 5)
                    {
                        MessageBox.Show("X: " + (trailpoints[x].X).ToString() + ", Y: " + (trailpoints[x].Y).ToString() + ", Z: " + (trailpoints[x].Z).ToString() + ", Time: " + ((float)((x + 1) * interval / 1000)).ToString());
                        x = drawtrailpoints.LongLength;
                    }
                }
            }
            if (Vectors.Checked && (StartTimeS.Text == "Reset" || PauseButton.Text == "Unpause") && (e.Button).ToString() == "Left")
            {
                Temp3d = new Point3D(CannonBall.SpeedX + CannonBall.MassCenter.X, CannonBall.SpeedY + CannonBall.MassCenter.Y, CannonBall.SpeedZ + CannonBall.MassCenter.Z);
                if (this.Views.SelectedIndex == 1)
                {
                    Temp3d.YtoZ();
                }
                else
                    if (this.Views.SelectedIndex == 2)
                    {
                        Temp3d.ZtoX();
                    }
                if (e.X > Transform3D(Temp3d).X - 5 && e.X < Transform3D(Temp3d).X + 5 && e.Y > Transform3D(Temp3d).Y - 5 && e.Y < Transform3D(Temp3d).Y + 5)
                {
                    MessageBox.Show("Speed: X: " + (CannonBall.SpeedX).ToString() + ", Y: " + (CannonBall.SpeedY).ToString() + ", Z: " + (CannonBall.SpeedZ).ToString());
                }

                Temp3d = new Point3D(CannonBall.MassCenter.X, CannonBall.MassCenter.Y, CannonBall.MassCenter.Z);
                if (Gravity.Checked)
                {
                    Temp3d.Y -= gravity;
                }
                if (Wind.Checked)
                {
                    Temp3d.X += (float)(WindAccelaration * Math.Cos((double)(WindDirection)));
                    Temp3d.Z += (float)(WindAccelaration * Math.Sin((double)(WindDirection)));
                }
                if (this.Views.SelectedIndex == 1)
                {
                    Temp3d.YtoZ();
                }
                else
                    if (this.Views.SelectedIndex == 2)
                    {
                        Temp3d.ZtoX();
                    }
                if (e.X > Transform3D(Temp3d).X - 5 && e.X < Transform3D(Temp3d).X + 5 && e.Y > Transform3D(Temp3d).Y - 5 && e.Y < Transform3D(Temp3d).Y + 5)
                {
                    MessageBox.Show("Acceleration: X: " + ((float)(WindAccelaration * Math.Cos((double)(WindDirection)))).ToString() + ", Y: " + (-1 * gravity).ToString() + ", Z: " + ((float)(WindAccelaration * Math.Sin((double)(WindDirection)))).ToString());
                }
            }
            if (GameStart.Checked && (StartTimeS.Text == "Reset" || PauseButton.Text == "Unpause") && (e.Button).ToString() == "Left")
            {
                if (this.Views.SelectedIndex == 1)
                {
                    targetloc.YtoZ();
                }
                else
                    if (this.Views.SelectedIndex == 2)
                    {
                        targetloc.ZtoX();
                    }
                if (/*hit && */e.X > (Transform3D(targetloc.X, targetloc.Y, targetloc.Z).X - 5) && e.X < (Transform3D(targetloc.X, targetloc.Y, targetloc.Z).X + 5) && e.Y > (Transform3D(targetloc.X, targetloc.Y, targetloc.Z).Y - 5) && e.Y < (Transform3D(targetloc.X, targetloc.Y, targetloc.Z).Y + 5))
                {
                    MessageBox.Show("X: " + (targetloc.X).ToString() + ", Y: " + (targetloc.Y).ToString() + ", Z: " + (targetloc.Z).ToString());
                }
                if (this.Views.SelectedIndex == 1)
                {
                    targetloc.YtoZ();
                }
                else
                    if (this.Views.SelectedIndex == 2)
                    {
                        targetloc.ZtoX();
                    }
            }
        }

        public void gridcheck_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        public void levelcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (GameStart.Checked)
            {
                if (levelcheck.Checked)
                {
                    levellabellabel.Visible = true;
                    levellabel.Visible = true;
                    levellabel.Text = "1";
                    windspeedlimit = true;

                    Random rand = new Random();
                    targetloc = new Point3D((float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F, (float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F, (float)(rand.Next((int)(-20 * targetdistance), (int)(20 * targetdistance + 1))) / 20F);
                    target = Presets.TargetPoints(targetloc);
                    windaccelaration1.Text = ((float)((float)(rand.Next(0, (int)(windlimit * 1000))) / 1000F)).ToString();
                    winddirection1.Text = ((float)(rand.Next(0, 360))).ToString();
                }
                else
                {
                    levellabellabel.Visible = false;
                    levellabel.Visible = false;
                    levellabel.Text = "0";

                    TargetDistanceParameter.Text = "15"; //Parameters
                    WindLimitParameter.Text = "0.01";
                    GridUnitsParameter.Text = "7.5";
                    WindSpeedLimitParameter.Text = "10";
                    WindSpeedLimitCheck.Checked = true;
                }
            }
            else
            {
                hits = 0;
                misses = 0;
                HitsLabel.Text = "0";
                Misseslabel.Text = "0";
                if (levelcheck.Checked)
                {
                    levellabellabel.Visible = true;
                    levellabel.Visible = true;
                    levellabel.Text = "1";

                    Room.Checked = true;
                    Room.Enabled = false;
                    RoomOX.Enabled = false;
                    RoomOY.Enabled = false;
                    RoomOZ.Enabled = false;
                    RoomX.Enabled = false;
                    RoomY.Enabled = false;
                    RoomZ.Enabled = false;
                    BallForce.Enabled = false;
                    Ballmass.Enabled = false;
                    BallSpeed.Enabled = false;
                    CannonAngleXY.Enabled = false;
                    CannonAngleXZ.Enabled = false;
                }
                else
                {
                    levellabellabel.Visible = false;
                    levellabel.Visible = false;
                    levellabel.Text = "0";

                    Room.Enabled = true;
                    RoomOX.Enabled = true;
                    RoomOY.Enabled = true;
                    RoomOZ.Enabled = true;
                    RoomX.Enabled = true;
                    RoomY.Enabled = true;
                    RoomZ.Enabled = true;
                    BallForce.Enabled = true;
                    Ballmass.Enabled = true;
                    BallSpeed.Enabled = true;
                    CannonAngleXY.Enabled = true;
                    CannonAngleXZ.Enabled = true;
                    GridUnitsParameter.Text = "7.5";
                }
                Properties_Click(sender, e);
                Properties_Click(sender, e);
            }
            panel1.Invalidate();
        }

        public void Room_CheckedChanged(object sender, EventArgs e)
        {
            if (Room.Checked)
            {
                if (panel2.Visible)
                {
                    panel3.Visible = true;
                    Roomxlabel.Visible = true;
                    Roomylabel.Visible = true;
                    Roomzlabel.Visible = true;
                    RoomXlabel2.Visible = true;
                    RoomYlabel2.Visible = true;
                    RoomZlabel2.Visible = true;
                    RoomX.Visible = true;
                    RoomX.ReadOnly = false;
                    RoomY.Visible = true;
                    RoomY.ReadOnly = false;
                    RoomZ.Visible = true;
                    RoomZ.ReadOnly = false;
                    RoomOX.Visible = true;
                    RoomOX.ReadOnly = false;
                    RoomOY.Visible = true;
                    RoomOY.ReadOnly = false;
                    RoomOZ.Visible = true;
                    RoomOZ.ReadOnly = false;
                }
            }
            else
            {
                panel3.Visible = false;
                Roomxlabel.Visible = false;
                Roomylabel.Visible = false;
                Roomzlabel.Visible = false;
                RoomXlabel2.Visible = false;
                RoomYlabel2.Visible = false;
                RoomZlabel2.Visible = false;
                RoomX.Visible = false;
                RoomX.ReadOnly = true;
                RoomY.Visible = false;
                RoomY.ReadOnly = true;
                RoomZ.Visible = false;
                RoomZ.ReadOnly = true;
                RoomOX.Visible = false;
                RoomOX.ReadOnly = true;
                RoomOY.Visible = false;
                RoomOY.ReadOnly = true;
                RoomOZ.Visible = false;
                RoomOZ.ReadOnly = true;
            }
            panel1.Invalidate();
        }

        public void WindSpeedLimitCheck_CheckedChanged(object sender, EventArgs e)
        {
            windspeedlimit = WindSpeedLimitCheck.Checked;
        }

        public void SurfaceCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (SurfaceCheck.Checked)
            {
                panel8.Visible = true;

                SurfacePoint1label.Visible = true;
                SurfacePoint2label.Visible = true;
                SurfacePoint3label.Visible = true;
                SurfaceP1Xlabel.Visible = true;
                SurfaceP1Ylabel.Visible = true;
                SurfaceP1Zlabel.Visible = true;
                SurfaceP2Xlabel.Visible = true;
                SurfaceP2Ylabel.Visible = true;
                SurfaceP2Zlabel.Visible = true;
                SurfaceP3Xlabel.Visible = true;
                SurfaceP3Ylabel.Visible = true;
                SurfaceP3Zlabel.Visible = true;

                SurfaceP1X.Visible = true;
                SurfaceP1X.ReadOnly = false;
                SurfaceP1Y.Visible = true;
                SurfaceP1Y.ReadOnly = false;
                SurfaceP1Z.Visible = true;
                SurfaceP1Z.ReadOnly = false;
                SurfaceP2X.Visible = true;
                SurfaceP2X.ReadOnly = false;
                SurfaceP2Y.Visible = true;
                SurfaceP2Y.ReadOnly = false;
                SurfaceP2Z.Visible = true;
                SurfaceP2Z.ReadOnly = false;
                SurfaceP3X.Visible = true;
                SurfaceP3X.ReadOnly = false;
                SurfaceP3Y.Visible = true;
                SurfaceP3Y.ReadOnly = false;
                SurfaceP3Z.Visible = true;
                SurfaceP3Z.ReadOnly = false;
                surfaceComboBox.Visible = true;
                surfaceComboBox.Enabled = true;
            }
            else
            {
                panel8.Visible = false;

                SurfacePoint1label.Visible = false;
                SurfacePoint2label.Visible = false;
                SurfacePoint3label.Visible = false;
                SurfaceP1Xlabel.Visible = false;
                SurfaceP1Ylabel.Visible = false;
                SurfaceP1Zlabel.Visible = false;
                SurfaceP2Xlabel.Visible = false;
                SurfaceP2Ylabel.Visible = false;
                SurfaceP2Zlabel.Visible = false;
                SurfaceP3Xlabel.Visible = false;
                SurfaceP3Ylabel.Visible = false;
                SurfaceP3Zlabel.Visible = false;

                SurfaceP1X.Visible = false;
                SurfaceP1X.ReadOnly = true;
                SurfaceP1Y.Visible = false;
                SurfaceP1Y.ReadOnly = true;
                SurfaceP1Z.Visible = false;
                SurfaceP1Z.ReadOnly = true;
                SurfaceP2X.Visible = false;
                SurfaceP2X.ReadOnly = true;
                SurfaceP2Y.Visible = false;
                SurfaceP2Y.ReadOnly = true;
                SurfaceP2Z.Visible = false;
                SurfaceP2Z.ReadOnly = true;
                SurfaceP3X.Visible = false;
                SurfaceP3X.ReadOnly = true;
                SurfaceP3Y.Visible = false;
                SurfaceP3Y.ReadOnly = true;
                SurfaceP3Z.Visible = false;
                SurfaceP3Z.ReadOnly = true;
                surfaceComboBox.Visible = false;
                surfaceComboBox.Enabled = false;
            }
            try
            {
                Array.Resize(ref surface[CurrentSurface], 3);
            }
            catch
            {
                Array.Resize(ref surface, 1);
                Array.Resize(ref surface[0], 3);
            }
            try
            {
                surface[CurrentSurface][0] = new Point3D(float.Parse(SurfaceP1X.Text), float.Parse(SurfaceP1Y.Text), float.Parse(SurfaceP1Z.Text));
            }
            catch { }
            try
            {
                surface[CurrentSurface][1] = new Point3D(float.Parse(SurfaceP2X.Text), float.Parse(SurfaceP2Y.Text), float.Parse(SurfaceP2Z.Text));
            }
            catch { }
            try
            {
                surface[CurrentSurface][2] = new Point3D(float.Parse(SurfaceP3X.Text), float.Parse(SurfaceP3Y.Text), float.Parse(SurfaceP3Z.Text));
            }
            catch { }
            panel1.Invalidate();
        }

        public void surfaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (surfaceComboBox.SelectedIndex <= HighSurfaceIndex)
            {
                try
                {
                    CurrentSurface = long.Parse(surfaceComboBox.SelectedItem.ToString()) - 1;
                    SurfaceP1X.Text = (surface[CurrentSurface][0].X).ToString();
                    SurfaceP1Y.Text = (surface[CurrentSurface][0].Y).ToString();
                    SurfaceP1Z.Text = (surface[CurrentSurface][0].Z).ToString();
                    SurfaceP2X.Text = (surface[CurrentSurface][1].X).ToString();
                    SurfaceP2Y.Text = (surface[CurrentSurface][1].Y).ToString();
                    SurfaceP2Z.Text = (surface[CurrentSurface][1].Z).ToString();
                    SurfaceP3X.Text = (surface[CurrentSurface][2].X).ToString();
                    SurfaceP3Y.Text = (surface[CurrentSurface][2].Y).ToString();
                    SurfaceP3Z.Text = (surface[CurrentSurface][2].Z).ToString();
                }
                catch
                {
                    if (surfaceComboBox.SelectedItem.ToString() == "Remove Surface")
                    {
                        if (CurrentSurface > 0)
                        {
                            if (CurrentSurface != HighSurfaceIndex - 2)
                            {
                                for (long t = CurrentSurface; t < surface.GetLongLength(0) - 1; t++)
                                {
                                    surface[t][0] = new Point3D(surface[t + 1][0]);
                                    surface[t][1] = new Point3D(surface[t + 1][1]);
                                    surface[t][2] = new Point3D(surface[t + 1][2]);
                                }
                            }
                            Array.Resize(ref surface, (int)(surface.GetLongLength(0) - 1));
                            HighSurfaceIndex = HighSurfaceIndex - 1;
                            surfaceComboBox.SelectedIndex = (int)(CurrentSurface + 2);
                        }
                        else
                        {
                            surfaceComboBox.SelectedIndex = 2;
                        }
                    }
                    else
                    {
                        if (surfaceComboBox.SelectedItem.ToString() == "New Surface" && HighSurfaceIndex < 11)
                        {
                            Array.Resize(ref surface, (int)(surface.GetLongLength(0) + 1));
                            Array.Resize(ref surface[surface.GetLongLength(0) - 1], 3);
                            CurrentSurface = surface.GetLongLength(0) - 1;
                            surface[CurrentSurface][0] = new Point3D(10, -10, -10);
                            surface[CurrentSurface][1] = new Point3D(10, -10, 10);
                            surface[CurrentSurface][2] = new Point3D(10, 10, 0);
                            HighSurfaceIndex = HighSurfaceIndex + 1;
                            surfaceComboBox.SelectedIndex = (int)(2 + CurrentSurface);
                        }
                    }
                }
            }
            else
            {
                surfaceComboBox.SelectedIndex = 2;
            }
        }

        public void Wind_CheckedChanged(object sender, EventArgs e)
        {
            if (Wind.Checked)
            {
                if (panel2.Visible)
                {
                    panel5.Visible = true;
                    winddirectionlabel.Visible = true;
                    windaccelarationlabel.Visible = true;
                    windforcelabel.Visible = true;
                    winddirection1.Visible = true;
                    windaccelaration1.Visible = true;
                    windforce1.Visible = true;
                    winddirection1.ReadOnly = false;
                    windaccelaration1.ReadOnly = false;
                    windforce1.ReadOnly = false;

                    if (StartTimeS.Text == "Stop")
                    {
                        winddirection1.ReadOnly = true;
                        windaccelaration1.ReadOnly = true;
                        windforce1.ReadOnly = true;
                    }
                }
            }
            else
            {
                panel5.Visible = false;
                winddirectionlabel.Visible = false;
                windaccelarationlabel.Visible = false;
                windforcelabel.Visible = false;
                winddirection1.Visible = false;
                windaccelaration1.Visible = false;
                windforce1.Visible = false;
                winddirection1.ReadOnly = true;
                windaccelaration1.ReadOnly = true;
                windforce1.ReadOnly = true;
            }
        }

        public void CannonX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cannonorigin.X = float.Parse(CannonX.Text);
            }
            catch
            { }
            panel1.Invalidate();
        }

        public void CannonY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cannonorigin.Y = float.Parse(CannonY.Text);
            }
            catch
            { }
            panel1.Invalidate();
        }

        public void CannonZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cannonorigin.Z = float.Parse(CannonZ.Text);
            }
            catch
            { }
            panel1.Invalidate();
        }

        public void CannonAngleXY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ballangleXY2 = double.Parse(CannonAngleXY.Text);
                if (ballangleXY2 >= 360)
                {
                    ballangleXY2 = ballangleXY2 - 360;
                    CannonAngleXY.Text = ballangleXY2.ToString();
                }
                else
                {
                    if (ballangleXY2 < 0)
                    {
                        ballangleXY2 = ballangleXY2 + 360;
                        CannonAngleXY.Text = ballangleXY2.ToString();
                    }
                }
            }
            catch
            { }
            panel1.Invalidate();
        }

        public void CannonAngleXZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ballangleXZ2 = double.Parse(CannonAngleXZ.Text) + 180;
                if (ballangleXZ2 >= 540)
                {
                    ballangleXZ2 = ballangleXZ2 - 540;
                    CannonAngleXZ.Text = ballangleXZ2.ToString();
                }
                else
                {
                    if (ballangleXZ2 < 180)
                    {
                        ballangleXZ2 = ballangleXZ2 + 180;
                        CannonAngleXZ.Text = ballangleXZ2.ToString();
                    }
                }
            }
            catch
            { }
            panel1.Invalidate();
        }

        public void BallForce_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (float.Parse(BallForce.Text) >= 0)
                {
                    cannonforce = float.Parse(BallForce.Text);

                    BallSpeed.Text = (Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass))).ToString();
                    cannonspeed = (float)(Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass)));
                }
            }
            catch
            {
            }
            BallSpeed.Text = (Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass))).ToString();
            cannonspeed = (float)(Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass)));
        }

        public void BallSpeed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (float.Parse(BallSpeed.Text) >= 0)
                {
                    cannonspeed = float.Parse(BallSpeed.Text);

                    BallForce.Text = (CannonBall.Mass * cannonspeed * cannonspeed / 4F).ToString();
                    cannonforce = (float)(CannonBall.Mass * cannonspeed * cannonspeed / 4F);
                }
            }
            catch
            {
            }
            BallForce.Text = (CannonBall.Mass * cannonspeed * cannonspeed / 4F).ToString();
            cannonforce = (float)(CannonBall.Mass * cannonspeed * cannonspeed / 4F);
        }

        public void Ballmass_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (float.Parse(Ballmass.Text) > 0)
                {
                    CannonBall.Mass = float.Parse(Ballmass.Text);

                    BallSpeed.Text = (Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass))).ToString();
                    cannonspeed = (float)(Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass)));
                    windaccelaration1.Text = (WindForce / CannonBall.Mass).ToString();
                    WindAccelaration = (float)(WindForce / CannonBall.Mass);
                }
            }
            catch
            {
            }
            BallSpeed.Text = (Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass))).ToString();
            cannonspeed = (float)(Math.Sqrt((double)(4 * cannonforce / CannonBall.Mass)));
            windaccelaration1.Text = (WindForce / CannonBall.Mass).ToString();
            WindAccelaration = (float)(WindForce / CannonBall.Mass);
        }

        public void RoomX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                roomend.X = float.Parse(RoomX.Text);
            }
            catch
            {
            }
            panel1.Invalidate();
        }

        public void RoomY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                roomend.Y = float.Parse(RoomY.Text);
            }
            catch
            {
            }
            panel1.Invalidate();
        }

        public void RoomZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                roomend.Z = float.Parse(RoomZ.Text);
            }
            catch
            {
            }
            if (Game2.Checked)
            {
                roomend.Z = Math.Abs(roomend.Z);
                TunnelSize.Z = roomend.Z - 2;
                PaddleLocation2.Z = roomend.Z + 1;
            }
            panel1.Invalidate();
        }

        public void RoomOX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                roomorigin.X = float.Parse(RoomOX.Text);
            }
            catch
            {
            }
            if (Game2.Checked)
            {
                roomorigin.X = Math.Abs(roomorigin.X);
                TunnelSize.X = roomorigin.X;
                roomend.X = -1 * roomorigin.X;
                RoomX.Text = (roomend.X).ToString();
            }
            panel1.Invalidate();
        }

        public void RoomOY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                roomorigin.Y = float.Parse(RoomOY.Text);
            }
            catch
            {
            }
            if (Game2.Checked)
            {
                roomorigin.Y = Math.Abs(roomorigin.Y);
                TunnelSize.Y = roomorigin.Y;
                roomend.Y = -1 * roomorigin.Y;
                RoomY.Text = (roomend.Y).ToString();
            }
            panel1.Invalidate();
        }

        public void RoomOZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                roomorigin.Z = float.Parse(RoomOZ.Text);
            }
            catch
            {
            }
            panel1.Invalidate();
        }

        public void winddirection1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                WindDirection = (float)(Math.PI * float.Parse(winddirection1.Text) / 180F);
            }
            catch { }
        }

        public void windforce1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                WindForce = float.Parse(windforce1.Text);

                windaccelaration1.Text = (WindForce / CannonBall.Mass).ToString();
                WindAccelaration = (float)(WindForce / CannonBall.Mass);
            }
            catch { }
            windaccelaration1.Text = (WindForce / CannonBall.Mass).ToString();
            WindAccelaration = (float)(WindForce / CannonBall.Mass);
        }

        public void windaccelaration1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                WindAccelaration = float.Parse(windaccelaration1.Text);

                windforce1.Text = (WindAccelaration * CannonBall.Mass).ToString();
                WindForce = (float)(WindAccelaration * CannonBall.Mass);
            }
            catch { }
            windforce1.Text = (WindAccelaration * CannonBall.Mass).ToString();
            WindForce = (float)(WindAccelaration * CannonBall.Mass);
        }

        public void levellabel_TextChanged(object sender, EventArgs e)
        {
            if (levelcheck.Checked)
            {
                level = 1;
                try
                {
                    level = long.Parse(levellabel.Text);
                }
                catch { } //Parameters
                if (GameStart.Checked)
                {
                    TargetDistanceParameter.Text = ((long)(level + minimumtargetdistance)).ToString();
                    WindLimitParameter.Text = ((float)(0.01F * level)).ToString();
                    GridUnitsParameter.Text = ((float)(level / 2F)).ToString();
                    WindSpeedLimitParameter.Text = ((long)(level)).ToString();
                }
                else
                {
                    level = level * 2;
                    BallSpeed.Text = (level).ToString();
                    level = level / 2;
                    Random rand = new Random();
                    ballangleXZ2 = 20 + 10 * rand.Next(0, 14);
                    CannonAngleXZ.Text = ballangleXZ2.ToString();
                    ballangleXY2 = 10 * rand.Next(-7, 8);
                    CannonAngleXY.Text = ballangleXY2.ToString();

                    if (level > 1)
                    {
                        StartTime_Click(sender, e);
                        StartTime_Click(sender, e);
                    }
                }
            }
            else
            {
            }
        }

        public void GravityParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gravity = float.Parse(GravityParameter.Text);
            }
            catch { }
        }

        public void IntervalParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                interval = (double)(float.Parse(IntervalParameter.Text) * 1000F);
            }
            catch { }
        }

        public void FuseParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fuse = float.Parse(FuseParameter.Text);
            }
            catch { }
        }

        public void TargetDistanceParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                targetdistance = (long)(float.Parse(TargetDistanceParameter.Text));
            }
            catch { }
        }

        public void MinTargetDistanceParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                minimumtargetdistance = (long)(float.Parse(MinTargetDistanceParameter.Text));
            }
            catch { }
        }

        public void GridUnitsParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gridunits = (float.Parse(GridUnitsParameter.Text));
            }
            catch { }
        }

        public void WindSpeedLimitParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                windspeed = (float.Parse(WindSpeedLimitParameter.Text));
            }
            catch { }
        }

        public void WindLimitParameter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                windlimit = (float.Parse(WindLimitParameter.Text));
            }
            catch { }
        }

        public void SurfaceP1X_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][0].X = float.Parse(SurfaceP1X.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP1Y_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][0].Y = float.Parse(SurfaceP1Y.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP1Z_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][0].Z = float.Parse(SurfaceP1Z.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP2X_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][1].X = float.Parse(SurfaceP2X.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP2Y_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][1].Y = float.Parse(SurfaceP2Y.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP2Z_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][1].Z = float.Parse(SurfaceP2Z.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP3X_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][2].X = float.Parse(SurfaceP3X.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP3Y_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][2].Y = float.Parse(SurfaceP3Y.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void SurfaceP3Z_TextChanged(object sender, EventArgs e)
        {
            try
            {
                surface[CurrentSurface][2].Z = float.Parse(SurfaceP3Z.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        private void speedpaddle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                paddlespeed = float.Parse(speedpaddle.Text);
            }
            catch { }
        }

        private void sizepaddle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                paddlesize = float.Parse(sizepaddle.Text);
            }
            catch { }
            panel1.Invalidate();
        }

        public void ZoomBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (float.Parse((ZoomBox.Text)) > 0)
                {
                    convert = float.Parse((ZoomBox.Text));
                }
            }
            catch
            {
            }
            panel1.Invalidate();
        }

        public void PerspectiveZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                perspective.Z = float.Parse((PerspectiveZ.Text));

            }
            catch
            {
            }
            panel1.Invalidate();
        }

        public void PerspectiveY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                perspective.Y = float.Parse((PerspectiveY.Text));
            }
            catch
            {
            }
            panel1.Invalidate();
        }

        public void PerspectiveX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                perspective.X = float.Parse((PerspectiveX.Text));
            }
            catch
            {
            }
            panel1.Invalidate();
        }

        private void Views_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Choose the view from which you see the simulation here");
            }
            else
            { }
        }

        private void GameStart_Click(object sender, EventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Fire at the targets to get points");
            }
            else
            { }
        }

        private void Game2_Click(object sender, EventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Make sure the ball does not hit the wall at your end");
            }
            else
            { }
        }

        private void Cannon_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Change these variables here: cannon location, starting ball speed, ball mass, cannon force and cannon angle. You can also turn on vectors and trails here");
            }
            else
            { }
        }

        private void Perspective_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("This turns on perspective viewing");
            }
            else
            { }
        }

        private void Oblique_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("This turns on colours for the objects in the simulation");
            }
            else
            { }
        }

        private void gridcheck_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("This turns on a grid");
            }
            else
            { }
        }

        private void Properties_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Here you can change the perspective point, turn wind and gravity on, add surfaces and also change the basic parameters of the program");
            }
            else
            { }
        }

        private void Gap_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float g = float.Parse(Gap.Text);
                Phys = new Physics(g, speedzerolimit);
            }
            catch { }
            panel1.Invalidate();
        }

        private void SurfaceCheck_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("Here you can add surfaces. Each surface has 3 points, which you can change at the bottom left. You can switch between surfaces, remove them and add new ones with the dropdown list");
            }
            else
            { }
        }

        private void Room_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cursor.Handle.ToInt64() == helpcursor)
            {
                MessageBox.Show("This adds 9 surfaces, a box, which the ball can bounce around on the inside or the outside. You can enter two opposite corners of the box");
            }
            else
            { }
        }
    }
}