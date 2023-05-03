#addin nuget:?package=Cake.Coverlet

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Build")
    .Does(() =>
{
    DotNetBuild("WebApiTaxbox.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
	var coverletSettings = new CoverletSettings {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover | CoverletOutputFormat.json,
        MergeWithFile = MakeAbsolute(new DirectoryPath("./coverage.json")).FullPath,
        CoverletOutputDirectory = MakeAbsolute(new DirectoryPath(@"./coverage")).FullPath
    };
	
	Coverlet(
        "./tests/Taxbox.Api.IntegrationTests/bin/Debug/net7.0/Taxbox.Api.IntegrationTests.dll", 
        "./tests/Taxbox.Api.IntegrationTests/Taxbox.Api.IntegrationTests.csproj", 
        coverletSettings);
		
	Coverlet(
        "./tests/Taxbox.Api.UnitTests/bin/Debug/net7.0/Taxbox.Api.UnitTests.dll", 
        "./tests/Taxbox.Api.UnitTests/Taxbox.Api.UnitTests.csproj", 
        coverletSettings);
	
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);