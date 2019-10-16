# Release notes
# 1.6.5
* Add overload with long type for AddQueryParameter method

# 1.6.4
* Add new nuget tags

# 1.6.3
* Now calculate dynamically the headers to add to resquest.

In this sample we get a custom token and add it to all our requests => 
```cs
client.Settings.CalculateHeadersHandler = async () =>
{
   var token = await GetACustomTokenAsync();

   var headers = new Headers
   {
       { "CustomToken", token },
   };
   return headers;
};
  ```

## 1.6.2
* Now HttpException expose the headers of the response
* Constructor of HttpException is now internal
* Add possibility to encapsulate HttpException automatically.

We can setup a global handler to provide a logic to encapsulate HttpException automatically.
For example I can choose to translate all HttpException with StatusCode NotFound in a NotFoundCustomException.
```cs
client.Settings.EncapsulateHttpExceptionHandler = (ex) =>
{
   if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
   {
      return new NotFoundCustomException();
   }

   return ex;
};
```

Now if I call an API wich respond with status code NotFound it will throw automaticaly my custom exception.
```cs
// Call an API wich throw NotFound error
await client.GetRequest("APIWhichNotExists").ExecuteAsync();
```

## 1.6.1
* Fix patch request which sent patch verb in lowercase

## 1.6.0
* Add support of Entity Tag (ETag)

ETag is not enabled by default to enable it :
```cs
client.Settings.ETagContainer = new ETagFileContainer(@"C:\ETagFolder");
```
You can also enable on only on specific request like below :
```cs
request.WithETagContainer(eTagContainer);
```
* Add support of string content (for mono part and multipart requests)
```cs
request.AddStringContent("myContent").ExecuteAsycnc();
```
* Now the assembly is strong named

## 1.5.5
* Fix a bug on cURL listener (when body was null) the cURL request wasn't displayed

## 1.5.4
* Fix a bug on cURL listener
* Add the possibility to debug the lib with source link (https://www.hanselman.com/blog/ExploringNETCoresSourceLinkSteppingIntoTheSourceCodeOfNuGetPackagesYouDontOwn.aspx)
* Add 2 new supported media tye for json : "application/json-patch+json", "application/*+json"

## 1.5.3
* Fix null reference exception when adding 2 times te same header

## 1.5.2
* Add new listener to print cURL request in debug output.
 To enable it :

```cs
client.Settings.Listeners.AddCurl();
```

It produce this type of output in debug window for each ExecuteAsync called :
```cs
curl -X POST "http://localhost:4242/api/PostTest/complex"-H "Accept: application/json" -H "Content-Type: application/json" -d "{\"Id\":42,\"Data\":\"DATA\"}"
```
* The XmlFormatter now produce not indented xml (lighter than previous)
* The XmlFormatter have ne property WriterSettings which allow to configure the way to write XML streams
* Add support of CamelCase (PropertyName => propertyName) for JsonFormatter
* Add support of SnakeCase (PropertyName => property_name) for JsonFormatter
* Add support of KebabCase also known as SpinalCase (PropertyName => property-name) for JsonFormatter


## 1.5.1
* Add support of Defalte
* Change the message of HttpException to be easier to understand

## 1.5.0
* Fix a bug on headers reading
* Fix a small bug on postman of generation of postman file
* Fix a bug when response is a byte array
* Fix a bug on Multipart which didn't took the serializer specified on the request
* Add support of Basic authentication
* Add support of OAuth 2.0 authentication
* Add support of .NET Framework 4.5, 4.6, 4.7
* Add support of .NET Standard 1.1, 1.2
* Add support of automatic decompression of GZIP

## 1.4.2
* Add the possibility to define timeout globaly or by request
* Now the API throw a TimeoutException when request is in timeout
* Add better support of cancellation tokens

Thanks to Thomas Levesque for the help.

## 1.4.1
* Rename PostMan to postman

## 1.4.0
* Add SerializerException
* Add PostMan listener
* Fix NullReferenceException when ContentType is null and the client try to deserialize it.
* Fix small bugs

## 1.3.5
* Fix small issue on formatting of debug formatter

## 1.3.4
* Support of netstandard1.3 (thanks to Benoit F) 
## 1.3.3
* Now the DebugListener use the class Debug to log data. (Thx to Thomas Levesque for the code review)

## 1.3.2 && 1.3.1
*  Fix some typos
## 1.3.0
* Rewrite the way to listen the client
* Add 'Settings' property with all settings of client
* Fix a bug on JsonFormatter which not used JsonSerializer config
* Now NewRequest use HttpMethod instead of HttpVerb enum

## 1.2.2
* Fix null reference exception if ContentType of response is null
## 1.2.1
* Fix AddQueryParameter when value is null (for server which are not in ASP.net Core)

## 1.2.0
* Add methods AddFormatter / RemoveFormatter
* Add a way to read responses headers
* Add the possibility to have raw string response
* Add the possibility to have raw HttpResponseMessage
* Add method to add file as content
* Add method to add file as content in multipart requests
* Add support of file content (multipart and normal request)
* Add overloads on method AddQueryStringParameters
* Fix an issue on AddHeader

## 1.1.3
* Add new overloads for PostRequest, PutRequest, PatchRequest

## 1.1.2
* Add overload on ExecuteAsync method (with less parameters)

## 1.1.1
* Remove hardcoded Headers and Add AddAcceptLanguageBasedOnCurrentCulture which add automaticaly current language

## 1.1.0
* Now the formatter have multiple accept types
* Add auto detect of which formatter to use
* Add methods : GetRequest PostRequest, PutRequest, DeleteRequest on TinyHttpClient
* Add support of multi-part form data

## 1.0.1
* Now the query parameters are url encoded. 
* Otpimizations. 
* Add documentation

## 1.0.0
Initial version

## 1.1.0-alpha0002
* Now the formatter surpport multi accept type

## 1.1.0-alpha0001 
* Otpimizations. Add support of multi-part form data
