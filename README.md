# Lufthansa.Api
A .NET9 API Client for the Lufthansa API

1. Clone the Repo
2. Create an account for the [Lufthansa API](https://developer.lufthansa.com/io-docs).  This will get you the necessary ClientId/Secret for use against Lufthansa REST Api routes.
3. Setup User Secrets for the `Lufthansa.Api.ClientExample` Project:
```
{  
  "Lufthansa": {    
    "ClientId": "",
    "ClientSecret": ""
  }
}
```
4. Run `Lufthansa.Api.ClientExample`

**Note:** May want to add some Polly retry policies as the dev api gives inconsistent results between runs, but should, ideally, run without exceptions given the test method setup.