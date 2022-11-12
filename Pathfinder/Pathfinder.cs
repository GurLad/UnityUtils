using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    private static int[,] map;
    private static Vector2Int size;

    public static void SetMap(int[,] newMap, Vector2Int newSize)
    {
        map = newMap;
        size = newSize;
    }

    public static float GetTrueDistance(Vector2Int start, Vector2Int end)
    {
        if (start == end)
        {
            return 0;
        }
        List<Vector2Int> parts = GetPath(start, end);
        float sum = 0;
        for (int i = 0; i < parts.Count - 1; i++)
        {
            sum += Vector2.Distance(parts[i], parts[i + 1]);
        }
        return sum;
    }

    public static bool HasLineOfSight(Vector2Int start, Vector2Int end)
    {
        return HasLineOfSight(new Node(start), new Node(end));
    }

    public static List<Vector2Int> GetPath(Vector2Int sourceVec, Vector2Int destinationVec)
    {
        // From Wikipedia...
        Node source = new Node(sourceVec);
        Node destination = new Node(destinationVec);
        if (source == destination)
        {
            throw new Exception("Same source & destination!");
        }
        if (!CanMove(destination.x, destination.y))
        {
            throw new Exception("Destination is a blocked tile! (" + destination + ")");
        }

        List<Node> openSet = new List<Node>();
        openSet.Add(source);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        Dictionary<Node, int> gScore = new Dictionary<Node, int>();
        gScore.Add(source, 0);

        Dictionary<Node, int> fScore = new Dictionary<Node, int>();
        fScore.Add(source, GetCost(source, destination));
        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            int minValue = int.MaxValue;
            openSet.ForEach(a => { if (fScore.SafeGetKey(a, int.MaxValue) < minValue) minValue = fScore.SafeGetKey(current = a, int.MaxValue); });
            if (current == destination)
            {
                return RecoverPath(cameFrom, current);
            }
            openSet.Remove(current);
            foreach (Node neighbor in current.GetNeighbors())
            {
                if (CanMove(neighbor.x, neighbor.y))
                { 
                    int tentativeScore = gScore[current] + GetDistance(current, neighbor); // No safe as the current should always have a gValue
                    if (tentativeScore < gScore.SafeGetKey(neighbor, int.MaxValue))
                    {
                        cameFrom.AddOrSet(neighbor, current);
                        gScore.AddOrSet(neighbor, tentativeScore);
                        fScore.AddOrSet(neighbor, tentativeScore + GetCost(neighbor, destination));
                        if (openSet.FindIndex(a => a == neighbor) < 0)
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }
        // This should be impossible
        return null;
    }

    private static List<Vector2Int> RecoverPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        void Squash(List<Node> totalPath, int curr, int next)
        {
            for (int i = curr + 1; i < next - 1; i++)
            {
                totalPath.RemoveAt(curr + 1);
            }
        }

        List<Node> totalPath = new List<Node>();
        totalPath.Add(current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        // Post-process path
        if (totalPath.Count > 2) // No need to squash if it's just 2 steps...
        {
            int curr = 0, next = 2;
            while (next < totalPath.Count)
            {
                if (!HasLineOfSight(totalPath[curr], totalPath[next]))
                {
                    Squash(totalPath, curr, next);
                    curr++;
                    next = curr + 2; // Must have a line of sight with neighbors, so no need to check them
                }
                else
                {
                    next++;
                }
            }
            Squash(totalPath, curr, next);
        }
        // Reverse & convert path
        List<Vector2Int> reversed = new List<Vector2Int>();
        for (int i = totalPath.Count - 1; i >= 0; i--)
        {
            reversed.Add(totalPath[i].ToVector2Int());
        }
        return reversed;
    }

    private static int GetCost(Node pos, Node destination)
    {
        return pos.GetDistance(destination);
    }

    private static int GetDistance(Node current, Node neighbor)
    {
        return 1; // No need to calculate, we know it's always 1
    }

    private static bool CanMove(int x, int y)
    {
        if (x < 0 || y < 0 || x >= size.x || y >= size.y)
        {
            return true;
        }
        return map[x, y] <= 0;
    }

    private static bool HasLineOfSight(Node start, Node end)
    {
        // Adapted from https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        int x = start.x, y = start.y;
        int x2 = end.x, y2 = end.y;
        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            if (!CanMove(x, y))
            {
                return false;
            }
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                if (i < longest && (!CanMove(x + dx1, y) || !CanMove(x, y + dy1)))
                {
                    //Debug.Log("No line of sight between " + new Vector2Int(x, y) + " and " + new Vector2Int(x + dx1, y + dy1) + " - checking " + start + " to " + end);
                    return false;
                }
                x += dx1;
                y += dy1;
            }
            else
            {
                if (i < longest && (!CanMove(x + dx2, y) || !CanMove(x, y + dy2)))
                {
                    //Debug.Log("No line of sight between " + new Vector2Int(x, y) + " and " + new Vector2Int(x + dx2, y + dy2) + " - checking " + start + " to " + end);
                    return false;
                }
                x += dx2;
                y += dy2;
            }
        }
        return true;
    }

    private class Node
    {
        public int x;
        public int y;

        public static bool operator ==(Node a, Node b)
        {
            if ((object)a == null) return (object)b == null;
            if ((object)b == null) return (object)a == null;
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }

        public Node(Vector2Int vector2Int)
        {
            x = vector2Int.x;
            y = vector2Int.y;
        }

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public List<Node> GetNeighbors()
        {
            List<Node> result = new List<Node>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i == 0 || j == 0) && (i != 0 || j != 0))
                    {
                        result.Add(new Node(x + i, y + j));
                    }
                }
            }
            return result;
        }

        public int GetDistance(Node other)
        {
            return Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(other.x - x, 2) + Mathf.Pow(other.y + y, 2))); // Mathf.Abs(other.x - x) + Mathf.Abs(other.y - y);
        }

        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(x, y);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
	
	// Dictionary extensions
	
    public static S SafeGetKey<T, S>(this Dictionary<T, S> dictionary, T key, S defaultValue = default)
    {
        return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
    }

    public static void AddOrSet<T, S>(this Dictionary<T, S> dictionary, T key, S value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, value);
        }
        else
        {
            dictionary[key] = value;
        }
    }
}
