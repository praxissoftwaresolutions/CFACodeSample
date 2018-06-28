using System;
using System.Collections.Generic;
using System.Linq;

namespace InfiniteSpiral
{
    class DataLocation
    {

        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public int dataNumber { get; private set; } = 0;

        // Dictionary containing the cartesian representation of memory directions possible
        public Dictionary<string, int[]> adjacentLocations = new Dictionary<string, int[]>()
        {
            { "right", new int[]{1, 0 } },
            { "left", new int[]{-1, 0 } },
            { "up", new int[]{0, 1 } },
            { "down", new int[]{0, -1 } },
            { "upright", new int[]{1, 1} },
            { "downleft", new int[]{-1, -1 } },
            { "downright", new int[]{1, -1 } },
            { "upleft", new int[]{-1, 1 } }
        };

        public DataLocation()
        {

        }

        public DataLocation(int num)
        {
            SetDataNumber(num);
        }

        public DataLocation(int x, int y, int num)
        {
            SetLocation(x, y);
            SetDataNumber(num);
        }

        public DataLocation(int x, int y)
        {
            SetLocation(x, y);
        }

        public void SetLocation(int x, int y)
        {
            this.X = x;
            this.Y = y;
            SetAdjacentLocations();

        }

        public void SetDataNumber(int num)
        {
            this.dataNumber = num;
        }

        public void SetAdjacentLocations()
        {
            this.adjacentLocations["right"][0] += this.X;
            this.adjacentLocations["right"][1] += this.Y;
            this.adjacentLocations["left"][0] += this.X;
            this.adjacentLocations["left"][1] += this.Y;
            this.adjacentLocations["up"][0] += this.X;
            this.adjacentLocations["up"][1] += this.Y;
            this.adjacentLocations["down"][0] += this.X;
            this.adjacentLocations["down"][1] += this.Y;
            this.adjacentLocations["upright"][0] += this.X;
            this.adjacentLocations["upright"][1] += this.Y;
            this.adjacentLocations["upleft"][0] += this.X;
            this.adjacentLocations["upleft"][1] += this.Y;
            this.adjacentLocations["downright"][0] += this.X;
            this.adjacentLocations["downright"][1] += this.Y;
            this.adjacentLocations["downleft"][0] += this.X;
            this.adjacentLocations["downleft"][1] += this.Y;
        }

        public int CalculateSteps(DataLocation endLocation)
        {
            int dataSteps = 0;

            // Calculate the number of steps a data location is from another data location
            dataSteps = Math.Abs(endLocation.X - this.X) + Math.Abs(this.Y - endLocation.Y);

            return dataSteps;
        }     
    }
    
    class TestDataLocation
    {
        // Dictionary containing the cartesian coordinates of the Data Locations
        static public Dictionary<int, DataLocation> spiralDataLocations = new Dictionary<int, DataLocation>()
        {
            // Initialize the spiralDataLocations with coordinates for Data Location 1 and Data Location 2
            { 1, new DataLocation(0, 0, 1)},
            { 2, new DataLocation(1, 0, 2)}
        };

        // Dictionary containing the Values associated with cartesian coordinates of a Data Location
        static public Dictionary<string, Int64> spiralDataValues = new Dictionary<string, Int64>()
        {
            // Initialize the spiralDataValues with values for Data Locations 0,0 (Data Location 1) and 1,0 (Data Location 2)
            {"0,0", 1},
            {"1,0", 1}
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
                        // Create data location for integer entered
                        //DataLocation dataPortLocation = new DataLocation(dataInt);

                        // Calculate cartesian coordinates for a given Data Location
                        DataLocation dataPortLocation = CalculateCartesianCoordinate(dataInt);

                        // Calculate the number of steps
                        dataDistance = dataPortLocation.CalculateSteps(accessPortLocation);
                        Console.WriteLine("Data from data location {0} is carried {1} steps and has a value of {2}", dataInt, dataDistance, spiralDataValues[dataPortLocation.X + "," + dataPortLocation.Y]);
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
            DataLocation finalLocation = new DataLocation(dataInt);
            bool first = false;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
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

                    if (spiralDataValues[spiralDataLocations[newDataInt].X + "," + spiralDataLocations[newDataInt].Y] > dataInt && first == false)
                    {
                        Console.WriteLine("First Value Greater than {0} is {1} for data location {2}", dataInt, spiralDataValues[spiralDataLocations[newDataInt].X + "," + spiralDataLocations[newDataInt].Y], newDataInt);
                        first = true;
                    }
                    finalLocation.SetLocation(spiralDataLocations[newDataInt].X, spiralDataLocations[newDataInt].Y);
                }


            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            return finalLocation;
        }

