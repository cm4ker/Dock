
public class Parameters
{
    public string Configuration { get; private set; }
    public string Artifacts { get; private set; }
    public string VersionSuffix { get; private set; }
    public string NuGetPushBranch { get; private set; }
    public string NuGetPushRepoName { get; private set; }
    public bool PushNuGet { get; private set; }
    public bool IsNugetRelease { get; private set; }
    public (string path, string name)[] BuildProjects { get; private set; }
    public (string path, string name)[] TestProjects { get; private set; }
    public (string path, string name, string framework, string runtime)[] PublishProjects { get; private set; }
    public (string path, string name)[] PackProjects { get; private set; }

    public Parameters(ICakeContext context)
    {
        Configuration = context.Argument("configuration", "Release");
        Artifacts = context.Argument("artifacts", "./artifacts");

        VersionSuffix = context.Argument("suffix", default(string));
        if (VersionSuffix == null)
        {
            var build = context.EnvironmentVariable("APPVEYOR_BUILD_VERSION");
            VersionSuffix = build != null ? $"-build{build}" : "";
        }

        NuGetPushBranch = "master";
        NuGetPushRepoName = "wieslawsoltes/Dock";

        var repoName = context.EnvironmentVariable("APPVEYOR_REPO_NAME");
        var repoBranch = context.EnvironmentVariable("APPVEYOR_REPO_BRANCH");
        var repoTag = context.EnvironmentVariable("APPVEYOR_REPO_TAG");
        var repoTagName = context.EnvironmentVariable("APPVEYOR_REPO_TAG_NAME");
        var pullRequestTitle = context.EnvironmentVariable("APPVEYOR_PULL_REQUEST_TITLE");

        if (pullRequestTitle == null 
            && string.Compare(repoName, NuGetPushRepoName, StringComparison.OrdinalIgnoreCase) == 0
            && string.Compare(repoBranch, NuGetPushBranch, StringComparison.OrdinalIgnoreCase) == 0)
        {
            PushNuGet = true;
        }

        if (pullRequestTitle == null 
            && string.Compare(repoTag, "True", StringComparison.OrdinalIgnoreCase) == 0
            && repoTagName != null)
        {
            IsNugetRelease = true;
            VersionSuffix = "";
        }

        BuildProjects = new []
        {
            ( "./src", "Dock.Model" ),
            ( "./src", "Dock.Model.INPC" ),
            ( "./src", "Dock.Model.ReactiveUI" ),
            ( "./src", "Dock.Avalonia" ),
            ( "./src", "Dock.Avalonia.Editor" ),
            ( "./src", "Dock.CodeGen" ),
            ( "./src", "Dock.Serializer" ),
            ( "./samples", "AvaloniaDemo.INPC" ),
            ( "./samples", "AvaloniaDemo.ReactiveUI" )
        };

        TestProjects = new []
        {
            ( "./tests", "Dock.Model.UnitTests" ),
            ( "./tests", "Dock.Model.INPC.UnitTests" ),
            ( "./tests", "Dock.Model.ReactiveUI.UnitTests" ),
            ( "./tests", "Dock.Avalonia.UnitTests" )
        };

        PublishProjects = new []
        {
            ( "./samples", "AvaloniaDemo.INPC", "netcoreapp2.1", "win7-x64" ),
            ( "./samples", "AvaloniaDemo.ReactiveUI", "netcoreapp2.1", "win7-x64" )
        };

        PackProjects = new []
        {
            ( "./src", "Dock.Model" ),
            ( "./src", "Dock.Model.INPC" ),
            ( "./src", "Dock.Model.ReactiveUI" ),
            ( "./src", "Dock.Avalonia" ),
            ( "./src", "Dock.Avalonia.Editor" ),
            ( "./src", "Dock.CodeGen" ),
            ( "./src", "Dock.Serializer" )
        };
    }
}
