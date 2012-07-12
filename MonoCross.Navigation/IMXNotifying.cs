using System;

namespace MonoCross.Navigation
{
	/// <summary>
	/// This interface is for things that notify when they have changed
	/// </summary>
	public interface IMXNotifying
	{
		event EventHandler<EventArgs> NotifyChange;
	}
}
