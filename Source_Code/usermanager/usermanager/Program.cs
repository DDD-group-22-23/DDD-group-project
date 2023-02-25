using System;
using io.fusionauth;
using io.fusionauth.domain;
using io.fusionauth.domain.api;
using io.fusionauth.domain.api.user;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace usermanager
{
    class Program
    {
        private static readonly string apiKey = Environment.GetEnvironmentVariable("fusionauth_api_key");
        private static readonly string fusionauthURL = "http://20.88.169.245:9011";

	private static readonly string tenantId = "13bbefea-b1d0-11ed-9c51-6adf74905334";
	private static readonly string applicationId = "75dc9e9f-ee9b-4baf-afb5-cde509ee94d7";

        static void Main(string[] args)
        {
	    if (args.Length != 3) {
                Console.WriteLine("Please provide email, password and favorite color.");
		Environment.Exit(1);
	    }
            string email= args[0];
            string password = args[1];
            string favoriteColor = args[2];

            FusionAuthSyncClient client = new FusionAuthSyncClient(apiKey, fusionauthURL, tenantId);

	    var userRequest = buildUserRequest(email, password, favoriteColor);
            var response = client.CreateUser(null, userRequest);
	    // debugging
	    //string json = JsonConvert.SerializeObject(response);
            //Console.WriteLine(json);

            if (response.WasSuccessful())
            {
                var user = response.successResponse.user;
		var registrationResponse = register(client, user);
		if (registrationResponse.WasSuccessful()) {
		    //var patchResponse = patch(client, "favcolorred",user);
                    //Console.WriteLine("patched user with email: "+user.email);
		} 
                else if (registrationResponse.statusCode != 200) 
                {
                    var statusCode = registrationResponse.statusCode;
                    Console.WriteLine("failed with status "+statusCode);
	            string json = JsonConvert.SerializeObject(response);
                    Console.WriteLine(json);
                } 
            } 
            else if (response.statusCode != 200) 
            {
                var statusCode = response.statusCode;
                Console.WriteLine("failed with status "+statusCode);
	        string json = JsonConvert.SerializeObject(response);
                Console.WriteLine(json);
            } 
        }

        static UserRequest buildUserRequest(string email, string password, string favoriteColor)
	{
	    User userToCreate = new User();
	    userToCreate.email = email;
	    userToCreate.password = password;
	    Dictionary<string, object> data = new Dictionary<string, object>();
	    data.Add("favoriteColor", favoriteColor);
	    userToCreate.data = data;

	    UserRequest userRequest = new UserRequest();
	    userRequest.sendSetPasswordEmail = false;
	    userRequest.user = userToCreate;
	    return userRequest;
	}

        static ClientResponse<RegistrationResponse> register(FusionAuthSyncClient client, User user)
        {
	    RegistrationRequest registrationRequest = new RegistrationRequest();
	    UserRegistration registration = new UserRegistration();
	    registration.applicationId = Guid.Parse(applicationId);
	    registrationRequest.sendSetPasswordEmail = false;
	    registrationRequest.skipRegistrationVerification = true;
	    registrationRequest.skipVerification = true;
	    registrationRequest.registration = registration;
            return client.Register(user.id, registrationRequest);
        }

        static ClientResponse<UserResponse> patch(FusionAuthSyncClient client, String newfavColor, User userToPatch)
        {
	    Dictionary<string, object> request = new Dictionary<string, object>();
	    Dictionary<string, object> user = new Dictionary<string, object>();
	    Dictionary<string, object> data = new Dictionary<string, object>();
            user.Add("data", data);
	    data.Add("favoriteColor", newfavColor); 
            data.Add("recoveryCode", "recoveryCode");
            user.Add("firstName", "D");
            request.Add("user", user);
	    string json = JsonConvert.SerializeObject(request);
            Console.WriteLine(json);

            return client.PatchUser(userToPatch.id, request);
        }
    }
}
