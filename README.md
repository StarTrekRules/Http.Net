# Http.Net
Easy to use C# HTTP library

# Usage
```cs
HttpServer serv = new HttpServer();

// Do this when a GET request to / is recieved
serv.Get("/", (string data) => {
  return "What do you want?";
});

// Do this when a POST request to /post is recieved
serv.Post("/post", (string data) => {
    return "oof";
});

// Enter accept loop
while (true) {
    serv.Accept();
}
```
