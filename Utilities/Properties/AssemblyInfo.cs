using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if DROID
[assembly: AssemblyTitle("MonoCross.Utilities.Android")]
#elif TOUCH
[assembly: AssemblyTitle("MonoCross.Utilities.Touch")]
#else
[assembly: AssemblyTitle("MonoCross.Utilities")]
#endif
[assembly: AssemblyDescription("MonoCross Utilities")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Zebra Technologies Corp.")]
[assembly: AssemblyProduct("MonoCross Utilities")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en")]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4581657f-aef0-4b78-a18e-17ec0434d40d")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("4.0.0.42")]
[assembly: AssemblyFileVersion("4.0.0.42")]
[assembly: AssemblyInformationalVersion("4.0.0.42-master.WorkingTree")]
[assembly: AssemblyCopyright("Copyright ©  2018")]