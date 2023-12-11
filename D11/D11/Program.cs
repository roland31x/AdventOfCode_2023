using System.Data.Common;
using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D11
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 11;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "cooooooooooooooooooooooooooooooking";
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

        private static string Part2(List<string> lines)
        {
            Map m = new Map(lines);

            long toreturn = m.GetScore(1000000);

            return toreturn.ToString();
        }
        private static string Part1(List<string> lines)
        {
            Map m = new Map(lines);

            long toreturn = m.GetScore(2);

            return toreturn.ToString();
        }

        public class Map
        {
            public List<List<int>> map = new List<List<int>>();
            public static List<int[]> dist4 = new List<int[]>() { new int[] {-1,0 }, new int[] {0, -1 }, new int[] {1, 0 }, new int[] {0,1 }, };
            public List<int> extrarowsat;
            public List<int> extracolsat;
            public Map(List<string> lines)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    map.Add(new List<int>());
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '#')
                            map[i].Add(1);
                        else
                            map[i].Add(0);
                    }
                }

                List<int> doublerowsidx = new List<int>();
                List<int> doublecolumnsidx = new List<int>();

                for (int i = 0; i < lines.Count; i++)
                    if (map[i].Max() == 0)
                        doublerowsidx.Add(i);

                for (int i = 0; i < lines[0].Length; i++)
                {
                    HashSet<int> set = new HashSet<int>();
                    for (int j = 0; j < lines.Count; j++)
                    {
                        set.Add(map[j][i]);
                    }
                    if (set.Count == 1)
                        doublecolumnsidx.Add(i);
                }

                extracolsat = doublecolumnsidx;
                extrarowsat = doublerowsidx;
            }
            public long GetScore(int galaxyexpansion)
            {
                List<(int, int)> galaxies = new List<(int, int)>();
                for(int i = 0; i < map.Count; i++)
                {
                    for(int j = 0; j < map[i].Count; j++)
                    {
                        if (map[i][j] == 1)
                        {
                            galaxies.Add((i, j));
                        }
                    }
                }
                List<long> dists = new List<long>();
                for(int i = 0; i < galaxies.Count; i++)
                {
                    for(int j = i + 1; j < galaxies.Count; j++)
                    {
                        (int g1i, int g1j) = galaxies[i];
                        (int g2i, int g2j) = galaxies[j];

                        long idiff = Math.Abs(g1i - g2i);
                        foreach(int extra in extrarowsat.Where(x => (x >= g1i && x <= g2i) || (x >= g2i && x <= g1i)))
                        {
                            idiff += galaxyexpansion - 1;
                        }

                        long jdiff = Math.Abs(g2j - g1j);
                        foreach (int extra in extracolsat.Where(x => (x >= g1j && x <= g2j) || (x >= g2j && x <= g1j)))
                        {
                            jdiff += galaxyexpansion - 1;
                        }

                        long local = idiff + jdiff;

                        dists.Add(local);
                    }
                }
                dists.Sort();

                long score = 0;
                for(int i = 0; i <  dists.Count; i++)
                    score += dists[i];

                return score;
            }
        }
    }
}
