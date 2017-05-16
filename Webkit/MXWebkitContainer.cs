using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

using MonoCross.Navigation;

namespace MonoCross.Webkit
{
    public static class MXWebkitNavigationExtensions
    {
    }

    public class MXWebkitContainer : MXContainer
    {
        protected MXWebkitContainer(MXApplication theApp)
            : base(theApp)
        {
        }

        static MXWebkitContainer()
        {
            MXContainer.GetSessionId = delegate { return HttpContext.Current.Session.SessionID; };
        }

        public override MXViewMap Views
        {
            get
            {
                if (HttpContext.Current.Session["ViewMap"] == null)
                    HttpContext.Current.Session["ViewMap"] = new MXViewMap();
                return (MXViewMap)HttpContext.Current.Session["ViewMap"];
            }
        }

        public static void Navigate(string url, HttpRequestBase request)
        {
            Dictionary<string, string> parameters = GetParameters(request);
            //IMXController controller = Instance.GetController(url, parameters);
            SetModelFromParameters(url, parameters);
            Navigate(null, url, parameters);
        }

        protected static void SetModelFromParameters(string url, Dictionary<string, string> parameters)
        {
            IMXController controller = Instance.GetController(url, ref parameters);
            if (controller.GetModel() != null)
            {
                foreach (string key in parameters.Keys)
                {
                    SetPropertyOnModel(key, controller.GetModel(), parameters[key]);
                }
            }
        }

        protected static void SetPropertyOnModel(string parameter, object model, string value)
        {
            int position = parameter.IndexOf('.');
            if (position > -1)
            {
                PropertyInfo property = model.GetType().GetProperty(parameter.Substring(0, position));
                object newmodel = property.GetValue(model, null);
                SetPropertyOnModel(parameter.Substring(position + 1, parameter.Length - (position + 1)), newmodel, value);
            }
            else
            {
                PropertyInfo property = model.GetType().GetProperty(parameter);
                if (property != null) property.SetValue(model, value, null);
            }
        }

        public static void Initialize(MXApplication theApp)
        {
            InitializeContainer(new MXWebkitContainer(theApp));

            // no threading in web generation, not needed as all output
            // is on the server and synchronous anyway
            Instance.ThreadedLoad = false;
        }

        public static Dictionary<string, string> GetParameters(HttpRequestBase request)
        {
            var retval = new Dictionary<string, string>();
            var strb = new System.Text.StringBuilder();

            if (HttpContext.Current.Session["ActionParameters"] != null && HttpContext.Current.Request.HttpMethod == "POST")
            {
                Dictionary<string, string> actionparms = (Dictionary<string, string>)HttpContext.Current.Session["ActionParameters"];
                foreach (KeyValuePair<string, string> parm in (actionparms))
                {
                    strb.Append(parm.Key + ": " + parm.Value);
                    retval.Add(parm.Key, parm.Value);
                }
                HttpContext.Current.Session.Remove("ActionParameters");
            }
            foreach (string key in request.Form.AllKeys.Where(key => !key.StartsWith("__")))
            {
                strb.Append(key + ": " + request.Form[key]);
                retval[key.Replace("rdl_", string.Empty)] = request.Form[key];
            }
            foreach (string key in request.QueryString.AllKeys.Where(key => !key.StartsWith("__")))
            {
                strb.Append(key + ": " + request.QueryString[key]);
                retval[key] = request.QueryString[key];
            }
            return retval;
        }

        protected override void OnSetDefinitions() { }

        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, string viewPerspective, string navigatedUri)
        {
            try
            {
                RenderViewFromPerspective(controller.ModelType, viewPerspective, controller.GetModel());
            }
            catch (ArgumentException ae)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Failed to Render a view");
#endif
                throw new Exception("Webkit container failed to find a view", ae);
            }
        }

        public override void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(Path.Combine(HttpContext.Current.Request.ApplicationPath, url));
            CancelLoad = true;
        }
    }
}
