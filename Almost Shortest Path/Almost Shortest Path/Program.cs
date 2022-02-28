using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Almost_Shortest_Path
{
    class Program
    {
        static void Main(string[] args)
        {


            string in1 =
                "7 9\n" +
                "0 6\n" +
                "0 1 1\n" +
                "0 2 1\n" +
                "0 3 2\n" +
                "0 4 3\n" +
                "1 5 2\n" +
                "2 6 4\n" +
                "3 6 2\n" +
                "4 6 4\n" +
                "5 6 1";

            string in2 =
                "4 6\n" +
                "0 2\n" +
                "0 1 1\n" +
                "1 2 1\n" +
                "1 3 1\n" +
                "3 2 1\n" +
                "2 0 3\n" +
                "3 0 2";

            string in3 =
                "6 8\n" +
                "0 1\n" +
                "0 1 1\n" +
                "0 2 2\n" +
                "0 3 3\n" +
                "2 5 3\n" +
                "3 4 2\n" +
                "4 1 1\n" +
                "5 1 1\n" +
                "3 0 1";

            WeightedGraph g1 = ToGraph(in1);
            Console.WriteLine(string.Format("The resulting shortest path is {0}.", AlmostShortestPath(g1, g1.startVert, g1.endVert)));

            WeightedGraph g2 = ToGraph(in2);
            Console.WriteLine(string.Format("The resulting shortest path is {0}.", AlmostShortestPath(g2, g2.startVert, g2.endVert)));

            WeightedGraph g3 = ToGraph(in3);
            Console.WriteLine(string.Format("The resulting shortest path is {0}.", AlmostShortestPath(g3, g3.startVert, g3.endVert)));
        }

        static List<Vertex> edgesToDelete = new List<Vertex>();
        static public int AlmostShortestPath(WeightedGraph graph, int start, int destination)
        {
            edgesToDelete.Clear();
            int shortestPath = Dijkstra(graph.GetEdges(), start, destination);
            int almostShortestPath = 0;
            while (true)
            {
                graph.RemoveEdges(edgesToDelete);
                almostShortestPath = Dijkstra(graph.GetEdges(), start, destination);
                if (shortestPath != almostShortestPath)
                {
                    break;
                }
            }
            return almostShortestPath;
        }

        static public int Dijkstra(int[][] vertices, int start, int destination)
        {
            Dictionary<int, Dictionary<int, int>> edges = new Dictionary<int, Dictionary<int, int>>();

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
            }
            return -1;
        }

        static public WeightedGraph ToGraph(string input)
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
                throw new InvalidInputException("Your input string does not match the required format:\n'0 0'\n'0 0'\n'0 0 0'\n'0 0 0' ...");
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
