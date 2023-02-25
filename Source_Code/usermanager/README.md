# FusionAuth and .NET core 

An example of using the FusionAuth API with .NET core.

This application uses FusionAuth APIs to add a user and then search for users from the command line. This can be used to pull a subset of data from the FusionAuth database, for instance to get the assigned laptop number.

## Blog post

You can read a blog post about building this example application here: https://fusionauth.io/blog/2020/04/28/dot-net-command-line-client

## Setup

Update `usermanager/Program.cs` where noted.

## To run


```
cd usermanager
fusionauth_api_key=<yourkey here> dotnet.exe run -- newuser@example.com mysecurepassword blue # on windows
fusionauth_api_key=<yourkey here> dotnet run -- newuser@example.com mysecurepassword blue # on macos or linux
```

Output:
```
created user with email: newuser@example.com
```

If you want to test patching the user, edit the code and uncomment the patching after registration.
