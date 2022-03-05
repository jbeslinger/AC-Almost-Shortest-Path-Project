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
                Console.WriteLine(string.Format("Graph has {0} vertices, {1} edges, begins on {2} and ends on {3}:", graph.vertCount, graph.edgeCount, graph.startVert, graph.endVert));
                int res = AlmostShortestPath(graph, graph.startVert, graph.endVert);
                writer.WriteLine(res);
                Console.WriteLine(string.Format("Almost shortest path of this graph is of length {0}.\n", res));
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
                    uint edgeCount = (uint) Convert.ToInt32(lines[i].Split(" ")[1]);
                    for (uint j = i; j < i + edgeCount + 2; j++)
                    {
                        sb.Append(lines[j] + '\n');
                    }
                    graphs.Add(new WeightedGraph(sb.ToString().TrimEnd('\n'), i));
                    i += edgeCount + 1;
                    sb.Clear();
                }
            }

            return graphs;
        }

        static public int AlmostShortestPath(WeightedGraph graph, int start, int destination)
        {
            var res = Dijkstra(graph, start, destination);
            return res.Item1[destination];
        }

        static public (int[], List<List<int>>[]) Dijkstra(WeightedGraph graph, int start, int destination)
        {
            const int INFINITY = Int32.MaxValue;
            const int UNDEFINED = -1;

            int[] pathCounts = new int[graph.vertCount];

            int[] dist = new int[graph.vertCount];
            List<List<int>>[] prev = new List<List<int>>[graph.vertCount];

            if (!graph.ContainsStartVert() || !graph.ContainsEndVert())
            {
                dist[0] = -1;
                return (dist, prev);
            }

            Tools.PriorityQueue<int> pq = new Tools.PriorityQueue<int>();
            pq.Enqueue(start, 0);

            foreach (var v in graph.vertices)
            {
                if (v != start)
                {
                    dist[v] = INFINITY;
                    prev[v] = new List<List<int>>();
                    prev[v].Add(new List<int>() { UNDEFINED });
                }
                pq.Enqueue(v, dist[v]);
            }

            while (!pq.IsEmpty())
            {
                int u = pq.Dequeue();
                foreach (int v in graph.GetNeighbors(u))
                {
                    int alt = dist[u] + graph.edges[(u, v)];
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v][pathCounts[v]].Clear();
                        prev[v][pathCounts[v]].Add(u);
                        pq.ChangePriority(v, alt);
                    }
                    else if (alt == dist[v])
                    {
                        dist[v] = alt;
                        prev[v].Add(new List<int>(prev[v][pathCounts[v]]));
                        pathCounts[v] += 1;
                        prev[v][pathCounts[v]].Add(u);
                    }
                }
            }

            return (dist, prev);
        }

        public struct WeightedGraph
        {
            public int vertCount, edgeCount, startVert, endVert;
            
            public Dictionary<(int, int), int> edges;
            public List<int> vertices;

            public WeightedGraph(string input, uint lineNumber)
            {
                int vertCount = -1;
                int edgeCount = -1;
                int startVert = -1;
                int endVert = -1;
                this.edges = new Dictionary<(int, int), int>();
                this.vertices = new List<int>();

                try
                {
                    var values = input.Split('\n');

                    vertCount = Convert.ToInt32(values[0].Split(' ')[0]);
                    edgeCount = Convert.ToInt32(values[0].Split(' ')[1]);
                    startVert = Convert.ToInt32(values[1].Split(' ')[0]);
                    endVert = Convert.ToInt32(values[1].Split(' ')[1]);

                    for (int i = 0; i < edgeCount; i++)
                    {
                        int v1, v2;
                        int w;

                        var row = values[i + 2].Split(' ');
                        v1 = Convert.ToInt32(row[0]);
                        v2 = Convert.ToInt32(row[1]);
                        w = Convert.ToInt32(row[2]);

                        edges.Add((v1, v2), w);
                    }

                    foreach (var edge in edges)
                    {
                        int v1, v2;
                        v1 = edge.Key.Item1;
                        v2 = edge.Key.Item2;
                        if (!vertices.Contains(v1))
                        {
                            vertices.Add(v1);
                        }
                        if (!vertices.Contains(v2))
                        {
                            vertices.Add(v2);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Your input string does not match the required format:\n'0 0'\n'0 0'\n'0 0 0'\n'0 0 0' ...");
                    Console.ReadLine();
                    Environment.Exit(1);
                }

                this.vertCount = vertCount;
                this.edgeCount = edgeCount;
                this.startVert = startVert;
                this.endVert = endVert;
            }

            public void RemoveEdges(List<(int, int)> edges)
            {
                foreach (var edge in edges)
                {
                    if (this.edges.Remove(edge))
                    {
                        this.edgeCount -= 1;
                    }
                }
            }

            public List<int> GetNeighbors(int vertex)
            {
                List<int> neighbors = new List<int>();
                foreach (var edge in this.edges)
                {
                    if (edge.Key.Item1 == vertex)
                    {
                        neighbors.Add(edge.Key.Item2);
                    }
                }
                return neighbors;
            }

            public bool ContainsVertex(int v)
            {
                return this.vertices.Contains(v);
            }

            public bool ContainsStartVert()
            {
                return this.vertices.Contains(startVert);
            }

            public bool ContainsEndVert()
            {
                return this.vertices.Contains(endVert);
            }
        }
    }
}
