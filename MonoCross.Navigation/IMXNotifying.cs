using System;

namespace MonoCross.Navigation
{
    /// <summary>
    /// This interface is for models that notify when they have changed.
    /// </summary>
    public interface IMXNotifying
    {
        /// <summary>
        /// Occurs when the implementing member notifies of a change.
        /// </summary>
        event EventHandler<EventArgs> NotifyChange;
    }
}