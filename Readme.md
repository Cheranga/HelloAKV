# Notes

## You can access the AKV secrets using the below methods.

> **Add the azure key vault as configuration.**
  * Install the below packages.
    * `Microsoft.Extensions.Configuration.AzureKeyVault`
	* `Microsoft.Azure.KeyVault`
	* `Microsoft.Azure.Services.AppAuthentication`

  * In the `program.cs` add the below code,

  ```CSharp
     public static IWebHostBuilder CreateWebHostBuilder(string[] args)
     {
         return WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration(builder =>
             {
                 var tokenProvider = new AzureServiceTokenProvider();
                 var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));

                 builder.AddAzureKeyVault(@"https://mystash.vault.azure.net", keyVaultClient, new DefaultKeyVaultSecretManager());
             })
             .UseStartup<Startup>();
     }
  ```

  * Finally, in your controller do the below modifications.

  ```CSharp
  public ValuesController(IConfiguration configuration)
  {
      _configuration = configuration;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<string>>> Get()
  {
      var secretValue = _configuration["DbConnectionString"];

      return new[] {secretValue};
  }
  ```

---

> **In the action method it self (But this is not ideal)**

```CSharp
[HttpGet]
public async Task<ActionResult<IEnumerable<string>>> Get()
{   
    var tokenProvider = new AzureServiceTokenProvider();
    var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
    var secretData = await keyVaultClient.GetSecretAsync(@"https://mystash.vault.azure.net/secrets/DbConnectionString").ConfigureAwait(false);

    if (secretData == null)
    {
        return new[] {"Secret cannot be accessed"};
    }

    return new[] {secretValue};
}
```

