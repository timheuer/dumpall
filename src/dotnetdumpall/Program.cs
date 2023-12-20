using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

var dotnetInfo = Process.Start(new ProcessStartInfo
{
    FileName = "dotnet",
    Arguments = "--info",
    RedirectStandardOutput = true,
    UseShellExecute = false
}).StandardOutput.ReadToEnd();

// now get the output of azd version
string azdVersion = string.Empty;

try
{
    azdVersion = Process.Start(new ProcessStartInfo
    {
        FileName = "azd",
        Arguments = "version",
        RedirectStandardOutput = true,
        UseShellExecute = false
    }).StandardOutput.ReadToEnd();
}
catch (Exception ex)
{
    if (ex.Message.Contains("The system cannot find the file specified."))
    {
        azdVersion = "Azure Devevloper CLI not installed";
    }
    else
    {
        azdVersion = $"An error occurred: {ex.Message}";
    }
}

// now get the output of az version
string azVersion = string.Empty;

// get the path to the executable for az.cmd
// on windows it will be in C:\Program Files\Microsoft SDKs\Azure\CLI2\wbin\az.cmd
// on linux it will be in /usr/bin/az
// on mac it will be in /usr/local/bin/az
// if it is not found, then write out that it is not installed
string azPath = string.Empty;
string windowsProgFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    azPath = Path.Combine(windowsProgFiles,@"Microsoft SDKs\Azure\CLI2\wbin\az.cmd");
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    azPath = "/usr/bin/az";
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    azPath = "/usr/local/bin/az";
}

try
{
    azVersion = Process.Start(new ProcessStartInfo
    {
        FileName = azPath,
        Arguments = "version",
        RedirectStandardOutput = true,
        UseShellExecute = false
    }).StandardOutput.ReadToEnd();
}
catch (Exception ex)
{
    if (ex.Message.Contains("The system cannot find the file specified."))
    {
        azVersion = "Azure CLI not installed";
    }
    else
    {
        azVersion = $"An error occurred: {ex.Message}";
    }
}


string[] extensionsToSearch = ["ms-dotnettools.csdevkit","ms-dotnettools.csharp", "ms-azuretools.azure-dev"];
StringBuilder extensionList = new();

extensionList.AppendLine("VS Code Extensions");

bool insiders = false;

// check for an argument to see if insiders is being used if '-i' is passed in
if (args.Contains("-i"))
{
    insiders = true;
}
string vscodeFolder = insiders ? ".vscode-insiders" : ".vscode";

string vscodeExtensionsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), vscodeFolder, "extensions");

string csdevkitVersion = string.Empty;
string csharpVersion = string.Empty;

try
{
    string[] directories = Directory.GetDirectories(vscodeExtensionsPath);


    for (int i = 0; i < extensionsToSearch.Length; i++)
    {
        foreach (string dir in directories)
        {
            if (dir.Contains(extensionsToSearch[i]))
            {
                string packageJsonPath = Path.Combine(dir, "package.json");
                if (File.Exists(packageJsonPath))
                {
                    string jsonText = File.ReadAllText(packageJsonPath);
                    var json = System.Text.Json.JsonDocument.Parse(jsonText);
                    if (json.RootElement.TryGetProperty("version", out var version))
                    {
                        extensionList.AppendLine($"  {extensionsToSearch[i]}: {version.GetString()}");
                    }
                }
            }
        }
    }
}
catch (DirectoryNotFoundException)
{
    extensionList.AppendLine("VS Code Extensions not found. Are you using Insiders? Pass `-i` to the command");
}


// combine the output of dotnetinfo with azdversion and write that output to the console
// if an argument of -o is passed, write it to a file else to the console window
var output = $"{dotnetInfo}{Environment.NewLine}Azure Developer CLI:{Environment.NewLine}  {azdVersion}{Environment.NewLine}Azure CLI:{Environment.NewLine}  {azVersion}{Environment.NewLine}{extensionList}";
if (args.Contains("-o"))
{
    // write it to the argument passed in after the -o
    int index = Array.IndexOf(args, "-o");
    if (index != -1 && args.Length > index + 1)
    {
        File.WriteAllText(args[index + 1], output);
    }
}
else
{
    Console.WriteLine(output);
}