Configuring an MVC client
=========================

An ASP.NET Core MVC (or Razor Pages) app typically uses cookie authentication plus the OpenID Connect handler, and treats IdentityServer as the authority.

::

    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie("Cookies")
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5001";
            options.ClientId = "mvc";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
        });

    app.UseAuthentication();
    app.UseAuthorization();

Register a matching ``Client`` on the IdentityServer host (authorization code + PKCE, the MVC redirect and logout URIs, and the scopes you need).

Step-by-step: :ref:`interactive applications <refInteractiveQuickstart>`. Sign-in details: :ref:`signin <refSignIn>`.
