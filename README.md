### Research project: exploring microservices architecture with Kafka

Hello, I am very glad to meet you reading my small article. In the modern world of software development, microservices architecture has become one of the most sought-after and efficient models. As part of my research project, I decided to get knowledge this architecture and apply my knowledge by creating two microservices that interact both synchronously (via HTTP) and asynchronously through Apache Kafka.

#### Project objectives

The main goal of the project was to familiarize myself with microservices architecture and explore asynchronous interaction between microservices. To achieve this, I developed two services:

1. **HelloWorld API** – a simple API that returns a greeting message and stores data in a database.
2. **Telegram bot** – a bot that sends messages with a 20-second delay, simulating a complex task.

#### Architecture and technologies

The project utilized modern technologies, such as Kafka for asynchronous communication, Redis for state management, PostgreSQL as the database, and Dapper for database operations. Both microservices were containerized and configured using a single Docker Compose file, simplifying deployment and management.

##### HelloWorld API

The first microservice is a simple API with two main endpoints:

- **/hello-world/with-notify**: this endpoint returns a JSON response containing a "Hello, World!" message, the current date and time, and the notification status. Example response:

  ```json
  {
    "text": "Hello, World!",
    "dateTime": "2024-11-04T03:51:13.3239389+00:00",
    "isNotified": true
  }
  ```

- **/hello-world/replace-state**: this endpoint switches the communication method between microservices – HTTP or Kafka. To manage this, I used the Factory pattern, allowing the selection of the appropriate service (producer or HTTP client) based on current requirements. The interaction state is stored in Redis, and all responses from `/hello-world/with-notify` are logged in a PostgreSQL database using Dapper.

##### Telegram bot

The second microservice is a Telegram bot that receives message texts via a GET request (an approach chosen for simplicity, even if not optimal) on the **/notify** endpoint and sends the message to a user with a 20-second delay. This delay simulates a complex task, showcasing the advantage of asynchronous microservice communication. The bot subscribes to a Kafka topic to receive messages from the first microservice, enabling efficient asynchronous communication.

#### Containerization and deployment

Both microservices were containerized using Docker. I created a single Docker Compose file (.yaml) that describes the configuration of all required services, including Kafka, ZooKeeper (for Kafka), a Kafka UI (to view messages), Redis, and PostgreSQL. This setup allowed me to deploy the entire project architecture with just a few terminal commands.

#### Testing

To assess the performance of the HelloWorld API endpoints, I developed a simple C# program that sends sequential requests to the API. Using `HttpClient`, I implemented a performance testing method to measure response times.

Here is a sample code for performance testing:

```csharp
using System.Diagnostics;
using System.Net.Http;

var handler = new HttpClientHandler()
{
    UseProxy = false
};
var client = new HttpClient(handler);

var requestsCount = 10;

async Task TestEndpointPerformance(string url)
{
    var sw = Stopwatch.StartNew();
    var tasks = new List<Task>();

    for (int i = 0; i < requestsCount; i++)
    {
        var res = await client.GetAsync(url);
        Console.WriteLine(res.StatusCode);
    }

    sw.Stop();
    Console.WriteLine($"{url} completed in {sw.ElapsedMilliseconds} ms");
}

await TestEndpointPerformance("http://127.0.0.1:1001/hello-world/with-notify");
await client.GetAsync("http://127.0.0.1:1001/hello-world/replace-state");
await TestEndpointPerformance("http://127.0.0.1:1001/hello-world/with-notify");
```

This code sends 10 requests to the `/hello-world/with-notify` endpoint, switches the communication state, and then sends another 10 requests. The results help analyze response times and identify performance bottlenecks.

##### Results of the testing

| Interaction Method | Total response time (ms) |
|---------------------|--------------------------|
| Synchronous (HTTP)  | 202,442                  |
| Asynchronous (Kafka)| 2,192                    |

#### Conclusion

This project not only enhanced my understanding of microservices architecture but also provided hands-on experience with asynchronous communication. I became familiar with Kafka, learned to use Redis and PostgreSQL for state and data management, and mastered containerization with Docker—essential skills for modern developers.

Ultimately, my project serves as a practical example of applying contemporary technologies to solve real-world challenges. I am confident that the experience gained will be a valuable asset in my career, allowing me to contribute effectively to teams developing distributed systems.
