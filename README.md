# AspNet.Security.OAuth.VK
VK (VKontakte) OAuth for ASP.NET Core 2  

## Reference  
Auth for ASP.NET Core 1.1 Vkontakte is placed in https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/tree/dev/src/AspNet.Security.OAuth.Vkontakte. This project provides such ability for Core 2 apps. 

## Pakage  
https://www.nuget.org/packages/AspNet.Security.OAuth.VK/

## Usage  
In ConfigureServices in Startup.cs add code:  
```csharp  
services
	.AddAuthentication()
	.AddVK(options =>
	{
		options.ClientId = "YOUR_CLIENT_ID";
		options.ClientId = "YOUR_CLIENT_ID";

		options.ApiVersion = "8.57"; // VK api version

		// Request for permissions https://vk.com/dev/permissions?f=1.%20Access%20Permissions%20for%20User%20Token
		options.Scope.Add("email");

		// Add fields https://vk.com/dev/objects/user
		options.Fields.Add("uid");
		options.Fields.Add("first_name");
		options.Fields.Add("last_name");

		// In this case email will return in OAuthTokenResponse, 
		// but all scope values will be merged with user response
		// so we can claim it as field
		options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
		options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
		options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
		options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
	})
```
