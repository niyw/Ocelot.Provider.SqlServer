Ó¢ÎÄ | [ÖÐÎÄ](README.zh-CN.md)
# Ocelot.Provider.SqlServer
Repo for store configuration in MS SqlServer with [Ocelot](http://threemammals.com/ocelot)

[<img src="http://threemammals.com/images/ocelot_logo.png">](http://threemammals.com/ocelot)

[![Build status](https://ci.appveyor.com/api/projects/status/jmkqqg6i24dx1crc?svg=true)](https://ci.appveyor.com/project/TomPallister/ocelot-provider-consul)
Windows (AppVeyor)
[![Build Status](https://travis-ci.org/ThreeMammals/Ocelot.Provider.Consul.svg?branch=develop)](https://travis-ci.org/ThreeMammals/Ocelot.Provider.Consul) Linux & OSX (Travis)

[![Coverage Status](https://coveralls.io/repos/github/ThreeMammals/Ocelot.Provider.Consul/badge.svg)](https://coveralls.io/github/ThreeMammals/Ocelot.Provider.Consul)

# Ocelot

This package adds [Consul](https://www.consul.io/) support to Ocelot via the package [Consul.NET](https://github.com/PlayFab/consuldotnet).

## How to install

Ocelot is designed to work with ASP.NET Core only and it targets `netstandard2.0`. This means it can be used anywhere `.NET Standard 2.0` is supported, including `.NET Core 2.1` and `.NET Framework 4.7.2` and up. [This](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) documentation may prove helpful when working out if Ocelot would be suitable for you.

Install Ocelot and it's dependencies using NuGet. 

`Install-Package Ocelot.Provider.SqlServer`

Or via the .NET Core CLI:

`dotnet add package Ocelot.Provider.SqlServer`

All versions can be found [here](https://www.nuget.org/packages/Ocelot.Provider.Consul/)