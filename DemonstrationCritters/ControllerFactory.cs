﻿using CritterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonstrationCritters
{
    class ControllerFactory : ICritterControllerFactory
    {
        public string Author => "Dave Voorhis";

        public ICritterController[] GetCritterControllers()
        {
            List<ICritterController> controllers = new List<ICritterController>();
            for (int i = 0; i < 5; i++)
            {
                //controllers.Add(new Wanderer("Wanderer" + (i + 1)));
                //controllers.Add(new Chaser("Chaser" + (i + 1)));
                //controllers.Add(new Brute("Betty Brute"));
                //controllers.Add(new Brute("Bitty Brute"));
            }
            return controllers.ToArray();
        }
    }
}
