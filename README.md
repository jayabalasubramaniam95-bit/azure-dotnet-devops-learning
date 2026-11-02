# azure-dotnet-devops-learning

## Authentication and API Security

This project demonstrates authentication and authorization concepts used in modern .NET APIs.

### Local Development

The Product API can be configured to use JWT bearer authentication during local development.

The API receives a token through:

```http
Authorization: Bearer <token>
```

The API can then validate the token before allowing access to protected endpoints.

### Enterprise Azure Architecture

For a production Azure deployment, Microsoft Entra ID could be used as the identity provider.

The architecture would be:

```text
Client
   |
   | Authentication
   v
Microsoft Entra ID
   |
   | Access Token
   v
Client
   |
   | Bearer Token
   v
Azure App Service
Product API
```

The Product API would validate the access token and authorize requests using scopes and/or roles.

### Authentication Concepts

This project documents the differences between:

* API keys
* JWT bearer tokens
* OAuth 2.0
* Microsoft Entra ID
* Scopes
* Claims

See:

`docs/authentication-comparison.md`

### Security

No passwords, API keys, access tokens, or other secrets should be committed to the repository.
