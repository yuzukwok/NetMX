using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace NetMX.Remote.HttpAdaptor.Controllers
{
    public class UIController : ApiController
    {
        public UIController(IMBeanServerConnection serverConnection, string baseUrl)
        {
        }

        public HttpResponseMessage Get(string contentFile)
        {
            const string basePath = @"C:\Users\Simon\Documents\Visual Studio 2010\Projects\NetMX\NetMX.Remote.HttpAdaptor\Content";
            var path = Path.Combine(basePath, contentFile);

            var response = new HttpResponseMessage
                                          {
                                              StatusCode = HttpStatusCode.OK,
                                              Content = new StreamContent(new FileStream(path,FileMode.Open)),
                                          };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(contentFile));
            return response;
        }

        private static string GetContentType(string contentFile)
        {
            var extension = Path.GetExtension(contentFile) ?? "";
            switch (extension.ToLowerInvariant())
            {
                case ".html":
                case ".htm":
                    return "text/html";
                case ".js":
                    return "text/javascript";
                case ".css":
                    return "text/css";
                default:
                    return "text/plain";
            }
        }
    }
}