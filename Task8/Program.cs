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
        #region Processing

        static void step(bool[][] graph1, bool[] visited, int n)
        {
            visited[n] = true;

            foreach (bool[] edge in graph1)
                if (edge[n])
                    for (int i = 0; i < edge.Length; i++)
                        if (edge[i] && i != n && !visited[i])
                            step(graph1, visited, i);
        }

        static IEnumerable<int> FindBridges(bool[][] graph1)
        {
            for (int i = 0; i < graph1.Length; i++)
            {
                var visited = new bool[graph1[0].Length];
                for (int j = 0; j < visited.Length; j++)
                    visited[j] = false;

                var tempgraph = new bool[graph1.Length - 1][];
                for (int j = 0; j < graph1.Length; j++)
                    if (j < i)
                        tempgraph[j] = graph1[j];
                else if (j > i)
                        tempgraph[j - 1] = graph1[j];

                step(tempgraph, visited, 0);

                if (visited.Contains(false))
                    yield return i;
            }
        }

        static bool[][] Shorten(bool[][] graph1)
        {
            var result1 = new List<bool>[graph1.Length];

            for (int i = 0; i < graph1.Length; i++)
                result1[i] = new List<bool>(0);

            for (int i = 0; i < graph1[0].Length; i++)
            {
                bool all = true;
                foreach (bool[] t in graph1)
                    all = all && (t[i] == false);

                if (all)
                    for (int j = 0; j < graph1.Length; j++)
                        result1[j].Add(graph1[j][i]);
            }

            var result = new bool[result1.Length][];
            for (int i = 0; i < result.Length; i++)
                result[i] = result1[i].ToArray();

            return result;
        }

        static bool CheckGraph(bool[][] graph1)
        {
            for (int i = 0; i < graph1.Length; i++)
            {
                int result = graph1[i].Sum(Convert.ToInt32);
                if (result != 2)
                    return false;

                for (int j = i + 1; j < graph1.Length; j++)
                {
                    bool match = graph1[i][0] == graph1[j][0];
                    for (int k = 1; k < graph1[j].Length; k++)
                        match = match && (graph1[i][k] == graph1[j][k]);

                    if (match)
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region GraphToString

        static string[][] ToString2D(bool[][] graph1)
        {
            var result = new string[graph1[0].Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new string[graph1.Length];

                for (int j = 0; j < result[i].Length; j++)
                    result[i][j] = Convert.ToInt32(graph1[j][i]).ToString();
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

        private static void SelectorXY(string[][] items)
        {
            var CursorBefore = CursorVisible;
            CursorVisible = false;

            var offset = new Tuple<int, int>(3, 1);
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

                switch (ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        graph[CurrentPosition.Item1][CurrentPosition.Item2] =
                            !graph[CurrentPosition.Item1][CurrentPosition.Item2];
                        items = ToString2D(graph);
                        break;

                    case ConsoleKey.Escape:
                    {
                        SetCursorPosition(0, defaultXY.Item2 + items.Length + 1);
                        CursorVisible = CursorBefore;
                    }
                        return;

                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition =
                            new Tuple<int, int>(
                                CurrentPosition.Item1 + 1 < items[CurrentPosition.Item2].Length
                                    ? CurrentPosition.Item1 + 1
                                    : 0,
                                CurrentPosition.Item2);
                    }
                        break;

                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition =
                            new Tuple<int, int>(CurrentPosition.Item1 - 1 >= 0
                                    ? CurrentPosition.Item1 - 1
                                    : items[0].Length - 1,
                                CurrentPosition.Item2);
                    }
                        break;

                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = new Tuple<int, int>(CurrentPosition.Item1,
                            CurrentPosition.Item2 - 1 >= 0
                                ? CurrentPosition.Item2 - 1
                                : items.Length - 1);
                    }
                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                    {
                        PreviousPosition = CurrentPosition;
                        CurrentPosition = new Tuple<int, int>(CurrentPosition.Item1,
                            CurrentPosition.Item2 + 1 < items.Length
                                ? CurrentPosition.Item2 + 1
                                : 0);
                    }
                        break;
                }
            }
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
                while (!int.TryParse(ReadLine(), out i1) || i1 <= 0 || i1 > i0 * (i0 - 1) / 2)
                    WriteLine("Введите число больше 0 и меньше {0}", i0 * (i0 - 1) / 2);

                graph = new bool[i1][];
                for (int i = 0; i < graph.Length; i++)
                {
                    graph[i] = new bool[i0];
                    for (int j = 0; j < graph[i].Length; j++)
                        graph[i][j] = false;
                }
            }

            while (true)
            {
                WriteLine(@"
    Введите матрицу
    Навигация с помощью стрелочек
    Изменение с помошью Enter
    Выход с помошью Escape");
                SelectorXY(ToString2D(graph));

                if (CheckGraph(graph))
                    break;

                WriteLine("Ошибка. Убедитесь что столбцы не повторяются и каждый из них имеет ровно 2 единицы");
            }

            var result = FindBridges(Shorten(graph)).ToArray();
            for (int i = 0; i < result.Length; i++)
                result[i]++;

            string output = string.Join(" ", result);
            if (output.Trim() == string.Empty)
                output = "Мостов не найдено";
            WriteLine("Найденные мосты:\n {0}", output);

            ReadKey(true);
        }
    }
}