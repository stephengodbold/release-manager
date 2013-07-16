#requires -version 2.0

param(
    [Parameter(Mandatory=$true)]
    [string]
    $build,
    
    [Parameter(Mandatory=$true)]
    [string]
    $environment,

    [string]
    $sourcePathOverride
)

$environments = @{
                Local = @{                
                    PhysicalPath = 'C:\inetpub\ReleaseManager';
                    SiteName = 'ReleaseManager';
                    IPAddress = '*';
                    Port = 80;
                    HostHeader = 'releasemanager.local';
                };
                Prod = @{
                    PhysicalPath = 'C:\inetpub\ReleaseManager';
                    SiteName = 'ReleaseManager';
                    IPAddress = '*';
                    Port = 80;
                    HostHeader = 'releasemanager.com';
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

    Get-ChildItem $sourcePath | 
        foreach {
            Copy-Item $_.FullName $targetPath -Recurse    
        }
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

    $siteName = $environment.get_Item('SiteName')
    $applicationPoolPath = Join-Path "IIS:\AppPools" $siteName

    if (Test-Path $applicationPoolPath) {
        Remove-WebAppPool -Name $siteName
    }

    $applicationPool = New-WebAppPool -Name $siteName
        
    $credential = Get-Credential
    $customIdentityType = 3
    $integratedPipelineMode = 0
    $password = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($credential.Password));

    $applicationPool.processModel.username = $credential.UserName
    $applicationPool.processModel.password = $password
    $applicationPool.processModel.identityType = $customIdentityType
    $applicationPool.enable32BitAppOnWin64 = $true
    $applicationPool.managedRuntimeVersion = 'v4.0'
    $applicationPool.managedPipelineMode = $integratedPipelineMode

    $applicationPool | Set-Item
}

function Set-WebSite {
    param(
        [Parameter(Mandatory=$true)]
        [Hashtable]
        $environment
    )

    $siteName = $environment.get_Item('SiteName')
    $physicalPath = $environment.get_Item('PhysicalPath')
    $sitePath = Join-Path "IIS:\Sites" $siteName

    if (Test-Path $sitePath) {
        return
    }

    if (-not (Test-Path $physicalPath)) {
        New-Item $physicalPath -ItemType Directory
    }
    
    New-Website -Name $siteName -ApplicationPool $siteName -PhysicalPath $physicalPath
}

function Set-Bindings {
    param(
        [Parameter(Mandatory=$true)]
        [Hashtable]
        $environment
    )

    $site = $environment.get_Item('SiteName')
    $ipAddress = $environment.get_Item('IPAddress')
    $header = $environment.get_Item('HostHeader')
    $port = $environment.get_Item('Port')

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

function Set-LocalRedirection {
    param(
        [Parameter(Mandatory=$true)]
        [Hashtable]
        $environment
    )
    $header = $environment.get_Item('HostHeader')
    $redirection = "127.0.0.1 	$header    #added by Install-ReleaseManager.ps1"
    $hostsPath = Join-Path $env:windir 'system32\drivers\etc\hosts'
    $hostsContent = Get-Content $hostsPath
    

    if ($hostsContent -contains $redirection) {
        return
    }

    $hostsContent += $redirection
    Set-Content $hostsPath $hostsContent
}

function Test-Elevation {
    $principal = New-Object `
        System.Security.Principal.WindowsPrincipal(
        [System.Security.Principal.WindowsIdentity]::GetCurrent())
    $role = [System.Security.Principal.WindowsBuiltInRole]::Administrator

    return $principal.IsInRole($role)
}


if (-not (Test-Elevation)) {
    Write-Error 'This script must be run in an elevated session'
}

$buildRoot = '\\BUILDROOT\'
$sourcePathRoot = Join-Path $buildRoot $build 
$sourcePath = Join-Path $sourcePathRoot '_PublishedWebsites\ReleaseManager'
$environmentParamaters = Get-Environment $environment 
$physicalPath = $environmentParamaters['PhysicalPath']

if (-not [String]::IsNullOrEmpty($sourcePathOverride)) {
    $sourcePath = $sourcePathOverride       
}

Set-ApplicationPool $environmentParamaters
Set-WebSite $environmentParamaters
Set-Bindings $environmentParamaters

Clear-Content $physicalPath
Copy-Content $sourcePath $physicalPath
& .\Set-Version.ps1 $build $physicalPath

if ($environment -eq 'Local') {
    Set-LocalRedirection $environmentParamaters
}