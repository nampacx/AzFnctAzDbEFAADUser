# AzFnctAzDbEFAADUser

Sample code for a azure function which uses a bearer token to authenticate an user on a azure sql database.



Needed application settings:
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "",
    "SqlServerName": "",
    "SqlDbName": ""
  }
}
```



Sample Curl
```
curl --location --request GET 'http://localhost:7187/api/Function1' \
--header 'Authorization: Bearer {{yourtoken}}'
```



Needed API permissions for the app registration


![image](https://user-images.githubusercontent.com/8090625/197340707-8b559166-7199-4b4d-a521-831ba37661bb.png)


Get token via postman:

![image](https://user-images.githubusercontent.com/8090625/197340781-c4cdc5b3-152a-43e0-afb3-b45105ccbb0d.png)

| Setting | Vaule |
| --- | --- |
|Auth URL|https://login.microsoftonline.com/{{tenant_id}}/oauth2/v2.0/authorize|
|Access Token URL| https://login.microsoftonline.com/{{tenant_id}}/oauth2/v2.0/token|
|Client ID| app registration client id |
|Client Secret | Appregistration secret value|
|Scope | https://sql.azuresynapse-dogfood.net/user_impersonation|


