# Introduction 
This is a .NetCore API to show how i would create an API for a database.  
Technologies and languages used:  
   -.NetCore 3.1  
   -swashbuckler (swagger)  
   -AutoMapper.  
   -Azure KeyVault to securly store AppSetings.  
   -Azure Devops CI/CD (Yaml Build Definition).  
   -EntityframeworkCore.  
   -Azure Sql Server database.  

The database is a sample from microsoft. 
[AdventureWorks Sql Database](https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver15)

# Getting Started
1.	Select the AdventureWorks.Database file and run a "Schema compare..." 
2.	select the location you would like to recreate the database.
3.	Right click on the AdventureWorks.Api project
4.	Click on "Manage User Secrets"
5.	Add in the connection string to the database you have created like so:

```
 "ConnectionString": {
    "AdventureWorksDbContext": "{ConnectionString}"
  }
```
