#!/usr/bin/env python3
"""
PingPong.App MAUI scaffold script.
Run from the repository root: python3 src/scaffold.py
Creates the full project structure, commits, and opens a PR.
"""
import os, subprocess, sys, textwrap

REPO_ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), ".."))

FILES = {}

# ── Solution ────────────────────────────────────────────────────────────────
FILES["src/PingPong.sln"] = """\

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "PingPong.App", "PingPong.App\\PingPong.App.csproj", "{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "PingPong.App.Tests", "PingPong.App.Tests\\PingPong.App.Tests.csproj", "{B2C3D4E5-F6A7-8901-BCDE-F12345678901}"
EndProject
Global
\tGlobalSection(SolutionConfigurationPlatforms) = preSolution
\t\tDebug|Any CPU = Debug|Any CPU
\t\tRelease|Any CPU = Release|Any CPU
\tEndGlobalSection
\tGlobalSection(ProjectConfigurationPlatforms) = postSolution
\t\t{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
\t\t{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}.Debug|Any CPU.Build.0 = Debug|Any CPU
\t\t{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}.Release|Any CPU.ActiveCfg = Release|Any CPU
\t\t{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}.Release|Any CPU.Build.0 = Release|Any CPU
\t\t{B2C3D4E5-F6A7-8901-BCDE-F12345678901}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
\t\t{B2C3D4E5-F6A7-8901-BCDE-F12345678901}.Debug|Any CPU.Build.0 = Debug|Any CPU
\t\t{B2C3D4E5-F6A7-8901-BCDE-F12345678901}.Release|Any CPU.ActiveCfg = Release|Any CPU
\t\t{B2C3D4E5-F6A7-8901-BCDE-F12345678901}.Release|Any CPU.Build.0 = Release|Any CPU
\tEndGlobalSection
\tGlobalSection(SolutionProperties) = preSolution
\t\tHideSolutionNode = FALSE
\tEndGlobalSection
EndGlobal
"""

# ── App csproj ───────────────────────────────────────────────────────────────
FILES["src/PingPong.App/PingPong.App.csproj"] = """\
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>net9.0-android;net9.0-ios</TargetFrameworks>
    <OutputType>Exe</OutputType>
    <RootNamespace>PingPong.App</RootNamespace>
    <UseMaui>true</UseMaui>
    <SingleProject>true</SingleProject>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <ApplicationTitle>Ping Pong</ApplicationTitle>
    <ApplicationId>com.ludekkolarcik.pingpong</ApplicationId>
    <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
    <ApplicationVersion>1</ApplicationVersion>

    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">15.0</SupportedOSPlatformVersion>
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
  </PropertyGroup>

  <ItemGroup>
    <MauiIcon Include="Resources/AppIcon/appicon.svg" ForegroundFile="Resources/AppIcon/appiconfg.svg" Color="#1A1A2E" />
    <MauiSplashScreen Include="Resources/Splash/splash.svg" Color="#1A1A2E" BaseSize="128,128" />
    <MauiImage Include="Resources/Images/*" />
    <MauiFont Include="Resources/Fonts/*" />
    <MauiAsset Include="Resources/Raw/**" LogicalName="%(RecursiveDir)%(Filename)%(Extension)" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
    <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.0" />
  </ItemGroup>

</Project>
"""

# ── MauiProgram.cs ───────────────────────────────────────────────────────────
FILES["src/PingPong.App/MauiProgram.cs"] = """\
using Microsoft.Extensions.Logging;
using PingPong.App.Services;

namespace PingPong.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IAudioService, AudioService>();
        builder.Services.AddSingleton<IHapticService, HapticService>();
        builder.Services.AddSingleton<IPreferencesService, PreferencesService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
"""

# ── App.xaml ─────────────────────────────────────────────────────────────────
FILES["src/PingPong.App/App.xaml"] = """\
<?xml version="1.0" encoding="UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="PingPong.App.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
"""

FILES["src/PingPong.App/App.xaml.cs"] = """\
namespace PingPong.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
"""

