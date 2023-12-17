using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.Diagnostics;

namespace D17
{
    internal class Program
    {
        static int YEAR = 2023;
        static int DAY = 17;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "please send cookies";
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
            long toreturn = long.MaxValue;
            List<List<int>> map = new List<List<int>>();
            List<int[]> dirs = new List<int[]>() { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };

            foreach (string line in lines)
            {
                map.Add(new List<int>());
                for (int i = 0; i < line.Length; i++)
                    map.Last().Add(line[i] - '0');
            }

            int minconsecutive = 1; // 0 doesn't work coz you need at least 1 anyway
            int maxconsecutive = 3;

            int[,,,] marked = new int[map.Count, map[0].Count, maxconsecutive, 4];
            PriorityQueue<(int, int, int, int, int), int> pq = new PriorityQueue<(int, int, int, int, int), int>();
            pq.Enqueue((0, 0, -1, 1, 1), 1);
            while(pq.Count > 0)
            {
                (int i, int j, int dir, int consec, int dist) = pq.Dequeue();
                if (i < 0 || i >= map.Count || j < 0 || j >= map[i].Count || consec > maxconsecutive || marked[i, j, consec - 1, dir == -1 ? 0 : dir] != 0)
                    continue;

                marked[i, j, consec - 1, dir == -1 ? 0 : dir] = dist;
                int dirhelper = dir;
                if (dirhelper == -1)
                    dirhelper = 0;
                for(int k = -1; k <= 1; k++)
                {
                    int nextdir = (dirhelper + 4 + k) % 4;
                    int nexti = i + dirs[nextdir][0];
                    int nextj = j + dirs[nextdir][1];
                    if (nexti < 0 || nexti >= map.Count || nextj < 0 || nextj >= map[i].Count)
                        continue;
                    if (dir != -1)
                        if (nextdir != dir && consec < minconsecutive)
                            continue;
                    pq.Enqueue((nexti, nextj, nextdir, nextdir == dir ? consec + 1 : 1, dist + map[nexti][nextj]), dist + map[nexti][nextj]);
                }                              
                
            }

            for (int i = minconsecutive - 1; i < maxconsecutive - 1; i++)
                for(int j = 0; j < 4; j++)
                    if (marked[map.Count - 1, map[0].Count - 1, i, j] != 0 && marked[map.Count - 1, map[0].Count - 1, i, j] < toreturn)
                        toreturn = marked[map.Count - 1, map[0].Count - 1, i, j];



            return (toreturn - 1).ToString();
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = long.MaxValue;
            List<List<int>> map = new List<List<int>>();
            List<int[]> dirs = new List<int[]>() { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };

            int minconsecutive = 4;
            int maxconsecutive = 10;

            foreach (string line in lines)
            {
                map.Add(new List<int>());
                for (int i = 0; i < line.Length; i++)
                    map.Last().Add(line[i] - '0');
            }

            int[,,,] marked = new int[map.Count, map[0].Count, maxconsecutive, 4];
            PriorityQueue<(int, int, int, int, int), int> pq = new PriorityQueue<(int, int, int, int, int), int>();
            pq.Enqueue((0, 0, -1, 1, 1), 1);

            while (pq.Count > 0)
            {
                (int i, int j, int dir, int consec, int dist) = pq.Dequeue();
                if (i < 0 || i >= map.Count || j < 0 || j >= map[i].Count || consec > maxconsecutive || marked[i, j, consec - 1, dir == -1 ? 0 : dir] != 0)
                    continue;

                marked[i, j, consec - 1, dir == -1 ? 0 : dir] = dist;
                int dirhelper = dir;
                if (dirhelper == -1)
                    dirhelper = 0;
                for (int k = -1; k <= 1; k++)
                {
                    int nextdir = (dirhelper + 4 + k) % 4;
                    int nexti = i + dirs[nextdir][0];
                    int nextj = j + dirs[nextdir][1];
                    if (nexti < 0 || nexti >= map.Count || nextj < 0 || nextj >= map[i].Count)
                        continue;

                    if (dir != -1)
                        if (nextdir != dir && consec < minconsecutive)
                            continue;
                    
                    pq.Enqueue((nexti, nextj, nextdir, nextdir == dir ? consec + 1 : 1, dist + map[nexti][nextj]), dist + map[nexti][nextj]);
                }

            }

            for (int i = minconsecutive - 1; i < maxconsecutive - 1; i++)
                for (int j = 0; j < 4; j++)
                    if (marked[map.Count - 1, map[0].Count - 1, i, j] != 0 && marked[map.Count - 1, map[0].Count - 1, i, j] < toreturn)
                        toreturn = marked[map.Count - 1, map[0].Count - 1, i, j];

            return (toreturn - 1).ToString();
        }
    }
}
