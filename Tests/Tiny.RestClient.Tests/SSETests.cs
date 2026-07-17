namespace Tiny.RestClient.Tests
{
    [TestClass]
    public class SSETests : BaseTest
    {
        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_Simple_YieldsAllEvents()
        {
            var client = GetNewClient();

            var events = new List<ServerSentEvent>();
            await foreach (var sse in client.GetRequest("SSETest/Simple").ExecuteAsSSEAsync())
            {
                events.Add(sse);
            }

            Assert.AreEqual(3, events.Count);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual($"message {i}", events[i].Data);

                // No 'event' field provided : must default to "message".
                Assert.AreEqual("message", events[i].Event);
                Assert.IsNull(events[i].Id);
                Assert.IsNull(events[i].Retry);
            }
        }

        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_AllFields_AreParsed()
        {
            var client = GetNewClient();

            var events = new List<ServerSentEvent>();
            await foreach (var sse in client.GetRequest("SSETest/AllFields").ExecuteAsSSEAsync())
            {
                events.Add(sse);
            }

            Assert.AreEqual(2, events.Count);

            Assert.AreEqual("1", events[0].Id);
            Assert.AreEqual("greeting", events[0].Event);
            Assert.AreEqual("hello", events[0].Data);
            Assert.AreEqual(5000, events[0].Retry);

            Assert.AreEqual("2", events[1].Id);
            Assert.AreEqual("farewell", events[1].Event);
            Assert.AreEqual("bye", events[1].Data);
            Assert.IsNull(events[1].Retry);
        }

        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_MultilineData_IsJoinedWithLineFeed()
        {
            var client = GetNewClient();

            var events = new List<ServerSentEvent>();
            await foreach (var sse in client.GetRequest("SSETest/MultilineData").ExecuteAsSSEAsync())
            {
                events.Add(sse);
            }

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("line1\nline2\nline3", events[0].Data);
        }

        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_CommentsAndUnknownFields_AreIgnored()
        {
            var client = GetNewClient();

            var events = new List<ServerSentEvent>();
            await foreach (var sse in client.GetRequest("SSETest/WithCommentsAndUnknownFields").ExecuteAsSSEAsync())
            {
                events.Add(sse);
            }

            // The comment-only block must not produce an event.
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual("real data", events[0].Data);
            Assert.AreEqual("second", events[1].Data);
        }

        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_NoLeadingSpace_KeepsFullValue()
        {
            var client = GetNewClient();

            var events = new List<ServerSentEvent>();
            await foreach (var sse in client.GetRequest("SSETest/NoLeadingSpace").ExecuteAsSSEAsync())
            {
                events.Add(sse);
            }

            Assert.AreEqual(1, events.Count);

            // Only a single leading space is stripped ; here there is none.
            Assert.AreEqual("nospace", events[0].Data);
        }

        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_Cancellation_StopsTheStream()
        {
            var client = GetNewClient();
            using var cts = new CancellationTokenSource();

            var events = new List<ServerSentEvent>();
            try
            {
                await foreach (var sse in client.GetRequest("SSETest/Infinite").ExecuteAsSSEAsync(cts.Token))
                {
                    events.Add(sse);

                    // Stop after having received a few events.
                    if (events.Count >= 3)
                    {
                        cts.Cancel();
                    }
                }

                Assert.Fail("The stream should have been cancelled.");
            }
            catch (OperationCanceledException)
            {
                // Expected : the cancellation aborts the stream.
            }

            Assert.IsTrue(events.Count >= 3, $"Expected at least 3 events before cancellation, got {events.Count}.");
        }

        [TestMethod]
        [Timeout(30000)]
        public async Task SSE_FillResponseHeaders_AreAvailable()
        {
            var client = GetNewClient();

            var events = new List<ServerSentEvent>();
            Headers headers;
            await foreach (var sse in client.
                GetRequest("SSETest/Simple").
                FillResponseHeaders(out headers).
                ExecuteAsSSEAsync())
            {
                events.Add(sse);
            }

            Assert.AreEqual(3, events.Count);
            Assert.IsNotNull(headers);
        }
    }
}
