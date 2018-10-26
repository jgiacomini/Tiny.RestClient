<img src="https://raw.githubusercontent.com/jgiacomini/Tiny.RestClient/master/icon.png" width="200" height="200" />

[![NuGet](https://img.shields.io/nuget/v/Tiny.RestClient.svg?label=NuGet)](https://www.nuget.org/packages/Tiny.RestClient/)
[![Build status](https://ci.appveyor.com/api/projects/status/08prv6a3pon8vx86?svg=true)](https://ci.appveyor.com/project/jgiacomini/tinyhttp)
[![Gitter chat](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/Tiny-RestClient/Lobby)
[![StackOverflow](https://img.shields.io/badge/questions-on%20StackOverflow-orange.svg?style=flat)](http://stackoverflow.com/questions/tagged/tiny.restclient)

Tiny.RestClient facilitates the dialog between your API and your application.
It hides all the complexity of communication, deserialisation ...


## Platform Support
The lib support **.NET Standard 1.1 to 2.0**, and **.NET Framework 4.5 to 4.7.**

The support of **.NET Standard 1.1 to 2.0** allow you to use it in :
- .Net Framework 4.6+
- Xamarin iOS et Android
- .Net Core
- UWP
- Windows Phone 8.1
- Windows 8.1

## Features
* Modern async http client for REST API.
* Support of verbs : GET, POST , PUT, DELETE, PATCH
* Support of custom http verbs
* Support of cancellation token on each requests
* Support of ETag
* Automatic XML and JSON serialization / deserialization
* Support of custom serialisation / deserialisation
* Support of camelCase, snakeCase kebabCase for json serialization
* Support of multi-part form data
* Download file
* Upload file
* Support of gzip/deflate (compression and decompression)
* Optimized http calls
* Typed exceptions which are easier to interpret
* Define timeout globally or per request
* Timeout exception thrown if the request is in timeout (by default HttpClient sends OperationCancelledException, so we can't distinguish between user cancellation and timeout)
* Provide an easy way to log : all sending of request, failed to get response, and the time get response.
* Support of export requests to postman collection
* Support of display cURL request in debug output
* Support of Basic Authentification
* Support of OAuth2 Authentification


## Basic usage

### Create the client

Define a global timeout for all client. (By default it's setted to 100 secondes)
```cs
using Tiny.RestClient;

var client = new TinyRestClient(new HttpClient(), "http://MyAPI.com/api");
```

### Headers

#### Default header for all requests

```cs
// Add default header for each calls
client.Settings.DefaultHeaders.Add("CustomHeader", "Header");
```

```cs
// Add Auth2.0 token
client.Settings.DefaultHeaders.AddBearer("token");
```

```cs
// Add default basic authentication header
client.Settings.DefaultHeaders.AddBasicAuthentication("username", "password");
```
#### Add header for current request

```cs
// Add header for this request only
client.GetRequest("City/All").
      AddHeader("CustomHeader", "Header").
      ExecuteAsync();
```

```cs
// Add header for this request only
client.GetRequest("City/All").
      WithOAuthBearer("MYTOKEN").
      ExecuteAsync();
```

```cs
// Add basic authentication for this request only
client.GetRequest("City/All").
      WithBasicAuthentication("username", "password").
      ExecuteAsync();
```

#### Read headers of response

```cs
await client.GetRequest("City/GetAll").
             FillResponseHeaders(out headersOfResponse Headers).
             ExecuteAsync();
foreach(var header in headersOfResponse)
{
    Debug.WriteLine($"{current.Key}");
    foreach (var item in current.Value)
    {
        Debug.WriteLine(item);
    }
}
```

### Basic GET http requests

```cs
var cities = client.GetRequest("City/All").ExecuteAsync<List<City>>();
// GET http://MyAPI.com/api/City/All an deserialize automaticaly the content

// Add a query parameter
var cities = client.
    GetRequest("City").
    AddQueryParameter("id", 2).
    AddQueryParameter("country", "France").
    ExecuteAsync<City>> ();

// GET http://MyAPI.com/api/City?id=2&country=France deserialize automaticaly the content

```

### Basic POST http requests
```cs
// POST
 var city = new City() { Name = "Paris" , Country = "France"};

// With content
var response = await client.PostRequest("City", city).
                ExecuteAsync<bool>();
// POST http://MyAPI.com/api/City with city as content

// With form url encoded data
var response = await client.
                PostRequest("City/Add").
                AddFormParameter("country", "France").
                AddFormParameter("name", "Paris").
                ExecuteAsync<Response>();
// POST http://MyAPI.com/api/City/Add with from url encoded content


var fileInfo = new FileInfo("myTextFile.txt");
var response = await client.
                PostRequest("City/Image/Add").
                AddFileContent(fileInfo, "text/plain").
                ExecuteAsync<Response>();
// POST text file at http://MyAPI.com/api/City/Add 
```


### Custom Http Verb requests
```cs
 await client.
       NewRequest(new System.Net.Http.HttpMethod("HEAD"), "City/All").
       ExecuteAsync<List<City>>();
```

### Define timeout

Define global timeout
```cs
client.Settings.DefaultTimeout = TimeSpan.FromSeconds(100);
```

Define the timeout for one request
```cs
request.WithTimeout(TimeSpan.FromSeconds(100));
```

### Download file
```cs
string filePath = "c:\map.pdf";
FileInfo fileInfo = await client.
                GetRequest("City/map.pdf").
                DownloadFileAsync("c:\map.pdf");
// GET http://MyAPI.com/api/City/map.pdf 
```

## Get raw HttpResponseMessage

```cs
var response = await client.
                PostRequest("City/Add").
                AddFormParameter("country", "France").
                AddFormParameter("name", "Paris").
                ExecuteAsHttpResponseMessageAsync();
// POST http://MyAPI.com/api/City/Add with from url encoded content
```

## Get raw string result

```cs
string response = await client.
                GetRequest("City/All").
                ExecuteAsStringAsync();
// GET http://MyAPI.com/api/City/All with from url encoded content
```

## Multi-part form data

```cs
// With 2 json content
var city1 = new City() { Name = "Paris" , Country = "France"};
var city2 = new City() { Name = "Ajaccio" , Country = "France"};
var response = await client.NewRequest(HttpVerb.Post, "City").
await client.PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddContent<City>(city1, "city1", "city1.json").
              AddContent<City>(city2, "city2", "city2.json").
              ExecuteAsync();


// With 2 byte array content
byte[] byteArray1 = ...
byte[] byteArray2 = ...           
              
await client.PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddByteArray(byteArray1, "request", "request2.bin").
              AddByteArray(byteArray2, "request", "request2.bin")
              ExecuteAsync();
  

// With 2 streams content        
Stream1 stream1 = ...
Stream stream2 = ...         
await client.PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddStream(stream1, "request", "request2.bin").
              AddStream(stream2, "request", "request2.bin")
              ExecuteAsync();
              
              
// With 2 files content           

var fileInfo1 = new FileInfo("myTextFile1.txt");
var fileInfo2 = new FileInfo("myTextFile2.txt");

var response = await client.
                PostRequest("City/Image/Add").
                AsMultiPartFromDataRequest().
                AddFileContent(fileInfo1, "text/plain").
                AddFileContent(fileInfo2, "text/plain").
                ExecuteAsync<Response>();

// With mixed content                  
await client.PostRequest("MultiPart/Test").
              AsMultiPartFromDataRequest().
              AddContent<City>(city1, "city1", "city1.json").
              AddByteArray(byteArray1, "request", "request2.bin").
              AddStream(stream2, "request", "request2.bin")
              ExecuteAsync();
```


## Streams and bytes array
You can use as content : streams or byte arrays.
If you use these methods no serializer will be used.

### Streams
```cs
// Read stream response
 Stream stream = await client.
              GetRequest("File").
              ExecuteAsStreamAsync();
// Post Stream as content
await client.PostRequest("File/Add").
            AddStreamContent(stream).
            ExecuteAsync();
```

### Byte array
```cs
// Read byte array response         
byte[] byteArray = await client.
              GetRequest("File").
              ExecuteAsByteArrayAsync();

// Read byte array as content
await client.
            PostRequest("File/Add").
            AddByteArrayContent(byteArray).
            ExecuteAsync();
```


## Error handling
All requests can throw 5 exceptions : 

* ConnectionException : thrown when the request can't reach the server
* HttpException : thrown when the server has invalid error code
* SerializeException : thrown when the serializer can't serialize the content
* DeserializeException : thrown when the deserializer can't deserialize the response
* TimeoutException : thrown when the request take too much time to be executed

### Catch a specific error code
```cs
string cityName = "Paris";
try
{ 
   var response = await client.
     GetRequest("City").
     AddQueryParameter("Name", cityName).
     ExecuteAsync<City>();
}
catch (HttpException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
{
   throw new CityNotFoundException(cityName);
}
catch (HttpException ex) when (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
{
   throw new ServerErrorException($"{ex.Message} {ex.ReasonPhrase}");
}
```
## ETag
The lib support the Entity tag but it's not enabled by default.

### Define an ETagContainer globally
An implementation of IETagContainer is provided. It stores all data in multiples files.

To enable it :
```cs
client.Settings.ETagContainer = new ETagFileContainer(@"C:\ETagFolder");
```

### Define an ETagContainer for one request
You can also define the ETagContainer only on specific request.
```cs
request.WithETagContainer(eTagContainer);
```

## Formatters 

By default : 
 * the Json is used as default Formatter.
 * Xml Formatter is added in Formatters

Each formatter has a list of supported media types.
It allows TinyRestClient to detect which formatter will be used.
If no formatter is found it uses the default formatter.

### Add a new formatter
Add a new custom formatter as default formatter.
```cs
bool isDefaultFormatter = true;
var customFormatter = new CustomFormatter();
client.Settings.Formatters.Add(customFormatter, isDefaultFormatter);
```

### Remove a formatter
```cs
var lastFormatter = client.Settings.Formatters.Where( f=> f is XmlSerializer>).First();
client.Remove(lastFormatter);
```

### Json custom formatting

You can enable 3 types of formatting on JsonFormatter :
- CamelCase (PropertyName => propertyName)
- SnakeCase (PropertyName => property_name)
- KebabCase (aslo known as SpinalCase) (PropertyName => property-name).

```cs
// Enable KebabCase
  client.Settings.Formatters.OfType<JsonFormatter>().First().UseKebabCase();
```


```cs
// Enable CamelCase
  client.Settings.Formatters.OfType<JsonFormatter>().First().UseCamelCase();
```

```cs
// Enable SnakeCase
  client.Settings.Formatters.OfType<JsonFormatter>().First().UseSkakeCase();
```

### Define a specific serialize for one request
```cs
IFormatter serializer = new XmlFormatter();
 var response = await client.
     PostRequest("City", city, serializer).
     ExecuteAsync();
```

### Define a specific deserializer for one request
```cs
IFormatter deserializer = new XmlFormatter();

 var response = await client.
     GetRequest("City").
     AddQueryParameter("Name", cityName).
     ExecuteAsync<City>(deserializer);
```

### Custom formatter

You create your own serializer/deserializer by implementing IFormatter

For example the implementation of XmlFormatter is really simple : 
```cs
public class XmlFormatter : IFormatter
{

   public string DefaultMediaType => "application/xml";

   public IEnumerable<string> SupportedMediaTypes
   {
      get
      {
         yield return "application/xml";
         yield return "text/xml";
      }
   }

   public T Deserialize<T>(Stream stream, Encoding encoding)
   {
      using (var reader = new StreamReader(stream, encoding))
      {
         var serializer = new XmlSerializer(typeof(T));
         return (T)serializer.Deserialize(reader);
      }
   }

   public string Serialize<T>(T data, Encoding encoding)
   {
         if (data == default)
         {
             return null;
         }

         var serializer = new XmlSerializer(data.GetType());
         using (var stringWriter = new DynamicEncodingStringWriter(encoding))
         {
            serializer.Serialize(stringWriter, data);
            return stringWriter.ToString();
         }
      }
   }
```
## Listeners 

You can easily add a listener to listen all the sent requests / responses received and all exceptions.

Two listeners are provided by the lib :
* A debug listener : which log all request in debug console
* A postman listener : which allow you to export all your request in postman collection


### Debug Listener

To add Debug listener you have to call AddDebug on Listeners property
```cs

client.Settings.Listeners.AddDebug();
```

### cURL Listener
To add cURL listener you have to call AddCurl on Listeners property

```cs
client.Settings.Listeners.AddCurl();
```

It produce this type of output in debug window for each ExecuteAsync called :
```cs
curl -X POST "http://localhost:4242/api/PostTest/complex"-H "Accept: application/json" -H "Content-Type: application/json" -d "{\"Id\":42,\"Data\":\"DATA\"}"
```

### Postman Listener
To add postman listener you have to call AddPostman on Listeners property
```cs
PostmanListerner postmanListener = client.Settings.Listeners.AddPostman("nameOfCollection");
```

When you want to save the postman collection you have to call SaveAsync
```cs
await postmanListener.SaveAsync(new FileInfo("postmanCollection.json");
```


If you only want the Json of collection you can call the method GetCollectionJson 
```cs
await listener.GetCollectionJson();
```

### Custom Listener
You can also create you own listener by implementing IListener.

```cs
IListener myCustomListerner = ..
client.Settings.Listeners.Add(myCustomListerner);
```

## Compression and Decompression
By default, the client support the decompression of Gzip and deflate.


If the server respond with the header ContentEncoding "gzip" or "deflate" the client will decompress it automaticly.

### Compression
For each request which post a content you can specified the compression algorithm like below
```cs
var response = await client.
                PostRequest("Gzip/complex", postRequest, compression: client.Settings.Compressions["gzip"]).
                ExecuteAsync<Response>();
```
Warning : the server must be able to decompress your content.

### Decompression
Even if it's supported the client didn't send Accept-Encoding header automaticaly.

You can add it for gzip all request like below :
```cs
var compression = client.Settings.Compressions["gzip"];
compression.AddAcceptEncodingHeader = true;
```

You can add it for deflate all request like below :
```cs
var compression = client.Settings.Compressions["deflate"];
compression.AddAcceptEncodingHeader = true;
```

If the server can compress response, it will respond with compressed content.

### Custom ICompression
You can add your own compression / decompression algorithm :
```cs
client.Settings.Add(new CustomCompression());
```
You class must implement the interface ICompression.
