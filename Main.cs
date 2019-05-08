using System;
using System.IO;
using Http.Net;

class MainClass {
    public static void Main() {
        HttpServer serv = new HttpServer();

        serv.Get("/", (string data) => {
            StreamReader reader = new StreamReader("./file.html");
            string file = reader.ReadToEnd();
            reader.Close();
            return file;
        });

        serv.Post("/save/", (string data) => {
            StreamReader reader = new StreamReader("./db.json");
            string content = reader.ReadToEnd();
            reader.Close();
            File.Delete("./db.json");
            StreamWriter writer = new StreamWriter("./db.json");
            content = content.Trim().Trim(']');
            content += content[content.Length - 1] != '[' ? ", " + data : data;
            content += ']';
            writer.Write(content);
            writer.Close();

            return "";
        });

        while (true) {
            serv.Accept();
        }
    }
}