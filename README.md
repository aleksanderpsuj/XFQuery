<h1 align="center">🤖 XFQuery - Another TeamSpeak Bot 🤖</h1>

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/aleksanderpsuj/XFQuery/build-test?style=for-the-badge)
![GitHub issues](https://img.shields.io/github/issues/aleksanderpsuj/XFQuery?style=for-the-badge)
![GitHub](https://img.shields.io/github/license/aleksanderpsuj/XFQuery?style=for-the-badge)
![GitHub contributors](https://img.shields.io/github/contributors/aleksanderpsuj/XFQuery?style=for-the-badge)

> #### **XFQuery** is a event driven **teamspeak bot** written in **C#**. The goal of this bot is to **automate** the **boring** side of teamspeak, such as keeping an eye on users or creating channels, it has many features that will **automate** and keep your server **secure**.

### 🏠 [Homepage](https://github.com/aleksanderpsuj/XFQuery)
### 🐞 [Issues](https://github.com/aleksanderpsuj/XFQuery/issues)
### 🚧 [TODO](https://github.com/aleksanderpsuj/XFQuery/projects/1)

## First run
### Build & Run
#### Linux
> [1] Install .NET Core Runtime (https://docs.microsoft.com/en-us/dotnet/core/install/linux)<br>
> [2] git clone https://github.com/aleksanderpsuj/XFQuery/<br>
> [3] cd XFQuery<br>
> [4] dotnet build --configuration Release<br>
> [5] cd XFQuery.Daemon/bin/Release/netcoreapp3.1/<br>
> [5] ./XFQuery.Daemon<br>
#### Windows
> [1] Install .NET Core Runtime (https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=netcore31)<br>
> [2] git clone https://github.com/aleksanderpsuj/XFQuery/<br>
> [3] cd XFQuery<br>
> [4] dotnet build --configuration Release<br>
> [5] cd XFQuery.Daemon/bin/Release/netcoreapp3.1/<br>
> [5] XFQuery.Daemon.exe<br>
>
## Functions
### General
> * Multi-Language Support [✔️]
> * Configs Loaders [✔️]
> * MongoDB Connection [✔️]


### Event-driven
* **Server**
> * Check VPN [🚧]

* **Channel**
> * Poke Channels [🚧]
> * Register Channels [✔️]

* **Text Message**
> * Welcome Message [✔️]

### Interval
> * Performance Check [✔️]

### Combined
> * Server Name [✔️]
> * Nickname Check [✔️]
> * Admin Status [🚧]

## Author

👨 **Aleksander Psuj**

* Github: [@aleksanderpsuj](https://github.com/aleksanderpsuj)
* LinkedIn: [@aleksander-psuj](https://linkedin.com/in/aleksander-psuj)

## Show your support

if you like this project hit that ⭐️ button 😄

XFQuery is based on top of other awesome repos:
> * [MongoDB](https://github.com/mongodb/mongo)<br>
> * [Autofac](https://github.com/autofac/Autofac)<br>
> * [AutoMapper](https://github.com/AutoMapper/AutoMapper)<br>
> * [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)<br>
> * [TS3QueryLib.Net.Core](https://github.com/Scordo/TS3QueryLib.Net.Core)<br>

## License

This project is [BSD 3-Clause License](https://github.com/aleksanderpsuj/XFQuery/blob/master/LICENSE) licensed.
