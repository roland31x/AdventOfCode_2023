using AOCApi;
using System.Collections.Generic;
using System.Net;
using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;

namespace D20
{
    public class Program
    {
        // dont forget to change day
        static int YEAR = 2023;
        static int DAY = 20;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "fuck lcm honestly, it doesn't really make sense";
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
            Dictionary<string, Module> modules = Module.modules;

            Module.Parse(lines);

            Queue<Module> q = new Queue<Module>();
            Module broadcaster = Module.GetModule("broadcaster");

            int counthi = 0;
            int countlo = 0;

            for(int t = 0; t < 1000; t++)
            {
                q.Enqueue(broadcaster);
                while (q.Any())
                {
                    Module m = q.Dequeue();
                    int currenttype = m.Type;
                    int currentpulse = 0;
                    if (m.q.Any())
                        currentpulse = m.q.Dequeue();    
                    
                    if (currentpulse == 0)
                        countlo++;
                    else if (currentpulse == 1)
                        counthi++;

                    if (currenttype == 0)
                    {
                        if (currentpulse == 1)
                            continue;

                        m.Value += 1;
                        m.Value %= 2;
                    }

                    foreach (Module tosend in m.Dest)
                    {
                        int pulsetosend = -1;
                        if (currenttype <= 0)
                            pulsetosend = m.Value;
                        else if (currenttype == 1)
                            pulsetosend = m.LastReceived.Min() == 0 ? 1 : 0;

                        q.Enqueue(tosend);
                        tosend.q.Enqueue(pulsetosend);
                        if (tosend.Type == 1)
                            tosend.LastReceived[tosend.From.IndexOf(m)] = pulsetosend;
                    }
                }
            }
         
            return (countlo * counthi).ToString();
        }
        public class Module
        {
            public static Dictionary<string,Module> modules = new Dictionary<string,Module>();

            public int Type = -1;
            public int Value = 0;
            
            public string Name;

            public Queue<int> q = new Queue<int>();

            public List<Module> Dest = new List<Module>();

            public List<Module> From = new List<Module>();
            public List<int> LastReceived = new List<int>();

            public Module(string name) 
            {
                Name = name;
                modules.Add(Name, this);                    
            }
            public override string ToString()
            {
                return Name;
            }
            public static Module GetModule(string name)
            {
                if (modules.ContainsKey(name))
                    return modules[name];
                else
                    return new Module(name);
            }
            public static void Parse(List<string> lines)
            {
                foreach (string line in lines)
                {
                    Module m = Module.GetModule(line.Split("->")[0].Replace("%", "").Replace("&", "").Trim());

                    if (line.Contains("%"))
                        m.Type = 0;
                    else if (line.Contains("&"))
                        m.Type = 1;

                    string[] dest = line.Split("->")[1].Trim().Split(",");
                    foreach (string n in dest)
                    {
                        m.Dest.Add(Module.GetModule(n.Trim()));
                    }
                }

                foreach (Module con in modules.Values.Where(x => x.Type == 1))
                {
                    foreach (Module m in modules.Values.Where(x => x.Dest.Contains(con)))
                    {
                        con.From.Add(m);
                        con.LastReceived.Add(0);
                    }
                }
            }
        }

        private static string Part2(List<string> lines)
        {
            Module.modules.Clear();

            Dictionary<string, Module> modules = Module.modules;

            Module.Parse(lines);

            Module important = modules.Values.First(x => x.Dest.Contains(Module.GetModule("rx"))); // should be a con

            Queue<Module> q = new Queue<Module>();
            Module broadcaster = Module.GetModule("broadcaster");

            long times = 0;
            int[] needed = new int[important.LastReceived.Count];
            bool done = false;
            int ct = 0;
            while(!done)
            {
                if (ct == needed.Length)
                    break;
                times++;
                q.Enqueue(broadcaster);
                while (q.Any())
                {
                    Module m = q.Dequeue();
                    int currenttype = m.Type;
                    int currentpulse = 0;
                    if (m.q.Any())
                        currentpulse = m.q.Dequeue();

                    if (currentpulse == 0) 
                    {
                        if (m.Name == "rx") // not happening btw
                        {
                            done = true;
                            break;
                        }
                    }

                    if (currenttype == 0)
                    {
                        if (currentpulse == 1)
                            continue;
                        m.Value += 1;
                        m.Value %= 2;
                    }

                    foreach (Module tosend in m.Dest)
                    {
                        int pulsetosend = -1;
                        if (currenttype <= 0)
                            pulsetosend = m.Value;
                        else if (currenttype == 1)
                            pulsetosend = m.LastReceived.Min() == 0 ? 1 : 0;
                        
                        q.Enqueue(tosend);
                        tosend.q.Enqueue(pulsetosend);
                        if (tosend.Type == 1)
                        {
                            tosend.LastReceived[tosend.From.IndexOf(m)] = pulsetosend;
                            if (tosend == important)
                            {
                                if (pulsetosend == 1)
                                {
                                    if (needed[tosend.From.IndexOf(m)] == 0)
                                    {
                                        needed[tosend.From.IndexOf(m)] = (int)times;
                                        ct++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            long toreturn = 1;
            for(int i = 0; i < needed.Length; i++)
                toreturn = lcm(toreturn, (long)needed[i]);

            return toreturn.ToString();
        }
        static long gcd(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return (a / gcd(a, b)) * b;
        }
    }
}
