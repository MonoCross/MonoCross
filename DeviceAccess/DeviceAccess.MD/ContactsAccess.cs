using System;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Database;

namespace DeviceAccess
{
  class ContactsAccess : Activity
  {
    public IntPtr LookupContactByName(string name, ContentResolver cr)
    {
      IntPtr handle = IntPtr.Zero;

      ICursor cur = cr.Query(ContactsContract.Contacts.ContentUri,
                                          null, null, null, null);
      if (cur.Count > 0)
      {
        while (cur.MoveToNext())
        {
          int lookupColumn = cur.GetColumnIndex(ContactsContract.ContactsColumnsConsts.LookupKey);
          int columnIndex = cur.GetColumnIndex(lookupColumn.ToString());
          String id = cur.GetString(columnIndex);

          string displayNameColumn = ContactsContract.ContactsColumnsConsts.DisplayName.ToString();
          string displayNameColumnIndex = cur.GetColumnIndex(displayNameColumn).ToString();
          String displayName = cur.GetString(cur.GetColumnIndex(displayNameColumnIndex));
          if (displayName.Contains(name))
          {

            handle = cur.Handle;
          }
        }
      }
      return handle;
    }
  }
}