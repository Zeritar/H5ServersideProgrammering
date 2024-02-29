update-database -Context ApplicationDbContext
update-database -Context AppDataContext

$dir = "."
$ns = @{ defaultNamespace = "http://schemas.microsoft.com/developer/msbuild/2003" }

$config = [xml] "<ItemGroup>
    <None Update=`"Data\appData.db`">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Update=`"Data\appIdentity.db`">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>"

Get-ChildItem $dir *.csproj | 
% { 
  $content = [xml](gc $_.FullName); 
  $importNode = $content.ImportNode($config.DocumentElement, $true) 
  $project = $content.Project;
  $project
  $project.AppendChild($importNode);
  $content.Save($_.FullName);
}