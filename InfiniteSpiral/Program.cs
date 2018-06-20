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
        static public Dictionary<int, Location> spiralDefinition = new Dictionary<int, Location>()
        {
            { 1, new Location(0, 0)},
            { 2, new Location(1, 0)}
        };
        static public Dictionary<string, Int64> spiralValueDefinition = new Dictionary<string, Int64>()
        {
            {"0,0", 1},
            {"1,0", 1}
        };
        static public Dictionary<string, Location> compassDefinition = new Dictionary<string, Location>()
        {
            { "right", new Location(1, 0)},
            { "left", new Location(-1, 0)},
            { "up", new Location(0, 1)},
            { "down", new Location(0, -1)},
            { "upright", new Location(1, 1)},
            { "downleft", new Location(-1, -1)},
            { "downright", new Location(1, -1)},
            { "upleft", new Location(-1, 1) }
        };


        static void Main(string[] args)
        {
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
                        Console.WriteLine("Data from square {0} is carried {1} steps and has a value of {2}", dataInt, dataDistance, spiralValueDefinition[dataPortLocation.x + "," + dataPortLocation.y]);

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
            bool first = false;
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
                    if (spiralValueDefinition[spiralDefinition[newDataInt].x + "," + spiralDefinition[newDataInt].y] > dataInt && first == false)
                    {
                        Console.WriteLine("First Value Greater than {0} is {1} for {2}", dataInt, spiralValueDefinition[spiralDefinition[newDataInt].x + "," + spiralDefinition[newDataInt].y], newDataInt);
                        first = true;
                    }
                    finalLocation.SetLocation(spiralDefinition[newDataInt].x, spiralDefinition[newDataInt].y);
                }


            }
            return finalLocation;
        }

        private static void UpdateSpiralDefinition(Location startLocation, int newDataInt)
        {
            Int64 value = 0;
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

                Location downLeftLocation = new Location(upLocation.x + compassDefinition["downleft"].x, upLocation.y + compassDefinition["downleft"].y);
                Location downRightLocation = new Location(upLocation.x + compassDefinition["downright"].x, upLocation.y + compassDefinition["downright"].y);
                Location newLeftLocation = new Location(upLocation.x + compassDefinition["left"].x, upLocation.y + compassDefinition["left"].y);
                Location newDownLocation = new Location(upLocation.x + compassDefinition["down"].x, upLocation.y + compassDefinition["down"].y);
                Location upLeftLocation = new Location(upLocation.x + compassDefinition["upleft"].x, upLocation.y + compassDefinition["upleft"].y);

                bool downRight = spiralDefinition.Values.Any(a => a.x.Equals(downRightLocation.x) && a.y.Equals(downRightLocation.y));
                bool downLeft = spiralDefinition.Values.Any(a => a.x.Equals(downLeftLocation.x) && a.y.Equals(downLeftLocation.y));
                bool newLeft = spiralDefinition.Values.Any(a => a.x.Equals(newLeftLocation.x) && a.y.Equals(newLeftLocation.y));
                bool newDown = spiralDefinition.Values.Any(a => a.x.Equals(newDownLocation.x) && a.y.Equals(newDownLocation.y));
                bool upLeft = spiralDefinition.Values.Any(a => a.x.Equals(upLeftLocation.x) && a.y.Equals(upLeftLocation.y));


                // Get the new left value if the spot is filled
                if (newLeft)
                {
                    string newLeftString = newLeftLocation.x + "," + newLeftLocation.y;
                    value += spiralValueDefinition[newLeftString];
                }

                // If down, down to the left or down to the right spot is filled get their value
                if (newDown)
                {
                    string newDownString = newDownLocation.x + "," + newDownLocation.y;
                    value += spiralValueDefinition[newDownString];
                }

                if (downLeft)
                {
                    string downLeftString = downLeftLocation.x + "," + downLeftLocation.y;
                    value += spiralValueDefinition[downLeftString];
                }

                if (downRight)
                {
                    string downRightString = downRightLocation.x + "," + downRightLocation.y;
                    value += spiralValueDefinition[downRightString];
                }

                if (upLeft)
                {
                    string upLeftString = upLeftLocation.x + "," + upLeftLocation.y;
                    value += spiralValueDefinition[upLeftString];
                }


                string upString = upLocation.x + "," + upLocation.y;
                spiralValueDefinition[upString] = value;
            }
            else if (left == false && up == false && down == true)
            {
                // Go Left
                spiralDefinition[newDataInt] = leftLocation;

                Location downLeftLocation = new Location(leftLocation.x + compassDefinition["downleft"].x, leftLocation.y + compassDefinition["downleft"].y);
                Location downRightLocation = new Location(leftLocation.x + compassDefinition["downright"].x, leftLocation.y + compassDefinition["downright"].y);
                Location newDownLocation = new Location(leftLocation.x + compassDefinition["down"].x, leftLocation.y + compassDefinition["down"].y);
                Location newRightLocation = new Location(leftLocation.x + compassDefinition["right"].x, leftLocation.y + compassDefinition["right"].y);

                bool downRight = spiralDefinition.Values.Any(a => a.x.Equals(downRightLocation.x) && a.y.Equals(downRightLocation.y));
                bool downLeft = spiralDefinition.Values.Any(a => a.x.Equals(downLeftLocation.x) && a.y.Equals(downLeftLocation.y));
                bool newDown = spiralDefinition.Values.Any(a => a.x.Equals(newDownLocation.x) && a.y.Equals(newDownLocation.y));
                bool newRight = spiralDefinition.Values.Any(a => a.x.Equals(newRightLocation.x) && a.y.Equals(newRightLocation.y));


                if (newRight)
                {
                    string newRightString = newRightLocation.x + "," + newRightLocation.y;
                    value += spiralValueDefinition[newRightString];
                }
                // Get the new down value if the spot is filled
                if (newDown)
                {
                    string newDownString = newDownLocation.x + "," + newDownLocation.y;
                    value += spiralValueDefinition[newDownString];
                }

                if (downLeft)
                {
                    string downLeftString = downLeftLocation.x + "," + downLeftLocation.y;
                    value += spiralValueDefinition[downLeftString];
                }

                if (downRight)
                {
                    string downRightString = downRightLocation.x + "," + downRightLocation.y;
                    value += spiralValueDefinition[downRightString];
                }

                string leftString = leftLocation.x + "," + leftLocation.y;
                spiralValueDefinition[leftString] = value;
            }
            else if (right == true && left == false && down == false)
            {
                // Go Down
                spiralDefinition[newDataInt] = downLocation;

                Location upRightLocation = new Location(downLocation.x + compassDefinition["upright"].x, downLocation.y + compassDefinition["upright"].y);
                Location downRightLocation = new Location(downLocation.x + compassDefinition["downright"].x, downLocation.y + compassDefinition["downright"].y);
                Location newRightLocation = new Location(downLocation.x + compassDefinition["right"].x, downLocation.y + compassDefinition["right"].y);
                Location newUpLocation = new Location(downLocation.x + compassDefinition["up"].x, downLocation.y + compassDefinition["up"].y);


                bool downRight = spiralDefinition.Values.Any(a => a.x.Equals(downRightLocation.x) && a.y.Equals(downRightLocation.y));
                bool upRight = spiralDefinition.Values.Any(a => a.x.Equals(upRightLocation.x) && a.y.Equals(upRightLocation.y));
                bool newRight = spiralDefinition.Values.Any(a => a.x.Equals(newRightLocation.x) && a.y.Equals(newRightLocation.y));
                bool newUp = spiralDefinition.Values.Any(a => a.x.Equals(newUpLocation.x) && a.y.Equals(newUpLocation.y));



                // Get the right value if the spot is filled
                if (newRight)
                {
                    string newRightString = newRightLocation.x + "," + newRightLocation.y;
                    value += spiralValueDefinition[newRightString];
                }

                // If up, up to the right or down to the right spot is filled get their value
                if (newUp)
                {
                    string newUpString = newUpLocation.x + "," + newUpLocation.y;
                    value += spiralValueDefinition[newUpString];
                }

                if (upRight)
                {
                    string upRightString = upRightLocation.x + "," + upRightLocation.y;
                    value += spiralValueDefinition[upRightString];
                }

                if (downRight)
                {
                    string downRightString = downRightLocation.x + "," + downRightLocation.y;
                    value += spiralValueDefinition[downRightString];
                }

                string downString = downLocation.x + "," + downLocation.y;
                spiralValueDefinition[downString] = value;
            }
            else if (right == false && up == true && down == false)
            {
                // Go Right
                spiralDefinition[newDataInt] = rightLocation;

                Location upRightLocation = new Location(rightLocation.x + compassDefinition["upright"].x, rightLocation.y + compassDefinition["upright"].y);
                Location upLeftLocation = new Location(rightLocation.x + compassDefinition["upleft"].x, rightLocation.y + compassDefinition["upleft"].y);
                Location newUpLocation = new Location(rightLocation.x + compassDefinition["up"].x, rightLocation.y + compassDefinition["up"].y);
                Location newLeftLocation = new Location(rightLocation.x + compassDefinition["left"].x, rightLocation.y + compassDefinition["left"].y);


                bool upRight = spiralDefinition.Values.Any(a => a.x.Equals(upRightLocation.x) && a.y.Equals(upRightLocation.y));
                bool upLeft = spiralDefinition.Values.Any(a => a.x.Equals(upLeftLocation.x) && a.y.Equals(upLeftLocation.y));
                bool newUp = spiralDefinition.Values.Any(a => a.x.Equals(newUpLocation.x) && a.y.Equals(newUpLocation.y));
                bool newLeft = spiralDefinition.Values.Any(a => a.x.Equals(newLeftLocation.x) && a.y.Equals(newLeftLocation.y));

                if (newLeft)
                {
                    string newLeftSring = newLeftLocation.x + "," + newLeftLocation.y;
                    value += spiralValueDefinition[newLeftSring];
                }

                // Get the up value if it is filled
                if (newUp)
                {
                    string newUpString = newUpLocation.x + "," + newUpLocation.y;
                    value += spiralValueDefinition[newUpString];
                }

                // If up to the right, up to left spot is filled get their value
                if (upRight)
                {
                    string upRightString = upRightLocation.x + "," + upRightLocation.y;
                    value += spiralValueDefinition[upRightString];
                }

                if (upLeft)
                {
                    string upLeftString = upLeftLocation.x + "," + upLeftLocation.y;
                    value += spiralValueDefinition[upLeftString];
                }

                string rightString = rightLocation.x + "," + rightLocation.y;
                spiralValueDefinition[rightString] = value;
            }

        }

    }
}
