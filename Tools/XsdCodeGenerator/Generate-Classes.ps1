# Script para gerar classes C# a partir de arquivos XSD usando xsd.exe
# Uso: .\Generate-Classes.ps1 -XsdFile "pacs.002.001.11.xsd" -MessageType "Pacs" -Version "00200111"

param(
    [Parameter(Mandatory=$true)]
    [string]$XsdFile,
    
    [Parameter(Mandatory=$true)]
    [string]$MessageType,
    
    [Parameter(Mandatory=$true)]
    [string]$Version,
    
    [Parameter(Mandatory=$false)]
    [string]$BasePath = (Get-Location)
)

# Definir caminhos
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent (Split-Path -Parent $scriptDir)
$xsdPath = Join-Path $BasePath $XsdFile
$messagesPath = Join-Path $projectRoot "Iso20022Library.Messages"
$outputBaseDir = Join-Path $messagesPath "Payments\$MessageType\Generated\$Version"
$toolPath = Join-Path $projectRoot "Tools\XsdCodeGenerator"

Write-Host "=== Gerador de Classes ISO 20022 ===" -ForegroundColor Green
Write-Host "XSD File: $xsdPath" -ForegroundColor Yellow
Write-Host "Message Type: $MessageType" -ForegroundColor Yellow
Write-Host "Version: $Version" -ForegroundColor Yellow
Write-Host "Output Directory: $outputBaseDir" -ForegroundColor Yellow

# Verificar se o arquivo XSD existe
if (-not (Test-Path $xsdPath)) {
    Write-Error "Arquivo XSD não encontrado: $xsdPath"
    exit 1
}

# Criar diretório de saída
if (-not (Test-Path $outputBaseDir)) {
    New-Item -ItemType Directory -Path $outputBaseDir -Force | Out-Null
    Write-Host "Criado diretório: $outputBaseDir" -ForegroundColor Green
}

# Definir namespace
$namespace = "Iso20022Library.Messages.Payments.$MessageType.Generated.$Version"

# Compilar a ferramenta se necessário
Write-Host "Compilando ferramenta XsdCodeGenerator..." -ForegroundColor Blue
Push-Location $toolPath
try {
    $buildResult = dotnet build --configuration Release --verbosity quiet
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Falha ao compilar XsdCodeGenerator"
        exit 1
    }
    Write-Host "Ferramenta compilada com sucesso!" -ForegroundColor Green
}
finally {
    Pop-Location
}

# Executar gerador
Write-Host "Gerando classes C#..." -ForegroundColor Blue
$toolExe = Join-Path $toolPath "bin\Release\net8.0\XsdCodeGenerator.exe"

if (-not (Test-Path $toolExe)) {
    # Se não for Windows ou não tiver .exe, usar dotnet run
    Push-Location $toolPath
    try {
        dotnet run --configuration Release -- "$xsdPath" "$namespace" "$outputBaseDir"
        $exitCode = $LASTEXITCODE
    }
    finally {
        Pop-Location
    }
} else {
    & $toolExe "$xsdPath" "$namespace" "$outputBaseDir"
    $exitCode = $LASTEXITCODE
}

if ($exitCode -ne 0) {
    Write-Error "Falha ao gerar classes"
    exit 1
}

Write-Host "Classes geradas com sucesso!" -ForegroundColor Green

# Listar arquivos gerados
Write-Host "`nArquivos gerados:" -ForegroundColor Blue
Get-ChildItem $outputBaseDir -Filter "*.cs" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor White
    Write-Host "    Tamanho: $([math]::Round($_.Length / 1KB, 2)) KB" -ForegroundColor Gray
}

# Verificar se precisa compilar o projeto Messages
Write-Host "`nCompilando projeto Messages..." -ForegroundColor Blue
Push-Location $messagesPath
try {
    $buildResult = dotnet build --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Projeto Messages compilado com sucesso!" -ForegroundColor Green
    } else {
        Write-Warning "Houve problemas na compilação do projeto Messages. Verifique os arquivos gerados."
    }
}
finally {
    Pop-Location
}

Write-Host "`n=== Processo Concluído ===" -ForegroundColor Green
Write-Host "Namespace: $namespace" -ForegroundColor White
Write-Host "Pasta: $outputBaseDir" -ForegroundColor White
