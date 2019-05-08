using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Http.Net {

    public delegate string RequestHandler(string input);

    public class HttpServer {

        public string Host = "localhost";
        public int Port = 8080;
        private HttpListener lis = new HttpListener();
        public Dictionary<string, RequestHandler> Gets = new Dictionary<string, RequestHandler>();
        public Dictionary<string, RequestHandler> Posts = new Dictionary<string, RequestHandler>();

        public void Get(string path, RequestHandler callback) {
            lis.Prefixes.Add($"http://{Host}:{Port}{path}");
            Gets.Add(path, callback);
        }

        public void Post(string path, RequestHandler callback) {
            lis.Prefixes.Add($"http://{Host}:{Port}{path}");
            Posts.Add(path, callback);
        }

        public void Accept() {
            HttpListenerContext context = lis.GetContext();

            HttpListenerRequest req = context.Request;
            HttpListenerResponse res = context.Response;
            string path = req.Url.AbsolutePath != "/" ? req.Url.AbsolutePath + '/' : "/";

            if (! Gets.ContainsKey(path) && ! Posts.ContainsKey(path)) {
                res.StatusCode = 404;

                StreamWriter writer2 = new StreamWriter(res.OutputStream);
                writer2.Write("<b>404 Not Found</b>");
                writer2.Close();
                return;
            }

            StreamReader reader = new StreamReader(req.InputStream);

            string RespStr = "";
            res.ContentType = "text/html";
            
            if (req.HttpMethod.ToLower() == "get")
                RespStr = Gets[path](reader.ReadToEnd());

            if (req.HttpMethod.ToLower() == "post")
                RespStr = Posts[path](reader.ReadToEnd());
            
            res.ContentLength64 = RespStr.Length;

            StreamWriter writer = new StreamWriter(res.OutputStream);

            writer.Write(RespStr);

            writer.Close();
            reader.Close();
        }

        public HttpServer() {
            lis.Start();
        }
    }
}