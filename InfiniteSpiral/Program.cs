using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiniteSpiral
{
    class DataLocation
    {
        public int x = 0;
        public int y = 0;

        public DataLocation()
        {

        }

        public DataLocation(int x, int y)
        {
            SetLocation(x, y);
        }

        public void SetLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class TestDataLocation
    {
        // Dictionary containing the cartesian coordinates of the Data Locations
        static public Dictionary<int, DataLocation> spiralDataLocations = new Dictionary<int, DataLocation>()
        {
            // Initialize the spiralDataLocations with coordinates for Data Location 1 and Data Location 2
            { 1, new DataLocation(0, 0)},
            { 2, new DataLocation(1, 0)}
        };

        // Dictionary containing the Values associated with cartesian coordinates of a Data Location
        static public Dictionary<string, Int64> spiralDataValues = new Dictionary<string, Int64>()
        {
            // Initialize the spiralDataValues with values for Data Locations 0,0 (Data Location 1) and 1,0 (Data Location 2)
            {"0,0", 1},
            {"1,0", 1}
        };

        // Dictionary containing the cartesian representation of memory directions possible
        static public Dictionary<string, DataLocation> memoryDirections = new Dictionary<string, DataLocation>()
        {
            { "right", new DataLocation(1, 0)},
            { "left", new DataLocation(-1, 0)},
            { "up", new DataLocation(0, 1)},
            { "down", new DataLocation(0, -1)},
            { "upright", new DataLocation(1, 1)},
            { "downleft", new DataLocation(-1, -1)},
            { "downright", new DataLocation(1, -1)},
            { "upleft", new DataLocation(-1, 1) }
        };


        static void Main(string[] args)
        {
            DataLocation accessPortLocation = new DataLocation();
            string dataString = "";

            // Prompt user for the Data Location
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
                        // Calculate cartesian coordinates for a given Data Location
                        DataLocation dataPortLocation = CalculateCartesianCoordinate(dataInt);

                        // Calculate the number of steps
                        dataDistance = Math.Abs(accessPortLocation.x - dataPortLocation.x) + Math.Abs(dataPortLocation.y - accessPortLocation.y);
                        Console.WriteLine("Data from data location {0} is carried {1} steps and has a value of {2}", dataInt, dataDistance, spiralDataValues[dataPortLocation.x + "," + dataPortLocation.y]);
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

        private static DataLocation CalculateCartesianCoordinate(int dataInt)
        {
            DataLocation finalLocation = new DataLocation();
            bool first = false;
            for (int n = 1; n <= dataInt; n++)
            {
                DataLocation location;
                if (spiralDataLocations.TryGetValue(n, out location))
                {
                    if (n == dataInt)
                    {
                        finalLocation = location;
                        return finalLocation;
                    }
                }
                else
                {
                    // Update spiralDataLocations
                    int prevDataInt = n - 1;
                    int newDataInt = n;

                    UpdateSpiralDataLocations(spiralDataLocations[prevDataInt], newDataInt);

                    if (spiralDataValues[spiralDataLocations[newDataInt].x + "," + spiralDataLocations[newDataInt].y] > dataInt && first == false)
                    {
                        Console.WriteLine("First Value Greater than {0} is {1} for data location {2}", dataInt, spiralDataValues[spiralDataLocations[newDataInt].x + "," + spiralDataLocations[newDataInt].y], newDataInt);
                        first = true;
                    }
                    finalLocation.SetLocation(spiralDataLocations[newDataInt].x, spiralDataLocations[newDataInt].y);
                }


            }
            return finalLocation;
        }


        private static void UpdateSpiralDataLocations(DataLocation startLocation, int newDataInt)
        {
            Int64 value = 0;

            // Generate potential Data Locations
            DataLocation rightLocation = GenerateLocation(startLocation, "right");
            DataLocation leftLocation = GenerateLocation(startLocation, "left");
            DataLocation upLocation = GenerateLocation(startLocation, "up");
            DataLocation downLocation = GenerateLocation(startLocation, "down");

            // Check whether the potential Data Locations are occupied
            bool right = CheckLocationAvailability(rightLocation);
            bool left = CheckLocationAvailability(leftLocation);
            bool up = CheckLocationAvailability(upLocation);
            bool down = CheckLocationAvailability(downLocation);

            // Identify the position of the new Data Location in reference to the starting Data Location
            if (right == false && left == true && up == false)
            {
                // Place up
                spiralDataLocations[newDataInt] = upLocation;

                // Identify potential adjacent Data Locations
                DataLocation downLeftLocation = GenerateLocation(upLocation, "downleft");
                DataLocation downRightLocation = GenerateLocation(upLocation, "downright");
                DataLocation newLeftLocation = GenerateLocation(upLocation, "left");
                DataLocation newDownLocation = GenerateLocation(upLocation, "down");
                DataLocation upLeftLocation = GenerateLocation(upLocation, "upleft");

                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newLeftLocation))
                {
                    string newLeftString = newLeftLocation.x + "," + newLeftLocation.y;
                    value += spiralDataValues[newLeftString];
                }

                if (CheckLocationAvailability(newDownLocation))
                {
                    string newDownString = newDownLocation.x + "," + newDownLocation.y;
                    value += spiralDataValues[newDownString];
                }

                if (CheckLocationAvailability(downLeftLocation))
                {
                    string downLeftString = downLeftLocation.x + "," + downLeftLocation.y;
                    value += spiralDataValues[downLeftString];
                }

                if (CheckLocationAvailability(downRightLocation))
                {
                    string downRightString = downRightLocation.x + "," + downRightLocation.y;
                    value += spiralDataValues[downRightString];
                }

                if (CheckLocationAvailability(upLeftLocation))
                {
                    string upLeftString = upLeftLocation.x + "," + upLeftLocation.y;
                    value += spiralDataValues[upLeftString];
                }

                // Update the Data Value for the New Data Location
                string upString = upLocation.x + "," + upLocation.y;
                spiralDataValues[upString] = value;
            }
            else if (left == false && up == false && down == true)
            {
                // Place Left
                spiralDataLocations[newDataInt] = leftLocation;

                // Identify potential adjacent Data Locations
                DataLocation downLeftLocation = GenerateLocation(leftLocation, "downleft");
                DataLocation downRightLocation = GenerateLocation(leftLocation, "downright");
                DataLocation newDownLocation = GenerateLocation(leftLocation, "down");
                DataLocation newRightLocation = GenerateLocation(leftLocation, "right");

                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newRightLocation))
                {
                    string newRightString = newRightLocation.x + "," + newRightLocation.y;
                    value += spiralDataValues[newRightString];
                }
                
                if (CheckLocationAvailability(newDownLocation))
                {
                    string newDownString = newDownLocation.x + "," + newDownLocation.y;
                    value += spiralDataValues[newDownString];
                }

                if (CheckLocationAvailability(downLeftLocation))
                {
                    string downLeftString = downLeftLocation.x + "," + downLeftLocation.y;
                    value += spiralDataValues[downLeftString];
                }

                if (CheckLocationAvailability(newRightLocation))
                {
                    string downRightString = downRightLocation.x + "," + downRightLocation.y;
                    value += spiralDataValues[downRightString];
                }

                // Update the Data Value for the New Data Location
                string leftString = leftLocation.x + "," + leftLocation.y;
                spiralDataValues[leftString] = value;
            }
            else if (right == true && left == false && down == false)
            {
                // Place Down
                spiralDataLocations[newDataInt] = downLocation;

                // Identify potential adjacent Data Locations
                DataLocation upRightLocation = GenerateLocation(downLocation, "upright");
                DataLocation downRightLocation = GenerateLocation(downLocation, "downright");
                DataLocation newRightLocation = GenerateLocation(downLocation, "right");
                DataLocation newUpLocation = GenerateLocation(downLocation, "up");

                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newRightLocation))
                {
                    string newRightString = newRightLocation.x + "," + newRightLocation.y;
                    value += spiralDataValues[newRightString];
                }

                if (CheckLocationAvailability(newUpLocation))
                {
                    string newUpString = newUpLocation.x + "," + newUpLocation.y;
                    value += spiralDataValues[newUpString];
                }

                if (CheckLocationAvailability(upRightLocation))
                {
                    string upRightString = upRightLocation.x + "," + upRightLocation.y;
                    value += spiralDataValues[upRightString];
                }

                if (CheckLocationAvailability(downRightLocation))
                {
                    string downRightString = downRightLocation.x + "," + downRightLocation.y;
                    value += spiralDataValues[downRightString];
                }

                // Update the Data Value for the New Data Location
                string downString = downLocation.x + "," + downLocation.y;
                spiralDataValues[downString] = value;
            }
            else if (right == false && up == true && down == false)
            {
                // Place to the Right of the starting Data Location
                spiralDataLocations[newDataInt] = rightLocation;

                // Identify potential adjacent Data Locations
                DataLocation upRightLocation = GenerateLocation(rightLocation, "upright");
                DataLocation upLeftLocation = GenerateLocation(rightLocation, "upleft");
                DataLocation newUpLocation = GenerateLocation(rightLocation, "up");
                DataLocation newLeftLocation = GenerateLocation(rightLocation, "left");

                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newLeftLocation))
                {
                    string newLeftSring = newLeftLocation.x + "," + newLeftLocation.y;
                    value += spiralDataValues[newLeftSring];
                }

                if (CheckLocationAvailability(newUpLocation))
                {
                    string newUpString = newUpLocation.x + "," + newUpLocation.y;
                    value += spiralDataValues[newUpString];
                }

                if (CheckLocationAvailability(upRightLocation))
                {
                    string upRightString = upRightLocation.x + "," + upRightLocation.y;
                    value += spiralDataValues[upRightString];
                }

                if (CheckLocationAvailability(upLeftLocation))
                {
                    string upLeftString = upLeftLocation.x + "," + upLeftLocation.y;
                    value += spiralDataValues[upLeftString];
                }

                // Update the Data Value for the New Data Location
                string rightString = rightLocation.x + "," + rightLocation.y;
                spiralDataValues[rightString] = value;
            }

        }

        private static bool CheckLocationAvailability(DataLocation location)
        {
            bool locationExist = spiralDataLocations.Values.Any(a => a.x.Equals(location.x) && a.y.Equals(location.y));
            return locationExist;
        }

        private static DataLocation GenerateLocation(DataLocation startLocation, string direction)
        {
            DataLocation location = new DataLocation(startLocation.x + memoryDirections[direction].x, startLocation.y + memoryDirections[direction].y); ;
            return location;
        }
    }
}
