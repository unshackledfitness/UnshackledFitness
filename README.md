
![Logo](https://my.unshackled.fitness/icon_x128.png)

# Unshackled Fitness

Tools for living a healthy lifestyle. 

A cross-platform, hosted WebAssembly Blazor PWA with a vertical slice architecture that supports Sqlite, MS SQL Server, MySQL, and PostgreSql databases through Entity Framework.

## Run Locally

Install the [.NET SDK](https://dotnet.microsoft.com/en-us/download) for your platform. Then, install the EF Core tools for the .NET CLI.
```bash
dotnet tool install --global dotnet-ef
```

Clone the project

```bash
  git clone https://github.com/tonyrix/Unshackled.Fitness.git Unshackled.Fitness
```

Go to the project directory

```bash
  cd Unshackled.Fitness
```

### Create App Settings files ###
Copy and rename the sample-appsettings.json files for the Blazor server and client projects

```bash
# Windows
copy .\src\Unshackled.Fitness.My\sample-appsettings.json .\src\Unshackled.Fitness.My\appsettings.json
copy .\src\Unshackled.Fitness.My\sample-appsettings.Development.json .\src\Unshackled.Fitness.My\appsettings.Development.json

# Mac OS/Linux
cp ./src/Unshackled.Fitness.My/sample-appsettings.json ./src/Unshackled.Fitness.My/appsettings.json
cp ./src/Unshackled.Fitness.My/sample-appsettings.Development.json ./src/Unshackled.Fitness.My/appsettings.Development.json
```

### Configure App Settings ###

Open the solution file or project folder in your favorite editor and complete the newly created appsettings files.

**Unshackled.Fitness.My/appsettings.json**

*DbConfiguration*
```json
"DbConfiguration": {
	"DatabaseType": "sqlite",
	"TablePrefix": "uf_"
}
```
* DatabaseType: The database you will be using. Use mssql, mysql, postgresql, or sqlite as the value.
* TablePrefix: The table name prefix in the database. Recommend that you leave this as "uf_" unless you have a reason to change it.If you do change it, you will need to remove the existing database migrations and create new ones.

*HashIdConfiguration*
```json
"HashIdConfiguration": {
	"Alphabet": "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890",
	"Salt": "randomstringofcharacters",
	"MinLength": 12
},
```
* Alphabet: The list of possible characters to use while hashing database IDs for display in URLs.
* Salt: A random string of characters to salt your hash. Make this sufficiently long (at least 15 chars)
* MinLength: The minimum length you want the resultant hash to be. Larger IDs may produce longer hashes, but lower ID numbers will not fall below this minimum.

*SiteConfiguration*
```json
"SiteConfiguration": {
	"SiteName": "Unshackled Fitness",
	"AppThemeColor": "#194480",
	"AllowRegistration": true,
	"RequireConfirmedAccount": true,
	"PasswordStrength": {
		"RequireDigit": true,
		"RequireLowercase": true,
		"RequireNonAlphanumeric": true,
		"RequireUppercase": true,
		"RequiredLength": 10,
		"RequiredUniqueChars": 7
	}
}
```
* SiteName: The default title of your app.
* AppThemeColor: The color of the loading screen when your PWA is first displayed.
* AllowRegistration: True, allows users to register for an account. False, prevents new user accounts registration.
* RequireConfirmedAccount: True, requires new users to confirm their email adddress before logging in. False, new users can log in immediately.
* PasswordStrength: Set the requirements you want for password strength.

*SmtpSettings*
```json
"SmtpSettings": {
	"Host": "smtp.host.domain",
	"Port": 587,
	"UseSSL": true,
	"DefaultReplyTo": "email@domain.com",
	"Username": "",
	"Password": ""
}
```
* Host: Your SMTP host URL.
* Port: 587 is the default. 465 if required by your host. 25 if your host doesn't support SSL
* UseSSL: Leave to true unless required otherwise.
* DefaultReplyTo: The default reply address to be used when sending emails.
* Username: Your host username. **This setting should be moved to a secrets file, key vault, or environment variable**
* PasswordStrength: Your host password. **This setting should be moved to a secrets file, key vault, or environment variable**

*ConnectionStrings*

**Unshackled.Fitness.My/appsettings.Development.json**

```json
"ConnectionStrings": {
	"DefaultDatabase": "Data Source=Data/unshackled-fitness.sqlite"
},
```
* DefaultDatabase: The connection string for the database you chose in the DbConfiguration section.

### Database Migrations ###

If you have changed the table prefix, you will need to create new database migrations. Delete the existing migrations for you database and create your own.

If you have set "ApplyMigrationsOnStartup" to false, you will need to apply your migrations to the database before starting the server.

### Run the Website ###
Move to the Unshackled.Fitness.My directory from the main project directory.
```bash
cd ./src/Fitness/Unshackled.Fitness.My
```
By default, the website will run at https://localhost:5580. You can change these settings under Properties/launchSettings.json.

Start the server
```bash
dotnet run
```

## Acknowledgements

Unshackled Fitness is built on top of these other great projects.

 - [AutoMapper](https://automapper.org/)
 - [Blazored.FluentValidation](https://github.com/Blazored/FluentValidation)
 - [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage)
 - [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
 - [Hashids.Net](https://github.com/ullmark/hashids.net)
 - [ImageSharp](https://sixlabors.com/products/imagesharp/)
 - [LinqKit](https://github.com/scottksmith95/LINQKit)
 - [MudBlazor](https://mudblazor.com/)
 - [MediatR](https://github.com/jbogard/MediatR)
 - [Npgsql EF Core Provider for PostgreSQL](https://github.com/npgsql/efcore.pg)
 - [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
 - [TimeZoneNames](https://github.com/mattjohnsonpint/TimeZoneNames)
 - [Z.EntityFramework.Plus.EfCore](https://entityframework-plus.net/)
 - And numerous Microsoft .NET Core libraries.
