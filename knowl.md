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