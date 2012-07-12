using System;
using System.Windows.Controls;
using MonoCross.Navigation;

namespace MonoCross.WPF
{
  public abstract class MXPageView<T> : Page, IMXView
  {
    public T Model { get; set; }
    public Type ModelType { get { return typeof(T); } }
    public abstract void Render();
    public void SetModel(object model)
    {
      Model = (T)model;
    }
  }
}