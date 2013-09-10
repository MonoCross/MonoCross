using System.Collections.Generic;
using CustomerManagement.Shared.Model;
using MonoCross.WPF;

namespace CustomerManagement.WPF.Views
{
  /// <summary>
  /// This class is used to simplify using a generic class in XAML
  /// </summary>
  public abstract class ListViewGlue : MXPageView<List<Customer>> { }
}