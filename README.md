# TinyHttp
TinyHttp make easier the dialog between you API and your application.
It hide all the complexity of communication, deserialisation ...

Features : 
* Modern async http client for REST API.
* Support of verbs : GET, POST , PUT, HEAD, DELETE, HEAD, PATCH 
* Automatic XML and JSON deserialization
* Support of custom serialisation / deserialisation
* Support of multi-part form data
* Optimized http calls

## Basic usage

### Create the client
```cs
using Tiny.Http;

var client = new TinyHttpClient("http://MyAPI.com/api");
```

### Define default headers
```cs
// Add default header for each calls
client.DefaultHeaders.Add("Token", "MYTOKEN");
```

### Basic GET http requests

```cs
var cities = client.NewRequest(HttpVerb.Get, "City/All").ExecuteAsync<List<City>>();
// GET http://MyAPI.com/api/City/All an deserialize automaticaly the content

// Add a query parameter
var cities = client.
    NewRequest(HttpVerb.Get, "City").
    AddQueryParameter("id", 2).
    AddQueryParameter("country", "France").
    ExecuteAsync<City>> ();

// GET http://MyAPI.com/api/City?id=2&country=France an deserialize automaticaly the content

```

### Basic POST http requests
```cs
// POST
 var city = new City() { Name = "Paris" , Country = "France"};

// With content
var response = await client.NewRequest(HttpVerb.Post, "City").
                AddContent(city).
                ExecuteAsync<bool>();
// POST http://MyAPI.com/api/City with city as content

// With form url encoded data
var response = await client.
                NewRequest(HttpVerb.Post, "City/Add").
                AddFormParameter("country", "France").
                AddFormParameter("name", "Paris").
                ExecuteAsync<Response>();
// POST http://MyAPI.com/api/City/Add with from url encoded content
```

#### Streams and bytes array
You can use as content streams and byte arrays.
If you use these methods no serializer will be used.

#### Streams
```cs

// Read stream response
 var stream = await client.
              NewRequest(HttpVerb.Get, "File").
              WithStreamResponse().
              ExecuteAsync();
// Post Stream as content
await client.
            NewRequest(HttpVerb.Post, "File/Add").
            ExecuteAsync();
```

#### Bytes array
```cs
// Read byte array response         
byte[] byteArray = await client.
              NewRequest(HttpVerb.Get, "Eile").
              WithByteArrayResponse().
              ExecuteAsync();

// Post  byte array as content
await client.
            NewRequest(HttpVerb.Post, "File/Add").
            WithByteArrayResponse().
            ExecuteAsync();
```


## Error handling
All requests can throw 3 exceptions : 

* ConnectionException : thrown when the request can't reach the server
* HttpException : thrown when the server has invalid error code
* DeserializeException : thrown when the deserializer can't deserialize the response

### Catch a specific error code
```cs
using Tiny.Http;
string cityName = "Paris";
try
{ 
   var response = await client.
     NewRequest(HttpVerb.Get, "City").
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


## Nuget
* Available on NuGet: [Tiny.Http](http://www.nuget.org/packages/Tiny.Http) [![NuGet](https://img.shields.io/nuget/v/Tiny.Http.svg?label=NuGet)](https://www.nuget.org/packages/Tiny.Http/)

## Platform Support
|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|.Net Standard|Yes|2.0|


## License
MIT © JGI
