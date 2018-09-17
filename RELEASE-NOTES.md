# Release notes
## 1.4.1
* Rename PostMan to Postman

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
