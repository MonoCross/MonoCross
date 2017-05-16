using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using MonoCross.Navigation;

namespace MonoCross.Touch
{
    public abstract class MXTouchDialogView<T> : DialogViewController, IMXView
    {
        public MXTouchDialogView(UITableViewStyle style, RootElement root, bool pushing) :
            base(style, root, pushing)
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