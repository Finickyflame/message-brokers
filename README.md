# message-brokers

## Prerequisites
* [Visual studio Code or Visual Studio 2019](https://visualstudio.microsoft.com/) (version 16.9.4 or later)
* [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
* [Docker Compose](https://docs.docker.com/compose/install/)

## Build and Test
Start required services:
* `docker-compose up -d`


Produce topic-order-created:
* `dotnet run -p examples/KafkaProduceConsole RunTask=ProduceOrdersCreatedTask ProduceCount=10`

Produce topic-order-canceled:
* `dotnet run -p examples/KafkaProduceConsole RunTask=ProduceOrdersCanceledTask ProduceCount=10`

Consume with console:
* `dotnet run -p examples/KafkaConsumeConsole RunTask=ConsumeMessagesTask`

Consume with background worker:
* `dotnet run -p examples/KafkaConsumeWorker`


Stop services:
* `docker-compose stop`
