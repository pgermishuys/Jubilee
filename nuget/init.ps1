param($installPath, $toolsPath, $package, $project)

$configFileFrom = $toolsPath + "\..\src\configuration.yaml"
$configFileTo = "configuration.yaml"

$jubileeCoreFrom = $toolsPath + "\..\src\Jubilee.Core.dll"
$jubileeCoreTo = "Jubilee.Core.dll"

$jubileeFrom = $toolsPath + "\..\src\Jubilee.exe"
$jubileeTo = "Jubilee.exe"

$ninjectFrom = $toolsPath + "\..\src\Ninject.dll"
$ninjectTo = "Ninject.dll"

$growlConnectorFrom = $toolsPath + "\..\src\Growl.Connector.dll"
$growlConnectorTo = "Growl.Connector.dll"

$growlCoreLibraryFrom = $toolsPath + "\..\src\Growl.CoreLibrary.dll"
$growlCoreLibraryTo = "Growl.CoreLibrary.dll"

$yamlDotNetCoreFrom = $toolsPath + "\..\src\YamlDotNet.Core.dll"
$yamlDotNetCoreTo = "YamlDotNet.Core.dll"

$yamlDotNetRepresentationModelFrom = $toolsPath + "\..\src\YamlDotNet.RepresentationModel.dll"
$yamlDotNetRepresentationModelTo = "YamlDotNet.RepresentationModel.dll"


if(!(Test-Path $configFileTo))
{
  Copy-Item $configFileFrom $configFileTo
  Copy-Item $jubileeCoreFrom $jubileeCoreTo
  Copy-Item $jubileeFrom $jubileeTo
  Copy-Item $ninjectFrom $ninjectTo
  Copy-Item $growlConnectorFrom $growlConnectorTo
  Copy-Item $growlCoreLibraryFrom $growlCoreLibraryTo
  Copy-Item $yamlDotNetCoreFrom $yamlDotNetCoreTo
  Copy-Item $yamlDotNetRepresentationModelFrom $yamlDotNetRepresentationModelTo
}
else
{
  Write-Host ""
  Write-Host "files were not copied to root directory, because it looks like they already exist, go to the package directory and copy all the files from " $toolsPath "\..\src to the root directory (unless you've changed them of course)"
}