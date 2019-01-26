# Simple Sql Connection Checker
### Simplest tool to check connectivity to MS SQL Server instance.
[![Docker Pulls](https://img.shields.io/docker/pulls/dmitrydoc/sqleconnchecker.svg)](https://hub.docker.com/r/dmitrydoc/sqleconnchecker/) [![Docker Automated build](https://img.shields.io/docker/automated/dmitrydoc/sqleconnchecker.svg)](https://hub.docker.com/r/dmitrydoc/sqleconnchecker/) [![Docker Build Status](https://img.shields.io/docker/build/dmitrydoc/sqleconnchecker.svg)](https://hub.docker.com/r/dmitrydoc/sqleconnchecker/)

Constantly checks connectivity to SQL Server each minute and writes OK/NOT OK in the output or file.  
Built on .net core, so could be run on win / linux / mac.

# How to run
Just run SqlConnChecker(-.exe) as binary file.

# Settings
* ConnectionString (string) - SQL connection string.
* CheckIntervalMs (int) - how often tool shouldcheck connection (6000 ms by default).
* VerboseOutput (bool) - show error message in output in case of connection is NOT OK (true by default).
* WriteToFile (bool) - write or not messages to the file.
* FileOutputPath (string) - path to the file.

### How to change settings
 * modify appsettings.json:

# How to build & publish:
1. Install .Net Core 2.2 SDK  or latest from the [Microsoft official site](https://dotnet.microsoft.com/download/dotnet-core/2.2) 
2. Go to src folder and run  
```
dotnet publish -c Release -r linux-x64
```

### All supported platforms (win8, debian, ubuntu, etc.) could be found [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)

# Docker
## How to run
```
docker run -v /opt:/opt dmitrydoc/sqleconnchecker 
	--ConnectionString="connection string" 
	--CheckIntervalMs=1000 
	--VerboseOutput=true 
	--WriteToFile=true 
	--FileOutputPath=/opt/logs
```

## How to build your own
```
docker build -t sqlchecker .
```