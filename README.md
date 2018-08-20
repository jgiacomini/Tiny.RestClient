# TinyHttp
TinyHttp make easier the dialog between you API and your application.
It hide all the complexity of communication, deserialisation ...


## Basic usage 
```cs
using Tiny.Http;

var client = new TinyHttpClient("http://MyAPI.com/api");

// Add default header for each calls
client.DefaultHeaders.Add("Token", "MYTOKEN");

// GET
var cities = client.NewRequest(HttpVerb.Get, "City/All").ExecuteAsync<List<City>>();
// Will call http://MyAPI.com/api/City/All an deserialize automaticaly the content

// Add a query parameter
var cities = client.
    NewRequest(HttpVerb.Get, "City").
    AddQueryParameter("id", 2).
    ExecuteAsync<City>> ();

// Will call GET http://MyAPI.com/api/City?id=2 an deserialize automaticaly the content

// POST
 var city = new City() { Name = "Paris"};
 var response = await client.NewRequest(HttpVerb.Post, "City").
                AddContent(city).
                ExecuteAsync<bool>();
// Will call POST http://MyAPI.com/api/City with city as content

```


## Error handling
All request can throw 3 exceptions : 

* ConnectionException : thrown when the request can't reach the server
* HttpException : thrown when the request can't reach the server
TODO finish this part



## Nuget
* Available on NuGet: [Tiny.Http](http://www.nuget.org/packages/Tiny.Http) [![NuGet](https://img.shields.io/nuget/v/Tiny.Http.svg?label=NuGet)](https://www.nuget.org/packages/Tiny.Http/)

## Platform Support
|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|.Net Standard|Yes|2.0|


### Created By: [@JeromeGiacomini](https://twitter.com/jeromegiacomini)
* Twitter: [@JeromeGiacomini](http://twitter.com/jeromegiacomini)
* Blog: [Blog perso](http://jeromegiacomini.net/Blog/), [Blog pro](http://blogs.infinitesquare.com/users/jgiacomini)
* Book: [Xamarin French only](https://www.editions-eni.fr/supports-de-cours/livre/xamarin-developpez-vos-applications-multiplateformes-pour-ios-android-et-windows-9782409007477)

## License
MIT © JGI
