using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Tiny.Http
{
    public class Test
    {
        public async Task MakeTest()
        {
            TinyHttpClient client = new TinyHttpClient(new System.Net.Http.HttpClient(), "google.fr");

            TestResult result = await client.
                NewRequest(HttpVerb.Get, "Product").
                AddHeader("token", "MYTOKEN").
                AddQueryParameter("id", 1).
                SerializeWith(new TinyJsonSerializer()).
                DeserializeWith(new TinyXmlDeserializer()).ExecuteAsync<TestResult>();

            List<string> result1 = await client.
                NewRequest(HttpVerb.Get, "Product").
                AddHeader("token", "MYTOKEN").
                AddQueryParameter("page", 1).
                AddQueryParameter("size", 10).
                ExecuteAsync<List<string>>();

            TestResult result2 = await client.NewRequest(HttpVerb.Post, "Product").
                AddHeader("token", "MYTOKEN").
                AddQueryParameter("id", 1).
                SerializeWith(new TinyJsonSerializer()).
                DeserializeWith(new TinyXmlDeserializer()).
                AddContent<TestPoco>(new TestPoco() { Toto = "A" }).
                ExecuteAsync<TestResult>();

            TestResult result3 = await client.NewRequest(HttpVerb.Post, "Authentication").
                AddHeader("token", "MYTOKEN").
                AddFormParameter("resource", "resource").
                AddFormParameter("password", "password").
                AddFormParameter("client_id", "clientId").
                AddFormParameter("username", "username").
                AddFormParameter("password", "password").
                ExecuteAsync<TestResult>();

            Stream stream = await client.NewRequest(HttpVerb.Put, "BAM").
                AddStreamContent(new MemoryStream()).
                WithStreamResponse().
                ExecuteAsync();
            byte[] byteArray = await client.NewRequest(HttpVerb.Put, "BAM").
                AddOctectStreamContent(new byte[42]).
                WithByteArrayResponse().
                ExecuteAsync();
        }
    }
}