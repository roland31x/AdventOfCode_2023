using System.Runtime.InteropServices;
using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D03
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 3;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "cookie here pls";
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
            int toreturn = 0;
            int[,] map = new int[lines.Count,lines[0].Length];
            int[,] vis = new int[lines.Count,lines[0].Length];

            for(int i = 0; i < lines.Count; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if (int.TryParse(lines[i][j].ToString(), out int res))
                        map[i, j] = res + 1;
                    else if (lines[i][j] != '.')
                        map[i, j] = -1;
                }
            }

            for(int i = 0; i < lines.Count; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if (map[i,j] == -1)
                    {
                        map[i, j] = 0;
                        DFS4(i, j + 1, map, vis ,ref toreturn);
                        DFS4(i - 1, j, map, vis ,ref toreturn);
                        DFS4(i, j - 1, map, vis ,ref toreturn);
                        DFS4(i + 1, j, map, vis, ref toreturn);

                        DFS4(i + 1, j + 1, map, vis, ref toreturn);
                        DFS4(i - 1, j + 1, map, vis, ref toreturn);
                        DFS4(i - 1, j - 1, map, vis, ref toreturn);
                        DFS4(i + 1, j - 1, map, vis, ref toreturn);
                    }
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (vis[i, j] != 0)
                    {
                        int local = 0;
                        int idx = j;
                        while (idx < lines[i].Length && vis[i, idx] != 0)
                        {
                            local *= 10;
                            local += map[i, idx] - 1;
                            vis[i, idx] = 0;
                            idx++;
                        }
                        toreturn += local;
                    }
                }
            }

            return toreturn.ToString();
        }
        static void DFS4(int i, int j, int[,] map, int[,] vis, ref int sc)
        {
            if (i < 0 || j < 0 || j >= map.GetLength(1) || i >= map.GetLength(0))
                return;
            if (map[i, j] == 0 || vis[i, j] != 0)
                return;           
            vis[i, j] = 1;
            DFS4(i, j + 1, map, vis, ref sc);
            DFS4(i, j - 1, map, vis, ref sc);
        }
        private static string Part2(List<string> lines)
        {
            int toreturn = 0;
            int[,] map = new int[lines.Count, lines[0].Length];
            int[,] vis = new int[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (int.TryParse(lines[i][j].ToString(), out int res))
                        map[i, j] = res + 1;
                    else if (lines[i][j] == '*')
                        map[i, j] = -1;
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (map[i, j] == -1)
                    {
                        map[i, j] = 0;
                        vis[i, j] = 2;

                        DFS4(i, j + 1, map, vis, ref toreturn);
                        DFS4(i - 1, j, map, vis, ref toreturn);
                        DFS4(i, j - 1, map, vis, ref toreturn);
                        DFS4(i + 1, j, map, vis, ref toreturn);

                        DFS4(i + 1, j + 1, map, vis, ref toreturn);
                        DFS4(i - 1, j + 1, map, vis, ref toreturn);
                        DFS4(i - 1, j - 1, map, vis, ref toreturn);
                        DFS4(i + 1, j - 1, map, vis, ref toreturn);
                    }
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (vis[i, j] == 2)
                    {
                        vis[i, j] = 0;

                        int[,] localvis = new int[vis.GetLength(0), vis.GetLength(1)];

                        DFS4(i, j + 1, map, localvis, ref toreturn);
                        DFS4(i - 1, j, map, localvis, ref toreturn);
                        DFS4(i, j - 1, map, localvis, ref toreturn);
                        DFS4(i + 1, j, map, localvis, ref toreturn);

                        DFS4(i + 1, j + 1, map, localvis, ref toreturn);
                        DFS4(i - 1, j + 1, map, localvis, ref toreturn);
                        DFS4(i - 1, j - 1, map, localvis, ref toreturn);
                        DFS4(i + 1, j - 1, map, localvis, ref toreturn);

                        int totalnr = 0;
                        int localsc = 1;
                        for (int k = 0; k < lines.Count; k++)
                        {
                            for (int l = 0; l < lines[k].Length; l++)
                            {
                                if (localvis[k, l] != 0)
                                {
                                    totalnr++;
                                    int evenmorelocal = 0;
                                    int idx = l;
                                    while (idx < lines[k].Length && localvis[k, idx] != 0)
                                    {
                                        evenmorelocal *= 10;
                                        evenmorelocal += map[k, idx] - 1;
                                        localvis[k, idx] = 0;
                                        idx++;
                                    }
                                    localsc *= evenmorelocal;
                                }
                            }
                        }
                        if (totalnr == 2)
                            toreturn += localsc;
                    }
                }
            }

            return toreturn.ToString();
        }
    }
}
