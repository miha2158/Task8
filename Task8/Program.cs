using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Task8
{
    class Program
    {
        static void step(bool[][] graph, bool[] visited, int n)
        {
            visited[n] = true;

            foreach (bool[] edge in graph)
                if(edge[n])
                    for (int i = 0; i < edge.Length; i++)
                        if(edge[i] && i != n && !visited[i])
                            step(graph, visited, i);
        }
        
        static IEnumerable<int> FindBridges(bool[][] graph)
        {
            for (int i = 0; i < graph.Length; i++)
            {
                var visited = new bool[graph[0].Length];
                for (int j = 0; j < visited.Length; j++)
                    visited[j] = false;

                var tempgraph = new bool[graph.Length - 1][];
                for (int j = 0; j < graph.Length; j++)
                    if (j != i)
                        tempgraph[j] = graph[j];

                step(tempgraph, visited, 0);

                if (visited.Contains(false))
                    yield return i;
            }
        }
        
        static void Main(string[] args)
        {
        }
    }
}
