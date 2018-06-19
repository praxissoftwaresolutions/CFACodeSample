using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiniteSpiral
{
    class Location
    {
        public int x = 0;
        public int y = 0;

        public Location()
        {

        }

        public Location(int x, int y)
        {
            SetLocation(x, y);
        }

        public void SetLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Program
    {
        static public Dictionary<int, Location> spiralDefinition = new Dictionary<int, Location>();
        static public Dictionary<string, Location> compassDefinition = new Dictionary<string, Location>();


        static void Main(string[] args)
        {
            compassDefinition.Add("right", new Location(1, 0));
            compassDefinition.Add("left", new Location(-1, 0));
            compassDefinition.Add("up", new Location(0, 1));
            compassDefinition.Add("down", new Location(0, -1));

            spiralDefinition.Add(1, new Location());
            spiralDefinition.Add(2, new Location(1, 0));

            Location accessPortLocation = new Location();
            string dataString = "";

            // Prompt user for the location of data
            Console.WriteLine("Enter Data Location (To quit, type 'exit'): ");
            dataString = Console.ReadLine().ToLower();

            while (dataString != "exit")
            {
                try
                {
                    int dataInt;
                    int dataDistance;
                    //Check that the string entered is numeric and a positive integer
                    if (int.TryParse(dataString, out dataInt) && dataInt > 0)
                    {
                        // Calculate Data Location
                        Location dataPortLocation = CalculateDataLocation(dataInt);

                        // Calculate the number of steps
                        dataDistance = Math.Abs(accessPortLocation.x - dataPortLocation.x) + Math.Abs(dataPortLocation.y - accessPortLocation.y);
                        Console.WriteLine("Data from square {0} is carried {1} steps.", dataInt, dataDistance);
                    }
                    else
                    {
                        // Throw Exception to inform the user of improper input
                        throw new ArgumentException("Error: Data Location entered must be a positing integer.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                    // Prompt user for the location of data
                    Console.WriteLine("Enter Data Location(To quit, type 'exit'): ");
                    dataString = Console.ReadLine().ToLower();

                    if (dataString.Equals("exit"))
                    {
                        return;
                    }
                }

                Console.WriteLine("Enter Data Location(To quit, type 'exit'): ");
                dataString = Console.ReadLine().ToLower();
            }
        }

        private static Location CalculateDataLocation(int dataInt)
        {
            Location finalLocation = new Location();
            for (int n = 1; n <= dataInt; n++)
            {
                Location location;
                if (spiralDefinition.TryGetValue(n, out location))
                {
                    if (n == dataInt)
                    {
                        finalLocation = location;
                        return finalLocation;
                    }
                }
                else
                {
                    // Update spiralDefinition
                    int prevDataInt = n - 1;
                    int newDataInt = n;

                    UpdateSpiralDefinition(spiralDefinition[prevDataInt], newDataInt);
                    finalLocation.SetLocation(spiralDefinition[newDataInt].x, spiralDefinition[newDataInt].y);
                }
            }
            return finalLocation;
        }

        private static void UpdateSpiralDefinition(Location startLocation, int newDataInt)
        {
            Location rightLocation = new Location(startLocation.x + compassDefinition["right"].x, startLocation.y + compassDefinition["right"].y);
            Location leftLocation = new Location(startLocation.x + compassDefinition["left"].x, startLocation.y + compassDefinition["left"].y);
            Location upLocation = new Location(startLocation.x + compassDefinition["up"].x, startLocation.y + compassDefinition["up"].y);
            Location downLocation = new Location(startLocation.x + compassDefinition["down"].x, startLocation.y + compassDefinition["down"].y);

            bool right = spiralDefinition.Values.Any(a => a.x.Equals(rightLocation.x) && a.y.Equals(rightLocation.y));
            bool left = spiralDefinition.Values.Any(a => a.x.Equals(leftLocation.x) && a.y.Equals(leftLocation.y));
            bool up = spiralDefinition.Values.Any(a => a.x.Equals(upLocation.x) && a.y.Equals(upLocation.y));
            bool down = spiralDefinition.Values.Any(a => a.x.Equals(downLocation.x) && a.y.Equals(downLocation.y));

            if (right == false && left == true && up == false)
            {
                // Go Up
                spiralDefinition[newDataInt] = upLocation;
            }
            else if (left == false && up == false && down == true)
            {
                // Go Left
                spiralDefinition[newDataInt] = leftLocation;
            }
            else if (right == true && left == false && down == false)
            {
                // Go Down
                spiralDefinition[newDataInt] = downLocation;
            }
            else if (right == false && up == true && down == false)
            {
                // Go Right
                spiralDefinition[newDataInt] = rightLocation;
            }
        }

    }
}
