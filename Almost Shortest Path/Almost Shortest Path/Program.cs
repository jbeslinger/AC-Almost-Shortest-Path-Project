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

            List<WeightedGraph> graphs = ParseGraphs(args[0]);

            foreach (WeightedGraph graph in graphs)
            {
                Console.WriteLine(AlmostShortestPath(graph, graph.startVert, graph.endVert));
            }

            Console.ReadLine();
        }

        static private List<WeightedGraph> ParseGraphs(string path)
        {
            List<WeightedGraph> graphs = new List<WeightedGraph>();
            string fileContent = File.ReadAllText(@path);
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
                        sb.Append(lines[j].Replace('\r', '\n'));
                    }
                    graphs.Add(ToGraph(sb.ToString().TrimEnd('\n'), i));
                    i += edgeCount + 1;
                    sb.Clear();
                }
            }

            return graphs;
        }

        static List<Vertex> edgesToDelete = new List<Vertex>();
        static public int AlmostShortestPath(WeightedGraph graph, int start, int destination)
        {
            edgesToDelete.Clear();
            int shortestPath = Dijkstra(graph, start, destination);

            if (shortestPath == -1 || shortestPath == 0)
            {
                return -1;
            }

            int almostShortestPath = 0;
            while (true)
            {
                graph.RemoveEdges(edgesToDelete);

                almostShortestPath = Dijkstra(graph, start, destination);

                if (shortestPath != almostShortestPath)
                {
                    break;
                }
            }
            return almostShortestPath;
        }

        static public int Dijkstra(WeightedGraph graph, int start, int destination)
        {
            if (!graph.ContainsStartVert() || !graph.ContainsEndVert())
            {
                return -1;
            }

            Dictionary<int, Dictionary<int, int>> edges = new Dictionary<int, Dictionary<int, int>>();

            int[][] vertices = graph.GetEdges();
            for (int i = 0; i < vertices.Length; i++)
            {
                int u = vertices[i][0], v = vertices[i][1], w = vertices[i][2];
                if (!edges.ContainsKey(u)) edges[u] = new Dictionary<int, int>();
                edges[u].Add(v, w);
            }

            Tools.PriorityQueue<Vertex> pq = new Tools.PriorityQueue<Vertex>();
            pq.Enqueue(new Vertex(start, 0, 0, null), 0);

            while (!pq.IsEmpty())
            {
                Vertex u = pq.Dequeue();
                if (u.v == destination)
                {
                    Vertex current = u;
                    while (current.previous != null)
                    {
                        edgesToDelete.Add(current);
                        current = current.previous;
                    }

                    return u.w;
                }

                if (edges.ContainsKey(u.v))
                {
                    foreach (var neighbor in edges[u.v])
                    {
                        pq.Enqueue(new Vertex(u.v, neighbor.Key, u.w + neighbor.Value, u), u.w + neighbor.Value);
                    }
                }

                // Failsafe to capture memory leak; logically speaking, the PQ should never grow to this size
                if (pq.GetSize() > Math.Pow(graph.edgeCount, 2))
                {
                    return -1;
                }
            }
            return -1;
        }

        static public WeightedGraph ToGraph(string input, uint lineNumber)
        {
            try
            {
                var values = input.Split('\n');

                int vertCount = Convert.ToInt32(values[0].Split(' ')[0]);
                int edgeCount = Convert.ToInt32(values[0].Split(' ')[1]);
                int startVert = Convert.ToInt32(values[1].Split(' ')[0]);
                int endVert = Convert.ToInt32(values[1].Split(' ')[1]);

                int[][] graph = new int[edgeCount][];
                for (int i = 0; i < edgeCount; i++)
                {
                    graph[i] = new int[3];
                    var row = values[i + 2].Split(' ');

                    for (int j = 0; j < 3; j++)
                    {
                        graph[i][j] = Convert.ToInt32(row[j]);
                    }
                }

                return new WeightedGraph(vertCount, edgeCount, startVert, endVert, graph);
            }
            catch
            {
                Console.WriteLine("Your input string does not match the required format:\n'0 0'\n'0 0'\n'0 0 0'\n'0 0 0' ...");
                Console.ReadLine();
                Environment.Exit(1);
                return new WeightedGraph();
            }
        }

        public struct WeightedGraph
        {
            public int vertCount, edgeCount, startVert, endVert;
            
            private List<Vertex> _edges;

            public WeightedGraph(int vertCount, int edgeCount, int startVert, int endVert, int[][] edges)
            {
                this.vertCount = vertCount;
                this.edgeCount = edgeCount;
                this.startVert = startVert;
                this.endVert = endVert;

                this._edges = new List<Vertex>();

                for (int i = 0; i < edgeCount; i++)
                {
                    Vertex n = new Vertex(edges[i][0], edges[i][1], edges[i][2]);
                    _edges.Add(n);
                }
            }

            public int[][] GetEdges()
            {
                int[][] graph = new int[edgeCount][];
                int i = 0;
                foreach (Vertex n in _edges)
                {
                    graph[i] = new int[3];
                    graph[i][0] = n.u;
                    graph[i][1] = n.v;
                    graph[i][2] = n.w;
                    i++;
                }
                return graph;
            }

            public void RemoveEdges(List<Vertex> vertices)
            {
                foreach (Vertex n in vertices)
                {
                    if (_edges.Remove(n))
                    {
                        this.edgeCount -= 1;
                    }
                }
            }

            public bool ContainsVertex(int vert)
            {
                if (edgeCount < 1)
                {
                    return false;
                }

                foreach (Vertex u in _edges)
                {
                    if (u.u == vert || u.v == vert)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool ContainsStartVert()
            {
                if (edgeCount < 1)
                {
                    return false;
                }

                foreach (Vertex u in _edges)
                {
                    if (u.u == startVert)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool ContainsEndVert()
            {
                if (edgeCount < 1)
                {
                    return false;
                }

                foreach (Vertex u in _edges)
                {
                    if (u.u == endVert || u.v == endVert)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public class Vertex : IComparable<Vertex>
        {
            public int u, v, w;
            public Vertex previous;

            public Vertex(int u, int v, int w)
            {
                this.u = u;
                this.v = v;
                this.w = w;
            }

            public Vertex(int u, int v, int w, Vertex previous)
            {
                this.u = u;
                this.v = v;
                this.w = w;
                this.previous = previous;
            }

            public override bool Equals(Object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is Vertex))
                {
                    return false;
                }
                return (this.u == ((Vertex)obj).u && this.v == ((Vertex)obj).v);
            }

            public int CompareTo(Vertex other)
            {
                return w.CompareTo(other.w);
            }
        }
    }
}
