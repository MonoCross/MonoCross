using System;

namespace MonoCross.Navigation
{
    /// <summary>
    /// MXObservingView watches for changes to the model and re-renders when they occur
    /// </summary>
    public abstract class ObservingMXView<T> : MonoCross.Navigation.MXView<T>
        where T : IMXNotifying
    {
        /// <summary>
        /// Sets the model and signs up for its notifications
        /// </summary>
        public override void SetModel(object model)
        {
            if (Model != null)
            {
                ((T)Model).NotifyChange -= ModelCollectionChangedHandler;
            }

            base.SetModel(model);

            if (model != null)
            {
                ((T)Model).NotifyChange += ModelCollectionChangedHandler;
            }
        }

        /// <summary>
        /// This default implementation of the handler for model changes simply calls <see cref="IMXView.Render()"/>
        /// </summary>
        protected virtual void ModelCollectionChangedHandler(object sender, EventArgs args)
        {
            Render();
        }
    }
}