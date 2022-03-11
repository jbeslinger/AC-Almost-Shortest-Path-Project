using Almost_Shortest_Path.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Almost_Shortest_Path
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide a file to the executable.");
                Console.ReadLine();
                return;
            }

            string path = @args[0];
            List<WeightedGraph> graphs = ParseGraphs(path);
            StreamWriter writer = new StreamWriter(@"output.txt");

            foreach (WeightedGraph graph in graphs)
            {
                int res = graph.AlmostShortestPath();
                Console.WriteLine(res);
                writer.WriteLine(res);
            }

            writer.Close();
            Console.Write("Done! Press return to exit.");
            Console.ReadLine();
        }

        static private List<WeightedGraph> ParseGraphs(string path)
        {
            List<WeightedGraph> graphs = new List<WeightedGraph>();
            string fileContent = File.ReadAllText(@path);
            fileContent = fileContent.Replace("\r", "");
            string[] lines = fileContent.Split("\n");

            StringBuilder sb = new StringBuilder();

            for (uint i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "0 0")
                {
                    break;
                }
                else
                {
                    uint edgeCount = (uint)Convert.ToInt32(lines[i].Split(" ")[1]);
                    for (uint j = i; j < i + edgeCount + 2; j++)
                    {
                        sb.Append(lines[j] + '\n');
                    }
                    graphs.Add(new WeightedGraph(sb.ToString().TrimEnd('\n')));
                    i += edgeCount + 1;
                    sb.Clear();
                }
            }

            return graphs;
        }
    }
}
