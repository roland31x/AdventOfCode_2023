using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D06
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 6;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "cookiepls";
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
            long toreturn = 1;

            List<int> times = new List<int>();
            string timesstring = lines[0].Split(':')[1].Trim();
            string[] timetok = timesstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string t in timetok)
                times.Add(int.Parse(t));

            List<int> lengths = new List<int>();
            string lengthsstring = lines[1].Split(':')[1].Trim();
            string[] lentok = lengthsstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string l in lentok)
                lengths.Add(int.Parse(l));

            for(int i = 0; i < times.Count; i++)
            {
                int time = times[i];
                int len = lengths[i];

                int sc = 0;
                for(int t = 0; t <= time; t++)
                {
                    int local = t * (time - t);
                    if (local > len)
                        sc++;
                }
                if(sc > 0)
                    toreturn *= sc;
            }

            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            List<int> times = new List<int>();
            string timesstring = lines[0].Split(':')[1].Trim();
            string[] timetok = timesstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            long fulltime = 0;
            string res = "";
            foreach (string t in timetok)
                res += t;
            fulltime = long.Parse(res);

            List<int> lengths = new List<int>();
            string lengthsstring = lines[1].Split(':')[1].Trim();
            string[] lentok = lengthsstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            long fulllen = 0;
            string resl = "";
            foreach (string l in lentok)
                resl += l;
            fulllen = long.Parse(resl);

            long left = 0;
            bool leftok = true;
            while (leftok)
            {
                long local = left * (fulltime - left);
                if (local >= fulllen)
                {
                    leftok = false;
                    break;
                }                   
                left++;
            }

            long right = fulltime;
            bool rightok = true;
            while (rightok)
            {
                long local = right * (fulltime - right);
                if (local >= fulllen)
                {
                    rightok = false;
                    break;
                }
                right--;
            }

            toreturn = right - left + 1;

            return toreturn.ToString();
        }
    }
}
