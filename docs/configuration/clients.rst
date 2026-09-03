Configuring clients
===================

A ``Client`` is an application that talks to IdentityServer: a machine client, a web app, a SPA, or a native app.

The important settings are ``ClientId``, ``AllowedGrantTypes``, ``ClientSecrets`` (when required), ``RedirectUris``, and ``AllowedScopes``. See the full :ref:`client reference <refClient>`.

Example client-credentials client::

    new Client
    {
        ClientId = "client",
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        ClientSecrets = { new Secret("secret".Sha256()) },
        AllowedScopes = { "api1" }
    }

Load clients with ``AddInMemoryClients`` or ``AddConfigurationStore``.

Pushed Authorization Requests can be required per client (``RequirePushedAuthorization``) or for every client (``options.PushedAuthorization.Required``). See :ref:`PAR <refPushedAuthorization>`.
