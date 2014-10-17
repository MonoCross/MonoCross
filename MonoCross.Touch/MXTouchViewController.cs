using System;

using MonoTouch.UIKit;
using MonoTouch.Dialog;

using MonoCross.Navigation;

namespace MonoCross.Touch
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MXTouchViewController<T>: UIViewController, IMXView
    {
        public MXTouchViewController ()
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
		public object GetModel()
		{
			return Model;
		}
    }

    public abstract class MXTouchTableViewController<T>: UITableViewController, IMXView
    {
        public MXTouchTableViewController()
        {
        }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
		public object GetModel()
		{
			return Model;
		}
    }
}
