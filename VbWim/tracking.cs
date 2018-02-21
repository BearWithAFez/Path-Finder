using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbWim
{
    class tracking
    {
        Dictionary<string, string> terrain = new Dictionary<string, string>();
        bool isRunning = true;
        List<string> results = new List<string>();
        public void init()
        {
            // Print header, Read file
            printHeader();
            if (tryReadTracking())
            {
                do
                {
                    string input = askUserInput();
                    input.ToUpper();
                    if (checkInput(input))
                    {
                        if(input.Contains("-")) recursivePath(input);
                    }
                    else isRunning = false;
                } while (isRunning);
            }
            printFooter();
        }

        private void recursivePath(string input)
        {
            string[] addres = input.Split('-');
            if (!terrain.ContainsKey(addres[0]) || !terrain.ContainsKey(addres[1]))
            {
                Console.WriteLine("Invalid address. Please retry...");
                return;
            }
            recursiveMeth(addres[0], addres[1], "");

            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
            results.Clear();
        }

        private void recursiveMeth(string loc, string goal, string path)
        {
            path += loc + " - ";
            if (loc.Equals(goal))
            {
                results.Add(path.Substring(0, path.Length - 3));
            }
            else
            {
                var x = terrain[loc].Split(',');
                foreach (string item in x)
                {
                    if (!path.Contains(item))
                    {
                        recursiveMeth(item, goal, path);
                    }
                }
            }
            path = path.Substring(0, path.Length - (3 + loc.Length));
        }

        private string askUserInput()
        {
            Console.Write("Enter start & destination as <start>-<destination>... (<enter> to finish): ");
            return Console.ReadLine();
        }
        

        private bool checkInput(string input)
        {
            if((input.Trim().ToUpper() == "STOP")||(input.Trim().ToUpper() == "EXIT"))
            {
                return false;
            }
            return true;
        }

        private void printFooter()
        {
            Console.WriteLine("Thank you for using this program...");
            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
        }

        private bool tryReadTracking()
        {
            try
            {
                //File inlezen
                string[] temp = System.IO.File.ReadAllLines(@".\Resources\Terrain.txt");

                //Filter input
                List<string> lines = new List<string>();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(temp[i])) lines.Add(temp[i].Replace(" ", string.Empty).ToUpper());
                }

                foreach (var item in lines)
                {
                    string[] x = item.Split('-');
                    if (terrain.ContainsKey(x[0])) terrain[x[0]] += "," + x[1];
                    else terrain.Add(x[0], x[1]);

                    string[] y = x[1].Split(',');
                    foreach (var line in y)
                    {
                        if (terrain.ContainsKey(line)) terrain[line] += "," + x[0];
                        else terrain.Add(line, x[0]);
                    }
                }
                
                foreach (string key in terrain.Keys.ToList())
                {
                    string[] x = terrain[key].Split(',');
                    x = x.Distinct().ToArray();
                    terrain[key] = "";
                    for (int i = 0; i < x.Length-1; i++)
                    {
                        terrain[key] += x[i] + ",";
                    }
                    terrain[key] += x[x.Length - 1];

                    Console.WriteLine(key + " - " + terrain[key]);
                }
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Troubles with the input file...");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        private void printHeader()
        {
            Console.WriteLine("*****************************************");
            Console.WriteLine("* Opgave 02: Pathfinder -- Wim Verlinde *");
            Console.WriteLine("*****************************************\r\n");
        }
    }
}