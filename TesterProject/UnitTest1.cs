using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HttpExample;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace LocalFunctionProjectTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestQuery()
        {
            var query = new Dictionary<String, StringValues>();
            query.TryAdd("name", "Michael");
            var body = "";

            IActionResult result = await Function1.Run(HttpRequestSetup(query, body), LoggerSetup());
            
            Console.WriteLine(result.ToString());
            // Assert what result came back and then do further assertions.
            OkObjectResult casted = (Microsoft.AspNetCore.Mvc.OkObjectResult) result;
            Console.WriteLine(casted.Value.ToString());

        }

        public HttpRequest HttpRequestSetup(Dictionary<String, StringValues> query, string body)
        {
            var request = new Mock<HttpRequest>();

            request.Setup(req => req.Query).Returns(new QueryCollection(query));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            request.Setup(req => req.Body).Returns(stream);
            return request.Object;
        }

        public ILogger LoggerSetup()
        {
            using (var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole()))
            {
                return loggerFactory.CreateLogger<UnitTest1>();
            }
        }
    }
}
