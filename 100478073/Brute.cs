using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace Ethan_100478073
{
    public class Brute : ICritterController
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
        public Brute(string name)
        {
            Name = name;
        }
        public void LaunchUI()
        {
        }
        Point escapeLocation = new Point(1, 1);
        Point foodLocation = new Point(1, 1);
        public int seekSpeed = 6;
        public int runForGoalSpeed = 10;

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    break;
                case "SCAN":
                    Scan(message);
                    break;
                case "BUMP":
                    Responder("RANDOM_DESTINATION");
                    break;
                case "SEE":
                    Responder("SCAN");
                    SeekCritter(message);
                    break;
                case "FIGHT":
                    SeekCritter(message);
                    Responder("GET_ENERGY");
                    Responder("GET_HEALTH");
                    Responder("GET_LEVEL_DURATION");
                    break;
                case "ENERGY":
                    CheckCurrentEnergy(message);
                    break;
                case "HEALTH":
                    CheckCurrentHealth(message);
                    break;
                case "LEVEL_DURATION":
                    CheckTimeRemaining(message);
                    break;
                case "CRASHED":
                    Log(message);
                    break;
                case "ERROR":
                    Log(message);
                    break;
            }
        }

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }
        private void RunForGoal(Point coordinate, int speed)
        {
            Responder("SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed);
        } //
        private void SeekCritter(string seeMessage)
        {
            string[] newlinePartition = seeMessage.Split('\n');
            string[] iCanSmell = newlinePartition[1].Split('\t');
            foreach (string item in iCanSmell)
            {
                string[] itemAttributes = item.Split(':');
                Point location = PointFrom(itemAttributes[1]);
                switch (itemAttributes[0])
                {
                    case "Critter":
                        Log($"I can see an enemy! ATTACK!");
                        Responder($"SET_DESTINATION:{location.X}:{location.Y}:{seekSpeed}");
                        break;
                    case "Food":
                        Responder("GET_ENERGY");
                        Responder("GET_HEALTH");
                        break;
                }
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
                        escapeLocation = location;
                        break;
                    case "Food":
                        Log("I need to eat something..");
                        foodLocation = location;
                        break;
                }
            }
        }
        private void CheckCurrentEnergy(string energyLevel)
        {
            string[] splitEnergyFromString = energyLevel.Split(':');
            int currentEnergy;
            Int32.TryParse(splitEnergyFromString[2], out currentEnergy); //Convert string to int & sets timeRemaining
            if (currentEnergy <= 50)
            {
                Responder($"SET_DESTINATION:{foodLocation.X}:{foodLocation.Y}:10");
            }
        }
        private void CheckCurrentHealth(string healthLevel)
        {
            string[] splitHealthFromString = healthLevel.Split(':');
            int health;
            Int32.TryParse(splitHealthFromString[2], out health); //Convert string to int & sets timeRemaining
            if (health <= 25)
            {
                Responder($"SET_DESTINATION:{foodLocation.X}:{foodLocation.Y}:10");
            }

        }
        private void CheckTimeRemaining(string currentRemainingTime)
        {
            string[] splitTimeFromString = currentRemainingTime.Split(':');
            Int32.TryParse(splitTimeFromString[2], out int timeRemaining); //Convert string to int & sets timeRemaining
            if (timeRemaining <= 30)
            {
                RunForGoal(escapeLocation, 10);
            }
        }
    }
}
