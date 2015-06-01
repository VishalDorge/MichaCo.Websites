#Accessing Environment Variables in ASP.NET 5 Apps

This is a quick walk-through of how to access environmental variables when writing applications using the [ASP.NET 5 DNX][1] execution environment.

## Use Case and History
When writing apps, you often need access to variables like the directory your app runs in, or the version of the hosting environment.

In .Net 4.x we used static variables like `AppDomain` which could cause all kinds of issues depending on where your app is running. With .Net portable libraries this was also not very friendly.

Targeting DNX Core, those constructs are not available anymore but more importantly, we don't need them anymore. Instead, we use dependency injection to access what we need within our app.

## Dependency Injection
The ASP.NET team introduced a lightweight DI framework which `DNX` can use to inject things into our apps. 

The DI framework supports constructor injection only, in case of a console application, you can inject to the constructor of our `Program.cs`. This is the same for ASP.NET web and console applications. In case of web applications you would inject to the `Startup.cs` class which is used to bootstrap your web app.

> **Note:** 
> Don't use a `static` main method, otherwise you'd have to set static properties from the constructor to access the injected objects, which would be bad

Now, the question is, what do we want to inject and how does that work?

The `Microsoft.Framework.Runtime.Abstractions` NuGet package provides all the interfaces which are available to us. 
If you add one of those interfaces to the constructor, at runtime, the host (DNX) will inject the instantiated concrete implementation. The concrete implementation may vary based on where our app is running, could be Windows, Linux or whatever `DNX` supports.

And that is the beauty of dependency injection and the new framework. At development time we don't have to know, and more importantly, care about the differences. The framework abstracts this away for us and it will just work (hopefully).

## Accessing Environment Variables
Now, let's write some code. We will use Visual Studio 2015 RC (or newer) and the new ASP.NET 5 project template for a console application. This should create a `project.json` targeting the new `DNX 4.5.1` and `DNX Core 5.0` and a simple `Program.cs`

    public class Program
    {
        public void Main(string[] args)
        {
        }
    }

There are several interfaces which provide you with different environmental information coming from the `Microsoft.Framework.Runtime.Abstractions` NuGet package:

* **`IApplicationEnvironment`**
Provides access to common application information
* **`IRuntimeEnvironment`**
Provides access to the runtime environment
* **`IRuntimeOptions`**
Represents the options passed into the runtime on boot

To use the package in our console app, we have to add the package as dependency to the `project.json` file:

	  "dependencies": {
	    "Microsoft.Framework.Runtime.Abstractions": "1.0.0-beta6-*"
	  },

> **Note:**
> The version may vary, at the time of writing this article, it was `beta6`.

Now, we add those interfaces as parameters to the constructor of our app. In the following example we use the console output to print some of the information those interfaces provide:

	public Program(IApplicationEnvironment app,
	               IRuntimeEnvironment runtime,
	               IRuntimeOptions options)
	{
	    Console.WriteLine("ApplicationName: {0} {1}", app.ApplicationName, app.Version);
	    Console.WriteLine("ApplicationBasePath: {0}", app.ApplicationBasePath);
	    Console.WriteLine("Framework: {0}", app.RuntimeFramework.FullName);
	    Console.WriteLine("Runtime: {0} {1} {2}", runtime.RuntimeType, runtime.RuntimeArchitecture, runtime.RuntimeVersion);
	    Console.WriteLine("System: {0} {1}", runtime.OperatingSystem, runtime.OperatingSystemVersion);
	}
	
	public void Main(string[] args) { ... }

## More Resources and Examples
* [DNX Overview][dnx_overview]
* [DNX Wiki][dnx_wiki]
* [Interesting discussion around retrieving type information][git_types]
* [Custom assembly loader][git_loader]
Injecting `IAssemblyLoaderContainer` and `IAssemblyLoadContextAccessor`

[1]: https://github.com/aspnet/dnx
[dnx_overview]: http://docs.asp.net/en/latest/dnx/overview.html
[dnx_wiki]: https://github.com/aspnet/Home/wiki/DNX-structure
[git_types]: https://github.com/dotnet/coreclr/issues/919
[git_loader]: https://github.com/aspnet/Entropy/blob/dev/samples/Runtime.CustomLoader/Program.cs

[TOC]