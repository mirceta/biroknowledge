## EntityFramework

#### To enable Birokrat to use EntityFramework for arbitrary table:
	
```alter table PostnaKnjiga add primary key RecNo```

Can also add SyncId as primary key if RecNo not available.

**Warning: Cannot use this in production because we would have to distribute changes to the database everywhere.**

then in the NuGet console...

```Scaffold-DbContext "...blablaconnstring..." -t Slike, PostnaKnjiga```