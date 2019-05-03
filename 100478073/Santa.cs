using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Timers;

namespace Ethan_100478073
{
    public class Santa : ICritterController
    {
        public string Name { get; set; }
        public Send Responder { get; set; }
        public Send Logger { get; set; }
        public string Filepath { get; set; }
        private void Log(string message)
        {
            if (Logger == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                Logger(message);
            }
        }
        public Santa(string name)
        {
            Name = name;
        }
        public void LaunchUI()
        {
        }
        public int requestNumber = 1;
        Timer lTime;
        Timer PatrolTime;
        Point hatchLocation = new Point(0, 0);
        public bool needToEscape = false;
        public bool needToEat = false;

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    lTime = new Timer();
                    lTime.Interval = 2000;
                    lTime.Elapsed += (obj, evt) => Tick();
                    lTime.Start();
                    Responder("SCAN:1");
                    break;
                case "SHUTDOWN":
                    lTime.Stop();
                    PatrolTime.Stop();
                    break;
                case "SEE":
                    Responder("SCAN:2");
                    break;
                case "SCAN":
                    Scan(message);
                    break;
                case "ENERGY":
                    CheckEnergyLevel(message);
                    break;
                case "LEVEL_TIME_REMAINING":
                    CheckLevelTime(message);
                    break;
                case "SCORED":
                    CheckCurrentScore(message);
                    break;
                case "FIGHT":
                    Responder("RANDOM_DESTINATION");
                    break;
                case "BUMP":
                    Responder("RANDOM_DESTINATION");
                    Responder("SCAN:2");
                    break;
                case "ERROR":
                    Log(message);
                    break;
            }
        } // Message controller
        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        } //converts string into Point
        private void Tick()
        {
            Responder("GET_LEVEL_TIME_REMAINING");
            Responder("GET_ENERGY");
            Responder("SCORED");
        }
        private void CollectGift(Point location, int speed)
        {
            if (!needToEscape && !needToEat)
            {
                Responder("SET_DESTINATION:" + location.X + ":" + location.Y + ":" + 5);
            }
        } // collect gift
        private void EatFood(Point foodLocation)
        {
            if (needToEscape)
            {
                Responder("SET_DESTINATION:" + hatchLocation.X + ":" + hatchLocation.Y + ":" + 5);
            }
            else if (!needToEscape)
            {
                Responder("SET_DESTINATION:" + foodLocation.X + ":" + foodLocation.Y + ":" + 5);
            }
        } //eats food
        private void Escape(Point coordinate, int speed)
        {
            Responder("SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed);
        }  //Run for the goal
        private void CheckEnergyLevel(string energyLevel)
        {
            string[] splitEnergyFromString = energyLevel.Split(':');
            int currentEnergy;
            Int32.TryParse(splitEnergyFromString[2], out currentEnergy); //Convert string to int & sets timeRemaining
            if (currentEnergy <= 50)
            {
                needToEat = true;
            }
            else
            {
                needToEat = false;
            }
            //if (currentEnergy <= 5)
            //{
            //    needToEscape = true;
            //    onDuty = false;
            //    Escape(hatchLocation, 10);
            //}
        } //fetches energy levels
        private void CheckLevelTime(string currentRemainingTime)
        {
            string[] splitTimeFromString = currentRemainingTime.Split(':');
            Int32.TryParse(splitTimeFromString[2], out int timeRemaining); //Convert string to int & sets timeRemaining
            if (timeRemaining <= 170)
            {
                needToEscape = true;
                Escape(hatchLocation, 10);
            }
        } //fetches remaining level time
        private void CheckCurrentScore(string currentScore) // Need to implement custom score set
        {
            string[] fetchScore = currentScore.Split(':');
            Int32.TryParse(fetchScore[2], out int score); 
            if ( score >= 10)
            {
                needToEscape = true;
                Escape(hatchLocation, 10);
                Log("My sack is full, Merry Christmas!");
                //timeToPatrol = true;
            }
        }
        private void Scan(string message)
        {
            string[] newlinePartition = message.Split('\n');
            string[] iCanSmell = newlinePartition[1].Split('\t');
            foreach (string item in iCanSmell)
            {
                string[] itemAttributes = item.Split(':');
                Point location = PointFrom(itemAttributes[1]);
                switch (itemAttributes[0])
                {
                    case "EscapeHatch":
                        Log($"I can smell the hatch at {location} and I must guard it!");
                        hatchLocation = location;
                        break;
                    case "Food":
                        Log($"There's food at {location}");
                        if (needToEat)
                        {
                            EatFood(location);
                        }
                        break;
                    case "Gift":
                        CollectGift(location, 5);
                        break;
                        

                }
            }
        } // scans the environment - returns found objectives
    }
}
