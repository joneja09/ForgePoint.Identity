Packaging and Builds
====================

ForgePoint.Identity ships as five NuGet packages from this repository:

`github <https://github.com/joneja09/ForgePoint.Identity>`_

* `ForgePoint.Identity <https://www.nuget.org/packages/ForgePoint.Identity/>`_ — protocol implementation and ASP.NET Core host integration
* `ForgePoint.Identity.Storage <https://www.nuget.org/packages/ForgePoint.Identity.Storage>`_ — store contracts and models (pulled in by the protocol package)
* `ForgePoint.Identity.EntityFramework <https://www.nuget.org/packages/ForgePoint.Identity.EntityFramework>`_ — EF Core configuration and operational stores
* `ForgePoint.Identity.EntityFramework.Storage <https://www.nuget.org/packages/ForgePoint.Identity.EntityFramework.Storage>`_ — EF Core entities, DbContexts, and mappings
* `ForgePoint.Identity.AspNetIdentity <https://www.nuget.org/packages/ForgePoint.Identity.AspNetIdentity>`_ — ASP.NET Core Identity integration

``AddIdentityServer()`` and related type names are unchanged. C# namespaces are ``ForgePoint.Identity.*``.

Quickstart UI
^^^^^^^^^^^^^
Login, logout, consent, and error UI samples live in this repository:

`samples/Quickstarts <https://github.com/joneja09/ForgePoint.Identity/tree/main/samples/Quickstarts>`_

Access token validation
^^^^^^^^^^^^^^^^^^^^^^^
APIs should validate JWTs with ``Microsoft.AspNetCore.Authentication.JwtBearer`` and reference tokens with IdentityModel OAuth 2.0 introspection. See :ref:`Protecting APIs <refProtectingApis>`.

CI builds
^^^^^^^^^
Release builds are published to nuget.org. For local development, ``./build.sh`` packs nupkgs into ``./nuget`` at the repository root.
