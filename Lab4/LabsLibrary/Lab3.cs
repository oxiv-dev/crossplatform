//variant 72
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace LabsLibrary
{
    
    public class Lab3
    {
        static List<List<int>> ReadFile(string path = "OUTPUT.txt")
        {
            var roads = new List<List<int>>();

            string[] file = File.ReadAllLines(path);

            if (file[0].Split(' ').Length != 2)
            {
                throw new Exception("First line of the file must contain exacly TWO numbers.");
            }

            try
            {
                int villages = Convert.ToInt32(file[0].Split(' ')[0]);
                int roadsNum = Convert.ToInt32(file[0].Split(' ')[1]);
                int maxRoads = MaxRoads(villages);

                if (file.Length - 1 != roadsNum || roadsNum > maxRoads)
                {
                    throw new Exception("Wrong roads number in first line of the file.");
                }
            }
            catch
            {
                throw new Exception("Remove all characters from file except numbers!");
            }


            for (int i = 1; i < file.Length; i++)
            {
                try
                {
                    roads.Add(new List<int> { Convert.ToInt32(file[i].Split(' ')[0]), Convert.ToInt32(file[i].Split(' ')[1]) });
                }
                catch
                {
                    throw new Exception("Remove all characters from file except numbers!");
                }
            }

            return roads;

        }
        static bool IsWaysCompatible(List<List<List<int>>> way, List<List<int>> waysPart)
        {
            for (int i = 0; i < way.Count; i++)
            {
                for (int j = 0; j < way[i].Count; j++)
                {
                    for (int k = 0; k < waysPart.Count; k++)
                    {
                        if (way[i][j] == waysPart[k])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        static int InnerLength(List<List<List<int>>> lst)
        {
            int listInnerLength = 0;
            for (int i = 0; i < lst.Count; i++)
            {
                listInnerLength += lst[i].Count;
            }
            return listInnerLength;
        }
        static List<List<List<List<int>>>> FindWays(List<List<List<int>>> way, List<List<List<int>>> ways, List<List<List<List<int>>>> compatibleWays, List<List<int>> roads)
        {
            for(int i = 0; i < ways.Count; i++)
            {
                if (!IsWaysCompatible(way, ways[i]))
                    continue;
                var wayCopy = new List<List<List<int>>>(way);
                var waysCopy = new List<List<List<int>>>(ways);
                wayCopy.Add(ways[i]);
                waysCopy.RemoveAt(i);
                if(InnerLength(wayCopy) == roads.Count)
                {
                    compatibleWays.Add(wayCopy);
                }
                else if (InnerLength(wayCopy) < roads.Count)
                {
                    FindWays(wayCopy, waysCopy, compatibleWays, roads);
                }
                else if (InnerLength(wayCopy) > roads.Count)
                {
                    continue;
                }
            }
            return compatibleWays;
        }
        static List<List<List<int>>> FindAllWays(List<List<int>> roads)
        {
            var ways = new List<List<List<int>>>();
            for(int i = 1; i < roads.Count + 1; i++)
            {
                FindAllWaysFromTown(roads, new List<List<int>>(), ways, i, i);
            }
            return ways;
        }
        static List<List<List<int>>> FindAllWaysFromTown(List<List<int>> roads, List<List<int>> way, List<List<List<int>>> ways, int startPoint, int startTown)
        { 
            for(int i = 0; i < roads.Count; i++)
            {
                if (roads[i][0] == startPoint || roads[i][1] == startPoint)
                {
                    var roads_copy = new List<List<int>>(roads);
                    var way_copy = new List<List<int>>(way);
                    roads_copy.RemoveAt(i);
                    way_copy.Add(roads[i]);
                    if (roads[i][0] == startPoint)
                    {
                        FindAllWaysFromTown(roads_copy, way_copy, ways, roads[i][1], startTown);
                    }
                    else
                    {
                        FindAllWaysFromTown(roads_copy, way_copy, ways, roads[i][0], startTown);
                    }
                    ways.Add(way_copy);
                }
            }

            return ways;
        }
        static int MaxRoads(int vilages)
        {
            int maxRoads = 0;
            for(int i = vilages-1; i > 0; i--) 
            { 
                maxRoads+=i;
            }
            return maxRoads;
        }
        static int FindMinNumOfMercenary(List<List<List<List<int>>>> compatibleWays)
        {
            int minNumOfMercenary = compatibleWays.Count;
            if (minNumOfMercenary == 0)
            {
                Console.WriteLine("There are no roads between the villages!");
                return minNumOfMercenary;
            }
            var way = compatibleWays[0];

            for(int i = 0; i < compatibleWays.Count; i++)
            {
                if (compatibleWays[i].Count < minNumOfMercenary)
                {
                    minNumOfMercenary = compatibleWays[i].Count;
                    way = compatibleWays[i];
                }
            }

            Console.Write("Ways of mercenaries: ");
            int counter = 1;
            foreach(var part in way)
            {
                Console.WriteLine("\nMercenary {0}: ", counter++);
                foreach (var road in part)
                {
                    Console.Write("[ ");
                    foreach (var letter in road)
                    {
                        Console.Write(letter);
                        Console.Write(" ");
                    }
                    Console.Write("]");
                }       
            }
            return minNumOfMercenary;
        }
        private static List<List<List<int>>> CutDublicatesWays(List<List<List<int>>> ways)
        {
            List<List<List<int>>> waysCopy = new List<List<List<int>>>();
            for(int i = 0; i < ways.Count; i++)
            {
                if (!waysCopy.Contains(ways[i]))
                {
                    waysCopy.Add(ways[i]);
                }
            }

            return waysCopy;
        }

        public static void Run(string input, string output)
        {
            var roads = ReadFile(input);
            var ways = FindAllWays(roads);
            ways = CutDublicatesWays(ways);
            var compatibleWays = new List<List<List<List<int>>>>();
            compatibleWays = FindWays(new List<List<List<int>>>(), ways, compatibleWays, roads);
            int minMer = FindMinNumOfMercenary(compatibleWays);
            File.WriteAllText(output, Convert.ToString(minMer));
        }

        // static void Main(string[] args)
        // {
        //     string input = "INPUT2.txt";
        //     string output = "OUTPUT.txt";
        //     Run(input, output);
        // }
    }
}
