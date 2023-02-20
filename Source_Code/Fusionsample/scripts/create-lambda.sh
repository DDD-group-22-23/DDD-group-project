
AUTH_HEADER=YOUR_API_KEY_HERE
APPLICATION_ID=YOUR_APP_ID_HERE


lambda_output=`curl -s -XPOST -H "Authorization: $AUTH_HEADER" -H"Content-Type: application/json" http://localhost:9011/api/lambda -d '{ "lambda" : {"body" : "function populate(jwt, user, registration) { jwt.favoriteColor = user.data.favoriteColor; }", "name": "exposeColor", "type" : "JWTPopulate" } }'`


lambda_id=`echo $lambda_output|sed -r 's/.*"id":"([^"]*)".*/\1/g'`
application_patch='{"application" : {"lambdaConfiguration" : { "accessTokenPopulateId" : "'$lambda_id'", "idTokenPopulateId" : "'$lambda_id'" } } }'

output=`curl -s -XPATCH -H "Authorization: $AUTH_HEADER" -H"Content-Type: application/json" http://localhost:9011/api/application/$APPLICATION_ID -d "$application_patch"`
