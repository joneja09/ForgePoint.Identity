.. _refQuickstartOverview:
Overview
========
The quickstarts provide step by step instructions for various common IdentityServer scenarios.
They start with the absolute basics and become more complex - 
it is recommended you do them in order.

* adding IdentityServer to an ASP.NET Core application
* configuring IdentityServer
* issuing tokens for various clients
* securing web applications and APIs
* adding support for EntityFramework based configuration
* adding support for ASP.NET Identity

Every quickstart has a reference solution - you can find the code in the 
`samples <https://github.com/joneja09/ForgePoint.Identity/tree/main/samples/Quickstarts>`_ folder.

Preparation
^^^^^^^^^^^
Clone this repository and start from the matching folder under ``samples/Quickstarts``.
There is no separate ``dotnet new`` template pack.

To follow a tutorial from scratch, create an ASP.NET Core web app and add the ``ForgePoint.Identity`` package::

    dotnet new web -n IdentityServer
    cd IdentityServer
    dotnet add package ForgePoint.Identity

Copy login, logout, consent, and error UI from ``samples/Quickstarts`` when you need interactive flows.

OK - let's get started!
