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

        static string[][] ToString2D(bool[][] graph)
        {
            var result = new string[graph[0].Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new string[graph.Length];

                for (int j = 0; j < result[i].Length; j++)
                    result[i][j] = Convert.ToInt32(graph[j][i]).ToString();
            }
            return result;
        }

        static string[] ToString1D(bool[][] graph)
        {
            var preResult = ToString2D(graph);
            var result = new string[graph[0].Length];

            for (var i = 0; i < preResult.Length; i++)
            {
                result[i] = string.Join(" ", preResult[i]);
            }

            return result;
        }

        static string ToString(bool[][] graph)
        {
            return string.Join("\n", ToString1D(graph));
        }

        private static void SwapColors()
        {
            var temp = BackgroundColor;
            BackgroundColor = ForegroundColor;
            ForegroundColor = temp;
        }


        private static int SelectorXY(string[][] items)
        {
            return 0;
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

            WriteLine(ToString(graph));



            ReadKey(true);
        }
    }
}