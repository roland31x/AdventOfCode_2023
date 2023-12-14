using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;

namespace D15
{
    public class Program
    {
        static int YEAR = 2023;
        static int DAY = 15;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "YOUR COOKIE HERE";
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

                //Thread.Sleep(100);

                //string resp2 = AOCOut.Submit(part2, YEAR, DAY, 2, SeshCookie);
                //Console.WriteLine(resp2);
            }
        }

        private static string Part1(List<string> lines)
        {
            long toreturn = 0;



            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            return "";
        }
    }
}
