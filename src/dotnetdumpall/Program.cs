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
        azdVersion = "Azure Developer CLI not installed";
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
    azPath = Path.Combine(windowsProgFiles, @"Microsoft SDKs\Azure\CLI2\wbin\az.cmd");
    //check to make sure the path exists
    if (!File.Exists(azPath))
    {
        // check the x86 program files folder
        azPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft SDKs\Azure\CLI2\wbin\az.cmd");
    }
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


string[] extensionsToSearch = ["ms-dotnettools.csdevkit", "ms-dotnettools.csharp", "ms-azuretools.azure-dev", "ms-dotnettools.dotnet-maui"];
StringBuilder extensionList = new();

// Get VS Code version
string vscodeVersion = "VS Code not found";
bool insiders = false;

// check for an argument to see if insiders is being used if '-i' is passed in
if (args.Contains("-i"))
{
    insiders = true;
}

string vscodeFolder = insiders ? ".vscode-insiders" : ".vscode";
string vscodePath = insiders ? "Microsoft VS Code Insiders" : "Microsoft VS Code";

// Try to find VS Code's product.json - check both user and system installations
string userProductJsonPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    vscodePath,
    "product.json"
);

string systemProductJsonPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
    vscodePath,
    "resources",
    "app",
    "product.json"
);

// Also check Program Files (x86) for 32-bit installations
string systemX86ProductJsonPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
    vscodePath,
    "resources",
    "app",
    "product.json"
);

try
{
    // Check all possible locations for product.json
    string[] possiblePaths = new[] { userProductJsonPath, systemProductJsonPath, systemX86ProductJsonPath };

    foreach (var path in possiblePaths)
    {
        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            var json = System.Text.Json.JsonDocument.Parse(jsonText);
            if (json.RootElement.TryGetProperty("version", out var version))
            {
                vscodeVersion = version.GetString() ?? "Version not found";
                break; // Exit loop once we find a valid version
            }
        }
    }
}
catch (Exception)
{
    vscodeVersion = "Could not determine VS Code version";
}

extensionList.AppendLine($"VS Code: {vscodeVersion}");
extensionList.AppendLine("\nVS Code Extensions:");

string vscodeExtensionsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), vscodeFolder, "extensions");

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