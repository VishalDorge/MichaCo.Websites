#Accessing Environment Variables in ASP.NET 5 Apps

This is a quick walk-through of how to access environmental variables when writing applications using the [ASP.NET 5 DNX][1] execution environment.

## Introduction
When writing web or console applications, you often need to access variables like the directory your app runs in, the version of the hosting environment or environment variables like the `TEMP` folder location or `PATH` and `USERPROFILE` variables.
We also might want to store security related information, like database passwords or security tokens in a way that it doesn't end up in a configuration file which gets checked into source control.

In .Net 4.x we used static variables like `AppDomain` or `ConfigurationManager.AppSettings` which could cause all kinds of issues depending on the type and environment your app is running on. With .Net portable libraries this was also not very friendly.

Writing new apps targeting `Core CLR`, those constructs are not available anymore but more importantly, we don't need them anymore. 
With [**`DNX`**][dnx_overview] the ASP.NET team wrote a great host environment for all kinds of apps, but it also comes with a lot of new concepts.
For example, there are different ways to access the variables of the environment our app is running in:

* Injecting strongly typed, host related data via dependency injection 
* Using `string key, string value` pairs via configuration 

## Dependency Injection
The ASP.NET team introduced a lightweight DI framework which is also used by `DNX` to inject things into your apps. 

> **Note:** 
> The build in DI framework supports constructor injection only. It can be replaced by a more heavyweight packages like *Autofaq* or *Ninject*. For this article we will use the build in DI.

One nice feature of `DNX` is the fact that it can inject useful data to our app's entry point.
In case of ASP.NET web apps we can inject to the `Startup.cs`'s constructor, for consoles, we use the constructor of our `Program.cs`.

Now, the question is, what do we have to inject and how does that work?

The `Microsoft.Framework.Runtime.Abstractions` NuGet package provides all the interfaces which are available to us. If you add one of those interfaces to the constructor, the host (DNX) will inject the instantiated concrete implementation at runtime. 
The concrete implementation may vary based on which environment our app is running on, e.g. Windows or Linux.

And that is the beauty of dependency injection and the new framework. At development time we don't have to know, and more importantly, we don't have to care about the differences. 
The framework abstracts this away for us and it will just work. And because those interfaces provide us a strongly typed contract, we can expect that those properties are set.

There are several interfaces which provide you with different environmental information coming from the `Microsoft.Framework.Runtime.Abstractions` NuGet package:

* **`IApplicationEnvironment`**
Provides access to common application information
* **`IRuntimeEnvironment`**
Provides access to the runtime environment
* **`IRuntimeOptions`**
Represents the options passed into the runtime on boot

> **Hint:** 
> There are more interfaces like `ICompilerOptions` or `IAssemblyLoader` which can be very useful but will not be discussed here. Play around with them yourself! 

### Example
To write an ASP.NET 5 console app, use Visual Studio 2015 (RC or newer) and the new ASP.NET 5 project template for a console application. This should create a `project.json` targeting the new `DNX 4.5.1` and `DNX Core 5.0` and a simple `Program.cs`

    public class Program
    {
        public void Main(string[] args)
        {
        }
    }

To use the `Microsoft.Framework.Runtime.Abstractions` NuGet package in our console app, we have to add it as dependency to the `project.json` file:

	  "dependencies": {
	    "Microsoft.Framework.Runtime.Abstractions": "1.0.0-beta6-*"
	  },

> **Note:**
> The version may vary, at the time of writing this article, it was `beta6`.
> Until all this gets released, there might be breaking changes across versions. Make sure that all Framework and System packages are of the same milestone, e.g. don't mix beta 4 and beta 5 packages.
> Also, make sure you run your app with a version of `DNX` which matches with the version of the packages you have installed. 

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

## Configuration
ASP.NET 5 also comes with a new configuration framework. We will not go into too much detail of how this replaces `app/web.config`. But in general it is collection of string based `key value` pairs.
The main NuGet package `Microsoft.Framework.Configuration` comes with a `ConfigurationBuilder` class which can be used to combine different configuration sources into one collection of `key value` pairs.

The subsequent packages, like `Microsoft.Framework.Configuration.EnvironmentVariables` and `Microsoft.Framework.Configuration.Json`, add extension methods to the `ConfigurationBuilder` to access specific configuration sources.

### Example
For our example, we want to retrieve all environment variables, like `PATH` or `USERPROFILE`, and print the `key` and `value` to the console output.

In addition to the `Microsoft.Framework.Runtime.Abstractions` above, we add a dependency reference to `Microsoft.Framework.Configuration.EnvironmentVariables` to the `project.json` file. 
This package already has a dependency to `Microsoft.Framework.Configuration` and, which means we don't have to reference it explicitly.

	  "dependencies": {
	    "Microsoft.Framework.Runtime.Abstractions": "1.0.0-beta6-*",
	    "Microsoft.Framework.Configuration.EnvironmentVariables": "1.0.0-beta6-*",
	    ...
	  },

In the constructor of our app, we can now instantiate a new `ConfigurationBuilder` and call the extension methods.

    var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

To print all variables to the console we can simply iterate over all available key value pairs.

    foreach(var config in configuration.GetConfigurationSections())
    {
        Console.WriteLine("{0}={1}", config.Key, configuration.Get(config.Key));
    }

There is another thing called user secrets. User secrets are retrieve from the profile of the user account the app's context is instantiate for, e.g. `%APPDATA%\microsoft\UserSecrets\<applicationId>\secrets.json` on windows.
Read more about how to configure user secrets on the [ASP.NET wiki page][aspnet_secrets].
The concept is the same as above mentioned environment variables, you can add the configured user secrets to the `key value` collection by calling the `AddUserSecrets` extension on the `ConfigurationBuilder`.

### Usage of Configuration
Both concepts, environment variables and user secrets configuration, can be used to keep security related information away from your source code. 
We never ever want to check in database passwords or Windows Azure tokens and keys to github or any other source control. 
This way we can define security related settings per environment and read them at runtime, and never add those to a configuration file which gets checked into source control!

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
[aspnet_secrets]: https://github.com/aspnet/Home/wiki/DNX-Secret-Configuration

[TOC]