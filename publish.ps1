param(
    [Parameter(Mandatory)]
    [ValidateSet('Debug','Release')]
    [System.String]$Target,
    
    [Parameter(Mandatory)]
    [System.String]$TargetPath,
    
    [Parameter(Mandatory)]
    [System.String]$TargetAssembly,

    [Parameter(Mandatory)]
    [System.String]$ValheimPath,

    [Parameter(Mandatory)]
    [System.String]$ProjectPath,
    
    [System.String]$DeployPath
)

# Make sure Get-Location is the script path
Push-Location -Path (Split-Path -Parent $MyInvocation.MyCommand.Path)

# Test some preliminaries
("$TargetPath",
 "$ValheimPath",
 "$(Get-Location)\libraries"
) | % {
  if (!(Test-Path "$_")) {Write-Error -ErrorAction Stop -Message "$_ folder is missing"}
}

$version = Get-Content ".\VERSION"

# Plugin name without ".dll"
$name = "$TargetAssembly" -Replace('.dll')

# Create the mdb file
$pdb = "$TargetPath\$name.pdb"
if (Test-Path -Path "$pdb") {
    Write-Host "Create mdb file for plugin $name"
    Invoke-Expression "& `"$(Get-Location)\libraries\Debug\pdb2mdb.exe`" `"$TargetPath\$TargetAssembly`""
}

# Main Script
Write-Host "Publishing for $Target from $TargetPath"

if ($Target.Equals("Debug")) {
    if ($DeployPath.Equals("")){
      $DeployPath = "$ValheimPath\BepInEx\plugins"
    }
    
    $plug = New-Item -Type Directory -Path "$DeployPath\$name" -Force
    Write-Host "Copy $TargetAssembly to $plug"
    Copy-Item -Path "$TargetPath\$name.dll" -Destination "$plug" -Force
    Copy-Item -Path "$TargetPath\$name.pdb" -Destination "$plug" -Force
    Copy-Item -Path "$TargetPath\$name.dll.mdb" -Destination "$plug" -Force
}

if ($Target.Equals("Release")) {
    Write-Host "Packaging for ThunderStore..."
    $Package="Package"
    $PackagePath="$ProjectPath\$Package"

    Write-Host "$PackagePath\$TargetAssembly"
    Copy-Item -Path "$TargetPath\$TargetAssembly" -Destination "$PackagePath\$TargetAssembly" -Force
    Copy-Item -Path "$PackagePath\README.md" -Destination "$ProjectPath\README.md" -Force
    Copy-Item -Path "$PackagePath\README.md" -Destination "$ProjectPath\..\README.md" -Force

    Remove-Item -Path "$PackagePath\Assets" -Force -Recurse

    # Copy-Item -Path "$ProjectPath\Assets" -Destination "$PackagePath\Assets" -Force -Recurse

    Copy-Item -Path "$ProjectPath\tombstone.png" -Destination "$PackagePath\tombstone.png" -Force

    (Get-Content -Path "$PackagePath\manifest.json") -replace '{VERSION}', $version | Out-File -Encoding UTF8 -FilePath "$PackagePath\manifest.json"

    Write-Host "$TargetPath\$($name)_$version.zip"
    Compress-Archive -Path "$PackagePath\*" -DestinationPath "$TargetPath\$($name)_$version.zip" -Force

    (Get-Content -Path "$PackagePath\manifest.json") -replace $version, '{VERSION}' | Out-File -Encoding UTF8 -FilePath "$PackagePath\manifest.json"
}

(Get-Content "$ProjectPath\$name.cs") -replace $version, '{VERSION}' | Out-File -encoding UTF8 "$ProjectPath\$name.cs"

# Pop Location
Pop-Location
