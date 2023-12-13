using System.Data;
using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D13
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 13;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "mirror mirror on the wall, why is my code uglier than my arse";
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

            int idx = 0;
            List<Mirror> ms = new List<Mirror>();
            Mirror current = new Mirror();
            while(idx < lines.Count)
            {
                if (lines[idx] == "")
                {
                    ms.Add(current);
                    current = new Mirror();
                }
                else
                {
                    current.Parts.Add(lines[idx].Replace("#","1").Replace(".","0"));
                }               
                idx++;
            }
            ms.Add(current);

            ms.ForEach(x => toreturn += x.Score());

            return toreturn.ToString();
        }
        public class Mirror
        {
            public List<string> Parts = new List<string>();
            int h => Parts.Count;
            int w => Parts[0].Length;
            int horzreflection = -1;
            int vertreflection = -1;
            

            public long Score()
            {
                int left = -1;
                bool found = false;
                for(int widx = 0; widx < w - 1; widx++)
                {
                    bool samecol = true;
                    for(int i = 0; i < h; i++)
                    {
                        if (Parts[i][widx] != Parts[i][widx + 1])
                        {
                            samecol = false;
                            break;
                        }                         
                    }
                    if (!samecol)
                        continue;
                    else
                    {
                        left = widx;

                        int l = left;
                        int r = left + 1;
                        try
                        {
                            while (true)
                            {
                                l--;
                                r++;
                                for (int i = 0; i < h; i++)
                                {
                                    if (Parts[i][l] != Parts[i][r])
                                    {
                                        samecol = false;
                                        break;
                                    }
                                }
                                if (!samecol)
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            found = true;
                            horzreflection = left;
                        }
                        if (found)
                            return left + 1;
                    }
                }


                for (int hidx = 0; hidx < h - 1; hidx++)
                {
                    bool samerow = true;
                    for (int i = 0; i < w; i++)
                    {
                        if (Parts[hidx][i] != Parts[hidx + 1][i])
                        {
                            samerow = false;
                            break;
                        }
                    }
                    if (!samerow)
                        continue;
                    else
                    {
                        left = hidx;

                        int l = left;
                        int r = left + 1;
                        try
                        {
                            while (true)
                            {
                                l--;
                                r++;
                                for (int i = 0; i < w; i++)
                                {
                                    if (Parts[l][i] != Parts[r][i])
                                    {
                                        samerow = false;
                                        break;
                                    }
                                }
                                if (!samerow)
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            found = true;
                            vertreflection = left;
                        }
                        if (found)
                            return (left + 1) * 100;
                    }
                }
                return 0;
                
            }
            public long Score2()
            {
                int left = -1;
                bool found = false;
                for (int widx = 0; widx < w - 1; widx++)
                {
                    bool fixedone = false;
                    bool samecol = true;
                    for (int i = 0; i < h; i++)
                    {
                        if (Parts[i][widx] != Parts[i][widx + 1])
                        {
                            if (!fixedone)
                            {
                                fixedone = true;
                                continue;
                            }
                            samecol = false;
                            break;
                        }
                    }
                    if (!samecol)
                        continue;
                    else
                    {
                        if (widx == horzreflection)
                            continue;
                        left = widx;

                        int l = left;
                        int r = left + 1;
                        try
                        {
                            while (true)
                            {
                                l--;
                                r++;
                                int diff = 0;

                                for (int i = 0; i < h; i++)                                   
                                    if (Parts[i][l] != Parts[i][r])
                                        diff++;
                                    
                                if (diff != 0)
                                {
                                    if (diff == 1 && !fixedone)
                                        fixedone = true;
                                    else
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            found = true;
                        }
                        if (found)
                            return left + 1;
                    }
                }


                for (int hidx = 0; hidx < h - 1; hidx++)
                {
                    bool fixedone = false;
                    bool samerow = true;
                    for (int i = 0; i < w; i++)
                    {
                        if (Parts[hidx][i] != Parts[hidx + 1][i])
                        {
                            if (!fixedone)
                            {
                                fixedone = true;
                                continue;
                            }
                            samerow = false;
                            break;
                        }
                    }
                    if (!samerow)
                        continue;
                    else
                    {
                        if (hidx == vertreflection)
                            continue;
                        left = hidx;

                        int l = left;
                        int r = left + 1;
                        try
                        {                            
                            while (true)
                            {
                                l--;
                                r++;
                                int diff = 0;
                                for (int i = 0; i < w; i++)
                                    if (Parts[l][i] != Parts[r][i])
                                        diff++;

                                if (diff != 0)
                                {
                                    if (diff == 1 && !fixedone)
                                        fixedone = true;
                                    else
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            found = true;
                        }
                        if (found)
                            return (left + 1) * 100;
                    }
                }
                return 0;
            }
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            int idx = 0;
            List<Mirror> ms = new List<Mirror>();
            Mirror current = new Mirror();
            while (idx < lines.Count)
            {
                if (lines[idx] == "")
                {
                    ms.Add(current);
                    current = new Mirror();
                }
                else
                {
                    current.Parts.Add(lines[idx].Replace("#", "1").Replace(".", "0"));
                }
                idx++;
            }
            ms.Add(current);

            ms.ForEach(x => x.Score());
            ms.ForEach(x => toreturn += x.Score2());

            return toreturn.ToString();
        }
    }
}
