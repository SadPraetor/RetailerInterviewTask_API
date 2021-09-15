# Retailer Interview - API <!-- omit in toc -->

## Table of contents <!-- omit in toc -->
- [1. Project description](#1-project-description)
- [2. Technology description](#2-technology-description)
- [3. Project dependencies](#3-project-dependencies)
- [4. Local project setup](#4-local-project-setup)
- [5. DEV Data Seed](#5-dev-data-seed)
- [6. Code](#6-code)
  - [6.1. Api Versioning](#61-api-versioning)
  - [6.2. Update endpoint](#62-update-endpoint)
  - [6.3. No repository pattern](#63-no-repository-pattern)
- [7. Unit Testing](#7-unit-testing)
  - [7.1. In Memory Database](#71-in-memory-database)
- [8. Known issues / possible improvements](#8-known-issues--possible-improvements)

<a name="1-project-description"></a>

## 1. Project description
This project was given as interview task by a retailer company. The task is to create REST API, with few basic endpoints, API versioning, data seed, unit tests and swagger documentation. <br/>
Full description of the task can be found in the file [task.txt](./task.txt) within the repository.

<br />
<a name="2-technology-description"></a>

## 2. Technology description
Solution is based on <span>ASP.</span>NET Core 3.1 -> this is as of day of writing the code long term supported .NET version. This was one of the requirements given. <br/>
Backend database implemented in the code is MSSQL, and access to data is managed by EF Core. <br/>
Unit tests are written in xUnit
<br/>
<br/>
Solution was written in Visual Studio 2019.

<br/>

<a name="3-project-dependencies"></a>

## 3. Project dependencies
???

<br/>
<a name="4-local-project-setup"></a>

## 4. Local project setup
Cloning the project the project and executing `dotnet run` in `./src` folder should start the project listening on `https://localhost:5001` and `http://localhost:5000`.<br/>
Starting project in Visual Studio should also produce similar result. Hosting on IIS Express is set to be on ports `http://localhost:7410` and `https://localhost:44309`.


<br/>
<a name="5-dev-data-seed"></a>

## 5. DEV Data Seed
If the projects runs on Development environment, on startup it will try to migrate the database and seed test data into it.

1. Adjust configuration file `appsettings.Development.json` connection string
    ```
    "ConnectionStrings": {
    "ProductsDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CommerceDb;Integrated Security=True;ApplicationIntent=ReadOnly"}
    ```

    Your user requires corresponding roles to execute the migration, something in lineof

    ```
    GRANT ALTER, INSERT, SELECT, DELETE, UPDATE, REFERENCES ON SCHEMA::schema_name TO migration_user
    GRANT CREATE TABLE TO migration_user
    ```

2. On startup part of code that runs migration and data seed is run
    ```
    public static void Main( string[] args ) {
        var hostBuilder = CreateHostBuilder( args ).Build();

        if ( Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" ) == Environments.Development ) {
            hostBuilder
                .MigrateDatabase()
                .SeedDatabaseIfEmpty(300);
        };

        hostBuilder.Run();
    }
    ```
    `.MigrateDatabase` -> runs the migration, creates table `Products` <br/>
    `.SeedDatabaseIfEmpty(int count)` -> seeds table with data, `count` value specifies how many records you want to add


<br/>

<a name="6-code"></a>

## 6. Code
<br/>

### 6.1. Api Versioning
API Versioning approach chosen was to use Header value. Header used is `X-Api-Version`.  If not specified defaults to v1.0.<br/>

> URL versioning was considered.
> I understand it allows understanding of versions easily at first glance, also exploration is easier. <br/>
> Header versioning follows practice of one resource is represented by one endpoint. It is more academically correct.

<br/>

### 6.2. Update endpoint
Update endpoint path is defined as `api/{id}/description` although `description` is a property of resource and not a resource. Together with method  `PATCH` it should clearly indicate to user that this endpoint only updates a description. Input is `text/plain`, a new description. 

<br/>

### 6.3. No repository pattern
DB context provided by EF is directly exposed in controller. It is not wrapped in repository pattern.
> This was considered, but for this solution I decided against. <br/><br/>
> EF Core already implements Repository pattern (DbSet) and Unit of Work pattern (DbContext) <br/>
> Generally speaking, back-end database format tends to be stable in applications, and swapping DB provider within EF Core context is relatively easy.<br/>
> Having LINQ directly exposed is neat. <br/>
> This is small project, so adding another layer seems redundant. I would just wrap few lines of code that are currently in controllers in a repository. On other hand, with huge projects building a repository that covers each permutation of request that can happen might be a challenge and a lot of work. <br/>
> <br/>
> It makes it more difficult to setup tests. Within the project though there is no business logic to be tested.<br/>
> I would lean towards use of repository pattern for complex data access (i.e. multiple database, API calls, SQL query)   

<br/>
<a name="7-tests"></a>

## 7. Unit Testing

### 7.1. In Memory Database
Unit tests are run against controller, with In Memory Database setup. Therefore they are closer to integration testing than unit testing. 

<br/>
<a name="8-known-issues"></a>

## 8. Known issues / possible improvements