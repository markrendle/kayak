﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kayak;
using Kayak.Framework;
using System.IO;

namespace KayakExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new KayakServer();
            var framework = server.UseFramework();

            Console.WriteLine("Kayak listening on " + server.ListenEndPoint);
            Console.ReadLine();

            // unsubscribe from server (close the listening socket)
            framework.Dispose();

            // any outstanding requests that may still be processing will be aborted.
            // currently no way to wait for pending requests. keep your own request
            // count and wait for it to drop to zero if you want.
        }
    }


    public class MyService : KayakService
    {
        [Path("/")]
        public object SayHello(string name)
        {
            return new { greeting = "hello, " + name + "." };
        }

        static object foo;

        [Verb("POST")]
        [Verb("PUT")]
        [Path("/foo")]
        public void PutFoo([RequestBody] object t)
        {
            foo = t;
        }

        [Path("/foo")]
        public object GetFoo()
        {
            return foo;
        }

        [Path("/error")]
        public void Error()
        {
            throw new Exception("Uh oh.");
        }

        [Path("/path/{p}")]
        public IEnumerable<object> PathParam(string p)
        {
            yield return Response.Body.WriteAsync(Encoding.UTF8.GetBytes("The value of p is '" + p + "'."));
        }

        [Path("/files/{name}")]
        public FileInfo GetFile(string name)
        {
            return new FileInfo(name);
        }

    }
}
