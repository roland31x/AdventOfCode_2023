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
                string springss = lines[i].Split(' ')[0];
                string infos = lines[i].Split(' ')[1];

                List<int> springs = new List<int>();
                int qcount = 0;

                for(int j = 0; j < springss.Length; j++)
                {
                    if (springss[j] == '.')
                        springs.Add(0);
                    else if (springss[j] == '#')
                        springs.Add(1);
                    else
                    {
                        springs.Add(2);
                        qcount++;
                    }
                        
                }
                
                List<int> groups = new List<int>();

                string[] tok = infos.Split(',');
                foreach(string t in tok)
                    groups.Add(int.Parse(t));

                for(int t = 0; t < Math.Pow(2,qcount); t++)
                {
                    Stack<int> s = new Stack<int>();
                    int nr = t;
                    while(nr > 0)
                    {
                        s.Push(nr % 2);
                        nr = nr / 2;
                    }
                    while (s.Count < qcount)
                        s.Push(0);

                    List<int> tocheck = new List<int>(springs);
                    for(int k = 0; k < springs.Count; k++)
                    {
                        if (springs[k] == 2)
                            tocheck[k] = s.Pop();
                    }

                    int localcount = 0;
                    List<int> check = new List<int>();
                    bool ok = true;
                    for(int k = 0; k < tocheck.Count; k++)
                    {
                        if (tocheck[k] == 1)
                        {
                            localcount++;
                        }
                        else if(localcount > 0)
                        {
                            check.Add(localcount);
                            localcount = 0;
                        }
                    }
                    if (localcount > 0)
                        check.Add(localcount);

                    if (check.Count != groups.Count)
                        ok = false;
                    else
                    {
                        for(int k = 0; k < groups.Count; k++)
                        {
                            if (groups[k] != check[k])
                            {
                                ok = false;
                                break;
                            }
                        }
                    }

                    if (ok)
                        toreturn++;
                }
                

            }

            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                string springss = lines[i].Split(' ')[0];
                springss = springss + "?" + springss + "?" + springss + "?" + springss + "?" + springss;
                springss += ".";

                string infos = lines[i].Split(' ')[1];
                infos = infos + "," + infos + "," + infos + "," + infos + "," + infos;

                List<int> springs = new List<int>();
                int qcount = 0;

                for (int j = 0; j < springss.Length; j++)
                {
                    if (springss[j] == '.')
                        springs.Add(0);
                    else if (springss[j] == '#')
                        springs.Add(1);
                    else
                    {
                        springs.Add(2);
                        qcount++;
                    }

                }

                List<int> groups = new List<int>();

                string[] tok = infos.Split(',');
                foreach (string t in tok)
                    groups.Add(int.Parse(t));

                Dictionary<(int, int, int),long> cache = new Dictionary<(int, int, int),long>();

                toreturn += DFS(0, 0, 0, cache, springss, groups);
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
                {
                    local += DFS(position + 1, currentgroup, grouplen + 1, cache, springs, groups);
                }

                cache.Add((position,currentgroup,grouplen),local);
                return local;
            }
        }
    }
}
