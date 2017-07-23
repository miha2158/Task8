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

        #region GraphToString

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
                result[i] = string.Join("  ", preResult[i]);
            }

            return result;
        }

        static string ToString(bool[][] graph)
        {
            return string.Join("\n", ToString1D(graph));
        }

        #endregion

        #region Menu

        private static void SwapColors()
        {
            var temp = BackgroundColor;
            BackgroundColor = ForegroundColor;
            ForegroundColor = temp;
        }
        static void DoSwapped(Action action)
        {
            SwapColors();
            action.Invoke();
            SwapColors();
        }

        private static Tuple<int, int> SelectorXY(string[][] items, int XOffset = 3)
        {
            var CursorBefore = CursorVisible;
            CursorVisible = false;

            var offset = new Tuple<int, int>(XOffset, 1);
            SetCursorPosition(CursorLeft + offset.Item1, CursorTop + offset.Item2);
            var defaultXY = new Tuple<int, int>(CursorLeft, CursorTop);

            var PreviousPosition = new Tuple<int, int>(0, 0);
            var CurrentPosition = new Tuple<int, int>(0, 0);

            int maxlength = items[0][0].Length;
            {
                foreach (string[] s1 in items)
                    foreach (string s in s1)
                        if (s.Length > maxlength)
                            maxlength = s.Length;
            }

            int spread = maxlength + offset.Item1;

            foreach (string[] item in items)
            {
                foreach (string s in item)
                {
                    Write(s);
                    CursorLeft = CursorLeft - s.Length + spread;
                }

                SetCursorPosition(defaultXY.Item1, CursorTop + 1);
            }

            while (true)
            {
                SetCursorPosition(defaultXY.Item1 + spread * PreviousPosition.Item1,
                    defaultXY.Item2 + PreviousPosition.Item2);
                Write(items[PreviousPosition.Item2][PreviousPosition.Item1]);

                SetCursorPosition(defaultXY.Item1 + spread * CurrentPosition.Item1,
                    defaultXY.Item2 + CurrentPosition.Item2);
                DoSwapped(delegate { Write(items[CurrentPosition.Item2][CurrentPosition.Item1]); });

                bool done = false;
                var key = ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Enter:
                        graph[CurrentPosition.Item1][CurrentPosition.Item2] =
                            !graph[CurrentPosition.Item1][CurrentPosition.Item2];
                        items = ToString2D(graph);
                    break;

                    case ConsoleKey.Escape:
                        done = true;
                        break;

                    case ConsoleKey.RightArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition =
                            new Tuple<int, int>(
                                CurrentPosition.Item1 + 1 < items[CurrentPosition.Item2].Length
                                    ? CurrentPosition.Item1 + 1
                                    : 0,
                                CurrentPosition.Item2);
                        break;
                    }

                    case ConsoleKey.LeftArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition =
                            new Tuple<int, int>(CurrentPosition.Item1 - 1 >= 0
                                    ? CurrentPosition.Item1 - 1
                                    : items[0].Length - 1,
                                CurrentPosition.Item2);
                        break;
                    }

                    case ConsoleKey.UpArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = new Tuple<int, int>(CurrentPosition.Item1,
                            CurrentPosition.Item2 - 1 >= 0
                                ? CurrentPosition.Item2 - 1
                                : items.Length - 1);
                        break;
                    }

                    case ConsoleKey.DownArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = new Tuple<int, int>(CurrentPosition.Item1,
                            CurrentPosition.Item2 + 1 < items.Length
                                ? CurrentPosition.Item2 + 1
                                : 0);
                        break;
                    }
                }

                if (done)
                {
                    break;
                }
            }



            SetCursorPosition(0,defaultXY.Item2 + items.Length + 1);
            CursorVisible = CursorBefore;
            return new Tuple<int, int>(0, 0);
        }

        #endregion

        static bool[][] graph;
        
        static void Main(string[] args)
        {

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

            SelectorXY(ToString2D(graph));





            WriteLine(ToString(graph));
            
            ReadKey(true);
        }
    }
}