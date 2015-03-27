ServiceStack.Text is an **independent, dependency-free** serialization library that contains ServiceStack's text processing functionality, including:

* [JsonSerializer](http://www.servicestack.net/mythz_blog/?p=344)
* [TypeSerializer (JSV-Format)](https://github.com/ServiceStack/ServiceStack.Text/wiki/JSV-Format)
* CsvSerializer
* [T.Dump extension method](http://www.servicestack.net/mythz_blog/?p=202)
* StringExtensions - Xml/Json/Csv/Url encoding, BaseConvert, Rot13, Hex escape, etc.
* Stream, Reflection, List, DateTime, etc extensions and utils

# Simple API

Like most of the interfaces in Service Stack, the API is simple. Methods that you would commonly use include:

    string TypeSerializer.SerializeToString<T>(T value)
    void TypeSerializer.SerializeToWriter<T>(T value, TextWriter writer)

    T TypeSerializer.DeserializeFromString<T>(string value)
    T TypeSerializer.DeserializeFromReader<T>(TextReader reader)

Where *T* can be any .NET POCO type. That's all there is - the API was intentionally left simple :)
    
### Dynamic JSON parsing API

    JsonObject.Parse()
    JsonArrayObjects.Parse()

### Extension Methods

    T FromJson()
    string ToJson(T)

    T FromJsv()
    string ToJsv(T)

Dump / Diagnostic Extensions:

    T Dump()
    T Print()
    T PrintDump()
    string Fmt(args)

URL Extensions:

    string GetStringFromUrl()
    string GetJsonFromUrl()
    string GetResponseStatus()
    string UrlEncode() / UrlDecode()
    string HexEscape() / HexUnescape()
    string UrlFormat() / AppendPath() / AppendPaths() / WithTrailingSlash()    
    string WithoutExtension() / ParentDirectory() / ReadAllText()

Stream Extensions:

    Stream WriteTo(Stream) / CopyTo()
    StreamReader ReadLines()
    Stream ReadFully() / ReadExactly()

String Utils:

    string SplitOnFirst() / SplitOnLast()
    string IndexOfAny()
    string StripHtml() / ToCamelCase()
    string SafeSubstring()
    string ToUtf8Bytes() / FromUtf8Bytes()

more String, Reflection, List, Dictionary, DateTime extensions, and More...