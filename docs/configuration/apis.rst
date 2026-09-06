Configuring APIs
================

APIs validate access tokens issued by ForgePoint.Identity. This repository does not ship ``IdentityServer4.AccessTokenValidation``.

JWTs
^^^^

::

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = "https://localhost:5001";
            options.Audience = "resource1";
            options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        });

Reference tokens
^^^^^^^^^^^^^^^^

Use the IdentityModel `OAuth 2.0 introspection handler <https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection>`_ and configure an ``ApiResource`` secret.

Same-host APIs
^^^^^^^^^^^^^^

If the API lives in the IdentityServer process, use ``AddLocalApiAuthentication()``. See :ref:`adding APIs to the host <refAddApis>` and :ref:`protecting APIs <refProtectingApis>`.
