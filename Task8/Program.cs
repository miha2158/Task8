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

        static string GraphToString(bool[][] graph)
        {
            var result = new string[graph[0].Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = string.Empty;

            foreach (bool[] edge in graph)
                for (int j = 0; j < edge.Length; j++)
                    result[j] += Convert.ToInt16(edge[j]) + " ";

            string output = string.Empty;
            foreach (string s in result)
                output += s +"\n";

            return output;
        }


        static void Main(string[] args)
        {
            bool[][] graph;

            {
                int i0;
                WriteLine("Введите количество вершин графа");
                while (!int.TryParse(ReadLine(), out i0) || i0 <= 1)
                    WriteLine("Введите число больше 1");

                int i1;
                WriteLine("Введите количество рёбер графа");
                while (!int.TryParse(ReadLine(), out i1) || i1 <= 0)
                    WriteLine("Введите число больше 0");

                graph = new bool[i1][];
                for (int i = 0; i < graph.Length; i++)
                {
                    graph[i] = new bool[i0];
                    for (int j = 0; j < graph[i].Length; j++)
                        graph[i][j] = false;
                }
            }

            WriteLine(GraphToString(graph));



            ReadKey(true);
        }
    }
}