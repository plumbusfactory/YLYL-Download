# Path to NSIS executable (Update this path if NSIS is installed elsewhere)
$nsisPath = "C:\Program Files (x86)\NSIS\makensis.exe"

# Path to the NSI script
$nsiScriptPath = ".\installer.nsi"

# Output directory for the compiled installer
$outputDirectory = ".\bin\Installer"

# Ensure the output directory exists
if (-not (Test-Path -Path $outputDirectory)) {
    New-Item -ItemType Directory -Path $outputDirectory -Force
}

# Check if the NSI script exists
if (-not (Test-Path -Path $nsiScriptPath)) {
    Write-Host "Error: NSI script not found at path: $nsiScriptPath" -ForegroundColor Red
    exit 1
}

# Compile the NSI script
Write-Host "Compiling NSI script: $nsiScriptPath" -ForegroundColor Cyan
& $nsisPath $nsiScriptPath

# Check the result of the compilation
if ($LASTEXITCODE -eq 0) {
    Write-Host "Compilation successful. Installer created in: $outputDirectory" -ForegroundColor Green
} else {
    Write-Host "Error: Compilation failed. Check the NSIS log for details." -ForegroundColor Red
}
