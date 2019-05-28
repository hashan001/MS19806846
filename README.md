# MS19806846

Login to Google Developer Console and Create a new project.
In Credetial tab select OAuth Client ID Then Generate Google Client ID and Client Secret.
Enable Google Calendar API.
Update the web.config file with the project Client ID and Client Secret from the developer console

Add the following Google NuGet Packages: Google.Apis, Google.Apis.Auth, Google.Apis.Auth.Mvc, Google.Apis.Core, Google.Apis.Calendar.v3

Update the Authorized redirect URIs. It will be something like http://yourLoclHostAddress/AuthCallback/IndexAsync.

For more detail : http://hashanslokuge.blogspot.com/2019/05/google-calender-update-using-oauth2.html
