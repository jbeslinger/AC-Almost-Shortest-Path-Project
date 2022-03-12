using System;
using System.Collections.Generic;
using System.Text;

namespace Almost_Shortest_Path.Tools
{
    public class WeightedGraph
    {
        #region Consts
        private const int INFINITY = Int32.MaxValue;
        #endregion

        #region Fields
        public int vertCount = -1;
        public int edgeCount = -1;
        public int startVert = -1;
        public int endVert = -1;
        #endregion

        #region Members
        private Dictionary<int, Vertex> _graph = new Dictionary<int, Vertex>();
        private HashSet<Edge> shortestPathEdges = new HashSet<Edge>();
        #endregion

        #region Constructors
        public WeightedGraph(string input)
        {
            try
            {
                var values = input.Split('\n');

                this.vertCount = Convert.ToInt32(values[0].Split(' ')[0]);
                this.edgeCount = Convert.ToInt32(values[0].Split(' ')[1]);
                this.startVert = Convert.ToInt32(values[1].Split(' ')[0]);
                this.endVert = Convert.ToInt32(values[1].Split(' ')[1]);

                for (int i = 0; i < edgeCount; i++)
                {
                    int v1, v2;
                    int w;

                    var row = values[i + 2].Split(' ');
                    v1 = Convert.ToInt32(row[0]);
                    v2 = Convert.ToInt32(row[1]);
                    w = Convert.ToInt32(row[2]);

                    AddEdge(v1, v2, w);
                }
            }
            catch
            {
                Console.WriteLine("Your input string does not match the required format:\n'0 0'\n'0 0'\n'0 0 0'\n'0 0 0' ...");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        public WeightedGraph(int vertCount, int edgeCount, int startVert, int endVert)
        {
            this.vertCount = vertCount;
            this.edgeCount = edgeCount;
            this.startVert = startVert;
            this.endVert = endVert;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gathers and returns all of the neighbors of Vertex u in a List.
        /// </summary>
        /// <param name="vertex">The Vertex you want the neighbors of.</param>
        /// <returns>All neighbors of Vertex u.</returns>
        public List<Edge> GetNeighbors(int vertex)
        {
            List<Edge> neighbors = new List<Edge>();
            foreach (var edge in _graph[vertex].edges)
            {
                neighbors.Add(edge.Value);
            }
            return neighbors;
        }

        /// <summary>
        /// Creates a new Edge and adds it to the graph. Also adds missing Vertices.
        /// </summary>
        /// <param name="u">Integer label of Vertex u.</param>
        /// <param name="v">Integer label of Vertex v.</param>
        /// <param name="w">Weight of Edge u,v.</param>
        public void AddEdge(int u, int v, int w)
        {
            if (!_graph.ContainsKey(u))
            {
                _graph.Add(u, new Vertex(u));
            }
            if (!_graph.ContainsKey(v))
            {
                _graph.Add(v, new Vertex(v));
            }
            AddEdge(new Edge(_graph[u], _graph[v], w));
        }

        /// <summary>
        /// Adds an edge to the graph based on the U and V of the provided edge.
        /// </summary>
        /// <param name="e">The Edge to add.</param>
        public void AddEdge(Edge e)
        {
            this._graph[e.u.label].edges.Add(e.v, e);
        }

        /// <summary>
        /// Deletes the provided Edge from the graph based on the U and V of the edge.
        /// </summary>
        /// <param name="e">The Edge to delete.</param>
        public void RemoveEdge(Edge e)
        {
            this._graph[e.u.label].edges.Remove(e.v);
        }

        /// <summary>
        /// Removes the shortest path edges from the graph and clears the buffer.
        /// Run this after Dijkstra before running AlmostShortestPath.
        /// </summary>
        public void RemoveShortestPathEdges()
        {
            foreach (Edge e in shortestPathEdges)
            {
                RemoveEdge(e);
            }
            shortestPathEdges.Clear();
        }

        /// <summary>
        /// Clears all of the parents of this graph's vertices and sets their costs back to infinity.
        /// </summary>
        private void ResetVertices()
        {
            foreach (Vertex v in _graph.Values)
            {
                v.parent.Clear();
                v.cost = INFINITY;
            }
        }

        /// <summary>
        /// Performs Dijkstra's algorithm to find the shortest paths on this WeightedGraph object,
        /// then clears the shortest edges and runs Dijkstra again.
        /// </summary>
        /// <param name="start">The starting Vertex.</param>
        /// <param name="destination">The ending Vertex.</param>
        /// <returns>The distance of the almost-shortest path.</returns>
        public int AlmostShortestPath()
        {
            int shortestPath = Dijkstra();
            RemoveShortestPathEdges();
            ResetVertices();
            return Dijkstra();
        }

        /// <summary>
        /// Performs Dijkstra's algorithm to find the shortest paths on this WeightedGraph object.
        /// </summary>
        /// <param name="start">The starting Vertex.</param>
        /// <param name="destination">The ending Vertex.</param>
        /// <returns>The distance of the shortest path.</returns>
        public int Dijkstra()
        {
            if (!_graph.ContainsKey(startVert) || !_graph.ContainsKey(endVert))
                return -1;

            Vertex start = _graph[startVert];
            Vertex destination = _graph[endVert];

            PriorityQueue<Vertex> pq = new PriorityQueue<Vertex>();
            for (int v = 0; v < vertCount; v++)
            {
                if (!_graph.ContainsKey(v))
                {
                    return -1;
                }
                Vertex currentVert = _graph[v];
                if (currentVert.label == startVert)
                {
                    currentVert.cost = 0;
                }
                pq.Enqueue(_graph[v], currentVert.cost);
            }

            HashSet<Vertex> visited = new HashSet<Vertex>();
            while (!pq.IsEmpty())
            {
                Vertex u = pq.Dequeue();
                visited.Add(u);
                foreach (Edge edge in GetNeighbors(u.label))
                {
                    Vertex v = edge.v;
                    int alt = u.cost + u.edges[v].weight;
                    if (!visited.Contains(v) && alt <= v.cost)
                    {
                        if (alt < v.cost)
                        {
                            v.parent.Clear();
                            v.cost = alt;
                        }
                        v.parent.Add(u);
                        pq.ChangePriority(v, alt);
                    }
                }
            }

            Queue<Vertex> q = new Queue<Vertex>();
            Vertex vert = destination;
            q.Enqueue(vert);
            while (q.Count != 0 && vert.parent != null)
            {
                vert = q.Dequeue();
                foreach (Vertex u in vert.parent)
                {
                    q.Enqueue(u);
                    shortestPathEdges.Add(u.edges[vert]);
                }
            }

            return vert == start ? destination.cost : -1;
        }
        #endregion
    }

    #region Internal classes
    /// <summary>
    /// A vertex is a point on a weighted graph.
    /// Holds information about each vertex on the weighted graph including parents, edges, cost, and label.
    /// </summary>
    public class Vertex
    {
        #region Consts
        private const int INFINITY = Int32.MaxValue;
        #endregion

        #region Fields
        public int label;
        public Dictionary<Vertex, Edge> edges;
        public List<Vertex> parent;
        public int cost;
        #endregion

        public Vertex(int label)
        {
            this.label = label;
            edges = new Dictionary<Vertex, Edge>();
            parent = new List<Vertex>();
            cost = INFINITY;
        }

        public override string ToString()
        {
            return "{" + this.label + "}";
        }
    }

    /// <summary>
    /// An edges is a pair of vertices on a directed, weighted graph with an assigned weight.
    /// Holds references to the start vertex, end vertex, and holds the associated weight.
    /// </summary>
    public class Edge
    {
        #region Fields
        public Vertex u;
        public Vertex v;
        public int weight;
        #endregion

        public Edge(Vertex u, Vertex v, int weight)
        {
            this.u = u;
            this.v = v;
            this.weight = weight;
        }

        public override string ToString()
        {
            return String.Format("{{0}-->{1}} {2}", u, v, weight);
        }
    }
    #endregion
}
