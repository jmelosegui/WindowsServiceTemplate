# Service Template
This is a simple service template to easilly install and debug a .Net Windows Service.

## Goals
 1. Easy to install and Uninstall
 2. Easy to Debug.

### Configure the service.
You can specify the "Service Name", "Service Display Name", and "Service Descrition".
Go to the Service.cs file, find 3 constants used by the service during the install process

```csharp
	//TODO: Setup more suitable names here
	private const string serviceName = "WindowsService";
	private const string serviceDisplayName = "Windows Service Template";
	private const string serviceDescription = "Windows Service Description";
```
 
### Install/Uninstall
 
For install and uninstall after you compile the application use the following commands
 
#### Install
Assuming your exe file is named Service.exe
```
 	Service.exe /i
```

Now go to the microsoft service console (services.msc) and you will see your service installed.

![serviceinstall](https://cloud.githubusercontent.com/assets/450246/10711782/b64790e8-7a54-11e5-9118-a9fbc1f8dbfd.png)

#### Uninstall
```
 	Service.exe /u
```
#### Usage
```
 	Service.exe /h
```
### Debug
On Visual Studio, open the solution explorer, right click on the project, then click properties.
In the properties window click on Debug tab and add a "-d" to the Command line arguments configuration.

![debugservice](https://cloud.githubusercontent.com/assets/450246/10711776/a2e326b6-7a54-11e5-99ef-c91f2b0450d5.png)

Now run the project in debug mode and you will able to debug your code.

