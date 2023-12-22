using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace D22
{
    internal class Program
    {

        static int YEAR = 2023;
        static int DAY = 22;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "nice day";
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

                Thread.Sleep(6000);

                string resp2 = AOCOut.Submit(part2, YEAR, DAY, 2, SeshCookie);
                Console.WriteLine(resp2);
            }
        }

        private static string Part1(List<string> lines)
        {
            List<Brick> bricks = new List<Brick>();
            foreach (string line in lines)
                bricks.Add(new Brick(line));

            bricks.Sort((x,y) => x.ends[2].CompareTo(y.ends[2]));

            int[,,] map = new int[20, 20, 500];
            foreach (Brick b in bricks)
                b.SettleOn(map,bricks.IndexOf(b) + 1);

            foreach(Brick current in bricks)
            {
                List<int> supports = current.GetAboves(map);
                foreach(int suppidx in supports)
                {
                    bricks[suppidx].supportedby.Add(current);
                    current.supports.Add(bricks[suppidx]);
                }
            }

            int count = 0;
            foreach(Brick b in bricks)
            {
                bool candisintegrate = true;
                foreach(Brick supp in b.supports)
                {
                    if (!supp.supportedby.Except(new List<Brick>() { b }).Any())
                        candisintegrate = false;
                }
                if (candisintegrate)
                    count++;
            }

            return count.ToString();
        }
        public class Brick
        {
            public int[] starts = new int[3]; // X , Y , Z
            public int[] ends = new int[3];
            public bool Settled = false;
            public bool removed = false;
            public List<Brick> supports = new List<Brick>();
            public List<Brick> supportedby = new List<Brick>();
            public Brick(string line)
            {
                string[] start = line.Split('~')[0].Split(',');
                string[] end = line.Split('~')[1].Split(',');
                for(int i = 0; i < 3; i++)
                {
                    starts[i] = int.Parse(start[i]);
                    ends[i] = int.Parse(end[i]);
                }
            }
            public Brick(List<Brick> supports, List<Brick> supportedby)
            {
                this.supports = new List<Brick>(supports);
                this.supportedby = new List<Brick>(supportedby);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" x: ")
                    .Append(starts[0] == ends[0] ? starts[0] : $"{starts[0]} -> {ends[0]} ");
                sb.Append(" y: ")
                    .Append(starts[1] == ends[1] ? starts[1] : $"{starts[1]} -> {ends[1]} ");
                sb.Append(" z: ")
                    .Append(starts[2] == ends[2] ? starts[2] : $"{starts[2]} -> {ends[2]} ");
                return sb.ToString();
            }
            public void SettleOn(int[,,] map, int mark)
            {
                while (!Settled)
                {
                    bool ok = true;
                    if (starts[2] == 1)
                        break;
                    for(int x = starts[0]; x <= ends[0]; x++)
                    {
                        for(int y = starts[1]; y <= ends[1]; y++)
                        {
                            int z = starts[2];
                            int checkz = z - 1;
                            if (map[x, y, checkz] != 0)
                                ok = false;
                        }
                    }
                    if (!ok)
                        break;
                    starts[2]--;
                    ends[2]--;
                }
                Settled = true;
                for (int x = starts[0]; x <= ends[0]; x++)
                {
                    for (int y = starts[1]; y <= ends[1]; y++)
                    {
                        for (int z = starts[2]; z <= ends[2]; z++)
                        {
                            map[x, y, z] = mark;
                        }
                    }
                }
            }
            public List<int> GetAboves(int[,,] map)
            {
                HashSet<int> hs = new HashSet<int>();

                for (int x = starts[0]; x <= ends[0]; x++)
                {
                    for (int y = starts[1]; y <= ends[1]; y++)
                    {
                        int z = ends[2];
                        int checkz = z + 1;
                        if (map[x, y, checkz] != 0)
                            hs.Add(map[x, y, checkz]);
                    }
                }


                List<int> toreturn = new List<int>();
                while (hs.Any())
                {
                    toreturn.Add(hs.First() - 1);
                    hs.Remove(hs.First());
                }                               

                return toreturn;
            }
        }

        private static string Part2(List<string> lines)
        {
            List<Brick> bricks = new List<Brick>();
            foreach (string line in lines)
                bricks.Add(new Brick(line));

            bricks.Sort((x, y) => x.ends[2].CompareTo(y.ends[2]));

            int[,,] map = new int[20, 20, 500];
            foreach (Brick b in bricks)
                b.SettleOn(map, bricks.IndexOf(b) + 1);

            foreach (Brick current in bricks)
            {
                List<int> supports = current.GetAboves(map);
                foreach (int suppidx in supports)
                {
                    bricks[suppidx].supportedby.Add(current);
                    current.supports.Add(bricks[suppidx]);
                }
            }

            long toreturn = 0;

            foreach(Brick b in bricks)
            {
                RemoveBrick(b, bricks);

                toreturn += bricks.Where(x => x.removed).Count() - 1;

                bricks.ForEach(x => x.removed = false);
            }
                

            return toreturn.ToString();
        }
        static void RemoveBrick(Brick b, List<Brick> bricks)
        {
            b.removed = true;
            foreach (Brick supp in b.supports)
            {
                if (!supp.supportedby.Where(x => !x.removed).Any())
                {
                    RemoveBrick(supp, bricks);
                }
            }
        }
    }
}
