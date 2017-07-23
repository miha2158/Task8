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

        static void Main(string[] args)
        {
        }
    }
}
