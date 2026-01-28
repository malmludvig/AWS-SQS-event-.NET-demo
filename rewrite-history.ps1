# Script to remove AWS URLs from git history
$files = @(
    "AWS-SQS-Test/appsettings.json",
    "AWS-SQS-Test/appsettings.Development.json",
    "AWS-SQS-Test/Controllers/QueueController.cs",
    "README.md"
)

foreach ($file in $files) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw
        # Replace URLs with empty string or generic text
        $content = $content -replace '"https://sqs\.eu-north-1\.amazonaws\.com/[^"]+"', '""'
        $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/[^\)\s"]+', ''
        $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/YOUR_ACCOUNT_ID/[^\s`]+', 'your-queue-url-here'
        $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/ACCOUNT_ID/[^\s\)]+', 'your-queue-url-here'
        $content = $content -replace '\(e\.g\. https://sqs\.eu-north-1\.amazonaws\.com/[^\)]+\)', ''
        Set-Content -Path $file -Value $content -NoNewline
    }
}
