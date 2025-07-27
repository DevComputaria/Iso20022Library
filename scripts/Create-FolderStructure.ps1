# Script to create the ISO 20022 message folder structure

# Get the script directory and build relative path to Messages project
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$baseDir = Join-Path -Path (Split-Path -Parent $scriptDir) -ChildPath "Iso20022Library.Messages"

# Create base directory if it doesn't exist
if (!(Test-Path $baseDir)) {
    New-Item -ItemType Directory -Path $baseDir -Force
}

# Define the folder structure
$folderStructure = @{
    "Payments"          = @("Pacs", "Pain")
    "CashManagement"    = @("Camt")
    "Securities"        = @("Seev", "Semt", "Sese", "Secl")
    "TradeServices"     = @("Tsmt")
    "AccountManagement" = @("Acmt")
    "ReferenceData"     = @("Reda")
    "ForeignExchange"   = @("Fxtr")
    "Authorities"       = @("Auth")
    "Collateral"        = @("Colr")
    "Cards"             = @("Caaa", "Caad", "Casr")
    "Treasury"          = @("Trea")
}

# Create the folders
foreach ($domain in $folderStructure.Keys) {
    $domainPath = Join-Path $baseDir $domain
    if (!(Test-Path $domainPath)) {
        New-Item -ItemType Directory -Path $domainPath -Force
        Write-Host "Created domain directory: $domainPath"
    }
    
    foreach ($messageType in $folderStructure[$domain]) {
        $messagePath = Join-Path $domainPath $messageType
        if (!(Test-Path $messagePath)) {
            New-Item -ItemType Directory -Path $messagePath -Force
            Write-Host "Created message type directory: $messagePath"
        }

        # Create 'Generated' folder
        $generatedPath = Join-Path $messagePath "Generated"
        if (!(Test-Path $generatedPath)) {
            New-Item -ItemType Directory -Path $generatedPath -Force
            Write-Host "Created 'Generated' folder: $generatedPath"
        }

        # Create 'Xsd' folder
        $xsdPath = Join-Path $messagePath "Xsd"
        if (!(Test-Path $xsdPath)) {
            New-Item -ItemType Directory -Path $xsdPath -Force
            Write-Host "Created 'Xsd' folder: $xsdPath"
        }
    }
}

Write-Host "Folder structure creation completed successfully."
