# IMS

## PIM

### DataMigrations PimDbContext

1. Open cmd and navigate to Harvey.PIM.Application folder
2. Run this command to create new migration: dotnet ef migrations add **migrationName** -o Infrastructure/Migrations --context PimDbContext
3. Migrations will be run automatically on application start

### DataMigrations CatalogDbContext

1. Open cmd and navigate to Harvey.PIM.Application folder
2. Run this command to create new migration: dotnet ef migrations add **migrationName** -o Infrastructure/CatalogMigrations --context CatalogDbContext
3. Migrations will be run automatically on application start

### DataMigrations ActiviyLog

1. Open cmd and navigate to Harvey.PIM.Application folder
2. Run this command to create new migration: dotnet ef migrations add **migrationName** -o Infrastructure/ActivityMigrations --context ActivityLogDbContext
3. Migrations will be run automatically on application start

### DataMigrations TransactionDbContext

1. Open cmd and navigate to Harvey.PIM.Application folder
2. Run this command to create new migration: dotnet ef migrations add **migrationName** -o Infrastructure/TransactionMigrations --context TransactionDbContext
3. Migrations will be run automatically on application start

## PurchaseControl

### DataMigrations

1. Open cmd and navigate to Harvey.PurchaseControl.Application folder
2. Run this command to create new migration: dotnet ef migrations add **migrationName** -o Infrastructure/Migrations
3. Migrations will be run automatically on application start

