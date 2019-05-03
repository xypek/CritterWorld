using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace Ethan_100478073
{
    public class SpeedRunner : ICritterController
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
        public SpeedRunner(string name)
        {
            Name = name;
        }
        public void LaunchUI()
        {
        }
        Point hatch = new Point(1, 1);

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "SEE":
                    Responder("SCAN:1");
                    break;
                case "BUMP":
                    Responder("RANDOM_DESTINATION");
                    break;
                case "FIGHT":
                    Responder("RANDOM_DESTINATION");
                    break;
                case "SCAN":
                    Scan(message);
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
        private void RunForHatch(Point coordinate, int speed)
        {
            Responder("SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed);
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
                    case "Nothing":
                        Responder("RANDOM_DESTINATION");
                        break;
                    case "EscapeHatch":
                        Log($"I can smell the hatch at {location}");
                        hatch = location;
                        RunForHatch(location, 10);
                        break;
                }
            }
        }
    }
}
