using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.ComponentModel.Design;

namespace D12
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 12;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "this day sucked";
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
            for(int i = 0; i < lines.Count; i++)
            {
                string springs = lines[i].Split(' ')[0] + '.';

                List<int> groups = new List<int>();
                lines[i].Split(' ')[1].Split(',').ToList().ForEach(x => groups.Add(int.Parse(x)));

                Dictionary<(int, int, int), long> cache = new Dictionary<(int, int, int), long>();

                toreturn += DFS(0, 0, 0, cache, springs, groups);
            }

            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                string springs = lines[i].Split(' ')[0];
                springs = springs + "?" + springs + "?" + springs + "?" + springs + "?" + springs;
                springs += ".";

                string infos = lines[i].Split(' ')[1];
                infos = infos + "," + infos + "," + infos + "," + infos + "," + infos;

                List<int> groups = new List<int>();
                infos.Split(',').ToList().ForEach(x => groups.Add(int.Parse(x)));

                Dictionary<(int, int, int),long> cache = new Dictionary<(int, int, int),long>();

                toreturn += DFS(0, 0, 0, cache, springs, groups);
            }

            return toreturn.ToString();
        }
        static long DFS(int position, int currentgroup, int grouplen, Dictionary<(int,int,int),long> cache, string springs, List<int> groups)
        {
            if (cache.ContainsKey((position, currentgroup, grouplen)))
                return cache[(position, currentgroup, grouplen)];
            else
            {
                if (position == springs.Length)
                {
                    if (currentgroup == groups.Count && grouplen == 0)
                        return 1;
                    return 0;
                }

                long local = 0;
                if (springs[position] == '.' || springs[position] == '?')
                {
                    if (grouplen == 0)
                        local += DFS(position + 1, currentgroup, grouplen, cache, springs, groups);
                    else if (currentgroup < groups.Count && grouplen == groups[currentgroup])
                        local += DFS(position + 1, currentgroup + 1, 0, cache, springs, groups);
                }

                if (springs[position] == '#' || springs[position] == '?')
                    local += DFS(position + 1, currentgroup, grouplen + 1, cache, springs, groups);

                cache.Add((position,currentgroup,grouplen),local);
                return local;
            }
        }
    }
}
