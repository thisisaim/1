using System;
using System.Collections.Generic;
using System.Linq;

class Graph
{
    private Dictionary<char, Dictionary<char, int>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<char, Dictionary<char, int>>();
    }

    public void AddEdge(char start, char end, int weight)
    {
        if (!adjacencyList.ContainsKey(start))
        {
            adjacencyList[start] = new Dictionary<char, int>();
        }
        adjacencyList[start][end] = weight;
    }

    public int GetRouteDistance(List<char> route)
    {
        int distance = 0;
        for (int i = 0; i < route.Count - 1; i++)
        {
            if (adjacencyList.ContainsKey(route[i]) && adjacencyList[route[i]].ContainsKey(route[i + 1]))
            {
                distance += adjacencyList[route[i]][route[i + 1]];
            }
            else
            {
                return -1; // Indicating NO SUCH ROUTE
            }
        }
        return distance;
    }

    public int CountTripsWithMaxStops(char start, char end, int maxStops)
    {
        return CountTripsWithMaxStopsHelper(start, end, maxStops, 0);
    }

    private int CountTripsWithMaxStopsHelper(char current, char end, int maxStops, int currentStops)
    {
        if (currentStops > maxStops) return 0;
        int count = 0;
        if (current == end && currentStops > 0)
        {
            count++;
        }
        if (!adjacencyList.ContainsKey(current)) return count;
        foreach (var neighbor in adjacencyList[current].Keys)
        {
            count += CountTripsWithMaxStopsHelper(neighbor, end, maxStops, currentStops + 1);
        }
        return count;
    }

    public int CountTripsWithExactStops(char start, char end, int exactStops)
    {
        return CountTripsWithExactStopsHelper(start, end, exactStops, 0);
    }

    private int CountTripsWithExactStopsHelper(char current, char end, int exactStops, int currentStops)
    {
        if (currentStops > exactStops) return 0;
        if (currentStops == exactStops && current == end) return 1;
        if (!adjacencyList.ContainsKey(current)) return 0;
        int count = 0;
        foreach (var neighbor in adjacencyList[current].Keys)
        {
            count += CountTripsWithExactStopsHelper(neighbor, end, exactStops, currentStops + 1);
        }
        return count;
    }

    public int ShortestRoute(char start, char end)
    {
        var distances = new Dictionary<char, int>();
        var priorityQueue = new SortedSet<Tuple<int, char>>();

        foreach (var node in adjacencyList.Keys)
        {
            distances[node] = int.MaxValue;
        }
        distances[start] = 0;
        priorityQueue.Add(new Tuple<int, char>(0, start));

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.First();
            priorityQueue.Remove(current);
            int currentDistance = current.Item1;
            char currentNode = current.Item2;

            if (currentNode == end && currentDistance != 0)
            {
                return currentDistance;
            }

            foreach (var neighbor in adjacencyList[currentNode])
            {
                int distance = currentDistance + neighbor.Value;
                if (distance < distances[neighbor.Key])
                {
                    priorityQueue.Remove(new Tuple<int, char>(distances[neighbor.Key], neighbor.Key));
                    distances[neighbor.Key] = distance;
                    priorityQueue.Add(new Tuple<int, char>(distance, neighbor.Key));
                }
            }
        }
        return distances[end];
    }

    public int ShortestRouteWithCycles(char start, char end)
    {
        var distances = new Dictionary<char, int>();
        var visited = new HashSet<char>();
        var priorityQueue = new SortedSet<Tuple<int, char>>();

        foreach (var node in adjacencyList.Keys)
        {
            distances[node] = int.MaxValue;
        }
        distances[start] = 0;
        priorityQueue.Add(new Tuple<int, char>(0, start));

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.First();
            priorityQueue.Remove(current);
            int currentDistance = current.Item1;
            char currentNode = current.Item2;

            if (currentNode == end && visited.Contains(end))
            {
                return currentDistance;
            }

            visited.Add(currentNode);

            foreach (var neighbor in adjacencyList[currentNode])
            {
                int distance = currentDistance + neighbor.Value;
                if (distance < distances[neighbor.Key])
                {
                    priorityQueue.Remove(new Tuple<int, char>(distances[neighbor.Key], neighbor.Key));
                    distances[neighbor.Key] = distance;
                    priorityQueue.Add(new Tuple<int, char>(distance, neighbor.Key));
                }
            }
        }
        return distances[end];
    }

    public int CountRoutesWithMaxDistance(char start, char end, int maxDistance)
    {
        return CountRoutesWithMaxDistanceHelper(start, end, maxDistance, 0);
    }

    private int CountRoutesWithMaxDistanceHelper(char current, char end, int maxDistance, int currentDistance)
    {
        if (currentDistance >= maxDistance) return 0;
        int count = 0;
        if (current == end && currentDistance > 0)
        {
            count++;
        }
        if (!adjacencyList.ContainsKey(current)) return count;
        foreach (var neighbor in adjacencyList[current].Keys)
        {
            count += CountRoutesWithMaxDistanceHelper(neighbor, end, maxDistance, currentDistance + adjacencyList[current][neighbor]);
        }
        return count;
    }
}

class Program
{
    static void Main()
    {
        Graph graph = new Graph();
        graph.AddEdge('A', 'B', 5);
        graph.AddEdge('B', 'C', 4);
        graph.AddEdge('C', 'D', 8);
        graph.AddEdge('D', 'C', 8);
        graph.AddEdge('D', 'E', 6);
        graph.AddEdge('A', 'D', 5);
        graph.AddEdge('C', 'E', 2);
        graph.AddEdge('E', 'B', 3);
        graph.AddEdge('A', 'E', 7);

        // Outputs
        List<char> route1 = new List<char> { 'A', 'B', 'C' };
        List<char> route2 = new List<char> { 'A', 'D' };
        List<char> route3 = new List<char> { 'A', 'D', 'C' };
        List<char> route4 = new List<char> { 'A', 'E', 'B', 'C', 'D' };
        List<char> route5 = new List<char> { 'A', 'E', 'D' };

        Console.WriteLine("Output #1: " + (graph.GetRouteDistance(route1) != -1 ? graph.GetRouteDistance(route1).ToString() : "NO SUCH ROUTE"));
        Console.WriteLine("Output #2: " + (graph.GetRouteDistance(route2) != -1 ? graph.GetRouteDistance(route2).ToString() : "NO SUCH ROUTE"));
        Console.WriteLine("Output #3: " + (graph.GetRouteDistance(route3) != -1 ? graph.GetRouteDistance(route3).ToString() : "NO SUCH ROUTE"));
        Console.WriteLine("Output #4: " + (graph.GetRouteDistance(route4) != -1 ? graph.GetRouteDistance(route4).ToString() : "NO SUCH ROUTE"));
        Console.WriteLine("Output #5: " + (graph.GetRouteDistance(route5) != -1 ? graph.GetRouteDistance(route5).ToString() : "NO SUCH ROUTE"));
        
        Console.WriteLine("Output #6: " + graph.CountTripsWithMaxStops('C', 'C', 3));
        Console.WriteLine("Output #7: " + graph.CountTripsWithExactStops('A', 'C', 4));
        Console.WriteLine("Output #8: " + graph.ShortestRoute('A', 'C'));
        Console.WriteLine("Output #9: " + graph.ShortestRouteWithCycles('B', 'B'));
        Console.WriteLine("Output #10: " + graph.CountRoutesWithMaxDistance('C', 'C', 30));
    }
}
