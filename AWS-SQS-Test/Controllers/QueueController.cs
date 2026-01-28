using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;

namespace AWS_SQS_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueueController : ControllerBase
{
    private readonly IAmazonSQS _sqsClient;
    private readonly IConfiguration _config;

    public QueueController(IAmazonSQS sqsClient, IConfiguration config)
    {
        _sqsClient = sqsClient;
        _config = config;
    }

    private string? QueueUrl => _config["AWS_SQS_QUEUE_URL"]?.Trim();

    private bool IsInvalidQueueUrl(string? url) =>
        string.IsNullOrWhiteSpace(url) ||
        !url.Contains(".amazonaws.com/", StringComparison.OrdinalIgnoreCase) ||
        url.TrimEnd('/').EndsWith(".amazonaws.com", StringComparison.OrdinalIgnoreCase);

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] QueueMessageRequest request)
    {
        if (IsInvalidQueueUrl(QueueUrl))
            return StatusCode(500, "Set AWS_SQS_QUEUE_URL in .env to the full URL (e.g. ). If you use .env, run the app from the project folder or ensure .env is in the same folder as the .csproj.");
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty.");

        try
        {
            var result = await _sqsClient.SendMessageAsync(QueueUrl, request.Message);
            return Ok(new { MessageId = result.MessageId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("receive")]
    public async Task<IActionResult> Receive()
    {
        if (IsInvalidQueueUrl(QueueUrl))
            return StatusCode(500, "Set AWS_SQS_QUEUE_URL in .env to the full URL (e.g. ).");
        try
        {
            var result = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                MaxNumberOfMessages = 1
            });

            var msg = result.Messages.FirstOrDefault();
            return Ok(new
            {
                MessageId = msg?.MessageId,
                Body = msg?.Body,
                ReceiptHandle = msg?.ReceiptHandle
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("purge")]
    public async Task<IActionResult> Purge()
    {
        if (IsInvalidQueueUrl(QueueUrl))
            return StatusCode(500, "Set AWS_SQS_QUEUE_URL in .env to the full URL.");
        try
        {
            await _sqsClient.PurgeQueueAsync(QueueUrl);
            return Ok(new { Message = "Queue emptied." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

public record QueueMessageRequest(string Message);
