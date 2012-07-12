using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;

using MonoCross.Navigation;

using CustomerManagement.Shared;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Controllers
{
    public class CustomerController : MXController<Customer>
    {
        public override string Load(Dictionary<string, string> parameters)
        {
            string perspective = ViewPerspective.Default;

            string customerId = null;
            parameters.TryGetValue("CustomerId", out customerId);
			
			// get the action, assumes 
			string action;
            if (!parameters.TryGetValue("Action", out action))  {
                // set default action if none specified
                action = "GET";
            }
			
            switch (action)
            {
			case "EDIT":
            case "GET":
                // populate the customer model
	            if (customerId == null)
					throw new Exception("No Customer Id found");
				if (string.Equals(customerId.ToUpper(), "NEW")) {
					// assigm Model a new customer for editing 
                    Model = new Customer();
					perspective = ViewPerspective.Update;
				} else {
					Model = GetCustomer(customerId);
					if (String.Equals(action, "EDIT"))
						perspective = ViewPerspective.Update;
					else
						perspective = ViewPerspective.Default;
				}
				break;

            case "DELETE":
	            if (customerId == null)
					customerId = Model.ID;
				// post delete request to the server
                DeleteCustomer(customerId);

                // return and let redirected controller execute, remaining navigation is ignored
                MXContainer.Instance.Redirect("Customers");
                return ViewPerspective.Delete;

            case "CREATE":
			    // process addition of new model
				if(AddNewCustomer(Model))
                    MXContainer.Instance.Redirect("Customers");
			    break;

            case "UPDATE":
				if(UpdateCustomer(Model))
                    MXContainer.Instance.Redirect("Customers");
                break;
            }

            return perspective;
        }

        public static Customer GetCustomer(string customerId)
        {
#if LOCAL_DATA
            return CustomerManagement.Data.XmlDataStore.GetCustomer(customerId);
#else
            string urlCustomers = string.Format("http://localhost/MXDemo/customers/{0}.xml", customerId);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            XmlSerializer serializer = new XmlSerializer(typeof(Customer));
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), true))
            {
                return (Customer)serializer.Deserialize(reader);
            }
#endif
        }
		
		public static bool UpdateCustomer(Customer customer)
        {
#if LOCAL_DATA
            CustomerManagement.Data.XmlDataStore.UpdateCustomer(customer);
#else
            string urlCustomers = "http://localhost/MXDemo/customers/customer.xml";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            request.Method = "PUT";
            request.ContentType = "application/xml";

            using (Stream dataStream = request.GetRequestStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Customer));
                serializer.Serialize(dataStream, customer);
            }

            request.GetResponse();
#endif
            return true;
		}
		
		public static bool AddNewCustomer(Customer customer)
        {
#if LOCAL_DATA
            CustomerManagement.Data.XmlDataStore.CreateCustomer(customer);
#else
            string urlCustomers = "http://localhost/MXDemo/customers/customer.xml";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            request.Method = "POST";
            request.ContentType = "application/xml";

            using (Stream dataStream = request.GetRequestStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Customer));
                serializer.Serialize(dataStream, customer);
            }

            request.GetResponse();
#endif
            return true;
        }
		
        public static bool DeleteCustomer(string customerId)
        {
#if LOCAL_DATA
        CustomerManagement.Data.XmlDataStore.DeleteCustomer(customerId);
#else
        string urlCustomers = "http://localhost/MXDemo/customers/" + customerId;

        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
        request.Method = "DELETE";
        request.GetResponse();
#endif
            return true;
        }
    }
}
