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
                //Console.WriteLine(string.Format("Graph has {0} vertices, {1} edges, begins on {2} and ends on {3}:", graph.vertCount, graph.edgeCount, graph.startVert, graph.endVert));
                int res = AlmostShortestPath(graph, graph.startVert, graph.endVert);
                writer.WriteLine(res);
                //Console.WriteLine(string.Format("Almost shortest path of this graph is of length {0}.\n", res));
                Console.WriteLine(res);
            }

            writer.Close();
            //Console.Write("Done! Press return to exit.");
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
            // Run Dijkstra's algo
            var res = Dijkstra(graph, start, destination);
            //Console.WriteLine(string.Format("Shortest path on this graph is {0}.", res.Item1[destination]));

            // Get a list of all edges to remove, that is every shortest path
            List<(int, int)> edgesToRemove = new List<(int, int)>();
            if (destination < res.Item2.Length)
            {
                for (int i = 0; i < res.Item2.Length; i++)
                {
                    if (res.Item2[i].id == destination)
                    {
                        res.Item2[i].ToEdges(edgesToRemove);
                        break;
                    }
                }
            }
            else
            {
                return -1;
            }

            // Remove the edges and run Dijkstra again
            graph.RemoveEdges(edgesToRemove);
            res = Dijkstra(graph, start, destination);
            return res.Item1[destination];
        }

        static public (int[], Vertex[]) Dijkstra(WeightedGraph graph, int start, int destination)
        {
            const int INFINITY = Int32.MaxValue;

            int[] pathCounts = new int[graph.vertCount];
            int[] dist = new int[graph.vertCount];

            Tools.PriorityQueue<int> pq = new Tools.PriorityQueue<int>();
            pq.Enqueue(start, 0);

            for (int v = 0; v < graph.vertCount; v++)
            {
                if (v != start)
                {
                    dist[v] = INFINITY;
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
                        graph.GetVertex(v).parents.Clear();
                        graph.GetVertex(v).parents.Add(graph.GetVertex(u));
                        pq.ChangePriority(v, alt);
                    }
                    else if (alt == dist[v])
                    {
                        graph.GetVertex(v).parents.Add(graph.GetVertex(u));
                    }
                }
            }

            for (int i = 0; i < dist.Length; i++)
                dist[i] = dist[i] == Int32.MaxValue || dist[i] < 0 ? -1 : dist[i];

            return (dist, graph.vertices.ToArray());
        }

        public class WeightedGraph
        {
            public int vertCount, edgeCount, startVert, endVert;
            
            public Dictionary<(int, int), int> edges;
            public List<Vertex> vertices;

            public WeightedGraph(string input, uint lineNumber)
            {
                int vertCount = -1;
                int edgeCount = -1;
                int startVert = -1;
                int endVert = -1;
                this.edges = new Dictionary<(int, int), int>();
                this.vertices = new List<Vertex>();

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
                        if (!ContainsVertex(v1))
                        {
                            vertices.Add(new Vertex(v1));
                        }
                        if (!ContainsVertex(v2))
                        {
                            vertices.Add(new Vertex(v2));
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

            public List<int> GetParents(int vertex)
            {
                List<int> parents = new List<int>();
                foreach (var edge in this.edges)
                {
                    if (edge.Key.Item2 == vertex)
                    {
                        parents.Add(edge.Key.Item1);
                    }
                }
                return parents;
            }

            public Vertex GetVertex(int u)
            {
                foreach (Vertex v in this.vertices)
                {
                    if (v.id == u)
                    {
                        return v;
                    }
                }
                return null;
            }

            public bool ContainsVertex(int u)
            {
                foreach (Vertex v in this.vertices)
                {
                    if (v.id == u)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public class Vertex
        {
            public int id;
            public List<Vertex> parents = new List<Vertex>();

            public Vertex(int id)
            {
                this.id = id;
            }

            public override string ToString()
            {
                return "{" + this.id + "}";
            }

            public void ToEdges(List<(int, int)> edges)
            {
                if (edges == null)
                {
                    edges = new List<(int, int)>();
                }
                
                foreach (Vertex parent in this.parents)
                {
                    if (parent.parents.Count > 0)
                    {
                        parent.ToEdges(edges);
                    }
                    if (!edges.Contains((parent.id, this.id)))
                    {
                        edges.Add((parent.id, this.id));
                    }
                    
                }
            }
        }
    }
}
