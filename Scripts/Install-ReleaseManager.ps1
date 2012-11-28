#requires -version 2.0

param(
    [Parameter(Mandatory=$true)]
    [string]
    $buildName,
    
    [Parameter(Mandatory=$true)]
    [string]
    $environment
)

$environments = @{
                Local = @{
                    PhysicalPath = '';
                    SiteName = 'ReleaseManager';
                    IPAddress = '*';
                    Port = 80;
                    HostHeader = 'rm.studygroup.com';
                };
                Prod = @{
                    PhysicalPath = '';
                    SiteName = 'ReleaseManager';
                    IPAddress = '*';
                    Port = 80;
                    HostHeader = 'rm.studygroup.com';
                };
            }

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Import-Module WebAdministration

function Copy-Content {
    param (
        [Parameter(Mandatory=$true)]
        [string]
        $sourcePath,

        [Parameter(Mandatory=$true)]
        [string]
        $targetPath
    )

    if (-not(Test-Path $targetPath)) {
        New-Item $targetPath -ItemType Directory
    }

    Copy-Item $sourcePath $targetPath -Recurse
}

function Clear-Content {
    param (
        [Parameter(Mandatory=$true)]
        [string]
        $targetPath
    )

    if (-not(Test-Path $targetPath)) {
        New-Item $targetPath -ItemType Directory
    }

    Remove-Item $targetPath -Recurse -Force
}

function Get-Environment {
    
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $name
    )

    return $environments[$name]
}

function Set-ApplicationPool {
    param(
        [Parameter(Mandatory=$true)]
        [Hashtable]
        $environment
    )

    $siteName = $environment.get_Value('SiteName')
    $applicationPoolPath = Join-Path "IIS:\ApplicationPools" $siteName

    if (Test-Path $applicationPoolPath) {
        Remove-WebAppPool -Name $siteName
    }

    $applicationPool = New-WebAppPool -Name $siteName

    $applicationPool | Set-ItemProperty -Name 'managedRuntimeVersion' -Value 'v4.0'
    $applicationPool | Set-ItemProperty -Name 'managedPipelineMode' -Value 'Integrated'

    $credential = Get-Credential
    $customIdentityType = 3

    $applicationPool.processModel.username = $credential.UserName
    $applicationPool.processModel.password = $credential.Password
    $applicationPool.processMOdel.identityType = $customIdentityType

    $applicationPool | Set-Item
}

function Set-WebSite {
    param(
        [Parameter(Mandatory=$true)]
        [Hashtable]
        $environment
    )

    $siteName = $environment.get_Value('SiteName')
    $physicalPath = $environment.get_Value('PhysicalPath')
    $sitePath = Join-Path "IIS:\Sites" $siteName

    if (Test-Path $sitePath) {
        return
    }
    
    New-Website -Name $siteName -ApplicationPool $siteName -PhysicalPath $physicalPath
}

function Set-Bindings {
    param(
        [Parameter(Mandatory=$true)]
        [Hashtable]
        $environment
    )

    $site = $environment.get_Value('SiteName')
    $ipAddress = $environment.get_Value('IPAddress')
    $header = $ipAddress = $environment.get_Value('HostHeader')
    $port = $environment.get_Value('Port')

    Get-WebBinding -Name $site |
        foreach { 
            Remove-WebBinding -InputObject $_
        }

    New-WebBinding -Name $site `
                -IPAddress $ipAddress `
                -HostHeader $header `
                -Port $port `
                -Protocol 'http'
}

$buildRoot = '\\AUSYDITDSVR0013\Builds2\'
$sourcePath = Join-Path $buildRoot $buildName
$environment = Get-Environment $environment 

Set-ApplicationPool $environment
Set-WebSite $environment
Set-Bindings $environment

Clear-Content $environment['PhysicalPath']
Copy-Content $sourcePath $physicalPath
& .\Set-Version.ps1 $buildName $physicalPath