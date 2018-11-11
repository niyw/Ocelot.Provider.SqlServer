[English](README.md) | 中文 | [更新说明](ReleaseNotes.md) | [Release Notes](ReleaseNotes.en-US.md)
# Ocelot.Provider.SqlServer
使用Microsoft SQL Server存储[Ocelot](http://threemammals.com/ocelot)配置项

# Ocelot

[<img src="http://threemammals.com/images/ocelot_logo.png">](http://threemammals.com/ocelot)

[![Build status](https://ci.appveyor.com/api/projects/status/jmkqqg6i24dx1crc?svg=true)](https://ci.appveyor.com/project/TomPallister/ocelot-provider-consul)
Windows (AppVeyor)
[![Build Status](https://travis-ci.org/ThreeMammals/Ocelot.Provider.Consul.svg?branch=develop)](https://travis-ci.org/ThreeMammals/Ocelot.Provider.Consul) Linux & OSX (Travis)

此包用于添加Ocelot对微软SQL Server数据库的支持，用于存储路由配置项。

## 如何安装

适用下面命令从NuGet安装 Niyw.Ocelot.Provider.SqlServer 及其依赖项. 

`Install-Package Niyw.Ocelot.Provider.SqlServer`

或者通过 .NET Core CLI:

`dotnet add package Niyw.Ocelot.Provider.SqlServer`

此包的所有版本 [在此](https://www.nuget.org/packages/Niyw.Ocelot.Provider.SqlServer/)

## 如何运行
- 切换到目录‘test\OcelotApiGw’，在.NET Core CLI中执行：

`dotnet ef migrations add InitialOcelotConfigDbMigration -c OcelotConfigDbContext -o DbMigrations/OcelotConfigDb`

- 启动测试项目

# 鸣谢
此包开发从[Ocelot.Provider.Consul](https://github.com/ThreeMammals/Ocelot.Provider.Consul)获取一些帮助。