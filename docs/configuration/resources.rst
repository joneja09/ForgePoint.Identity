Configuring resources
=====================

Resources are what a client can request: identity data for the user, and API access.

* **Identity resources** (``IdentityResource``) — scopes such as ``openid`` and ``profile`` that map to claims in the identity token. See :ref:`identity resources <refIdentityResource>`.
* **API scopes** (``ApiScope``) — the ``scope`` values a client asks for when calling APIs. See :ref:`API scopes <refApiScope>`.
* **API resources** (``ApiResource``) — named APIs, audiences, and optional API secrets for introspection. See :ref:`API resources <refApiResource>`.

Load them from code::

    builder.Services.AddIdentityServer()
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryApiResources(Config.ApiResources);

Or from EF Core with ``AddConfigurationStore``. Design guidance is in :ref:`resources <refResources>`.