        private static void UpdateSpiralDataLocations(DataLocation startLocation, int newDataInt)
        {
            Int64 value = 0;

            bool right = CheckLocationAvailability(startLocation.adjacentLocations["right"]);
            bool left = CheckLocationAvailability(startLocation.adjacentLocations["left"]);
            bool up = CheckLocationAvailability(startLocation.adjacentLocations["up"]);
            bool down = CheckLocationAvailability(startLocation.adjacentLocations["down"]);

            // Identify the position of the new Data Location in reference to the starting Data Location
            if (right == false && left == true && up == false)
            {
                // up, left, down, downleft, downright,upleft
                // Place up
                // Create new data location
                DataLocation newDataLocation = new DataLocation(startLocation.adjacentLocations["up"][0], startLocation.adjacentLocations["up"][1]);

                // Update spiral data locations dictionary
                spiralDataLocations[newDataInt] = newDataLocation;

                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if(CheckLocationAvailability(newDataLocation.adjacentLocations["left"]))
                {
                    // Create data location
                    DataLocation newLeftLocation = new DataLocation(newDataLocation.adjacentLocations["left"][0], newDataLocation.adjacentLocations["left"][1]);
                    string newLeftString = newLeftLocation.X + "," + newLeftLocation.Y;
                    value += spiralDataValues[newLeftString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["down"]))
                {
                    // Create data location
                    DataLocation newDownLocation = new DataLocation(newDataLocation.adjacentLocations["down"][0], newDataLocation.adjacentLocations["down"][1]);
                    string newDownString = newDownLocation.X + "," + newDownLocation.Y;
                    value += spiralDataValues[newDownString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["downleft"]))
                {
                    // Create data location
                    DataLocation downLeftLocation = new DataLocation(newDataLocation.adjacentLocations["downleft"][0], newDataLocation.adjacentLocations["downleft"][1]);
                    string downLeftString = downLeftLocation.X + "," + downLeftLocation.Y;
                    value += spiralDataValues[downLeftString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["downright"]))
                {
                    // Create data location
                    DataLocation downRightLocation = new DataLocation(newDataLocation.adjacentLocations["downright"][0], newDataLocation.adjacentLocations["downright"][1]);
                    string downRightString = downRightLocation.X + "," + downRightLocation.Y;
                    value += spiralDataValues[downRightString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["upleft"]))
                {
                    // Create data location
                    DataLocation upLeftLocation = new DataLocation(newDataLocation.adjacentLocations["upleft"][0], newDataLocation.adjacentLocations["upleft"][1]);
                    string upLeftString = upLeftLocation.X + "," + upLeftLocation.Y;
                    value += spiralDataValues[upLeftString];
                }

                // Update the Data Value for the New Data Location
                string upString = newDataLocation.X + "," + newDataLocation.Y;
                spiralDataValues[upString] = value;
            }
            else if (left == false && up == false && down == true)
            {
                // Place Left
                // Create new data location
                DataLocation newDataLocation = new DataLocation(startLocation.adjacentLocations["left"][0], startLocation.adjacentLocations["left"][1]);
                spiralDataLocations[newDataInt] = newDataLocation;
                
                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newDataLocation.adjacentLocations["right"]))
                {
                    // Create data location
                    DataLocation newRightLocation = new DataLocation(newDataLocation.adjacentLocations["right"][0], newDataLocation.adjacentLocations["right"][1]);
                    string newRightString = newRightLocation.X + "," + newRightLocation.Y;
                    value += spiralDataValues[newRightString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["down"]))
                {
                    // Create data location
                    DataLocation newDownLocation = new DataLocation(newDataLocation.adjacentLocations["down"][0], newDataLocation.adjacentLocations["down"][1]);
                    string newDownString = newDownLocation.X + "," + newDownLocation.Y;
                    value += spiralDataValues[newDownString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["downleft"]))
                {
                    // Create data location
                    DataLocation downLeftLocation = new DataLocation(newDataLocation.adjacentLocations["downleft"][0], newDataLocation.adjacentLocations["downleft"][1]);
                    string downLeftString = downLeftLocation.X + "," + downLeftLocation.Y;
                    value += spiralDataValues[downLeftString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["downright"]))
                {
                    // Create data location
                    DataLocation downRightLocation = new DataLocation(newDataLocation.adjacentLocations["downright"][0], newDataLocation.adjacentLocations["downright"][1]);
                    string downRightString = downRightLocation.X + "," + downRightLocation.Y;
                    value += spiralDataValues[downRightString];
                }

