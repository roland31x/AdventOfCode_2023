using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D14
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 14;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "aaaaaaaaaa";
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

            RockMap m = new RockMap(lines);
            m.TiltNorth();

            toreturn = m.LoadScore();

            return toreturn.ToString();
        }
        public class RockMap
        {
            public List<List<int>> map = new List<List<int>>();
            public RockMap(List<string> lines)
            {
                for(int i = 0; i < lines.Count; i++)
                {
                    map.Add(new List<int>());
                    for(int j = 0; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == '#')
                            map[i].Add(1);
                        else if (lines[i][j] == 'O')
                            map[i].Add(2);
                        else
                            map[i].Add(0);
                    }
                }
            }
            public void Cycle()
            {
                TiltNorth();
                TiltWest();
                TiltSouth();
                TiltEast();
            }
            public long LoadScore()
            {
                long toreturn = 0;
                for(int i = 0; i < map.Count; i++)
                {
                    for(int j = 0; j < map[i].Count; j++)
                    {
                        //Console.Write(map[i][j]);
                        if (map[i][j] == 2)
                        {
                            toreturn += map.Count - i;
                        }
                    }
                   // Console.WriteLine();
                }

                return toreturn;
            }
            public void TiltNorth()
            {
                for(int i = 1; i < map.Count; i++)
                {
                    for(int j = 0; j < map[i].Count; j++)
                    {
                        if (map[i][j] == 2) 
                        {
                            int ci = i;
                            int cj = j;
                            while (ci - 1 >= 0 && map[ci - 1][cj] == 0)
                            {
                                (map[ci - 1][cj], map[ci][cj]) = (map[ci][cj], map[ci - 1][cj]);
                                ci--;
                            }
                        }                   
                    }
                }
            }
            public void TiltSouth()
            {
                for (int i = map.Count - 2; i >= 0; i--)
                {
                    for (int j = 0; j < map[i].Count; j++)
                    {
                        if (map[i][j] == 2)
                        {
                            int ci = i;
                            int cj = j;
                            while (ci + 1 < map.Count && map[ci + 1][cj] == 0)
                            {
                                (map[ci + 1][cj], map[ci][cj]) = (map[ci][cj], map[ci + 1][cj]);
                                ci++;
                            }
                        }
                    }
                }
            }
            public void TiltEast() // tilt right
            {
                for (int j = map[0].Count - 2; j >= 0; j--)
                {
                    for(int i = 0; i < map.Count; i++)
                    {
                        if (map[i][j] == 2)
                        {
                            int ci = i;
                            int cj = j;
                            while (cj + 1 < map[0].Count && map[ci][cj + 1] == 0)
                            {
                                (map[ci][cj + 1], map[ci][cj]) = (map[ci][cj], map[ci][cj + 1]);
                                cj++;
                            }
                        }
                    }                           
                }
            }
            public void TiltWest() // tilt left
            {
                for (int j = 1; j < map[0].Count; j++)
                {
                    for (int i = 0; i < map.Count; i++)
                    {
                        if (map[i][j] == 2)
                        {
                            int ci = i;
                            int cj = j;
                            while (cj - 1 >= 0 && map[ci][cj - 1] == 0)
                            {
                                (map[ci][cj - 1], map[ci][cj]) = (map[ci][cj], map[ci][cj - 1]);
                                cj--;
                            }
                        }
                    }
                }
            }
        }
        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            RockMap m = new RockMap(lines);

            List<long> ss = new List<long>();

            int firstrepeat = -1;
            int len = -1;

            for(int i = 0; i < 300; i++)
            {
                m.Cycle();                                             
                ss.Add(m.LoadScore());
            }

            for(int i = 0; i < ss.Count; i++)
            {               
                for (int k = 2; k < ss.Count / 2; k++)
                {
                    bool ok = true;
                    int times = 0;

                    while (ok)
                    {
                        for (int h = 0; h < k; h++)
                        {
                            try
                            {
                                if (ss[i + h] != ss[i + (k * (times + 1)) + h])
                                {
                                    ok = false;
                                }
                                if (!ok)
                                    break;
                            }
                            catch (Exception)
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                            times++;                           
                    }
                    if (times >= 6)
                    {
                        firstrepeat = i;
                        len = k;
                        break;
                    }
                }
                if (firstrepeat != -1)
                    break;
            }

            RockMap final = new RockMap(lines);
            for(int i = 0; i < firstrepeat; i++)
                final.Cycle();

            long total = 1_000_000_000;
            total -= firstrepeat;
            total %= len;

            for (int i = 0; i < total; i++)
                final.Cycle();

            toreturn = final.LoadScore();

            return toreturn.ToString();
        }
    }
}
