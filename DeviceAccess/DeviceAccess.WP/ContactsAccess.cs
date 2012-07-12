using System;
using Microsoft.Phone.UserData;
using System.Collections.Generic;
using System.Threading;

namespace DeviceAccess
{
  public class ContactsAccess
  {
    public List<string> LookupContactName(string Name)
    {
      // create a contact object and it's search handler
      Contacts cons = new Contacts();
      cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);

      // start the search
      cons.SearchAsync(Name, FilterKind.None, string.Empty);

      // block on the search until the async result return
      List<string> results;
      lock (_locker)
      {
        while (name == null)
        {
          Monitor.Pulse(_locker);
        }

        results = name;
        name = null;
      }
      return results;
    }
    static readonly object _locker = new object();
    List<string> name = null;

    void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
    {
      lock (_locker)
      {
        // reset list
        name = new List<string>();

        // build new result list
        if (e.Results != null)
        {
          var en = e.Results.GetEnumerator();
          while (en.MoveNext())
          {
            name.Add(en.Current.DisplayName);
          }
        }
        Monitor.Pulse(_locker);
      }
    }
  }
}
