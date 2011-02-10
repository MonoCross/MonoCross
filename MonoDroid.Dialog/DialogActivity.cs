using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class DialogActivity : ListActivity
	{
		public DialogActivity(RootElement root)
			: base()
		{
			this.Root = root;
			this.DialogAdapter = new DialogAdapter(this, root);
		}

		public RootElement Root
		{
			get;
			set;
		}

		public DialogAdapter DialogAdapter
		{
			get;
			private set;
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			this.ListAdapter = this.DialogAdapter;
		}
	}
}