.. _refPushedAuthorization:
Pushed Authorization Requests
=============================

ForgePoint.Identity implements `RFC 9126 <https://www.rfc-editor.org/rfc/rfc9126>`_ at ``/connect/par``.

The client POSTs the authorize parameters (and authenticates) to PAR. The server stores them and returns a short-lived ``request_uri``. The browser authorize redirect then sends only ``client_id`` and that ``request_uri``, so secrets and PII are not placed on the front channel.

Discovery
^^^^^^^^^

When ``options.Endpoints.EnablePushedAuthorizationEndpoint`` is ``true`` (the default), the discovery document includes ``pushed_authorization_request_endpoint``. If ``options.PushedAuthorization.Required`` is ``true``, discovery also advertises that PAR is required.

Request
^^^^^^^

``POST /connect/par`` with ``application/x-www-form-urlencoded``, using the same client authentication as the token endpoint, plus the usual authorize parameters (``response_type``, ``scope``, ``redirect_uri``, ``state``, PKCE, and so on).

A successful response looks like::

    {
      "request_uri": "urn:ietf:params:oauth:request_uri:…",
      "expires_in": 60
    }

Then send the user to the authorize endpoint with ``client_id`` and ``request_uri``.

Requiring PAR
^^^^^^^^^^^^^

* One client: ``Client.RequirePushedAuthorization = true``
* Every client: ``options.PushedAuthorization.Required = true``

``options.PushedAuthorization.Lifetime`` is the stored-request lifetime in seconds (default ``60``).

Storage
^^^^^^^

The default store keeps pushed requests in memory. When you add an operational store (``AddOperationalStore``), PAR uses the persisted-grant store. IdentityServer4 4.x configuration databases also need the ``RequirePushedAuthorization`` column; see ``docs/migrations/add-require-pushed-authorization.sql``.
