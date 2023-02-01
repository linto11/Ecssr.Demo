# Ecssr.Demo
This is a demo application for the ECSSR Team

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites--installation">Prerequisites & Installation</a></li>
        <li><a href="#usage">Usage</a>
          <ul>
            <li><a href="#in-memory">In Memory</a></li>
            <li><a href="#ms-sql-server">MS SQL Server</a></li>
          </ul>
        </li>
      </ul>
    </li>
    <li><a href="#screenshots">Screenshots</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project
This entire application is made up of angular as the front-end and .net core 6 as the backend. The simple idea of this applciation is to show a list of information and on click of a particular list, the entire details are populated on the next screen.

Even though it is a basic application, I have tried to make the application very modern and efficient using all the latest technology inside it and I have given my best to build this application.

> **--**
> I understand there would be still some short comings or mistakes or I would have done some wrong implementation. Please don't consider it as my in efficiency.

**Listing Screen**
![Landing Screenshot][product-screenshot]


### Built With

* [![Netcore][ASP.NETCore]][aspnet-url]
* [![C#][c#]][c#-url]
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]
* [![Angular][Angular.io]][Angular-url]
* And Many More

## Getting Started

The project already has all set up. Nothing extra needs to be configured. Please check the below pre-requisites

### Prerequisites & Installation

It is assumed the below tool are already installed. If not please install:
> Visual Studio (.net 6+) with Angular (14+).

> dotnet(6+) ef for running migration commands.

### Usage
This application runs in two environments:
> In Memory

> MS SQL SERVER

Choose the way required:

> Clone the repository to you local path from Visual Studio

> Clean and Rebuild the solution. This should restore all the packages and should give `Rebuild All Succeed`. Once this is achieved do one of the following

#### In Memory
> Go to appsettings.development.json and do this change
```js
"DatabaseSetting": {
      "IsInMemory": true,
      "SeedData": true
    }
```
> Run the Project

#### MS SQL Server
> Go to appsettings.development.json and do this change
```js
"DatabaseSetting": {
      "IsInMemory": false,
      "SeedData": false,
      "ConnectionString": "<GIVE-THE-CONNECTION-STRING>"
    }
```

> After the above changes, run the below update migration command. (Run the command where the `solution` file is located. Consider it as the root path.)
```cmd
dotnet ef database update --project Ecssr.Demo.Infrastructure --startup-project Ecssr.Demo
```

> Once migration is successful, update the appsettings and change to this
```js
"DatabaseSetting": {
      "SeedData": true,
    }
```

> Run the Project

## Screenshots
Once the application is up and running. You should see the below screens as shown in the screenshot.

**Home**
![Home Screenshot][home-screenshot]
**Listing**
![Listing Screenshot][listing-screenshot]
**Detail**
![Detail Screenshot][detail-screenshot]

**THANKS**

<!-- MARKDOWN LINKS & IMAGES -->
[product-screenshot]: Ecssr.Demo/wwwroot/screenshot/SS1.png
[ASP.NETCore]: https://img.shields.io/badge/aspnetcore-%23512BD4?style=for-the-badge&logo=aspnetcore&logoColor=512BD4
[aspnet-url]: https://get.asp.net/
[c#]: https://img.shields.io/badge/C%23-%20?style=for-the-badge&logo=Csharp&logoColor=white
[c#-url]: https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[home-screenshot]: Ecssr.Demo/wwwroot/screenshot/SS2.png
[listing-screenshot]: Ecssr.Demo/wwwroot/screenshot/SS1.png
[detail-screenshot]: Ecssr.Demo/wwwroot/screenshot/SS3.png
