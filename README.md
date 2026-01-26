# AWS SQS Test

A small ASP.NET Core API to **send**, **receive**, and **purge** messages on an AWS SQS queue. Useful for local testing and learning SQS.

## Features

- **Send** a message to the queue (POST)
- **Receive** one message from the queue (GET)
- **Purge** the queue — delete all messages (DELETE)
- Swagger UI for trying the API in the browser
- Configuration via `.env` (credentials and queue URL)

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- An AWS account with an SQS queue and IAM user that has `AmazonSQSFullAccess`

## Setup

1. **Clone the repo**
   ```bash
   git clone <your-repo-url>
   cd AWS-SQS-Test
   ```

2. **Create a `.env` file** in the `AWS-SQS-Test` project folder (same folder as `AWS-SQS-Test.csproj`). Copy from `.env.example` and fill in your values:
   ```env
   AWS_ACCESS_KEY_ID=your-access-key-id
   AWS_SECRET_ACCESS_KEY=your-secret-access-key
   AWS_REGION=eu-north-1
   AWS_SQS_QUEUE_URL=
   ```
   Get the queue URL from AWS Console → SQS → your queue. Do not commit `.env` (it is in `.gitignore`).

3. **Run the app**
   ```bash
   cd AWS-SQS-Test
   dotnet run
   ```

4. Open **Swagger** at `https://localhost:7xxxx/swagger` (check the console for the URL).

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST   | `/api/queue/send`   | Send a message. Body: `{ "message": "your text" }` |
| GET    | `/api/queue/receive`| Receive one message (returns `messageId`, `body`, `receiptHandle`; all null if queue is empty) |
| DELETE | `/api/queue/purge`  | Delete all messages in the queue (subject to AWS’s once-per-60-seconds limit per queue) |

## Project structure

```
AWS-SQS-Test/
├── AWS-SQS-Test/           # Web API project
│   ├── Controllers/
│   │   └── QueueController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   ├── .env.example
│   └── .env               # Your secrets (create from .env.example, do not commit)
├── .gitignore
└── README.md
```

## License

MIT
