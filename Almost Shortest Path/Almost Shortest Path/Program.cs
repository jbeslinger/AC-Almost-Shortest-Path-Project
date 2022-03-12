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
            #region My original file-based input
            /*if (args.Length != 1)
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
            Console.ReadLine();*/
            #endregion

            #region The much faster Beecrowd input method
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "0 0" || line == null)
                {
                    break;
                }
                else
                {
                    var graphInfo1 = line.Split(' ');
                    var graphInfo2 = Console.ReadLine().Split(' ');

                    WeightedGraph graph = new WeightedGraph
                        (
                        Int32.Parse(graphInfo1[0]),
                        Int32.Parse(graphInfo1[1]),
                        Int32.Parse(graphInfo2[0]),
                        Int32.Parse(graphInfo2[1])
                        );

                    for (int i = 0; i < graph.edgeCount; i++)
                    {
                        var graphInfo3 = Console.ReadLine().Split(' ');
                        graph.AddEdge
                            (
                            Int32.Parse(graphInfo3[0]),
                            Int32.Parse(graphInfo3[1]),
                            Int32.Parse(graphInfo3[2])
                            );
                    }

                    Console.WriteLine(graph.AlmostShortestPath());
                }
            }
            #endregion
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
