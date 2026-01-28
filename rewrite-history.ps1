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
        
        # Remove URLs from appsettings files (replace with empty string)
        if ($file -like "*appsettings*") {
            $content = $content -replace '"https://sqs\.eu-north-1\.amazonaws\.com/[^"]+"', '""'
            $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/[^"]+', ''
        }
        
        # Remove example URLs from QueueController.cs error messages
        if ($file -like "*QueueController.cs") {
            $content = $content -replace '\(e\.g\. https://sqs\.eu-north-1\.amazonaws\.com/[^\)]+\)', ''
            $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/[^\)\s]+', ''
        }
        
        # Remove example URLs from README.md
        if ($file -like "*README.md") {
            $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/[^\s`]+', 'your-queue-url-here'
        }
        
        Set-Content -Path $file -Value $content -NoNewline
    }
}
