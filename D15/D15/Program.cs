using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;

namespace D15
{
    public class Program
    {
        static int YEAR = 2023;
        static int DAY = 15;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "cooooooooooooookieeeeeeeee";
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

            string line = lines[0];
            string[] coms = line.Split(',');
            foreach(string com in coms)
            {
                long val = 0;
                for(int i = 0; i < com.Length; i++)
                {
                    val += com[i];
                    val *= 17;
                    val %= 256;
                }
                toreturn += val;
            }

            return toreturn.ToString();
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = 0;

            List<List<Lens>> boxes = new List<List<Lens>>();
            for (int i = 0; i < 256; i++)
                boxes.Add(new List<Lens>());

            string line = lines[0];
            string[] coms = line.Split(',');
            foreach (string com in coms)
            {
                if (com.Contains('='))
                {
                    int val = 0;
                    string label = com.Split('=')[0];
                    long value = long.Parse(com.Split('=')[1]);
                    for (int i = 0; i < label.Length; i++)
                    {
                        val += label[i];
                        val *= 17;
                        val %= 256;
                    }
                    Lens l = new Lens(label, value);
                    if (boxes[val].Where(x => x.ID == label).Any())
                    {
                        Lens toreplace = boxes[val].First(x => x.ID == label);
                        int addidx = boxes[val].IndexOf(toreplace);
                        boxes[val].Remove(toreplace);
                        boxes[val].Insert(addidx, l);
                    }
                    else
                        boxes[val].Add(l);
                    
                }
                else
                {
                    string label = com.Split('-')[0];
                    foreach (List<Lens> lenses in boxes)
                        if(lenses.Where(x => x.ID == label).Any())
                            lenses.Remove(lenses.First(x => x.ID == label));
                }
                
            }

            boxes.ForEach((list) =>
            {
                long result = 0;
                long index = boxes.IndexOf(list) + 1;
                for(int i = 0; i < list.Count; i++)
                    result += index * (i + 1) * list[i].Value;

                toreturn += result;
            });

            return toreturn.ToString();
           
        }
        public class Lens
        {
            public string ID;
            public long Value;
            public Lens(string name, long val) 
            {
                ID = name;
                Value = val;
            }
        }
    }
}
