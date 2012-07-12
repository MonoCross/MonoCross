using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;

namespace $safeprojectname$.Views
{
    public class MessageView : MXView<string>
    {
        public override void Render()
        {
            System.Console.WriteLine(Model);
        }
    }
}
