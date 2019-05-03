using CritterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ethan_100478073
{
    class ControllerFactory : ICritterControllerFactory
    {
        public string Author => "100478073";

        public ICritterController[] GetCritterControllers()
        {
            List<ICritterController> controllers = new List<ICritterController>();
            for (int i = 0; i < 5; i++)
            {
                controllers.Add(new SpeedRunner("Speedy Sally"));
                controllers.Add(new Santa("Santa Clause"));
                controllers.Add(new Brute("Betty Brute"));
            }
            return controllers.ToArray();
        }
    }
}
