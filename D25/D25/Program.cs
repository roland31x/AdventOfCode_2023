using AOCOut = AOCApi.OutputSubmitter;
using AOCIn = AOCApi.InputGetter;
using System.ComponentModel;

namespace D25
{
    internal class Program
    {
        static int YEAR = 2023;
        static int DAY = 25;
        public readonly static string InputPath = @"..\..\..\input.txt";
        public readonly static string SeshCookie = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        public static void Main(string[] args)
        {
            List<string> lines = AOCIn.GetInput(InputPath, YEAR, DAY, SeshCookie);

            string part1 = Part1(lines);
            Console.WriteLine("Solution: " + part1);

            Console.WriteLine("Submit? Y/N");
            string command = Console.ReadLine();
            if (command.ToUpper() == "Y")
            {
                string resp = AOCOut.Submit(part1, YEAR, DAY, 1, SeshCookie);
                Console.WriteLine(resp);
            }
        }

        private static string Part1(List<string> lines)
        {
            List<Con> cons = new List<Con>();
            foreach (string line in lines)
            {
                string left = line.Split(':')[0].Trim();
                string[] right = line.Split(':')[1].Trim().Split(' ');
                Component leftcomp = Component.GetComponent(left);
                foreach(string tok in right)
                {
                    Component rightcomp = Component.GetComponent(tok);
                    rightcomp.Connections.Add(leftcomp);
                    leftcomp.Connections.Add(rightcomp);
                    Con c = new Con(rightcomp, leftcomp);
                    cons.Add(c);
                }
            }

            List<Component> components = Component.components.Values.ToList();            
            
            foreach(Con c in cons)
            {
                Component a = c.A;
                Component b = c.B;
                
                Queue<Component> q = new Queue<Component>();
                a.mark = 1;
                q.Enqueue(a);
                while (true)
                {
                    Component deq = q.Dequeue();
                    if(deq == b)
                        break;
                    foreach(Component n in deq.Connections)
                    {
                        if (n == b && deq == a)
                            continue;
                        if (n.mark != 0)
                            continue;
                        n.mark = deq.mark + 1;
                        q.Enqueue(n);
                    }
                }
                int dist = b.mark;
                c.BestDist = dist;

                components.ForEach(c => c.mark = 0);
            }

            cons.Sort((x,y) => -1 * x.BestDist.CompareTo(y.BestDist));

            for(int i = 0; i < 3; i++)
            {
                Con toremove = cons[i];

                toremove.A.Connections.Remove(toremove.B);
                toremove.B.Connections.Remove(toremove.A);
            }

            Queue<Component> gr1 = new Queue<Component>();
            Component start = components.First();
            start.mark = 1;
            gr1.Enqueue(start);
            while (gr1.Any())
            {
                Component deq = gr1.Dequeue();
                foreach (Component n in deq.Connections)
                {
                    if (n.mark != 0)
                        continue;
                    n.mark = 1;
                    gr1.Enqueue(n);
                }
            }

            return (components.Where(x => x.mark == 0).Count() * components.Where(x => x.mark == 1).Count()).ToString();
        }
        public class Con
        {
            public Component A;
            public Component B;
            public int BestDist = -1;
            public Con(Component a, Component b)
            {
                A = a;
                B = b;
            }
            public override string ToString()
            {
                return A.Name + " - " + B.Name;
            }
        }
        public class Component
        {
            public static Dictionary<string,Component> components = new Dictionary<string,Component>();
            public string Name;
            public int mark = 0;
            public List<Component> Connections = new List<Component>();
            public Component(string name)
            {
                Name = name;
                components.Add(Name, this);  
            }
            public override string ToString()
            {
                return Name + ":" + Connections.Count;
            }
            public static Component GetComponent(string name)
            {
                if (components.ContainsKey(name))
                    return components[name];
                else
                    return new Component(name);
            }
        }
    }
}
