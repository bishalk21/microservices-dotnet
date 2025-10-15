# JSON Web Tokens (JWT) & JSON Web Key Sets (JWKS)

## JSON Web Tokens (JWT)

- [https://www.jwt.io/](https://www.jwt.io/)

JSON Web Token (JWT) is a compact, URL-safe means of representing claims to be transferred between two parties. The claims in a JWT are encoded as a JSON object that is used as the payload of a JSON Web Signature (JWS) structure or as the plaintext of a JSON Web Encryption (JWE) structure, enabling the claims to be digitally signed or integrity protected with a Message Authentication Code (MAC) and/or encrypted.
A JWT is a string consisting of three parts, separated by dots (.), and serialized using base64. In the most common serialization format, compact serialization, the JWT looks like this: `xxxxx.yyyyy.zzzzz`.

- The first part (xxxxx) is the header.
- The second part (yyyyy) is the payload.
- The third part (zzzzz) is the signature.

The JOSE header contains the type of token (JWT) and the signing algorithm (e.g., HMAC SHA256 or RSA).
The payload contains the claims. This is usually where the user information is stored.
The signature is used to verify that the sender of the JWT is who it says it is and to ensure that the message wasn't changed along the way.

## JWTs in Authentication in Microservices (with AWS API Gateway & Lambda)

In a microservices architecture, JWTs can be used for authentication and authorization. When a user logs in, the authentication service generates a JWT and sends it to the client. The client then includes this JWT in the Authorization header of subsequent requests to the microservices. Each microservice can verify the JWT to ensure that the request is authenticated and authorized.

### Example of a JWT

Here is an example of a JWT:

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

This JWT consists of three parts:

1. Header: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9`
2. Payload: `eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9`
3. Signature: `SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c`

### Decoding the JWT

You can decode the JWT using a JWT decoder tool, such as [https://jwt.io/](https://jwt.io/). When you decode the above JWT, you will see the following:

#### Header

- Header is a JSON object that contains metadata about the token, such as the signing algorithm and token type.

```json
{
  "kid": "EkG21q2pStVdf45fh56gh456hpX3OlfU6EdfgsdfgdhfgdfhF5IHq436f9pB+fDj6ublZI=",
  "alg": "RS256"
}
```

- `kid`
  - it is created by the identity provider (e.g., Auth0, AWS Cognito) when the JWT is issued.
  - Key ID, used to identify the key used to sign the JWT.
  - we need kid to match it with the public key in the JWKS to verify the signature.
  - It is a base64-encoded string that represents the public key used to sign the JWT.
- `alg`
  - Algorithm used to sign the JWT (RS256 in this case).
  - RS256 is an asymmetric algorithm that uses a pair of keys (public and private) for signing and verification.

#### Payload

- Payload is a JSON object that contains the claims. Claims are statements about an entity (typically, the user) and additional data.

```json
{
  "at_hash": "ZjSDjGJ3jXZ_b5nwSU2yaQasdfadgsdfasdfsadf",
  "sub": "396e0428-e0d1-7044-1618-9fb8846f792543d",
  "cognito:groups": ["Admin"],
  "email_verified": true,
  "address": {
    "formatted": "Sydney"
  },
  "iss": "https://cognito.ap-southeast-2.amazonaws.com/ap-southeast-2_2Logfdk2348S7dsfV",
  "phone_number_verified": false,
  "cognito:username": "396e042s3458-e0d1-7044-1618-9fb8846fdsgf47925",
  "given_name": "Bishal ",
  "aud": "12g9vpsdafads4uhg2jg2cbc88iep4jg2f",
  "event_id": "2d004e83-8481-42f1-9b14-21a13sdgdf43hfd1d49ab1",
  "token_use": "id",
  "auth_time": 1760504026,
  "phone_number": "+61433982563",
  "exp": 1760507626,
  "iat": 1760504026,
  "family_name": "Karki",
  "jti": "7b69d0f5-b2f0-447b-833e-9b8508sdfad34sdv334gdfgb4c562",
  "email": "karkibishal00@gmail.com"
}
```

- `at_hash`
  - Access Token hash value.
  - It is used to validate the access token that is issued alongside the ID token.
- `sub`
  - Subject - Identifier for the user.
  - It is a unique identifier for the user in the identity provider (e.g., AWS Cognito).
  - It is a UUID (Universally Unique Identifier) that is unique to each user.
- `cognito:groups`
  - Groups the user belongs to (e.g., Admin).
  - It is used to define the roles and permissions of the user.
  - In this case, the user belongs to the "Admin" group.
- `email_verified`
  - Indicates if the user's email is verified (true or false).
- `address`
  - User's address information.
  - In this case, it contains a formatted address "Sydney".
- `iss`
  - Issuer - The identity provider that issued the JWT.
  - It is a URL that represents the identity provider (e.g., AWS Cognito).
- `phone_number_verified`
  - Indicates if the user's phone number is verified (true or false).
- `cognito:username`
  - Username of the user in the identity provider.
- `aud`
  - Audience - The client ID of the application that the JWT is intended for.
  - It is used to ensure that the JWT is being used by the intended application.
- `event_id`
  - Unique identifier for the authentication event.
- `token_use`
  - Indicates the type of token (e.g., id, access).
  - In this case, it is an ID token.
- `auth_time`
  - The time when the user was authenticated (in Unix timestamp format).
- `phone_number`
  - User's phone number.
- `exp`
  - Expiration time - The time when the JWT expires (in Unix timestamp format).
- `iat`
  - Issued at - The time when the JWT was issued (in Unix timestamp format).
- `family_name`
  - User's family name (last name).
- `jti`
  - JWT ID - A unique identifier for the JWT.
- `email`
  - User's email address.

#### Signature

The signature is used to verify the authenticity of the JWT. It is created by taking the encoded header, the encoded payload, a secret (or private key), and the algorithm specified in the header, and signing that.
To verify the signature, you need to use the public key that corresponds to the private key used to sign the JWT. This is where JSON Web Key Sets (JWKS) come into play.

## JSON Web Key Sets (JWKS)

- [https://auth0.com/docs/secure/tokens/json-web-tokens/json-web-key-sets](https://auth0.com/docs/secure/tokens/json-web-tokens/json-web-key-sets)

A JSON Web Key Set (JWKS) is a set of keys that contains the public keys used to verify any JSON Web Token (JWT) issued by the authorization server and signed using the RS256 signing algorithm. The JWKS is a JSON object that contains an array of JWKs (JSON Web Keys). Each JWK contains information about a public key, such as its key type, usage, algorithm, and key ID.

When a JWT is issued, it is signed with a private key. To verify the signature of the JWT, you need to use the corresponding public key. The JWKS provides a way to retrieve the public keys used by the authorization server to sign the JWTs.

### Example of a JWKS

Here is an example of a JWKS:

```json
{
  "keys": [
    {
      "alg": "RS256",
      "e": "AQAB",
      "kid": "EkG21q2pStVhpX3Olffdghdfg45yfgh4fghdf56hgfh34hfghU6EdfgsdfgdhfgdfhF5IHq436f9pB+fDj6ublZI=",
      "kty": "RSA",
      "n": "sXchdXVhbnRlZC1rZXktc3RyaW5nLXdpdGgtc29tZS1zcGVjaWFsLWNodXJzLWFuZC1vdGhlci1jaGFycy1pbi1pdC1pcy1xdWl0ZS1sb25nLW5vdy1iYXNlNjQtZW5jb2RlZC1zdHJpbmctZm9yLXJzYS1wdWIta2V5",
      "use": "sig"
    }
  ]
}
```

- `keys`
  - An array of JWKs (JSON Web Keys).
  - Each JWK contains information about a public key, such as its key type, usage, algorithm, and key ID.
- `alg`
  - Algorithm used to sign the JWT (RS256 in this case).
- `e`
  - Exponent value for the RSA public key.
- `kid`
  - Key ID, used to identify the key used to sign the JWT.
  - It should match the `kid` in the JWT header.
- `kty`
  - Key type (e.g., RSA).
- `n`
  - Modulus value for the RSA public key.
- `use`
  - Intended use of the public key (e.g., sig for signature).

### Using JWKS to Verify JWTs

- Using aws-jwt-verify library:

  - go to https://docs.aws.amazon.com/cognito/latest/developerguide/amazon-cognito-user-pools-using-tokens-verifying-a-jwt.html
  - Validate tokens with aws-jwt-verify
  - https://cognito-idp.<Region>.amazonaws.com/<userPoolId>/.well-known/jwks.json

  ```json
  {
    "keys": [
      {
        "alg": "RS256",
        "e": "AQAB",
        "kid": "EkG214564sadfasdhq2pfh5yhgfhdhf456StVhpX3OlfU6dfghEF5IHq436f9pB+f56e456Dj6ublZI=",
        "kty": "RSA",
        "n": "xEhNlesfdgsd456gfyrjgfhj4564e6C1J5OjZ0rT67epNpmyi6lFy_AydOfGYBh_gWuQWiyn7zt_Ax7DZUCeDDYZenT-Xb34fHBtaWjpIvw_fC85sjk5PCu84zRNR8O_Iq9oleEWJTbJwp789SC0OCCTCqE_tsscLUcchgbCwgsC-N0Q7Cc6M1hPLnKe_Bfghgfhhn-TMxwbsfghsf6lTikY1DMF1TtpdgnK3IAeJMiRQbQZd_YYjCthFP3OYQZaubbhd5Y9q1aR_3X8ddq3iRNm-HNHJjdLqoaMc-3jFGG5X7vZyVTddzuYy98d8-jObGbtkOo2S4_61K8X2u5M2aDxsECUDIFQzFunb_rckVTwNKYeJhwuwQ6Lw",
        "use": "sig"
      },
      {
        "alg": "RS256",
        "e": "AQAB",
        "kid": "EBQA5HRXQdgh5r6ghUgcts0pNfgh45yaIWdfghCk89OudfghdfghtQ/456456fghdfhirbyDKhh3pKunY=",
        "kty": "RSA",
        "n": "56NgkdD7fKUwv-ThWHqcuJmxe-dbFk-brNhXBj8m2F0mZ9dM_JWFzEa2cG15oH5SxL5CZPdJhjibFNw7LRdwp5qMcA3YQcAxGZffgcng45dfgh6756Zmpfe5ErtRPejghjgL1SHWNxvJQdT1ezMpvl7qQzMSnEdLawf9VpBZShqAnEEyV8q_1mNBFwSTd2HrhGJfLzt161_qnhJOhENiJGJ_pxOgjUGlofAI9gR7-5h7XB7aWfBXpvzE81HNnVSt03NSgk2IM6rjoFm1gs0KKFQi7y6lhINmdfghdfgh4lV5symmNzq-u45BtwhL8yVzmo8Wo44Sf3NlygR1ifcwKZaLhIXuxqva9auc0M6vFmpuP1GfUQ",
        "use": "sig"
      }
    ]
  }
  ```

  - There are two keys in the JWKS. Each key has a unique `kid`.
    - one is for signing access key
    - another is for signing id token
  - to know which key is for which token, you have to check and match the `kid` in the JWT header with the `kid` in the JWKS.
  - to secure key, use aws secrets manager service or parameter store
    - https://aws.amazon.com/secrets-manager/
    - create a new secret
    - choose Other type of secret
    - paste the JWKS json in the key/value pairs and click next
    - give a name to the secret (e.g., mySecret), and click next
    - keep the default values and click next
    - click on Create secret
    - we can access this secret in our lambda function to verify the JWT token using lambda authorizer
  - Lambda authorizer will read the token from secret manager, and using jwt library (e.g., aws-jwt-verify) it will verify the token and return the policy document to API Gateway to allow or deny the request.

- To verify a JWT using a JWKS, you need to follow these steps:

1. Decode the JWT to extract the header and payload.
2. Extract the `kid` from the JWT header.
3. Retrieve the JWKS from the authorization server.
4. Find the JWK in the JWKS that matches the `kid`.
5. Use the public key from the JWK to verify the JWT signature.
6. If the signature is valid, you can trust the claims in the JWT payload.
7. Additionally, check the standard claims such as `iss` (issuer), `aud` (audience), and `exp` (expiration) to ensure the token is valid for your application and has not expired.

```plaintext
HMACSHA256(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload),
  secret)
```

The output will be three Base64-URL strings separated by dots that can be easily passed in HTML and HTTP environments, while being more compact than other serialization formats such as XML or JSON.
