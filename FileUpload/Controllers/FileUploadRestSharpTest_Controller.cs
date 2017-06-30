using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FileUpload.Controllers
{
    public class FileUploadRestSharpTest_Controller : ApiController
    {
        [HttpGet]

        [Route("api/FileUploadRestSharpTest")]
        public void Get()
        {
            var client = new RestClient("http://localhost:54320/api/FileUploadRestSharpTest");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "multipart/form-data; boundary=---011000010111000001101001");
            request.AddParameter("multipart/form-data; boundary=---011000010111000001101001", "-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"g\"; filename=\"[object Object]\"\r\nContent-Type: false\r\n\r\n\r\n-----011000010111000001101001--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        [HttpGet]
        [Route("api/FileUploadRestSharpTest")]
        public void Post()
        {
            var client = new RestClient("http://localhost:54320/api/File/hello");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "multipart/form-data; boundary=---011000010111000001101001");
            request.AddParameter("multipart/form-data; boundary=---011000010111000001101001", "-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"g\"; filename=\"[object Object]\"\r\nContent-Type: false\r\n\r\n\r\n-----011000010111000001101001--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}
