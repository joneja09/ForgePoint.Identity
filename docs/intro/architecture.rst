Architecture
============

ForgePoint.Identity is ASP.NET Core middleware plus a set of services you register in DI.
The host application owns login, logout, consent, and any other UI. The middleware adds the OpenID Connect and OAuth 2.0 protocol endpoints.

.. image:: images/middleware.png

Typical host
^^^^^^^^^^^^

* Call ``AddIdentityServer()`` in ``ConfigureServices`` and chain store / key / user extensions.
* Call ``UseIdentityServer()`` in the pipeline (it includes routing and the protocol endpoints).
* Provide MVC (or another UI) for the pages in ``IdentityServerOptions.UserInteraction``.

Stores
^^^^^^

IdentityServer asks stores for configuration and operational state:

* ``IClientStore`` / ``IResourceStore`` — clients, identity resources, API scopes and resources
* ``IPersistedGrantStore`` — authorization codes, refresh tokens, reference tokens, consents
* ``IDeviceFlowStore`` — device authorization
* ``IPushedAuthorizationStore`` — Pushed Authorization Requests (in-memory by default; persisted-grant backed when you add an operational store)

In-memory implementations are for development. EF Core implementations live in ``ForgePoint.Identity.EntityFramework``.

Keys and health
^^^^^^^^^^^^^^^

Token signing uses ``ISigningCredentialStore`` / ``IValidationKeysStore``. See :ref:`cryptography <refCrypto>`.
``AddHealthChecks().AddIdentityServer()`` reports unhealthy when no signing credential is configured. See :ref:`health checks <refHealthChecks>`.

Further reading
^^^^^^^^^^^^^^^

* :ref:`Startup <refStartup>`
* :ref:`Options <refOptions>`
* :ref:`Protecting APIs <refProtectingApis>`
