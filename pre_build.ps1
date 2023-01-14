# powershell -executionpolicy remotesigned -File "$(SolutionDir)pre_build.ps1" $(SolutionDir) $(SolutionName)

$csFilePath = $args[0] + $args[1] + '\\' + $args[1] + '.cs'

(Get-Content $csFilePath) -replace '{VERSION}', (Get-Content "$($args[0])VERSION") | Out-File -encoding UTF8 $csFilePath
