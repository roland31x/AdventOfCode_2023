using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.Diagnostics;
using System.Security.AccessControl;
using System;
using System.Drawing;
using System.Numerics;
using System.Xml.Serialization;

namespace D18
{
    internal class Program
    {
        static int YEAR = 2023;
        static int DAY = 18;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "what the fuck is picks theorem anyway";

        public static List<int[]> dirs = new List<int[]>() { new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 } };

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

                Thread.Sleep(100);

                string resp2 = AOCOut.Submit(part2, YEAR, DAY, 2, SeshCookie);
                Console.WriteLine(resp2);
            }
        }

        private static string Part1(List<string> lines)
        {
            long toreturn = 0;

            int[,] m = new int[500, 500];
            int curri = 250;
            int currj = 250;
            
            m[curri, currj] = 1;
            foreach(string line in lines)
            {
                string dirc = line.Split(' ')[0];
                int amount = int.Parse(line.Split(' ')[1]);
                string hex = line.Split(" ")[2];
                int dir = 0;
                switch (dirc)
                {
                    case "R":
                        dir = 1;
                        break;
                    case "U":
                        dir = 0;
                        break;
                    case "D":
                        dir = 2;
                        break;
                    case "L":
                        dir = 3;
                        break;
                }
                for (int t = 0; t < amount; t++)
                {
                    curri += dirs[dir][0];
                    currj += dirs[dir][1];
                    m[curri, currj] = 1;
                }
            }

            for(int j = 0; j < m.GetLength(1); j++)
            {
                if (m[0,j] == 0)
                {
                    BFSMark(0, j, m);
                    break;
                }
            }

            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    if (m[i, j] != 2)
                    {
                        toreturn++;
                    }
                }
            }

            return toreturn.ToString();
        }
        private static void BFSMark(int i, int j, int[,] m)
        {
            Queue<(int,int)> q = new Queue<(int, int)>();
            m[i, j] = 2;
            q.Enqueue((i, j));
            while(q.Count > 0)
            {
                (int deqi, int deqj) = q.Dequeue();
                foreach (int[] dir in dirs)
                {
                    int nexti = deqi + dir[0];
                    int nextj = deqj + dir[1];
                    if (nexti < 0 || nexti >= m.GetLength(0) || nextj < 0 || nextj >= m.GetLength(1))
                        continue;
                    if (m[nexti, nextj] != 0)
                        continue;
                    m[nexti, nextj] = 2;
                    q.Enqueue((nexti, nextj));
                }
            }           
        }

        private static string Part2(List<string> lines)
        {

            List<Point> pts = new List<Point>();

            int currx = 0;
            int curry = 0;

            pts.Add(new Point(currx, curry));

            foreach (string line in lines)
            {
                string hex = line.Split(" ")[2];

                int amount = Convert.ToInt32(hex.Substring(2, 5), 16);

                string dirc = hex[7].ToString();

                int dir = 0;
                switch (dirc)
                {
                    case "0":
                        dir = 1;
                        break;
                    case "1":
                        dir = 2;
                        break;
                    case "2":
                        dir = 3;
                        break;
                    case "3":
                        dir = 0;
                        break;
                }

                currx = currx + (amount) * dirs[dir][0];
                curry = curry + (amount) * dirs[dir][1];

                pts.Add(new Point(currx, curry));                
            }
            pts.Remove(pts.Last());
            pts.Reverse();

            BigInteger area = 0;
            BigInteger border = 0;

            for(int i = 0; i < pts.Count; i++)
            {
                Point p1 = pts[i];
                Point p2 = pts[(i + 1) % pts.Count];

                area += ((BigInteger)p1.X * (BigInteger)p2.Y) - ((BigInteger)p1.Y * (BigInteger)p2.X);
                border += Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
            }

            area /= 2;
            if(area < 0)
                area *= -1;

            BigInteger interior = area - (border / 2) + 1; // man i forgot about this, i assumed we just need area

            return (interior + border).ToString();
        }
    }
}
