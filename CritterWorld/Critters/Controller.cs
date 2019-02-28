using CritterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critters
{
    class ControllerFactory : ICritterControllerFactory
    {
        public string Author => "Ethan Gilbert 100478073";

        public ICritterController[] GetCritterControllers()
        {
            List<ICritterController> controllers = new List<ICritterController>();
            for (int i = 0; i < 5; i++)
            {
                controllers.Add(new Brute("Betty Brute" + (i + 1)));
                controllers.Add(new Brute("Botty Brute" + (i + 1)));
            }
            return controllers.ToArray();
        }
    }
}