# ── AppShell ─────────────────────────────────────────────────────────────────
FILES["src/PingPong.App/AppShell.xaml"] = """\
<?xml version="1.0" encoding="UTF-8" ?>
<Shell x:Class="PingPong.App.AppShell"
       xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:local="clr-namespace:PingPong.App"
       Shell.FlyoutBehavior="Disabled">
    <ShellContent Title="Ping Pong"
                  ContentTemplate="{DataTemplate local:MainPage}"
                  Route="MainPage" />
</Shell>
"""

FILES["src/PingPong.App/AppShell.xaml.cs"] = """\
namespace PingPong.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }
}
"""

# ── MainPage ─────────────────────────────────────────────────────────────────
FILES["src/PingPong.App/MainPage.xaml"] = """\
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="PingPong.App.MainPage"
             Title="Ping Pong">
    <VerticalStackLayout Spacing="25" Padding="30,0" VerticalOptions="Center">
        <Label Text="Ping Pong"
               FontSize="40"
               FontAttributes="Bold"
               HorizontalOptions="Center" />
        <Label Text="Game coming soon..."
               FontSize="18"
               HorizontalOptions="Center"
               TextColor="{StaticResource Secondary}" />
    </VerticalStackLayout>
</ContentPage>
"""

FILES["src/PingPong.App/MainPage.xaml.cs"] = """\
namespace PingPong.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}
"""

# ── Services ─────────────────────────────────────────────────────────────────
FILES["src/PingPong.App/Services/IAudioService.cs"] = """\
namespace PingPong.App.Services;

public interface IAudioService
{
    Task PlayAsync(string soundName);
    Task StopAllAsync();
    float Volume { get; set; }
}
"""

FILES["src/PingPong.App/Services/AudioService.cs"] = """\
namespace PingPong.App.Services;

/// <summary>
/// Platform-neutral stub; platform-specific implementations live in Platforms/.
/// </summary>
public partial class AudioService : IAudioService
{
    public float Volume { get; set; } = 1.0f;

    public partial Task PlayAsync(string soundName);
    public partial Task StopAllAsync();
}
"""

FILES["src/PingPong.App/Services/IHapticService.cs"] = """\
namespace PingPong.App.Services;

public interface IHapticService
{
    void Vibrate(HapticFeedbackType type = HapticFeedbackType.Click);
}

public enum HapticFeedbackType
{
    Click,
    LongPress,
    Success,
    Warning
}
"""

FILES["src/PingPong.App/Services/HapticService.cs"] = """\
namespace PingPong.App.Services;

/// <summary>
/// Platform-neutral stub; platform-specific implementations live in Platforms/.
/// </summary>
public partial class HapticService : IHapticService
{
    public partial void Vibrate(HapticFeedbackType type);
}
"""

FILES["src/PingPong.App/Services/IPreferencesService.cs"] = """\
namespace PingPong.App.Services;

public interface IPreferencesService
{
    T Get<T>(string key, T defaultValue);
    void Set<T>(string key, T value);
    void Remove(string key);
}
"""

FILES["src/PingPong.App/Services/PreferencesService.cs"] = """\
using Microsoft.Maui.Storage;

namespace PingPong.App.Services;

public class PreferencesService : IPreferencesService
{
    public T Get<T>(string key, T defaultValue) =>
        Preferences.Default.Get(key, defaultValue);

    public void Set<T>(string key, T value) =>
        Preferences.Default.Set(key, value);

    public void Remove(string key) =>
        Preferences.Default.Remove(key);
}
"""

# ── Platforms/Android ────────────────────────────────────────────────────────
FILES["src/PingPong.App/Platforms/Android/AndroidManifest.xml"] = """\
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    <application
        android:allowBackup="true"
        android:icon="@mipmap/appicon"
        android:roundIcon="@mipmap/appicon_round"
        android:supportsRtl="true"
        android:label="@string/app_name">
    </application>
    <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
    <uses-permission android:name="android.permission.VIBRATE" />
</manifest>
"""

FILES["src/PingPong.App/Platforms/Android/MainActivity.cs"] = """\
using Android.App;
using Android.Content.PM;

namespace PingPong.App.Platforms.Android;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize
        | ConfigChanges.Orientation
        | ConfigChanges.UiMode
        | ConfigChanges.ScreenLayout
        | ConfigChanges.SmallestScreenSize
        | ConfigChanges.Density
)]
public class MainActivity : MauiAppCompatActivity
{
}
"""

