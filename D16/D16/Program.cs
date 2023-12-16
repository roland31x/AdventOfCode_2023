using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace D16
{
    internal class Program
    {
        static int YEAR = 2023;
        static int DAY = 16;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "coooookies";
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
            long toreturn = SimMap(0, 0, 0, lines);

            return toreturn.ToString();
        }
        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            List<List<Tile>> Map = GetNewMap(lines);

            for (int i = 0; i < Map.Count; i++)
            {
                for (int j = 0; j < Map[i].Count; j++)
                {
                    if (i == 0)
                    {
                        int local = SimMap(0, j, 1, lines);
                        if (local > toreturn)
                            toreturn = local;
                    }
                    if (j == 0)
                    {
                        int local = SimMap(i, 0, 0, lines);
                        if (local > toreturn)
                            toreturn = local;
                    }
                    if (i == Map.Count - 1)
                    {
                        int local = SimMap(Map.Count - 1, j, 3, lines);
                        if (local > toreturn)
                            toreturn = local;
                    }
                    if (j == Map[i].Count - 1)
                    {
                        int local = SimMap(i, Map[i].Count - 1, 2, lines);
                        if (local > toreturn)
                            toreturn = local;
                    }
                }
            }

            return toreturn.ToString();
        }

        public class Beam
        {
            public static List<int[]> dirs = new List<int[]>() { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } }; // R 0   D 1    L 2    U 3
        }

        public class Tile
        {
            public char C;
            public int Val = 0;
            public Tile(char c)
            {
                this.C = c;
            }
        }

        public static int SimMap(int starti, int startj, int startdir, List<string> lines)
        {
            List<List<Tile>> current = GetNewMap(lines);
            List<(int, int, int)> seen = new List<(int, int, int)>();
            Queue<(int, int, int)> alive = new Queue<(int, int, int)>();
            alive.Enqueue((starti, startj, startdir));
            while (alive.Count > 0)
            {
                (int nexti, int nextj, int dir) = alive.Dequeue();
                DFS(current, nexti, nextj, dir, seen, alive);
            }

            int local = 0;
            foreach (List<Tile> l in current)
                foreach (Tile t in l)
                    if (t.Val != 0)
                        local++;

            return local;
        }
        static void DFS(List<List<Tile>> m, int curri, int currj, int dir, List<(int, int, int)> seen, Queue<(int, int, int)> q)
        {
            if (curri < 0 || curri >= m.Count || currj < 0 || currj >= m[curri].Count)
                return;

            m[curri][currj].Val = 1;
            if (m[curri][currj].C == '.')
            {
                int nexti = Beam.dirs[dir][0] + curri;
                int nextj = Beam.dirs[dir][1] + currj;
                if (!seen.Contains((nexti, nextj, dir)))
                {
                    seen.Add((nexti, nextj, dir));
                    //q.Enqueue((nexti, nextj, dir));
                    DFS(m, nexti, nextj, dir, seen, q);
                }
            }
            else if (m[curri][currj].C == '|')
            {
                if (dir == 0 || dir == 2)
                {

                    int nexti = Beam.dirs[1][0] + curri;
                    int nextj = Beam.dirs[1][1] + currj;
                    if (!seen.Contains((nexti, nextj, 1)))
                    {
                        seen.Add((nexti, nextj, 1));
                        q.Enqueue((nexti, nextj, 1));
                        //DFS(m, nexti, nextj, 1, seen);
                    }

                    nexti = Beam.dirs[3][0] + curri;
                    nextj = Beam.dirs[3][1] + currj;
                    if (!seen.Contains((nexti, nextj, 3)))
                    {
                        seen.Add((nexti, nextj, 3));
                        q.Enqueue((nexti, nextj, 3));
                        //DFS(m, nexti, nextj, 3, seen);
                    }
                }
                else
                {
                    int nexti = Beam.dirs[dir][0] + curri;
                    int nextj = Beam.dirs[dir][1] + currj;
                    if (!seen.Contains((nexti, nextj, dir)))
                    {
                        seen.Add((nexti, nextj, dir));
                        q.Enqueue((nexti, nextj, dir));
                        //DFS(m, nexti, nextj, dir, seen);
                    }
                }
            }
            else if (m[curri][currj].C == '-')
            {
                if (dir == 1 || dir == 3)
                {
                    int nexti = Beam.dirs[0][0] + curri;
                    int nextj = Beam.dirs[0][1] + currj;
                    if (!seen.Contains((nexti, nextj, 0)))
                    {
                        seen.Add((nexti, nextj, 0));
                        q.Enqueue((nexti, nextj, 0));
                        //DFS(m, nexti, nextj, 0, seen);
                    }

                    nexti = Beam.dirs[2][0] + curri;
                    nextj = Beam.dirs[2][1] + currj;
                    if (!seen.Contains((nexti, nextj, 2)))
                    {
                        seen.Add((nexti, nextj, 2));
                        q.Enqueue((nexti, nextj, 2));
                        //DFS(m, nexti, nextj, 2, seen);
                    }
                }
                else
                {
                    int nexti = Beam.dirs[dir][0] + curri;
                    int nextj = Beam.dirs[dir][1] + currj;
                    if (!seen.Contains((nexti, nextj, dir)))
                    {
                        seen.Add((nexti, nextj, dir));
                        q.Enqueue((nexti, nextj, dir));
                        //DFS(m, nexti, nextj, dir, seen);
                    }
                }
            }
            else if (m[curri][currj].C == '/')
            {
                int newdir = 0;
                if (dir == 0)
                    newdir = 3;
                else if (dir == 1)
                    newdir = 2;
                else if (dir == 2)
                    newdir = 1;
                else if (dir == 3)
                    newdir = 0;


                int nexti = Beam.dirs[newdir][0] + curri;
                int nextj = Beam.dirs[newdir][1] + currj;
                if (!seen.Contains((nexti, nextj, newdir)))
                {
                    seen.Add((nexti, nextj, newdir));
                    q.Enqueue((nexti, nextj, newdir));
                    //DFS(m, nexti, nextj, newdir, seen);
                }
            }
            else if (m[curri][currj].C == '\\')
            {
                int newdir = 0;

                if (dir == 0)
                    newdir = 1;
                else if (dir == 1)
                    newdir = 0;
                else if (dir == 2)
                    newdir = 3;
                else if (dir == 3)
                    newdir = 2;

                int nexti = Beam.dirs[newdir][0] + curri;
                int nextj = Beam.dirs[newdir][1] + currj;
                if (!seen.Contains((nexti, nextj, newdir)))
                {
                    seen.Add((nexti, nextj, newdir));
                    q.Enqueue((nexti, nextj, newdir));
                    //DFS(m, nexti, nextj, newdir, seen);
                }
            }
        }
        public static List<List<Tile>> GetNewMap(List<string> lines)
        {
            List<List<Tile>> sub = new List<List<Tile>>();
            foreach (string line in lines)
            {
                sub.Add(new List<Tile>());
                for (int k = 0; k < line.Length; k++)
                    sub.Last().Add(new Tile(line[k]));
            }
            return sub;
        }
    }
}
