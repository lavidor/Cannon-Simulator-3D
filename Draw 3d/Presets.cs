using System;
using System.Collections.Generic;
using System.Text;

namespace Draw3D
{
    public class Presets
    {
        public Presets()
        {

        }
        public Point3D[] TargetPoints()
        {
            return TargetPoints(0, 0, 0);
        }
        public Point3D[] TargetPoints(Point3D p)
        {
            return TargetPoints(p.X, p.Y, p.Z);
        }
        public Point3D[] TargetPoints(float x, float y, float z)
        {
            float tempz = -0.25F;
            int a = 0, b = 0;
            Point3D[] Targets = new Point3D[32];
            for (a = 0; a < 17; a = a + 16)
            {
                tempz = tempz - a * tempz / 8;
                Targets[a] = new Point3D(0.25F, 0.25F, tempz);
                Targets[1 + a] = new Point3D(-1 * 0.25F, 0.25F, tempz);
                Targets[2 + a] = new Point3D(-1 * 0.25F, -1 * 0.25F, tempz);
                Targets[3 + a] = new Point3D(0.25F, -1 * 0.25F, tempz);
                Targets[4 + a] = new Point3D(0.2F, 0.25F, tempz);
                Targets[5 + a] = new Point3D(0.25F, 0.2F, tempz);
                Targets[6 + a] = new Point3D(0.25F, -1 * 0.2F, tempz);
                Targets[7 + a] = new Point3D(0.2F, -1 * 0.25F, tempz);
                Targets[8 + a] = new Point3D(-1 * 0.2F, -1 * 0.25F, tempz);
                Targets[9 + a] = new Point3D(-1 * 0.25F, -1 * 0.2F, tempz);
                Targets[10 + a] = new Point3D(-1 * 0.25F, 0.2F, tempz);
                Targets[11 + a] = new Point3D(-1 * 0.2F, 0.25F, tempz);
                Targets[12 + a] = new Point3D(0, 0.05F, tempz);
                Targets[13 + a] = new Point3D(0.05F, 0, tempz);
                Targets[14 + a] = new Point3D(0, -1 * 0.05F, tempz);
                Targets[15 + a] = new Point3D(-1 * 0.05F, 0, tempz);
                for (b = 0; b < 4; b++)
                    Array.Resize(ref Targets[b + a].lconnected, 3);
                for (b = 4; b < 12; b++)
                    Array.Resize(ref Targets[b + a].lconnected, 2);
                for (b = 12; b < 16; b++)
                    Array.Resize(ref Targets[b + a].lconnected, 3);
                for (b = 0; b < 16; b++)
                    Targets[b + a].lconnected[0] = 16 + b - a + 1;
                Targets[a].lconnected[1] = 2 + a;
                Targets[a].lconnected[2] = 4 + a;
                Targets[1 + a].lconnected[1] = 3 + a;
                Targets[1 + a].lconnected[2] = 1 + a;
                Targets[2 + a].lconnected[1] = 4 + a;
                Targets[2 + a].lconnected[2] = 2 + a;
                Targets[3 + a].lconnected[1] = 1 + a;
                Targets[3 + a].lconnected[2] = 3 + a;
                Targets[4 + a].lconnected[1] = 13 + a;
                Targets[5 + a].lconnected[1] = 14 + a;
                Targets[6 + a].lconnected[1] = 14 + a;
                Targets[7 + a].lconnected[1] = 15 + a;
                Targets[8 + a].lconnected[1] = 15 + a;
                Targets[9 + a].lconnected[1] = 16 + a;
                Targets[10 + a].lconnected[1] = 16 + a;
                Targets[11 + a].lconnected[1] = 13 + a;

                Targets[12 + a].lconnected[1] = 5 + a;
                Targets[12 + a].lconnected[2] = 12 + a;
                Targets[13 + a].lconnected[1] = 6 + a;
                Targets[13 + a].lconnected[2] = 7 + a;
                Targets[14 + a].lconnected[1] = 8 + a;
                Targets[14 + a].lconnected[2] = 9 + a;
                Targets[15 + a].lconnected[1] = 10 + a;
                Targets[15 + a].lconnected[2] = 11 + a;
            }
            Targets[0].bconnected = new int[4];
            Targets[0].bconnected[0] = 1;
            Targets[0].bconnected[1] = 2;
            Targets[0].bconnected[2] = 3;
            Targets[0].bconnected[3] = 4;

            Targets[1].bconnected = new int[4];
            Targets[1].bconnected[0] = 17;
            Targets[1].bconnected[1] = 18;
            Targets[1].bconnected[2] = 19;
            Targets[1].bconnected[3] = 20;

            Targets[2].bconnected = new int[4];
            Targets[2].bconnected[0] = 1;
            Targets[2].bconnected[1] = 2;
            Targets[2].bconnected[2] = 18;
            Targets[2].bconnected[3] = 17;

            Targets[3].bconnected = new int[4];
            Targets[3].bconnected[0] = 3;
            Targets[3].bconnected[1] = 4;
            Targets[3].bconnected[2] = 20;
            Targets[3].bconnected[3] = 19;

            Targets[4].bconnected = new int[4];
            Targets[4].bconnected[0] = 1;
            Targets[4].bconnected[1] = 4;
            Targets[4].bconnected[2] = 20;
            Targets[4].bconnected[3] = 17;

            Targets[5].bconnected = new int[4];
            Targets[5].bconnected[0] = 2;
            Targets[5].bconnected[1] = 3;
            Targets[5].bconnected[2] = 19;
            Targets[5].bconnected[3] = 18;

            Targets[6].bconnected = new int[16];
            Targets[6].bconnected[0] = 1;
            Targets[6].bconnected[1] = 6;
            Targets[6].bconnected[2] = 14;
            Targets[6].bconnected[3] = 7;
            Targets[6].bconnected[4] = 4;
            Targets[6].bconnected[5] = 8;
            Targets[6].bconnected[6] = 15;
            Targets[6].bconnected[7] = 9;
            Targets[6].bconnected[8] = 3;
            Targets[6].bconnected[9] = 10;
            Targets[6].bconnected[10] = 16;
            Targets[6].bconnected[11] = 11;
            Targets[6].bconnected[12] = 2;
            Targets[6].bconnected[13] = 12;
            Targets[6].bconnected[14] = 13;
            Targets[6].bconnected[15] = 5;

            Targets[7].bconnected = new int[16];
            Targets[7].bconnected[0] = 1 + 16;
            Targets[7].bconnected[1] = 6 + 16;
            Targets[7].bconnected[2] = 14 + 16;
            Targets[7].bconnected[3] = 7 + 16;
            Targets[7].bconnected[4] = 4 + 16;
            Targets[7].bconnected[5] = 8 + 16;
            Targets[7].bconnected[6] = 15 + 16;
            Targets[7].bconnected[7] = 9 + 16;
            Targets[7].bconnected[8] = 3 + 16;
            Targets[7].bconnected[9] = 10 + 16;
            Targets[7].bconnected[10] = 16 + 16;
            Targets[7].bconnected[11] = 11 + 16;
            Targets[7].bconnected[12] = 2 + 16;
            Targets[7].bconnected[13] = 12 + 16;
            Targets[7].bconnected[14] = 13 + 16;
            Targets[7].bconnected[15] = 5 + 16;

            Shape stemp = new Shape();
            stemp.RecievePoints(Targets);
            stemp.MoveShape(x, y, z);
            return stemp.Points;
        }
        public Point3D[] GridPoints(float gridunitsft, float targetdistancef)
        {
            float gridunitsf = targetdistancef / gridunitsft;
            int targetdistance = (int)(targetdistancef), gridunits = (int)(gridunitsf);
            int a = 0, n1 = 0;
            float x = 0, y = 0, z = 0, t = 0;
            n1 = (int)(2 * targetdistance / gridunitsft - 1);
            for (t = 0; t <= targetdistancef - gridunitsft; t = t + gridunitsft) ;
            Point3D[] grid = new Point3D[(int)(n1 * n1 * 6 + 12 * n1 + 8)];

            grid[0] = new Point3D(-t, -t, -t);
            grid[1] = new Point3D(t, -t, -t);
            grid[2] = new Point3D(t, t, -t);
            grid[3] = new Point3D(-t, t, -t);
            grid[4] = new Point3D(-t, -t, t);
            grid[5] = new Point3D(t, -t, t);
            grid[6] = new Point3D(t, t, t);
            grid[7] = new Point3D(-t, t, t);
            Array.Resize(ref grid[0].lconnected, 3);
            Array.Resize(ref grid[1].lconnected, 3);
            Array.Resize(ref grid[2].lconnected, 3);
            Array.Resize(ref grid[3].lconnected, 3);
            Array.Resize(ref grid[4].lconnected, 3);
            Array.Resize(ref grid[5].lconnected, 3);
            Array.Resize(ref grid[6].lconnected, 3);
            Array.Resize(ref grid[7].lconnected, 3);
            grid[0].lconnected[0] = 2;
            grid[0].lconnected[1] = 4;
            grid[0].lconnected[2] = 5;
            grid[1].lconnected[0] = 3;
            grid[1].lconnected[1] = 1;
            grid[1].lconnected[2] = 6;
            grid[2].lconnected[0] = 4;
            grid[2].lconnected[1] = 2;
            grid[2].lconnected[2] = 7;
            grid[3].lconnected[0] = 1;
            grid[3].lconnected[1] = 3;
            grid[3].lconnected[2] = 8;
            grid[4].lconnected[0] = 6;
            grid[4].lconnected[1] = 8;
            grid[4].lconnected[2] = 1;
            grid[5].lconnected[0] = 7;
            grid[5].lconnected[1] = 5;
            grid[5].lconnected[2] = 2;
            grid[6].lconnected[0] = 8;
            grid[6].lconnected[1] = 6;
            grid[6].lconnected[2] = 3;
            grid[7].lconnected[0] = 5;
            grid[7].lconnected[1] = 7;
            grid[7].lconnected[2] = 4;

            a = 8;

            for (z = -t; z <= t; z = z + t * 2)
            {
                for (x = -t; x <= t; x = x + t * 2)
                {
                    for (y = gridunitsft - t; y + gridunitsft <= t; y = y + gridunitsft)
                    {
                        grid[a] = new Point3D(x, y, z);
                        Array.Resize(ref grid[a].lconnected, 2);
                        if (x < 0)
                        {
                            grid[a].lconnected[0] = (int)(a + n1 + 1);
                        }
                        else
                        {
                            grid[a].lconnected[0] = (int)(a - n1 + 1);
                        }
                        if (z < 0)
                        {
                            grid[a].lconnected[1] = (int)(a + 2 * n1 + 1);
                        }
                        else
                        {
                            grid[a].lconnected[1] = (int)(a - 2 * n1 + 1);
                        }
                        a++;
                    }
                }
            }
            for (z = -t; z <= t; z = z + t * 2)
            {
                for (y = -t; y <= t; y = y + t * 2)
                {
                    for (x = gridunitsft - t; x + gridunitsft <= t; x = x + gridunitsft)
                    {
                        grid[a] = new Point3D(x, y, z);
                        Array.Resize(ref grid[a].lconnected, 2);
                        if (y < 0)
                        {
                            grid[a].lconnected[0] = (int)(a + n1 + 1);
                        }
                        else
                        {
                            grid[a].lconnected[0] = (int)(a - n1 + 1);
                        }
                        if (z < 0)
                        {
                            grid[a].lconnected[1] = (int)(a + 2 * n1 + 1);
                        }
                        else
                        {
                            grid[a].lconnected[1] = (int)(a - 2 * n1 + 1);
                        }
                        a++;
                    }
                }
            }
            for (y = -t; y <= t; y = y + t * 2)
            {
                for (x = -t; x <= t; x = x + t * 2)
                {
                    for (z = gridunitsft - t; z + gridunitsft <= t; z = z + gridunitsft)
                    {
                        grid[a] = new Point3D(x, y, z);
                        Array.Resize(ref grid[a].lconnected, 2);
                        if (x < 0)
                        {
                            grid[a].lconnected[0] = (int)(a + n1 + 1);
                        }
                        else
                        {
                            grid[a].lconnected[0] = (int)(a - n1 + 1);
                        }
                        if (y < 0)
                        {
                            grid[a].lconnected[1] = (int)(a + 2 * n1 + 1);
                        }
                        else
                        {
                            grid[a].lconnected[1] = (int)(a - 2 * n1 + 1);
                        }
                        a++;
                    }
                }
            }

            for (x = -t; x <= t; x = x + t * 2)
            {
                for (z = gridunitsft - t; z + gridunitsft <= t; z = z + gridunitsft)
                {
                    for (y = gridunitsft - t; y + gridunitsft <= t; y = y + gridunitsft)
                    {
                        grid[a] = new Point3D(x, y, z);
                        Array.Resize(ref grid[a].lconnected, 1);
                        if (x < 0)
                        {
                            grid[a].lconnected[0] = (int)(a + (n1) * (n1) + 1);
                        }
                        else
                        {
                            grid[a].lconnected[0] = (int)(a - (n1) * (n1) + 1);
                        }
                        a++;
                    }
                }
            }
            for (z = -t; z <= t; z = z + t * 2)
            {
                for (y = gridunitsft - t; y + gridunitsft <= t; y = y + gridunitsft)
                {
                    for (x = gridunitsft - t; x + gridunitsft <= t; x = x + gridunitsft)
                    {
                        grid[a] = new Point3D(x, y, z);
                        Array.Resize(ref grid[a].lconnected, 1);
                        if (z < 0)
                        {
                            grid[a].lconnected[0] = (int)(a + (n1) * (n1) + 1);
                        }
                        else
                        {
                            grid[a].lconnected[0] = (int)(a - (n1) * (n1) + 1);
                        }
                        a++;
                    }
                }
            }
            for (y = -t; y <= t; y = y + t * 2)
            {
                for (x = gridunitsft - t; x + gridunitsft <= t; x = x + gridunitsft)
                {
                    for (z = gridunitsft - t; z + gridunitsft <= t; z = z + gridunitsft)
                    {
                        grid[a] = new Point3D(x, y, z);
                        Array.Resize(ref grid[a].lconnected, 1);
                        if (y < 0)
                        {
                            grid[a].lconnected[0] = (int)(a + (n1) * (n1) + 1);
                        }
                        else
                        {
                            grid[a].lconnected[0] = (int)(a - (n1) * (n1) + 1);
                        }
                        a++;
                    }
                }
            }

            return grid;
        }
        public Point3D[] SpherePoints()
        {
            int a = 0;
            Point3D[] sphere = new Point3D[42];
            sphere[0] = new Point3D(0, 1, 0);

            sphere[1] = new Point3D(0, (float)(2F / 3F), (float)(Math.Sqrt(5) / 3));
            sphere[2] = new Point3D((float)(Math.Sqrt(5) / 3), (float)(2F / 3F), 0);
            sphere[3] = new Point3D(0, (float)(2F / 3F), (float)(-Math.Sqrt(5) / 3));
            sphere[4] = new Point3D((float)(-Math.Sqrt(5) / 3), (float)(2F / 3F), 0);

            sphere[5] = new Point3D(0, 1F / 3F, (float)(Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))));
            sphere[6] = new Point3D(2F / 3F, 1F / 3F, 2F / 3F);
            sphere[7] = new Point3D((float)(Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))), 1F / 3F, 0);
            sphere[8] = new Point3D(2F / 3F, 1F / 3F, -2F / 3F);
            sphere[9] = new Point3D(0, 1F / 3F, (float)(-Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))));
            sphere[10] = new Point3D(-2F / 3F, 1F / 3F, -2F / 3F);
            sphere[11] = new Point3D((float)(-Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))), 1F / 3F, 0);
            sphere[12] = new Point3D(-2F / 3F, 1F / 3F, 2F / 3F);

            sphere[13] = new Point3D(0, 0, 1);
            sphere[14] = new Point3D((float)(Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))), 0, (float)(Math.Sqrt(Math.Sqrt(0.5))));
            sphere[15] = new Point3D((float)(1 / Math.Sqrt(2)), 0, (float)(1 / Math.Sqrt(2)));
            sphere[16] = new Point3D((float)(Math.Sqrt(Math.Sqrt(0.5))), 0, (float)(Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))));
            sphere[17] = new Point3D(1, 0, 0);
            sphere[18] = new Point3D((float)(Math.Sqrt(Math.Sqrt(0.5))), 0, (float)(-Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))));
            sphere[19] = new Point3D((float)(1 / Math.Sqrt(2)), 0, (float)(-1 / Math.Sqrt(2)));
            sphere[20] = new Point3D((float)(Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))), 0, (float)(-Math.Sqrt(Math.Sqrt(0.5))));
            sphere[21] = new Point3D(0, 0, -1);
            sphere[22] = new Point3D((float)(-Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))), 0, (float)(-Math.Sqrt(Math.Sqrt(0.5))));
            sphere[23] = new Point3D((float)(-1 / Math.Sqrt(2)), 0, (float)(-1 / Math.Sqrt(2)));
            sphere[24] = new Point3D((float)(-Math.Sqrt(Math.Sqrt(0.5))), 0, (float)(-Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))));
            sphere[25] = new Point3D(-1, 0, 0);
            sphere[26] = new Point3D((float)(-Math.Sqrt(Math.Sqrt(0.5))), 0, (float)(Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))));
            sphere[27] = new Point3D((float)(-1 / Math.Sqrt(2)), 0, (float)(1 / Math.Sqrt(2)));
            sphere[28] = new Point3D((float)(-Math.Sqrt((Math.Sqrt(2) - 1) / Math.Sqrt(2))), 0, (float)(Math.Sqrt(Math.Sqrt(0.5))));

            sphere[29] = new Point3D(0, -1F / 3F, (float)(Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))));//
            sphere[30] = new Point3D(2F / 3F, -1F / 3F, 2F / 3F);
            sphere[31] = new Point3D((float)(Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))), -1F / 3F, 0);
            sphere[32] = new Point3D(2F / 3F, -1F / 3F, -2F / 3F);
            sphere[33] = new Point3D(0, -1F / 3F, (float)(-Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))));
            sphere[34] = new Point3D(-2F / 3F, -1F / 3F, -2F / 3F);
            sphere[35] = new Point3D((float)(-Math.Sqrt((double)(1 - (1F / 3F) * (1F / 3F)))), -1F / 3F, 0);
            sphere[36] = new Point3D(-2F / 3F, -1F / 3F, 2F / 3F);

            sphere[37] = new Point3D(0, -2F / 3F, (float)(Math.Sqrt((double)(1 - (2F / 3F) * (2F / 3F)))));
            sphere[38] = new Point3D((float)(Math.Sqrt((double)(1 - (2F / 3F) * (2F / 3F)))), -2F / 3F, 0);
            sphere[39] = new Point3D(0, -2F / 3F, (float)(-Math.Sqrt((double)(1 - (2F / 3F) * (2F / 3F)))));
            sphere[40] = new Point3D((float)(-Math.Sqrt((double)(1 - (2F / 3F) * (2F / 3F)))), -2F / 3F, 0);

            sphere[41] = new Point3D(0, -1, 0);

            Array.Resize(ref sphere[0].lconnected, 4);
            Array.Resize(ref sphere[41].lconnected, 4);

            Array.Resize(ref sphere[1].lconnected, 4);
            Array.Resize(ref sphere[2].lconnected, 4);
            Array.Resize(ref sphere[3].lconnected, 4);
            Array.Resize(ref sphere[4].lconnected, 4);
            Array.Resize(ref sphere[37].lconnected, 4);
            Array.Resize(ref sphere[38].lconnected, 4);
            Array.Resize(ref sphere[39].lconnected, 4);
            Array.Resize(ref sphere[40].lconnected, 4);

            Array.Resize(ref sphere[5].lconnected, 4);
            Array.Resize(ref sphere[6].lconnected, 3);
            Array.Resize(ref sphere[7].lconnected, 4);
            Array.Resize(ref sphere[8].lconnected, 3);
            Array.Resize(ref sphere[9].lconnected, 4);
            Array.Resize(ref sphere[10].lconnected, 3);
            Array.Resize(ref sphere[11].lconnected, 4);
            Array.Resize(ref sphere[12].lconnected, 3);
            Array.Resize(ref sphere[29].lconnected, 4);
            Array.Resize(ref sphere[30].lconnected, 3);
            Array.Resize(ref sphere[31].lconnected, 4);
            Array.Resize(ref sphere[32].lconnected, 3);
            Array.Resize(ref sphere[33].lconnected, 4);
            Array.Resize(ref sphere[34].lconnected, 3);
            Array.Resize(ref sphere[35].lconnected, 4);
            Array.Resize(ref sphere[36].lconnected, 3);

            Array.Resize(ref sphere[13].lconnected, 4);
            Array.Resize(ref sphere[14].lconnected, 2);
            Array.Resize(ref sphere[15].lconnected, 4);
            Array.Resize(ref sphere[16].lconnected, 2);
            Array.Resize(ref sphere[17].lconnected, 4);
            Array.Resize(ref sphere[18].lconnected, 2);
            Array.Resize(ref sphere[19].lconnected, 4);
            Array.Resize(ref sphere[20].lconnected, 2);
            Array.Resize(ref sphere[21].lconnected, 4);
            Array.Resize(ref sphere[22].lconnected, 2);
            Array.Resize(ref sphere[23].lconnected, 4);
            Array.Resize(ref sphere[24].lconnected, 2);
            Array.Resize(ref sphere[25].lconnected, 4);
            Array.Resize(ref sphere[26].lconnected, 2);
            Array.Resize(ref sphere[27].lconnected, 4);
            Array.Resize(ref sphere[28].lconnected, 2);

            sphere[0].lconnected[0] = 2;
            sphere[0].lconnected[1] = 3;
            sphere[0].lconnected[2] = 4;
            sphere[0].lconnected[3] = 5;
            sphere[41].lconnected[0] = 38;
            sphere[41].lconnected[1] = 39;
            sphere[41].lconnected[2] = 40;
            sphere[41].lconnected[3] = 41;

            sphere[1].lconnected[0] = 1;
            sphere[1].lconnected[1] = 3;
            sphere[1].lconnected[2] = 5;
            sphere[1].lconnected[3] = 6;
            sphere[2].lconnected[0] = 1;
            sphere[2].lconnected[1] = 4;
            sphere[2].lconnected[2] = 2;
            sphere[2].lconnected[3] = 8;
            sphere[3].lconnected[0] = 1;
            sphere[3].lconnected[1] = 5;
            sphere[3].lconnected[2] = 3;
            sphere[3].lconnected[3] = 10;
            sphere[4].lconnected[0] = 1;
            sphere[4].lconnected[1] = 2;
            sphere[4].lconnected[2] = 4;
            sphere[4].lconnected[3] = 12;
            sphere[37].lconnected[0] = 42;
            sphere[37].lconnected[1] = 39;
            sphere[37].lconnected[2] = 41;
            sphere[37].lconnected[3] = 30;
            sphere[38].lconnected[0] = 42;
            sphere[38].lconnected[1] = 40;
            sphere[38].lconnected[2] = 38;
            sphere[38].lconnected[3] = 32;
            sphere[39].lconnected[0] = 42;
            sphere[39].lconnected[1] = 41;
            sphere[39].lconnected[2] = 39;
            sphere[39].lconnected[3] = 33;
            sphere[40].lconnected[0] = 42;
            sphere[40].lconnected[1] = 38;
            sphere[40].lconnected[2] = 40;
            sphere[40].lconnected[3] = 36;

            sphere[5].lconnected[0] = 2;
            sphere[5].lconnected[1] = 7;
            sphere[5].lconnected[2] = 13;
            sphere[5].lconnected[3] = 14;
            sphere[7].lconnected[0] = 3;
            sphere[7].lconnected[1] = 9;
            sphere[7].lconnected[2] = 7;
            sphere[7].lconnected[3] = 18;
            sphere[9].lconnected[0] = 4;
            sphere[9].lconnected[1] = 11;
            sphere[9].lconnected[2] = 9;
            sphere[9].lconnected[3] = 22;
            sphere[11].lconnected[0] = 5;
            sphere[11].lconnected[1] = 13;
            sphere[11].lconnected[2] = 11;
            sphere[11].lconnected[3] = 26;
            sphere[29].lconnected[0] = 38;//38
            sphere[29].lconnected[1] = 31;//31
            sphere[29].lconnected[2] = 37;//37
            sphere[29].lconnected[3] = 14;//14
            sphere[31].lconnected[0] = 39;
            sphere[31].lconnected[1] = 33;
            sphere[31].lconnected[2] = 31;
            sphere[31].lconnected[3] = 18;
            sphere[33].lconnected[0] = 40;
            sphere[33].lconnected[1] = 35;
            sphere[33].lconnected[2] = 33;
            sphere[33].lconnected[3] = 22;
            sphere[35].lconnected[0] = 41;
            sphere[35].lconnected[1] = 37;
            sphere[35].lconnected[2] = 35;
            sphere[35].lconnected[3] = 26;

            sphere[6].lconnected[0] = 8;
            sphere[6].lconnected[1] = 6;
            sphere[6].lconnected[2] = 16;
            sphere[8].lconnected[0] = 10;
            sphere[8].lconnected[1] = 8;
            sphere[8].lconnected[2] = 20;
            sphere[10].lconnected[0] = 12;
            sphere[10].lconnected[1] = 10;
            sphere[10].lconnected[2] = 24;
            sphere[12].lconnected[0] = 6;
            sphere[12].lconnected[1] = 12;
            sphere[12].lconnected[2] = 28;
            sphere[30].lconnected[0] = 32;
            sphere[30].lconnected[1] = 30;
            sphere[30].lconnected[2] = 16;
            sphere[32].lconnected[0] = 34;
            sphere[32].lconnected[1] = 32;
            sphere[32].lconnected[2] = 20;
            sphere[34].lconnected[0] = 36;
            sphere[34].lconnected[1] = 34;
            sphere[34].lconnected[2] = 24;
            sphere[36].lconnected[0] = 37;//?38
            sphere[36].lconnected[1] = 36;
            sphere[36].lconnected[2] = 28;

            for (a = 14; a < 29; a = a + 2)
            {
                sphere[a].lconnected[1] = a;
                sphere[a].lconnected[0] = a + 2;
            }
            sphere[13].lconnected[2] = 29;
            sphere[13].lconnected[1] = 15;
            sphere[27].lconnected[2] = 27;
            sphere[27].lconnected[1] = 14;
            sphere[28].lconnected[0] = 14;
            for (a = 15; a < 28; a = a + 2)
            {
                sphere[a].lconnected[2] = a;
                sphere[a].lconnected[1] = a + 2;
            }
            sphere[13].lconnected[0] = 6;
            sphere[13].lconnected[3] = 30;
            sphere[15].lconnected[0] = 7;
            sphere[15].lconnected[3] = 31;
            sphere[17].lconnected[0] = 8;
            sphere[17].lconnected[3] = 32;
            sphere[19].lconnected[0] = 9;
            sphere[19].lconnected[3] = 33;
            sphere[21].lconnected[0] = 10;
            sphere[21].lconnected[3] = 34;
            sphere[23].lconnected[0] = 11;
            sphere[23].lconnected[3] = 35;
            sphere[25].lconnected[0] = 12;
            sphere[25].lconnected[3] = 36;
            sphere[27].lconnected[0] = 13;
            sphere[27].lconnected[3] = 37;

            return sphere;
        }
        public Point3D[] SpherePoints2(float r, int resolution)
        {
            int a = 0, b = 0, c = 0;
            float t = 0;
            Point3D[] sphere = new Point3D[(int)(4 * resolution * (resolution - 1) + 2)];
            sphere[0] = new Point3D(0, 1 * r, 0);
            sphere[sphere.Length - 1] = new Point3D(0, -1 * r, 0);

            for (a = 1; a < sphere.Length - 1; a = a + 2 * resolution - 2)
            {
                t = 0;
                for (b = 0; b < resolution - 1; b++)
                {
                    t = (float)(t + Math.PI / resolution);
                    sphere[a + b] = new Point3D((float)(r * Math.Sin((double)t)), (float)(r * Math.Cos((double)t)), 0);
                    sphere[a + resolution - 1 + b] = new Point3D((float)(-1 * r * Math.Sin((double)t)), (float)(r * Math.Cos((double)t)), 0);

                    sphere[a + b].lconnected = new int[2];
                    sphere[a + resolution - 1 + b].lconnected = new int[2];
                    {
                        if (b == 0)
                        {
                            sphere[a + b].lconnected[0] = 1;
                            sphere[a + b].lconnected[1] = a + b + 2;

                            sphere[a + b + resolution - 1].lconnected[0] = 1;
                            sphere[a + b + resolution - 1].lconnected[1] = a + b + resolution + 1;

                            Array.Resize(ref sphere[0].lconnected, sphere[0].lconnected.Length + 2);
                            sphere[0].lconnected[sphere[0].lconnected.Length - 2] = a + b + 1;
                            sphere[0].lconnected[sphere[0].lconnected.Length - 1] = a + b + resolution;
                        }
                        else
                        {
                            if (b == resolution - 2)
                            {
                                sphere[a + b].lconnected[0] = a + b;
                                sphere[a + b].lconnected[1] = sphere.Length;

                                sphere[a + b + resolution - 1].lconnected[0] = a + b + resolution - 1;
                                sphere[a + b + resolution - 1].lconnected[1] = sphere.Length;

                                Array.Resize(ref sphere[sphere.Length - 1].lconnected, sphere[sphere.Length - 1].lconnected.Length + 2);
                                sphere[sphere.Length - 1].lconnected[sphere[sphere.Length - 1].lconnected.Length - 2] = a + b + 1;
                                sphere[sphere.Length - 1].lconnected[sphere[sphere.Length - 1].lconnected.Length - 1] = a + b + resolution;
                            }
                            else
                            {
                                sphere[a + b].lconnected[0] = a + b;
                                sphere[a + b].lconnected[1] = a + b + 3;

                                sphere[a + b + resolution - 1].lconnected[0] = a + b + resolution - 1;
                                sphere[a + b + resolution - 1].lconnected[1] = a + b + resolution + 1;
                            }
                        }
                    }
                }
                for (c = 1; c < (a + b + resolution - 2); c++)
                {
                    sphere[c].RotatePointReal((float)(Math.PI / resolution), 0, 0, 0, 0, 1, 0);
                }
            }

            return sphere;
        }
        public Point3D[] CannonBasePoints()
        {
            Point3D[] cannon = new Point3D[18];
            for (float x = 0; x < 18; x++)
            {
                cannon[(int)(x)] = new Point3D();
                Array.Resize(ref cannon[(int)(x)].lconnected, 3);
            }
            Array.Resize(ref cannon[2].lconnected, 4);
            Array.Resize(ref cannon[6].lconnected, 4);
            Array.Resize(ref cannon[11].lconnected, 4);
            Array.Resize(ref cannon[15].lconnected, 4);

            cannon[0].lconnected[0] = 2;
            cannon[0].lconnected[1] = 6;
            cannon[0].lconnected[2] = 10;
            cannon[1].lconnected[0] = 3;
            cannon[1].lconnected[1] = 1;
            cannon[1].lconnected[2] = 11;
            cannon[2].lconnected[0] = 4;
            cannon[2].lconnected[1] = 2;
            cannon[2].lconnected[2] = 7;
            cannon[2].lconnected[3] = 12;
            cannon[3].lconnected[0] = 5;
            cannon[3].lconnected[1] = 3;
            cannon[3].lconnected[2] = 13;
            cannon[4].lconnected[0] = 9;
            cannon[4].lconnected[1] = 4;
            cannon[4].lconnected[2] = 14;
            cannon[5].lconnected[0] = 1;
            cannon[5].lconnected[1] = 7;
            cannon[5].lconnected[2] = 15;
            cannon[6].lconnected[0] = 6;
            cannon[6].lconnected[1] = 8;
            cannon[6].lconnected[2] = 3;
            cannon[6].lconnected[3] = 16;
            cannon[7].lconnected[0] = 7;
            cannon[7].lconnected[1] = 9;
            cannon[7].lconnected[2] = 17;
            cannon[8].lconnected[0] = 8;
            cannon[8].lconnected[1] = 5;
            cannon[8].lconnected[2] = 18;

            cannon[9].lconnected[0] = 1;
            cannon[9].lconnected[1] = 15;
            cannon[9].lconnected[2] = 11;
            cannon[10].lconnected[0] = 2;
            cannon[10].lconnected[1] = 10;
            cannon[10].lconnected[2] = 12;
            cannon[11].lconnected[0] = 3;
            cannon[11].lconnected[1] = 16;
            cannon[11].lconnected[2] = 11;
            cannon[11].lconnected[3] = 13;
            cannon[12].lconnected[0] = 4;
            cannon[12].lconnected[1] = 12;
            cannon[12].lconnected[2] = 14;
            cannon[13].lconnected[0] = 5;
            cannon[13].lconnected[1] = 13;
            cannon[13].lconnected[2] = 18;
            cannon[14].lconnected[0] = 6;
            cannon[14].lconnected[1] = 16;
            cannon[14].lconnected[2] = 10;
            cannon[15].lconnected[0] = 7;
            cannon[15].lconnected[1] = 12;
            cannon[15].lconnected[2] = 15;
            cannon[15].lconnected[3] = 17;
            cannon[16].lconnected[0] = 8;
            cannon[16].lconnected[1] = 16;
            cannon[16].lconnected[2] = 18;
            cannon[17].lconnected[0] = 9;
            cannon[17].lconnected[1] = 15;
            cannon[17].lconnected[2] = 17;

            cannon[0].X = 0;
            cannon[0].Y = -2;
            cannon[0].Z = -1.25F;
            cannon[1].X = 0.75F;
            cannon[1].Y = -1.75F;
            cannon[1].Z = -1.25F;
            cannon[2].X = 1;
            cannon[2].Y = -1;
            cannon[2].Z = -1.25F;
            cannon[3].X = 1.5F;
            cannon[3].Y = -1;
            cannon[3].Z = -1.25F;
            cannon[4].X = 0.5F;
            cannon[4].Y = 0.5F;
            cannon[4].Z = -1.25F;
            cannon[5].X = -0.75F;
            cannon[5].Y = -1.75F;
            cannon[5].Z = -1.25F;
            cannon[6].X = -1;
            cannon[6].Y = -1;
            cannon[6].Z = -1.25F;
            cannon[7].X = -1.5F;
            cannon[7].Y = -1;
            cannon[7].Z = -1.25F;
            cannon[8].X = -0.5F;
            cannon[8].Y = 0.5F;
            cannon[8].Z = -1.25F;

            cannon[9].X = 0;
            cannon[9].Y = -2;
            cannon[9].Z = -1;
            cannon[10].X = 0.75F;
            cannon[10].Y = -1.75F;
            cannon[10].Z = -1;
            cannon[11].X = 1;
            cannon[11].Y = -1;
            cannon[11].Z = -1;
            cannon[12].X = 1.5F;
            cannon[12].Y = -1;
            cannon[12].Z = -1;
            cannon[13].X = 0.5F;
            cannon[13].Y = 0.5F;
            cannon[13].Z = -1;
            cannon[14].X = -0.75F;
            cannon[14].Y = -1.75F;
            cannon[14].Z = -1;
            cannon[15].X = -1;
            cannon[15].Y = -1;
            cannon[15].Z = -1;
            cannon[16].X = -1.5F;
            cannon[16].Y = -1;
            cannon[16].Z = -1;
            cannon[17].X = -0.5F;
            cannon[17].Y = 0.5F;
            cannon[17].Z = -1;

            cannon[0].bconnected = new int[5] { 1, 2, 3, 7, 6 };
            cannon[1].bconnected = new int[5] { 10, 11, 12, 16, 15 };
            cannon[2].bconnected = new int[4] { 4, 5, 9, 8 };
            cannon[3].bconnected = new int[4] { 13, 14, 18, 17 };
            cannon[4].bconnected = new int[4] { 1, 2, 11, 10 };
            cannon[5].bconnected = new int[4] { 2, 3, 12, 11 };
            cannon[6].bconnected = new int[4] { 3, 4, 13, 12 };
            cannon[7].bconnected = new int[4] { 4, 5, 14, 13 };
            cannon[8].bconnected = new int[4] { 5, 9, 18, 14 };
            cannon[9].bconnected = new int[4] { 9, 8, 17, 18 };
            cannon[10].bconnected = new int[4] { 8, 7, 16, 17 };
            cannon[11].bconnected = new int[4] { 7, 6, 15, 16 };
            cannon[12].bconnected = new int[4] { 6, 1, 10, 15 };

            return cannon;
        }
        public Point3D[] CannonPoints()
        {
            Shape temp = new Shape();
            int num = 16;
            Point3D[] p = new Point3D[num * 2];
            float radius = 1;
            float height = 3;
            float originx = 0;
            float originy = -1;
            float originz = 0;
            int x;

            for (x = 0; x < p.Length; x++)
                p[x] = new Point3D();

            p[0].X = radius + originx;
            p[0].Y = originy;
            p[0].Z = originz;
            Array.Resize(ref p[0].lconnected, 4);
            p[0].lconnected[0] = num;
            p[0].lconnected[1] = 2;
            p[0].lconnected[2] = num + 1;
            p[0].lconnected[3] = num / 2 + 1;

            p[num].X = radius + originx;
            p[num].Y = originy + height;
            p[num].Z = originz;
            Array.Resize(ref p[num].lconnected, 3);
            p[num].lconnected[0] = num + 2;
            p[num].lconnected[1] = 1;
            p[num].lconnected[2] = num * 2;

            p[num - 1].X = (float)(radius * Math.Cos((double)((360 / num) * (num - 1) * Math.PI / 180)) + originx);
            p[num - 1].Y = originy;
            p[num - 1].Z = (float)(radius * Math.Sin((double)((360 / num) * (num - 1) * Math.PI / 180)) + originz);
            Array.Resize(ref p[num - 1].lconnected, 4);
            p[num - 1].lconnected[0] = 1;
            p[num - 1].lconnected[1] = num - 1;
            p[num - 1].lconnected[2] = num * 2;
            p[num - 1].lconnected[3] = num / 2;


            p[num * 2 - 1].X = (float)(radius * Math.Cos((double)((360 / num) * (num - 1) * Math.PI / 180)) + originx);
            p[num * 2 - 1].Y = originy + height;
            p[num * 2 - 1].Z = (float)(radius * Math.Sin((double)((360 / num) * (num - 1) * Math.PI / 180)) + originz);
            Array.Resize(ref p[num * 2 - 1].lconnected, 3);
            p[num * 2 - 1].lconnected[0] = num;
            p[num * 2 - 1].lconnected[1] = num + 1;
            p[num * 2 - 1].lconnected[2] = num * 2 - 1;
            for (x = 1; x < p.Length / 2 - 1; x++)
            {
                p[x].Y = originy;
                p[x].X = (float)(radius * Math.Cos((double)((360 / num) * x * Math.PI / 180)) + originx);
                p[x].Z = (float)(radius * Math.Sin((double)((360 / num) * x * Math.PI / 180)) + originz);
                Array.Resize(ref p[x].lconnected, 4);
                p[x].lconnected[0] = x;
                p[x].lconnected[1] = x + 2;
                p[x].lconnected[2] = x + num + 1;
                if (x + num / 2 < num)
                    p[x].lconnected[3] = x + num / 2 + 1;
                else
                    p[x].lconnected[3] = x - num / 2 + 1;


                p[x + num].Y = originy + height;
                p[x + num].X = (float)(radius * Math.Cos((double)((360 / num) * x * Math.PI / 180)) + originx);
                p[x + num].Z = (float)(radius * Math.Sin((double)((360 / num) * x * Math.PI / 180)) + originz);
                Array.Resize(ref p[x + num].lconnected, 3);
                p[x + num].lconnected[0] = x + num;
                p[x + num].lconnected[1] = x + num + 2;
                p[x + num].lconnected[2] = x + 1;
            }

            for (x = 1; x < num; x++)
            {
                p[x].bconnected = new int[4] { x, x + 1, x + num + 1, x + num };
            }
            p[x].bconnected = new int[4] { x, 1, num + 1, x + num };
            p[0].bconnected = new int[num];
            for (x = 0; x < num; x++)
            {
                p[0].bconnected[x] = x + 1;
            }
            p[num + 1].bconnected = new int[num];
            p[num + 1].Q = 10;
            for (x = 0; x < num; x++)
            {
                p[num + 1].bconnected[x] = x + num + 1;
            }

            temp.Points = p;
            temp.RotateShapebyline(90, 0, 0, 0, 0, 0, 1);
            p = temp.Points;
            return p;
        }
        public Shape MakeConditions(int type)
        {
            Shape s = new Shape();
            if (type == 0)
            {
                s.Points = s.preset.MakeHouse();
                s.RotateShapebyline((float)(0.5), 4, 2, 0, 2, 1, 1);

                //s.SortPoints((float)(Math.PI - 1), 1);
            }
            else
            {
                if (type == 1)
                {
                    s.Points = s.preset.MakeHouse();
                    s.RotateShapebyline((float)(Math.PI / 2), 0, 0, 0, 0, 0, 1);
                }
                else
                {
                    if (type == 2)
                    {
                        s.Points = s.preset.MakeCylindar();
                        s.MoveShapetopoint(-5, 0, 0);
                    }
                }
            }
            return s;
        }
        public Point3D[] MakeCylindar()
        {
            int num = 36;
            Point3D[] p = new Point3D[num * 2];
            float radius = 1;
            float height = 3;
            float originx = 0;
            float originy = -2;
            float originz = 0;
            int x;

            for (x = 0; x < p.Length; x++)
                p[x] = new Point3D();

            p[0].X = radius + originx;
            p[0].Y = originy;
            p[0].Z = originz;
            Array.Resize(ref p[0].lconnected, 3);
            p[0].lconnected[0] = num;
            p[0].lconnected[1] = 2;
            p[0].lconnected[2] = num + 1;
            Array.Resize(ref p[0].bconnected, 3);
            p[0].bconnected[0] = num;
            p[0].bconnected[1] = 2;
            p[0].bconnected[2] = num + 1;

            p[num].X = radius + originx;
            p[num].Y = originy + height;
            p[num].Z = originz;
            Array.Resize(ref p[num].lconnected, 3);
            p[num].lconnected[0] = num + 2;
            p[num].lconnected[1] = 1;
            p[num].lconnected[2] = num * 2;
            Array.Resize(ref p[num].bconnected, 3);
            p[num].bconnected[0] = num + 2;
            p[num].bconnected[1] = 1;
            p[num].bconnected[2] = num * 2;

            p[num - 1].X = (float)(radius * Math.Cos((double)((360 / num) * (num - 1) * Math.PI / 180)) + originx);
            p[num - 1].Y = originy;
            p[num - 1].Z = (float)(radius * Math.Sin((double)((360 / num) * (num - 1) * Math.PI / 180)) + originz);
            Array.Resize(ref p[num - 1].lconnected, 3);
            p[num - 1].lconnected[0] = 1;
            p[num - 1].lconnected[1] = num - 1;
            p[num - 1].lconnected[2] = num * 2;
            Array.Resize(ref p[num - 1].bconnected, 3);
            p[num - 1].bconnected[0] = 1;
            p[num - 1].bconnected[1] = num - 1;
            p[num - 1].bconnected[2] = num * 2;


            p[num * 2 - 1].X = (float)(radius * Math.Cos((double)((360 / num) * (num - 1) * Math.PI / 180)) + originx);
            p[num * 2 - 1].Y = originy + height;
            p[num * 2 - 1].Z = (float)(radius * Math.Sin((double)((360 / num) * (num - 1) * Math.PI / 180)) + originz);
            Array.Resize(ref p[num * 2 - 1].lconnected, 3);
            p[num * 2 - 1].lconnected[0] = num;
            p[num * 2 - 1].lconnected[1] = num + 1;
            p[num * 2 - 1].lconnected[2] = num * 2 - 1;
            for (x = 1; x < p.Length / 2 - 1; x++)
            {
                p[x].Y = originy;
                p[x].X = (float)(radius * Math.Cos((double)((360 / num) * x * Math.PI / 180)) + originx);
                p[x].Z = (float)(radius * Math.Sin((double)((360 / num) * x * Math.PI / 180)) + originz);
                Array.Resize(ref p[x].lconnected, 3);
                p[x].lconnected[0] = x;
                p[x].lconnected[1] = x + 2;
                p[x].lconnected[2] = x + num + 1;
                Array.Resize(ref p[x].bconnected, 4);
                p[x].bconnected[0] = x;
                p[x].bconnected[1] = x + 2;
                p[x].bconnected[2] = x + num + 1;
                p[x].bconnected[3] = 1;


                p[x + num].Y = originy + height;
                p[x + num].X = (float)(radius * Math.Cos((double)((360 / num) * x * Math.PI / 180)) + originx);
                p[x + num].Z = (float)(radius * Math.Sin((double)((360 / num) * x * Math.PI / 180)) + originz);
                Array.Resize(ref p[x + num].lconnected, 3);
                p[x + num].lconnected[0] = x + num;
                p[x + num].lconnected[1] = x + num + 2;
                p[x + num].lconnected[2] = x + 1;
                Array.Resize(ref p[x + num].bconnected, 4);
                p[x + num].bconnected[0] = x + num;
                p[x + num].bconnected[1] = x + num + 2;
                p[x + num].bconnected[2] = x + 1;
                p[x + num].bconnected[3] = num + 1;
            }

            return p;
        }
        public Point3D[] MakeHouse()
        {
            Point3D[] p = new Point3D[10];
            p[0] = new Point3D();
            p[1] = new Point3D();
            p[2] = new Point3D();
            p[3] = new Point3D();
            p[4] = new Point3D();
            p[5] = new Point3D();
            p[6] = new Point3D();
            p[7] = new Point3D();
            p[8] = new Point3D();
            p[9] = new Point3D();
            float depth = -1;
            p[0].X = 0;
            p[0].Y = 0;
            p[0].Z = depth;
            Array.Resize(ref p[0].lconnected, 3);
            p[0].lconnected[0] = 2;
            p[0].lconnected[1] = 6;
            p[0].lconnected[2] = 5;
            Array.Resize(ref p[0].bconnected, 3);
            p[0].bconnected[0] = 2;
            p[0].bconnected[1] = 6;
            p[0].bconnected[2] = 5;
            p[1].X = 1;
            p[1].Y = 0;
            p[1].Z = depth;
            Array.Resize(ref p[1].lconnected, 3);
            p[1].lconnected[0] = 1;
            p[1].lconnected[1] = 7;
            p[1].lconnected[2] = 3;
            Array.Resize(ref p[1].bconnected, 3);
            p[1].bconnected[0] = 1;
            p[1].bconnected[1] = 7;
            p[1].bconnected[2] = 3;
            p[2].X = 1;
            p[2].Y = 1;
            p[2].Z = depth;
            Array.Resize(ref p[2].lconnected, 3);
            p[2].lconnected[0] = 2;
            p[2].lconnected[1] = 4;
            p[2].lconnected[2] = 8;
            Array.Resize(ref p[2].bconnected, 4);
            p[2].bconnected[0] = 2;
            p[2].bconnected[1] = 4;
            p[2].bconnected[2] = 8;
            p[2].bconnected[3] = 5;
            p[3].X = 0.5F;
            p[3].Y = 1.5F;
            p[3].Z = depth;
            Array.Resize(ref p[3].lconnected, 3);
            p[3].lconnected[0] = 3;
            p[3].lconnected[1] = 5;
            p[3].lconnected[2] = 9;
            Array.Resize(ref p[3].bconnected, 3);
            p[3].bconnected[0] = 3;
            p[3].bconnected[1] = 5;
            p[3].bconnected[2] = 9;
            p[4].X = 0;
            p[4].Y = 1;
            p[4].Z = depth;
            Array.Resize(ref p[4].lconnected, 3);
            p[4].lconnected[0] = 4;
            p[4].lconnected[1] = 1;
            p[4].lconnected[2] = 10;
            Array.Resize(ref p[4].bconnected, 4);
            p[4].bconnected[0] = 4;
            p[4].bconnected[1] = 1;
            p[4].bconnected[2] = 10;
            p[4].bconnected[3] = 3;


            p[5].X = 0;
            p[5].Y = 0;
            Array.Resize(ref p[5].lconnected, 3);
            p[5].lconnected[0] = 7;
            p[5].lconnected[1] = 1;
            p[5].lconnected[2] = 10;
            Array.Resize(ref p[5].bconnected, 3);
            p[5].bconnected[0] = 7;
            p[5].bconnected[1] = 1;
            p[5].bconnected[2] = 10;
            p[6].X = 1;
            p[6].Y = 0;
            Array.Resize(ref p[6].lconnected, 3);
            p[6].lconnected[0] = 6;
            p[6].lconnected[1] = 8;
            p[6].lconnected[2] = 2;
            Array.Resize(ref p[6].bconnected, 3);
            p[6].bconnected[0] = 6;
            p[6].bconnected[1] = 8;
            p[6].bconnected[2] = 2;
            p[7].X = 1;
            p[7].Y = 1;
            Array.Resize(ref p[7].lconnected, 3);
            p[7].lconnected[0] = 7;
            p[7].lconnected[1] = 3;
            p[7].lconnected[2] = 9;
            Array.Resize(ref p[7].bconnected, 3);
            p[7].bconnected[0] = 7;
            p[7].bconnected[1] = 3;
            p[7].bconnected[2] = 9;
            p[8].X = 0.5F;
            p[8].Y = 1.5F;
            Array.Resize(ref p[8].lconnected, 3);
            p[8].lconnected[0] = 8;
            p[8].lconnected[1] = 4;
            p[8].lconnected[2] = 10;
            Array.Resize(ref p[8].bconnected, 3);
            p[8].bconnected[0] = 8;
            p[8].bconnected[1] = 4;
            p[8].bconnected[2] = 10;
            p[9].X = 0;
            p[9].Y = 1;
            Array.Resize(ref p[9].lconnected, 3);
            p[9].lconnected[0] = 9;
            p[9].lconnected[1] = 5;
            p[9].lconnected[2] = 6;
            Array.Resize(ref p[9].bconnected, 3);
            p[9].bconnected[0] = 9;
            p[9].bconnected[1] = 5;
            p[9].bconnected[2] = 6;
            return p;
        }
    }
}
