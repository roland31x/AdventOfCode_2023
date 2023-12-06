using AOCIn = AOCApi.InputGetter;
using AOCOut = AOCApi.OutputSubmitter;
namespace D05
{
    internal class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 5;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "cookie";
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

            List<long> seeds = new List<long>();
            string seedsstring = lines[0].Split(':')[1].Trim();
            foreach(string s in seedsstring.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)) 
            {
                seeds.Add(long.Parse(s));
            }
            List<List<Mapper>> mappers = new List<List<Mapper>>();
            for(int i = 1; i < lines.Count; i++)
            {
                if (lines[i] == "" || lines[i] == string.Empty)
                    continue;
                if (lines[i].Contains("map"))
                {
                    mappers.Add(new List<Mapper>());
                    continue;
                }
                long destrange = long.Parse(lines[i].Split(' ')[0].Trim());
                long sourcerange = long.Parse(lines[i].Split(' ')[1].Trim());
                long len = long.Parse(lines[i].Split(' ')[2].Trim());
                Mapper current = new Mapper(sourcerange,destrange,len);
                mappers.Last().Add(current);
            }

            foreach(long seed in seeds)
            {
                long location = seed;
                foreach(List<Mapper> transform in mappers)
                {
                    foreach(Mapper mapper in transform)
                    {
                        if(mapper.sourcerange <= location && mapper.sourcerange + mapper.length >= location)
                        {
                            location = location - mapper.sourcerange + mapper.destinationrange;
                            break;
                        }
                    }
                }
                if (location < toreturn)
                    toreturn = location;
            }

            return toreturn.ToString();
        }
        public class SeedRange 
        {
            public long start;
            public long len;
            public SeedRange(long start, long len)
            {
                this.start = start;
                this.len = len;
            }
        }

        public class Mapper
        {
            public long sourcerange;
            public long destinationrange;
            public long length;
            public Mapper(long sr, long dr, long len)
            {
                sourcerange = sr;
                destinationrange = dr;
                length = len;
            }
        }

        private static string Part2(List<string> lines)
        {
            long toreturn = long.MaxValue;

            List<SeedRange> seeds = new List<SeedRange>();
            string seedsstring = lines[0].Split(':')[1].Trim();
            string[] tokens = seedsstring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i += 2)
            {
                seeds.Add(new SeedRange(long.Parse(tokens[i]), long.Parse(tokens[i + 1])));
            }
            List<List<Mapper>> mappers = new List<List<Mapper>>();
            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i] == "" || lines[i] == string.Empty)
                    continue;
                if (lines[i].Contains("map"))
                {
                    mappers.Add(new List<Mapper>());
                    continue;
                }
                long destrange = long.Parse(lines[i].Split(' ')[0].Trim());
                long sourcerange = long.Parse(lines[i].Split(' ')[1].Trim());
                long len = long.Parse(lines[i].Split(' ')[2].Trim());
                Mapper current = new Mapper(sourcerange, destrange, len);
                mappers.Last().Add(current);
            }
            foreach (List<Mapper> transform in mappers)
            {
                List<SeedRange> nextgen = new List<SeedRange>();
                for (int i = 0; i < seeds.Count; i++)
                {
                    SeedRange seedr = seeds[i];
                    

                    foreach (Mapper mapper in transform)
                    {
                        if (mapper.sourcerange <= seedr.start && mapper.sourcerange + mapper.length >= seedr.start)
                        {
                            long newlen = 0;
                            if (mapper.length >= seedr.len)
                                newlen = seedr.len;
                            else
                                newlen = seedr.start - mapper.sourcerange + mapper.length;
                            SeedRange sr = new SeedRange(seedr.start - mapper.sourcerange + mapper.destinationrange, newlen);
                            nextgen.Add(sr);
                            seedr.start = sr.start + sr.len;
                            seedr.len = seedr.len - sr.len;                          
                        }
                        else if(mapper.sourcerange <= seedr.start + seedr.len && mapper.sourcerange + mapper.length >= seedr.start + seedr.len)
                        {
                            long newlen = seedr.start + seedr.len - mapper.sourcerange;
                            SeedRange sr = new SeedRange(mapper.destinationrange, newlen);
                            nextgen.Add(sr);
                            seedr.len = seedr.len - sr.len;
                        }
                        else if(seedr.start <= mapper.sourcerange && seedr.start + seedr.len >= mapper.sourcerange + mapper.length)
                        {
                            SeedRange sr = new SeedRange(mapper.destinationrange, mapper.length);
                            nextgen.Add(sr);
                            nextgen.Add(new SeedRange(mapper.destinationrange + mapper.length, seedr.start + seedr.len - (mapper.sourcerange + mapper.length)));
                            seedr.len = seedr.start + seedr.len - mapper.sourcerange;
                            break;
                        }
                    }
                    if (seedr.len != 0)
                        nextgen.Add(seedr);
                    
                }
                seeds = nextgen.Where(x => x.len > 0).ToList();
            }

            foreach(SeedRange s in seeds)
            {
                if(s.len > 0)
                    if (s.start < toreturn)
                        toreturn = s.start;
            }
            return toreturn.ToString();
        }
    }
}