FILES["src/PingPong.App/Platforms/Android/MainApplication.cs"] = """\
using Android.App;
using Android.Runtime;

namespace PingPong.App.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
"""

FILES["src/PingPong.App/Platforms/Android/AudioService.Android.cs"] = """\
using Android.Media;

namespace PingPong.App.Services;

public partial class AudioService
{
    private SoundPool? _soundPool;
    private readonly Dictionary<string, int> _soundIds = new();

    public partial async Task PlayAsync(string soundName)
    {
        // TODO: load sound from raw resources and play via SoundPool
        await Task.CompletedTask;
    }

    public partial async Task StopAllAsync()
    {
        _soundPool?.AutoPause();
        await Task.CompletedTask;
    }
}
"""

FILES["src/PingPong.App/Platforms/Android/HapticService.Android.cs"] = """\
using Android.OS;
using Android.Content;

namespace PingPong.App.Services;

public partial class HapticService
{
    public partial void Vibrate(HapticFeedbackType type)
    {
        var vibrator = Platform.AppContext.GetSystemService(Context.VibratorService) as Vibrator;
        if (vibrator is null || !vibrator.HasVibrator) return;

        long durationMs = type switch
        {
            HapticFeedbackType.LongPress => 50,
            HapticFeedbackType.Success   => 30,
            HapticFeedbackType.Warning   => 40,
            _                            => 10,
        };

        if (OperatingSystem.IsAndroidVersionAtLeast(26))
            vibrator.Vibrate(VibrationEffect.CreateOneShot(durationMs, VibrationEffect.DefaultAmplitude));
        else
#pragma warning disable CA1422
            vibrator.Vibrate(durationMs);
#pragma warning restore CA1422
    }
}
"""

# ── Platforms/iOS ────────────────────────────────────────────────────────────
FILES["src/PingPong.App/Platforms/iOS/AppDelegate.cs"] = """\
using Foundation;

namespace PingPong.App.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
"""

FILES["src/PingPong.App/Platforms/iOS/Program.cs"] = """\
using ObjCRuntime;
using UIKit;

namespace PingPong.App.Platforms.iOS;

public class Program
{
    static void Main(string[] args)
    {
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}
"""

FILES["src/PingPong.App/Platforms/iOS/Info.plist"] = """\
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>LSRequiresIPhoneOS</key>
    <true/>
    <key>UIDeviceFamily</key>
    <array>
        <integer>1</integer>
        <integer>2</integer>
    </array>
    <key>UIRequiredDeviceCapabilities</key>
    <array>
        <string>arm64</string>
    </array>
    <key>UISupportedInterfaceOrientations</key>
    <array>
        <string>UIInterfaceOrientationPortrait</string>
        <string>UIInterfaceOrientationLandscapeLeft</string>
        <string>UIInterfaceOrientationLandscapeRight</string>
    </array>
    <key>UISupportedInterfaceOrientations~ipad</key>
    <array>
        <string>UIInterfaceOrientationPortrait</string>
        <string>UIInterfaceOrientationPortraitUpsideDown</string>
        <string>UIInterfaceOrientationLandscapeLeft</string>
        <string>UIInterfaceOrientationLandscapeRight</string>
    </array>
    <key>CFBundleDisplayName</key>
    <string>Ping Pong</string>
    <key>UIStatusBarStyle</key>
    <string>UIStatusBarStyleLightContent</string>
</dict>
</plist>
"""

FILES["src/PingPong.App/Platforms/iOS/AudioService.iOS.cs"] = """\
using AVFoundation;

namespace PingPong.App.Services;

public partial class AudioService
{
    private AVAudioPlayer? _player;

    public partial async Task PlayAsync(string soundName)
    {
        // TODO: load from Resources/Raw and play via AVAudioPlayer
        await Task.CompletedTask;
    }

    public partial async Task StopAllAsync()
    {
        _player?.Stop();
        await Task.CompletedTask;
    }
}
"""

