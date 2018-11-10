英文 | [中文](README.zh-CN.md)
# Ocelot.Provider.SqlServer
Repo for store configuration in Microsoft SqlServer with [Ocelot](http://threemammals.com/ocelot)
# Ocelot

[<img src="http://threemammals.com/images/ocelot_logo.png">](http://threemammals.com/ocelot)

[![Build status](https://ci.appveyor.com/api/projects/status/jmkqqg6i24dx1crc?svg=true)](https://ci.appveyor.com/project/TomPallister/ocelot-provider-consul)
Windows (AppVeyor)
[![Build Status](https://travis-ci.org/ThreeMammals/Ocelot.Provider.Consul.svg?branch=develop)](https://travis-ci.org/ThreeMammals/Ocelot.Provider.Consul) Linux & OSX (Travis)



This package adds Microsoft SQL Server support to Ocelot configuration.

## How to install

Install Niyw.Ocelot.Provider.SqlServer and it's dependencies using NuGet. 

`Install-Package Niyw.Ocelot.Provider.SqlServer`

Or via the .NET Core CLI:

`dotnet add package Niyw.Ocelot.Provider.SqlServer`

All versions can be found [here](https://www.nuget.org/packages/Niyw.Ocelot.Provider.SqlServer/)

## How to Run

cd to 'test\OcelotApiGw' forder, via the .NET Core CLI:

`dotnet ef migrations add InitialOcelotConfigDbMigration -c OcelotConfigDbContext -o DbMigrations/OcelotConfigDb`

# Thanks
Get some ideas from [Ocelot.Provider.Consul](https://github.com/ThreeMammals/Ocelot.Provider.Consul)
