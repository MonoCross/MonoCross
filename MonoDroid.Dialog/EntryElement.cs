using System;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class EntryElement : Element
	{
		public string Value
		{
			get
			{
				return val;
			}
			set
			{
				val = value;
				if (entry != null)
					entry.Text  = value;
			}
		}
		
		private string val;
		private string placeholder;
		private bool isPassword;
		private EditText entry;

		public event EventHandler Changed;

		public EntryElement(string caption, string placeholder, string value)
			: base(caption)
		{
			Value = value;
			this.placeholder = placeholder;
		}

		public EntryElement(string caption, string placeholder, string value, bool isPassword)
			: base(caption)
		{
			Value = value;
			this.isPassword = isPassword;
			this.placeholder = placeholder;
		}

		public override string Summary()
		{
			return Value;
		}
		public void FetchValue()
		{
		}
	}
}