                // Update the Data Value for the New Data Location
                string leftString = newDataLocation.X + "," + newDataLocation.Y;
                spiralDataValues[leftString] = value;
            }
            else if (right == true && left == false && down == false)
            {
                // Place Down
                // Create new data location
                DataLocation newDataLocation = new DataLocation(startLocation.adjacentLocations["down"][0], startLocation.adjacentLocations["down"][1]);
                spiralDataLocations[newDataInt] = newDataLocation;

                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newDataLocation.adjacentLocations["right"]))
                {
                    // Create data location
                    DataLocation newRightLocation = new DataLocation(newDataLocation.adjacentLocations["right"][0], newDataLocation.adjacentLocations["right"][1]);
                    string newRightString = newRightLocation.X + "," + newRightLocation.Y;
                    value += spiralDataValues[newRightString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["up"]))
                {
                    // Create data location
                    DataLocation newUpLocation = new DataLocation(newDataLocation.adjacentLocations["up"][0], newDataLocation.adjacentLocations["up"][1]);
                    string newUpString = newUpLocation.X + "," + newUpLocation.Y;
                    value += spiralDataValues[newUpString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["upright"]))
                {
                    // Create data location
                    DataLocation upRightLocation = new DataLocation(newDataLocation.adjacentLocations["upright"][0], newDataLocation.adjacentLocations["upright"][1]);
                    string upRightString = upRightLocation.X + "," + upRightLocation.Y;
                    value += spiralDataValues[upRightString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["downright"]))
                {
                    // Create data location
                    DataLocation downRightLocation = new DataLocation(newDataLocation.adjacentLocations["downright"][0], newDataLocation.adjacentLocations["downright"][1]);
                    string downRightString = downRightLocation.X + "," + downRightLocation.Y;
                    value += spiralDataValues[downRightString];
                }

                // Update the Data Value for the New Data Location
                string downString = newDataLocation.X + "," + newDataLocation.Y;
                spiralDataValues[downString] = value;
            }
            else if (right == false && up == true && down == false)
            {
                // Place to the Right of the starting Data Location
                // Create new data location
                DataLocation newDataLocation = new DataLocation(startLocation.adjacentLocations["right"][0], startLocation.adjacentLocations["right"][1]);
                //spiralDataLocations[newDataInt] = rightLocation;
                spiralDataLocations[newDataInt] = newDataLocation;
                
                // Check whether the potential adjacent Data Locations are occupied.
                // If they are capture and sum the values associated with the Data Locations
                if (CheckLocationAvailability(newDataLocation.adjacentLocations["left"]))
                {
                    // Create data location
                    DataLocation newLeftLocation = new DataLocation(newDataLocation.adjacentLocations["left"][0], newDataLocation.adjacentLocations["left"][1]);
                    string newLeftSring = newLeftLocation.X + "," + newLeftLocation.Y;
                    value += spiralDataValues[newLeftSring];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["up"]))
                {
                    // Create data location
                    DataLocation newUpLocation = new DataLocation(newDataLocation.adjacentLocations["up"][0], newDataLocation.adjacentLocations["up"][1]);
                    string newUpString = newUpLocation.X + "," + newUpLocation.Y;
                    value += spiralDataValues[newUpString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["upright"]))
                {
                    // Create data location
                    DataLocation upRightLocation = new DataLocation(newDataLocation.adjacentLocations["upright"][0], newDataLocation.adjacentLocations["upright"][1]);
                    string upRightString = upRightLocation.X + "," + upRightLocation.Y;
                    value += spiralDataValues[upRightString];
                }

                if (CheckLocationAvailability(newDataLocation.adjacentLocations["upleft"]))
                {
                    // Create data location
                    DataLocation upLeftLocation = new DataLocation(newDataLocation.adjacentLocations["upleft"][0], newDataLocation.adjacentLocations["upleft"][1]);
                    string upLeftString = upLeftLocation.X + "," + upLeftLocation.Y;
                    value += spiralDataValues[upLeftString];
                }

                // Update the Data Value for the New Data Location
                string rightString = newDataLocation.X + "," + newDataLocation.Y;
                spiralDataValues[rightString] = value;
            }

        }

        private static bool CheckLocationAvailability(int[] location)
        {
            bool locationExist = spiralDataLocations.Values.Any(a => a.X.Equals(location[0]) && a.Y.Equals(location[1]));
            return locationExist;
        }

    }
}
