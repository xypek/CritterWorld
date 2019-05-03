using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;

namespace DemonstrationCritters
{
    public class Brute : ICritterController
    {
        Point goal = new Point(-1, -1);
        System.Timers.Timer getInfoTimer;
        bool headingForGoal = false;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LaunchUI()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Receive(string message)
        {
            throw new NotImplementedException();
        }

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
    }
}
