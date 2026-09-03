.. _refHealthChecks:
Health checks
=============

Register the IdentityServer health check next to the host's other checks::

    builder.Services.AddHealthChecks()
        .AddIdentityServer();

    app.MapHealthChecks("/health");

The check (``IdentityServerHealthCheck``) asks ``IKeyMaterialService`` for signing credentials. It returns **healthy** when a credential is configured and **unhealthy** when none is.

That is a readiness signal for token issuance, not a full protocol probe. It does not call authorize or token endpoints, and it does not check your user store or database.

The default check name is ``identityserver``. You can pass a name and tags::

    builder.Services.AddHealthChecks()
        .AddIdentityServer(name: "idsrv", tags: new[] { "ready" });
