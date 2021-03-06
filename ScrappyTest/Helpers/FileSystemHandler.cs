﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScrappyTest.Helpers
{
    public class FileSystemHandler : HttpMessageHandler
    {
        public bool FailOn404 { get; set; }

        public FileSystemHandler(bool failOn404 = true)
        {
            FailOn404 = failOn404;
            Requests = new List<HttpRequestMessage>();
        }


        public List<HttpRequestMessage> Requests { get; set; }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);



            try
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(File.ReadAllText(Path.Combine("Content", request.RequestUri.LocalPath.Substring(1))))
                });
            }
            catch (FileNotFoundException)
            {
                if (FailOn404)
                {
                    Assert.Fail("Requested file was not found.");
                }
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });
        }
    }
}