FILES["src/PingPong.App/Platforms/iOS/HapticService.iOS.cs"] = """\
using UIKit;

namespace PingPong.App.Services;

public partial class HapticService
{
    public partial void Vibrate(HapticFeedbackType type)
    {
        var style = type switch
        {
            HapticFeedbackType.Success   => UINotificationFeedbackType.Success,
            HapticFeedbackType.Warning   => UINotificationFeedbackType.Warning,
            HapticFeedbackType.LongPress => UINotificationFeedbackType.Success, // handled below
            _                            => UINotificationFeedbackType.Success,
        };

        if (type is HapticFeedbackType.Click or type is HapticFeedbackType.LongPress)
        {
            var impactStyle = type is HapticFeedbackType.LongPress
                ? UIImpactFeedbackStyle.Heavy
                : UIImpactFeedbackStyle.Light;
            var impact = new UIImpactFeedbackGenerator(impactStyle);
            impact.ImpactOccurred();
        }
        else
        {
            var notification = new UINotificationFeedbackGenerator();
            notification.NotificationOccurred(style);
        }
    }
}
"""

# ── Resources ────────────────────────────────────────────────────────────────
FILES["src/PingPong.App/Resources/AppIcon/appicon.svg"] = """\
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
  <rect width="100" height="100" rx="20" fill="#1A1A2E"/>
  <circle cx="50" cy="50" r="18" fill="white"/>
</svg>
"""

FILES["src/PingPong.App/Resources/AppIcon/appiconfg.svg"] = """\
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
  <circle cx="50" cy="50" r="18" fill="white"/>
</svg>
"""

FILES["src/PingPong.App/Resources/Splash/splash.svg"] = """\
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 128 128">
  <rect width="128" height="128" fill="#1A1A2E"/>
  <circle cx="64" cy="64" r="24" fill="white"/>
</svg>
"""

FILES["src/PingPong.App/Resources/Styles/Colors.xaml"] = """\
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <Color x:Key="Primary">#1A1A2E</Color>
    <Color x:Key="Secondary">#E94560</Color>
    <Color x:Key="Tertiary">#0F3460</Color>
    <Color x:Key="White">White</Color>
    <Color x:Key="Black">Black</Color>
    <Color x:Key="Gray100">#E1E1E1</Color>
    <Color x:Key="Gray900">#212121</Color>
    <Color x:Key="PageBackgroundColor">#1A1A2E</Color>
    <Color x:Key="PrimaryTextColor">White</Color>
</ResourceDictionary>
"""

FILES["src/PingPong.App/Resources/Styles/Styles.xaml"] = """\
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    <Style TargetType="Page" ApplyToDerivedTypes="True">
        <Setter Property="BackgroundColor" Value="{StaticResource PageBackgroundColor}" />
    </Style>
    <Style TargetType="Label">
        <Setter Property="TextColor" Value="{StaticResource PrimaryTextColor}" />
        <Setter Property="FontFamily" Value="OpenSansRegular"/>
    </Style>
</ResourceDictionary>
"""

FILES["src/PingPong.App/Resources/Fonts/.gitkeep"] = ""
FILES["src/PingPong.App/Resources/Images/.gitkeep"] = ""
FILES["src/PingPong.App/Resources/Raw/.gitkeep"] = ""

# ── Directory placeholders ────────────────────────────────────────────────────
FILES["src/PingPong.App/Game/Core/.gitkeep"] = ""
FILES["src/PingPong.App/Game/Systems/.gitkeep"] = ""
FILES["src/PingPong.App/Views/.gitkeep"] = ""
FILES["src/PingPong.App/ViewModels/.gitkeep"] = ""

# ── Test project ─────────────────────────────────────────────────────────────
FILES["src/PingPong.App.Tests/PingPong.App.Tests.csproj"] = """\
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

</Project>
"""

FILES["src/PingPong.App.Tests/UnitTest1.cs"] = """\
namespace PingPong.App.Tests;

public class UnitTest1
{
    [Fact]
    public void Placeholder_Test_Passes()
    {
        Assert.True(true);
    }
}
"""


def run(cmd: list[str], cwd: str = REPO_ROOT, check: bool = False) -> str:
    result = subprocess.run(cmd, cwd=cwd, capture_output=True, text=True)
    if result.returncode != 0:
        msg = f"WARN: {' '.join(cmd)}\n  stderr: {result.stderr.strip()}"
        if check:
            raise RuntimeError(msg)
        print(msg)
    return result.stdout.strip()


