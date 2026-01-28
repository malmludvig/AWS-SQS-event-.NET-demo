# Script to remove AWS URLs from git history
$files = @("AWS-SQS-Test/appsettings.json", "AWS-SQS-Test/appsettings.Development.json")

foreach ($file in $files) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw
        # Replace URLs with empty string, handling both quoted and unquoted cases
        $content = $content -replace '"https://sqs\.eu-north-1\.amazonaws\.com/[^"]+"', '""'
        $content = $content -replace 'https://sqs\.eu-north-1\.amazonaws\.com/[^"]+', ''
        Set-Content -Path $file -Value $content -NoNewline
    }
}
