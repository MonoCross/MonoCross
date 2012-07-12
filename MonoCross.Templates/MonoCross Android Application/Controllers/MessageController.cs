using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;

namespace $safeprojectname$.Controllers
{
    public class MessageController : MXController<string>
    {
        public override string Load(Dictionary<string,string> parameters)
        {
            Model = "Hello World";
            return ViewPerspective.Default;
        }
    }
}
