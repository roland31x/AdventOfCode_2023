using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.Drawing;
using ExtendedNumerics;

namespace D21
{
    public class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 21;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "53616c7465645f5fd7e9d8c2e9314c17545e7e20013b8f93d4b4655003aa200c829fa30cd5ec7e713440fa5c2384671a859b9792ebf26bf4aac5bd79681dfbd6";
        public static void Main(string[] args)
        {
            List<string> lines = AOCIn.GetInput(InputPath, YEAR, DAY, SeshCookie);

            string part1 = Part1(lines);
            Console.WriteLine("Part 1 solution: " + part1);

            string part2 = Part2(lines);
            Console.WriteLine("Part 2 solution: " + part2);

            Console.WriteLine("Submit? Y/N");
            string command = Console.ReadLine();
            if (command.ToUpper() == "Y")
            {
                string resp = AOCOut.Submit(part1, YEAR, DAY, 1, SeshCookie);
                Console.WriteLine(resp);

                Thread.Sleep(6000);

                string resp2 = AOCOut.Submit(part2, YEAR, DAY, 2, SeshCookie);
                Console.WriteLine(resp2);
            }
        }

        private static string Part1(List<string> lines)
        {
            List<int[]> dirs = new List<int[]>() { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
            int starti = -1;
            int startj = -1;
            int[,] map = new int[lines.Count, lines[0].Length];
            for(int i = 0; i < lines.Count; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                    else if (lines[i][j] == 'S')
                    {
                        starti = i;
                        startj = j;
                    }
                }    
            }

            HashSet<(int,int)> q = new HashSet<(int, int)>();
            HashSet<(int, int)> nextq = new HashSet<(int, int)>();
            q.Add((starti, startj));
            for(int t = 0; t < 64; t++)
            {
                foreach((int deqi, int deqj) in q)
                {
                    foreach (int[] dir in dirs)
                    {
                        int nexti = deqi + dir[0];
                        int nextj = deqj + dir[1];
                        if (map[nexti, nextj] == 1)
                            continue;
                        nextq.Add((nexti, nextj));
                    }
                }                                                         
                q = nextq;
                nextq = new HashSet<(int, int)>();
            }
            
            long toreturn = 0;

            toreturn = q.Count;

            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            List<int[]> dirs = new List<int[]>() { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
            int starti = -1;
            int startj = -1;
            int[,] map = new int[lines.Count, lines[0].Length];
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                    else if (lines[i][j] == 'S')
                    {
                        starti = i;
                        startj = j;
                    }
                }
            }

            int stepsneeded = 26501365;
            List<Point> pts = new List<Point>();

            HashSet<(int, int)> q = new HashSet<(int, int)>();
            HashSet<(int, int)> nextq = new HashSet<(int, int)>();
            q.Add((starti, startj));
            for (int t = 1; t < 5000; t++)
            {
                foreach ((int deqi, int deqj) in q)
                {
                    foreach (int[] dir in dirs)
                    {
                        int nexti = deqi + dir[0];
                        int nextj = deqj + dir[1];
                        if (map[(nexti + lines.Count * 100000) % lines.Count, (nextj + lines[0].Length*100000) % lines[0].Length] == 1)
                            continue;
                        nextq.Add((nexti, nextj));
                    }
                }
                q = nextq;
                nextq = new HashSet<(int, int)>();
                if ((t + 66) % 131 == 0)
                    pts.Add(new Point(t,q.Count));
                if (pts.Count > 3)
                    break;
            }


            Polynomial p = GetBestPolynomial(pts, 2);

            BigDecimal toreturn = p.EvaluateAt(stepsneeded);

            return BigDecimal.Round(toreturn).ToString();
        }
        public static Polynomial GetBestPolynomial(List<Point> pts, int degree)
        {
            Polynomial p = new Polynomial(degree);
            degree += 1;
            Matrix xs = new Matrix(degree, degree);
            for (int i = 0; i < degree; i++)
                for (int j = 0; j < degree; j++)
                    foreach (Point pt in pts)
                        xs[i, j] += BigDecimal.Pow((double)pt.X, i + j);

            Matrix ys = new Matrix(degree, 1);
            for (int i = 0; i < degree; i++)
                foreach (Point pt in pts)
                    ys[i, 0] += (BigDecimal)Math.Pow((double)pt.X, i) * (BigDecimal)pt.Y;

            Matrix coefs = xs.Inverse().Multiply(ys);

            for (int i = degree - 1; i >= 0; i--)
                p[degree - 1 - i] = coefs[i, 0];
            return p;
        }
    }
}
