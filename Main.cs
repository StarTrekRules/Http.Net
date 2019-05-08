using System;
using System.IO;
using Http.Net;

class MainClass {
    public static void Main() {
        HttpServer serv = new HttpServer();

        serv.Get("/", (string data) => {
            return "Hello World!";
        });

        while (true) {
            serv.Accept();
        }
    }
}