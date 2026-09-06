Configuring IdentityServer
==========================

Add the services and middleware in your host startup::

    builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseErrorEvents = true;
            options.PushedAuthorization.Required = false;
        })
        .AddInMemoryClients(Clients.Get())
        .AddInMemoryIdentityResources(Resources.IdentityResources)
        .AddInMemoryApiScopes(Resources.ApiScopes)
        .AddDeveloperSigningCredential();

    builder.Services.AddHealthChecks()
        .AddIdentityServer();

    app.UseIdentityServer();
    app.MapHealthChecks("/health");

Pass an ``IdentityServerOptions`` delegate to ``AddIdentityServer`` for issuer, endpoints, CORS, PAR, and UI paths. See :ref:`options <refOptions>` and :ref:`startup services <refStartup>`.

For production, replace ``AddDeveloperSigningCredential`` with ``AddSigningCredential`` and persist operational data. See :ref:`cryptography <refCrypto>` and :ref:`Entity Framework <refEF>`.
