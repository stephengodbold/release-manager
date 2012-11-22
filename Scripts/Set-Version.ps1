#requires -version 2.0
param(
    [Parameter(Mandatory=$true)]
    [string]
    $versionText,

    [Parameter(Mandatory=$true)]
    [string]
    $rootPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-PreviousVersion {
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $versionTextPath
    )

    if (-not(Test-Path $versionTextPath)) {
        return
    }
    $versionFileContent = Import-Csv $versionTextPath
    return $versionFileContent.CurrentVersion
}

$versionTextPath = Join-Path $rootPath 'version.csv'

if (-not(Test-Path $versionTextPath)) {
    New-Item $versionTextPath -ItemType File
}

$previousVersion = Get-PreviousVersion $versionTextPath
$version = New-Object PSObject -Property `
            @{ 
                CurrentVersion = $versionText;
                PreviousVersion = $previousVersion;
                ReleaseDate = Get-Date -Format 'F'
           }

Export-Csv -Path $versionTextPath -InputObject $version