def main():
    os.chdir(REPO_ROOT)
    print(f"Working in: {REPO_ROOT}")

    # Create feature branch
    branches = run(["git", "branch", "--list", "feature/s1-scaffold"])
    if branches:
        run(["git", "checkout", "feature/s1-scaffold"])
    else:
        run(["git", "checkout", "-b", "feature/s1-scaffold"])
    print("Branch: feature/s1-scaffold")

    # Write all files
    created = []
    for rel_path, content in FILES.items():
        abs_path = os.path.join(REPO_ROOT, rel_path)
        os.makedirs(os.path.dirname(abs_path), exist_ok=True)
        if not os.path.exists(abs_path):
            with open(abs_path, "w", encoding="utf-8") as f:
                f.write(content)
            created.append(rel_path)
        else:
            print(f"  skip (exists): {rel_path}")

    print(f"\nCreated {len(created)} files:")
    for p in created:
        print(f"  {p}")

    # Verify dotnet build (optional)
    dotnet = run(["which", "dotnet"])
    if dotnet:
        print("\nRunning dotnet build...")
        result = subprocess.run(
            ["dotnet", "build", "src/PingPong.sln", "-v", "minimal"],
            cwd=REPO_ROOT,
        )
        build_ok = result.returncode == 0
        print("Build:", "✅ passed" if build_ok else "❌ failed")
    else:
        print("\ndotnet not found — skipping build verification")
        build_ok = None

    # Commit
    run(["git", "add"] + [os.path.join(REPO_ROOT, p) for p in created])
    # Also add the already-written sln file and this script
    run(["git", "add", os.path.join(REPO_ROOT, "src/PingPong.sln"),
         os.path.join(REPO_ROOT, "src/bootstrap.sh"),
         os.path.join(REPO_ROOT, "src/scaffold.py")])
    commit_msg = textwrap.dedent("""\
        feat: scaffold MAUI project structure

        - Add PingPong.sln with PingPong.App and PingPong.App.Tests projects
        - Create Game/Core, Game/Systems, Views, ViewModels, Services directories
        - Configure targets for net9.0-android and net9.0-ios
        - Add IAudioService, IHapticService, IPreferencesService with partial
          platform implementations for Android and iOS
        - Set up xUnit test project
        - Add MAUI resources: AppIcon, Splash, Colors, Styles

        Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>
    """)
    run(["git", "commit", "-m", commit_msg], check=True)
    print("\nCommit created.")

    # Push
    run(["git", "push", "-u", "origin", "feature/s1-scaffold"], check=True)
    print("Pushed to origin.")

    # Create PR
    pr_body = (
        "Sprint 1 Phase 1: Project scaffolding\n\n"
        "## Changes\n"
        "- `PingPong.sln` solution with App + Tests projects\n"
        "- App targets `net9.0-android` and `net9.0-ios`\n"
        "- `IAudioService`, `IHapticService`, `IPreferencesService` interfaces\n"
        "- Partial platform implementations (Android + iOS) for Audio & Haptics\n"
        "- `PreferencesService` using `Microsoft.Maui.Storage.Preferences`\n"
        "- Directory placeholders: `Game/Core`, `Game/Systems`, `Views`, `ViewModels`\n"
        "- MAUI resources: AppIcon SVG, Splash SVG, Colors, Styles\n"
        "- xUnit test project skeleton\n\n"
        "## Build\n"
        f"{'✅ passed' if build_ok else ('❌ failed' if build_ok is False else '⏭ skipped (dotnet unavailable)')}\n"
    )
    pr_result = subprocess.run(
        [
            "gh", "pr", "create",
            "--title", "feat: scaffold MAUI project",
            "--body", pr_body,
            "--base", "main",
        ],
        cwd=REPO_ROOT,
        capture_output=True,
        text=True,
    )
    if pr_result.returncode == 0:
        pr_url = pr_result.stdout.strip()
        print(f"\nPR created: {pr_url}")
    else:
        print(f"\nPR creation failed: {pr_result.stderr.strip()}")
        pr_url = None

    print("\n=== Summary ===")
    print(f"Files created : {len(created)}")
    print(f"Build         : {'✅' if build_ok else ('❌' if build_ok is False else '⏭ skipped')}")
    print(f"PR            : {pr_url or 'not created'}")
    return 0 if (build_ok is not False) else 1


if __name__ == "__main__":
    sys.exit(main())
