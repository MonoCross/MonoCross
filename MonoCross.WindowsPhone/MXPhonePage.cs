using Microsoft.Phone.Controls;
using MonoCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCross.WindowsPhone
{
    public abstract class MXPhonePage<T>: PhoneApplicationPage, IMXView
    {
        public virtual void Render() { }

        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public void SetModel(object model)
        {
            Model = (T)model;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // check if we've been here before!
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                // fetch the model before rendering!!!
                Type t = typeof(T);
                if (MXPhoneContainer.ViewModels.ContainsKey(t))
                {
                    SetModel(MXPhoneContainer.ViewModels[t]);
                }
                else
                {
                    var mapping = MXContainer.Instance.App.NavigationMap.FirstOrDefault(layer => layer.Controller.ModelType == t);
                    if (mapping == null)
                    {
                        throw new Exception("The navigation map does not contain any controllers for type " + t);
                    }

                    mapping.Controller.Load(new Dictionary<string, string>());
                    SetModel(mapping.Controller.GetModel());
                }

                Render();
            }
        }
    }
}
