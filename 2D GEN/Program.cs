using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D_GEN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 63;
            Generation Gen = new Generation(Console.WindowWidth, Console.WindowHeight);
            int Seed = 0;
            bool Error = false;
            
            while (true)
            {               
                Gen.Generate(ref Seed, 50, Error);
                Gen.AddTrees(10);
                Gen.AddOres(10);
                Gen.AddCaves(1, 4);
                Gen.ShowInConsole();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Seed = {0}, Has errors {1}", Seed, Error);        
                Console.ReadLine();
                Console.Clear();
            }                  
        }    
    }

    public class Generation
    {
        static int Width, Height = 0;

        Random Rand = new Random();
        Block[,] World;

        public Generation(int width, int height)
        {
            Width = width;
            Height = height;
            World = new Block[Width, Height];
        }

        public Block[,] Generate(ref int Seed, float HeightAddition = 0, bool Error = false)
        {
            Error = false;
            HeightAddition = Height - HeightAddition;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    World[i, j] = new Block { Symbol = ' ', color = ConsoleColor.Black };
                }
            }

            float Smoothness = 7;
            float HeightMultiplier = 7;
            Seed = Rand.Next(-10000, 10000);

            PerlinNoise Perl = new PerlinNoise(Seed);

            for (int i = 0; i < Width; i++)
            {
                double H = Math.Round(Perl.Noise(Seed, i / Smoothness) * HeightMultiplier) + HeightAddition;

                for (int j = Height - 1; j >= H; j--)
                {
                    char b = '8';
                    ConsoleColor color = ConsoleColor.Gray;
                    
                    if (j < H + 2) color = ConsoleColor.Green;
                    else
                    if (j < H + 5) color = ConsoleColor.DarkGreen;
                    else
                    if (j < H + 40) color = ConsoleColor.DarkRed;
                    else
                    color = ConsoleColor.Magenta;

                    World[i, j] = new Block { color = color, Symbol = b };
                }
            }
            return World;
        }    

        public void AddOres(int Count)
        {
            List<int> X = new List<int>();
            List<int> Y = new List<int>();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (World[i, j].color == ConsoleColor.DarkRed)
                    {
                        X.Add(i);
                        Y.Add(j);
                    }
                }
            }

            for (int i = 0; i < Count; i++)
            {
                int a = Rand.Next(0, X.Count);
                World[X[a], Y[a]] = new Block { color = ConsoleColor.Yellow, Symbol = '1' };

                for (int j = 0; j < 7; j++)
                {
                    int type = Rand.Next(0, 5);
                    if (X[a] + 1 < Width && X[a] - 1 > 0 && Y[a] + 1 < Height && Y[a] - 1 > 0)
                    {
                        switch (type)
                        {
                            case 0: World[X[a] + 1, Y[a]] = new Block { color = ConsoleColor.Yellow, Symbol = '1' }; X[a] = X[a] + 1; break;
                            case 1: World[X[a], Y[a] + 1] = new Block { color = ConsoleColor.Yellow, Symbol = '1' }; Y[a] = Y[a] + 1; break;
                            case 2: World[X[a] - 1, Y[a]] = new Block { color = ConsoleColor.Yellow, Symbol = '1' }; X[a] = X[a] - 1; break;
                            case 3: World[X[a], Y[a] - 1] = new Block { color = ConsoleColor.Yellow, Symbol = '1' }; Y[a] = Y[a] - 1; break;
                            case 4: World[X[a] + 1, Y[a] - 1] = new Block { color = ConsoleColor.Yellow, Symbol = '1' }; X[a] = X[a] + 1; Y[a] = Y[a] - 1; break;
                            case 5: World[X[a] - 1, Y[a] + 1] = new Block { color = ConsoleColor.Yellow, Symbol = '1' }; X[a] = X[a] - 1; Y[a] = Y[a] + 1; break;
                        }
                    }
                    else i = i - 1;
                }

            }
        }

        public void AddTrees(int Count)
        {
            List<int> X = new List<int>();
            List<int> Y = new List<int>();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (World[i, j].color == ConsoleColor.Green)
                    {
                        X.Add(i);
                        Y.Add(j);
                        break;
                    }
                }
            }

            for (int i = 0; i < Count; i++)
            {
                int a = Rand.Next(0, X.Count);
                if (X[a] + 2 < Width && X[a] - 2 > 0 && Y[a] + 6 < Height && Y[a] - 6 > 0)
                {
                    World[X[a], Y[a]] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a], Y[a] - 1] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a], Y[a] - 2] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a], Y[a] - 3] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };

                    World[X[a], Y[a] - 4] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] + 1, Y[a] - 4] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] - 1, Y[a] - 4] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };

                    World[X[a], Y[a] - 5] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] + 1, Y[a] - 5] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] + 2, Y[a] - 5] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] - 1, Y[a] - 5] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] - 2, Y[a] - 5] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };

                    World[X[a], Y[a] - 6] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] + 1, Y[a] - 6] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                    World[X[a] - 1, Y[a] - 6] = new Block { color = ConsoleColor.Cyan, Symbol = '1' };
                }
                else i = i - 1;
            }
        }

        public bool AddCaves(int Count, int Width)
        {
            if (Width < Height)
            {               
                for (int c = 0; c < Count; c++)
                {
                    List<int> Y = new List<int>();

                    for (int i = 0; i < Width; i++)
                    {
                        for (int p = 0; p < Height; p++)
                        {
                            if (World[i, p].color == ConsoleColor.DarkRed)
                            {
                                Y.Add(p);
                            }
                        }
                    }
                    
                    int a = Rand.Next(0, Y.Count);

                    for (int i = 0; i < Generation.Width; i++)
                    {
                        if (Y[a] + Width < Height)
                        {
                            if (Y[a] - Width > 0)
                            {
                                for (int f = 0; f < Width; f++)
                                {
                                    World[i, Y[a] - int.Parse(Math.Round(Width / 2D).ToString()) + f] = new Block { color = ConsoleColor.Black, Symbol = '8' };
                                }
                                int r = Rand.Next(1, 3);
                                switch (r)
                                {
                                    case 1: if (a > 0) a = a - 1; break;
                                    case 2: if (a < Height) a = a + 1; break;
                                }
                            }
                            else
                            {
                                a++;
                                i--;
                            }
                        }
                        else
                        {
                            a--;
                            i--;
                        }                  
                    }
                }
                return true;
            }
            else return false;           
        }

        public void ShowInConsole()
        {
            ConsoleColor temp = Console.ForegroundColor;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.ForegroundColor = World[i, j].color;
                    Console.Write(World[i, j].Symbol);
                    Console.ForegroundColor = temp;
                }
            }
        }

        public class Block
        {
            public char Symbol;
            public ConsoleColor color;
        }

        class PerlinNoise
        {
            private const int GradientSizeTable = 256;
            private readonly Random _random;
            private readonly double[] _gradients = new double[GradientSizeTable * 3];
            /* Borrowed from Darwyn Peachey (see references above).
               The gradient table is indexed with an XYZ triplet, which is first turned
               into a single random index using a lookup in this table. The table simply
               contains all numbers in [0..255] in random order. */
            private readonly byte[] _perm = new byte[] {
              225,155,210,108,175,199,221,144,203,116, 70,213, 69,158, 33,252,
                5, 82,173,133,222,139,174, 27,  9, 71, 90,246, 75,130, 91,191,
              169,138,  2,151,194,235, 81,  7, 25,113,228,159,205,253,134,142,
              248, 65,224,217, 22,121,229, 63, 89,103, 96,104,156, 17,201,129,
               36,  8,165,110,237,117,231, 56,132,211,152, 20,181,111,239,218,
              170,163, 51,172,157, 47, 80,212,176,250, 87, 49, 99,242,136,189,
              162,115, 44, 43,124, 94,150, 16,141,247, 32, 10,198,223,255, 72,
               53,131, 84, 57,220,197, 58, 50,208, 11,241, 28,  3,192, 62,202,
               18,215,153, 24, 76, 41, 15,179, 39, 46, 55,  6,128,167, 23,188,
              106, 34,187,140,164, 73,112,182,244,195,227, 13, 35, 77,196,185,
               26,200,226,119, 31,123,168,125,249, 68,183,230,177,135,160,180,
               12,  1,243,148,102,166, 38,238,251, 37,240,126, 64, 74,161, 40,
              184,149,171,178,101, 66, 29, 59,146, 61,254,107, 42, 86,154,  4,
              236,232,120, 21,233,209, 45, 98,193,114, 78, 19,206, 14,118,127,
               48, 79,147, 85, 30,207,219, 54, 88,234,190,122, 95, 67,143,109,
              137,214,145, 93, 92,100,245,  0,216,186, 60, 83,105, 97,204, 52};

            public PerlinNoise(int seed)
            {
                _random = new Random(seed);
                InitGradients();
            }

            public double Noise(double x, double y)
            {
                double z = 0;
                /* The main noise function. Looks up the pseudorandom gradients at the nearest
                   lattice points, dots them with the input vector, and interpolates the
                   results to produce a single output value in [0, 1] range. */

                int ix = (int)Math.Floor(x);
                double fx0 = x - ix;
                double fx1 = fx0 - 1;
                double wx = Smooth(fx0);

                int iy = (int)Math.Floor(y);
                double fy0 = y - iy;
                double fy1 = fy0 - 1;
                double wy = Smooth(fy0);

                int iz = (int)Math.Floor(z);
                double fz0 = z - iz;
                double fz1 = fz0 - 1;
                double wz = Smooth(fz0);

                double vx0 = Lattice(ix, iy, iz, fx0, fy0, fz0);
                double vx1 = Lattice(ix + 1, iy, iz, fx1, fy0, fz0);
                double vy0 = Lerp(wx, vx0, vx1);

                vx0 = Lattice(ix, iy + 1, iz, fx0, fy1, fz0);
                vx1 = Lattice(ix + 1, iy + 1, iz, fx1, fy1, fz0);
                double vy1 = Lerp(wx, vx0, vx1);

                double vz0 = Lerp(wy, vy0, vy1);

                vx0 = Lattice(ix, iy, iz + 1, fx0, fy0, fz1);
                vx1 = Lattice(ix + 1, iy, iz + 1, fx1, fy0, fz1);
                vy0 = Lerp(wx, vx0, vx1);

                vx0 = Lattice(ix, iy + 1, iz + 1, fx0, fy1, fz1);
                vx1 = Lattice(ix + 1, iy + 1, iz + 1, fx1, fy1, fz1);
                vy1 = Lerp(wx, vx0, vx1);

                double vz1 = Lerp(wy, vy0, vy1);
                return Lerp(wz, vz0, vz1);
            }

            private void InitGradients()
            {
                for (int i = 0; i < GradientSizeTable; i++)
                {
                    double z = 1f - 2f * _random.NextDouble();
                    double r = Math.Sqrt(1f - z * z);
                    double theta = 2 * Math.PI * _random.NextDouble();
                    _gradients[i * 3] = r * Math.Cos(theta);
                    _gradients[i * 3 + 1] = r * Math.Sin(theta);
                    _gradients[i * 3 + 2] = z;
                }
            }

            private int Permutate(int x)
            {
                const int mask = GradientSizeTable - 1;
                return _perm[x & mask];
            }

            private int Index(int ix, int iy, int iz)
            {
                // Turn an XYZ triplet into a single gradient table index.
                return Permutate(ix + Permutate(iy + Permutate(iz)));
            }

            private double Lattice(int ix, int iy, int iz, double fx, double fy, double fz)
            {
                // Look up a random gradient at [ix,iy,iz] and dot it with the [fx,fy,fz] vector.
                int index = Index(ix, iy, iz);
                int g = index * 3;
                return _gradients[g] * fx + _gradients[g + 1] * fy + _gradients[g + 2] * fz;
            }

            private double Lerp(double t, double value0, double value1)
            {
                // Simple linear interpolation.
                return value0 + t * (value1 - value0);
            }

            private double Smooth(double x)
            {
                /* Smoothing curve. This is used to calculate interpolants so that the noise
                  doesn't look blocky when the frequency is low. */
                return x * x * (3 - 2 * x);
            }
        }
    }
}
