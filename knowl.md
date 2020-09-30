## EntityFramework

#### Use EntityFramework with some tables

https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/existing-db

```
Scaffold-DbContext "Server=192.168.0.123;Database=biroside;Trusted_Connection=false;User=turizem;Password=q" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models2
```

#### To enable Birokrat to use EntityFramework for arbitrary table:
	
```alter table PostnaKnjiga add primary key RecNo```

Can also add SyncId as primary key if RecNo not available.

**Warning: Cannot use this in production because we would have to distribute changes to the database everywhere.**

then in the NuGet console...

```Scaffold-DbContext "...blablaconnstring..." -t Slike, PostnaKnjiga```

## SQL operations

- Adding a new user to your sql server instance

Creating a Login is easy and must (obviously) be done before creating a User account for the login in a specific database:

```
CREATE LOGIN NewAdminName WITH PASSWORD = 'ABCD'
GO
```

Here is how you create a User with db_owner privileges using the Login you just declared:

```
Use YourDatabase;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'NewAdminName')
BEGIN
    CREATE USER [NewAdminName] FOR LOGIN [NewAdminName]
    EXEC sp_addrolemember N'db_owner', N'NewAdminName'
END;
GO
```

### CREATE NEW USER WITH INTEGRATED SECURITY

- Let **A** be the PC from which you want to connect, and **B** the PC you're connecting to. Let **u** be the user on **A** with which we want to connect to **B**.
- On **B** create a windows user with the same username AND password as **u**.
- On **B** create a SQL user with Integrated security and choose **u(B)** as the user, give sysadmin permissions.
- On **A**, if a password change has been required in the case that the password didn't comply to standards on **B**, you should sign out and sign into the user on **A**

## EntityFramework object model to SQL Server
https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database


## EntityFramework code first

context.Database.EnsureCreated() // autocreates the database

In production, better use migrations!!


## Starting .NET Core console application without HostedServices, and using DI

void CorrectWayToRunCoreAppWithoutHostedServices() {
    var serviceProvider = new ServiceCollection()
            .AddLogging(opt => {
                opt.ClearProviders();
                opt.AddConsole();
                opt.SetMinimumLevel(LogLevel.Debug);
            })
            .AddSingleton<IServerAddress>(
                new ServerAddress("http://www.birokrat.si/media/resource-monitoring/server-address.txt"))
            .AddSingleton<IRemoteResources, RemoteResources>()
            .AddSingleton<IMetricRecordCache, MetricRecordCache>()
            .AddSingleton<IResourceMeasurementClient, ResourceMeasurementClient>()
            .BuildServiceProvider();

    var logger = serviceProvider.GetService<ILoggerFactory>()
        .CreateLogger<Program>();
    logger.LogInformation("Starting application");

    var client = serviceProvider.GetService<IResourceMeasurementClient>();
    while (true)
    {
        client.Work();
    }
}

### And with HostedServices...

// using NuGet download Microsoft.Extensions.Hosting
static void Main(string[] args)
{
    new HostBuilder()
        .ConfigureServices((hostContext, services) => {
            services.AddLogging(opt =>
            {
                opt.ClearProviders();
                opt.AddConsole();
                opt.SetMinimumLevel(LogLevel.Debug);
            })
            .AddSingleton<IServerAddress>(
                new ServerAddress("http://www.birokrat.si/media/resource-monitoring/server-address.txt"))
            .AddSingleton<IRemoteResources, RemoteResources>()
            .AddSingleton<IMetricRecordCache, MetricRecordCache>()


            .AddHostedService<ComputationEnqueuerService>()
            .AddSingleton<IComputationQueue, ComputationQueue>()
            .AddHostedService<ComputationDequeuerService>()

            .AddSingleton<IResourceMeasurementClient, ResourceMeasurementClient>();
        })
        .RunConsoleAsync();
}

## Publish a nuget package to nuget.org

References: https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package


### Works only if you package has no dependencies.
- Install nuget.exe from https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
- Go to visual studio project folder in powershell and
- ```nuget spec``` will generate the .nuspec file used as manifest for the nuget package
- ```nuget pack```


#### Test the package

- Move the .nupkg to local folder
- Add the folder as nuget repository with ```nuget sources add -name <name> -source <path>```
- Install pacakge with ```nuget install <packageID> -source <name>```

#### Publish the package to nuget.org

- Login to nuget.org
- Upload the .nupkg file and wait 1 hour
