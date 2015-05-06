using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SweetSoft.APEM.WebApp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SweetSoft")]
[assembly: AssemblyCompanyWebsite("http://www.sweetsoft.vn")]
[assembly: AssemblyProduct("SweetSoft.APEM.WebApp")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f2fba973-f85a-4bf4-a402-72f4d394dca0")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class AssemblyCompanyWebsite : Attribute
{
    private readonly string _conflicts;

    public string Conflicts
    {
        get { return _conflicts; }
    }

    public AssemblyCompanyWebsite(string conflicts)
    {
        _conflicts = conflicts;
    }
}
