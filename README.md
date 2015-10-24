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
 
For install and uninstall after you compile the application use the followings command
 
#### Install
Assuming your exe file is named Service.exe
```
 	Service.exe /i
```

Now go to the microsoft service console (services.msc) and you will see your service installed.

[Service image] (link)

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

[image](link)

Now run the project in debug mode and you will able to debug your code

