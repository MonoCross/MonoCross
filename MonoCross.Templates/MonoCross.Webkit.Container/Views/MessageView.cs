using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MonoCross.Navigation;
using MonoCross.Webkit;

namespace $safeprojectname$.Views
{
    public class MessageView : MXWebkitView<string>
    {
        public override void Render()
        {
            HtmlGenericControl list = new HtmlGenericControl("div");
            list.Attributes.Add("class", "iMenu");

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "iArrow");
            ul.Attributes.Add("style", "background-color: #FFFFFF; color: #000000");

            HtmlGenericControl li = new HtmlGenericControl("li");
            li.InnerText = Model;
            ul.Controls.Add(li);
           
            list.Controls.Add(ul);
            WriteToResponse("CategoryList", "Categories", list);
        }
    }
}