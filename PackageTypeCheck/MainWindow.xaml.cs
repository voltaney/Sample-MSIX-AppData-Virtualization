using System;
using System.Diagnostics;
using System.IO;

using Microsoft.UI.Xaml;

using Windows.Storage;

namespace PackageTypeCheck;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // パスの設定
        var myAppName = "MSIX_TEST";
        var fileName = "sample.txt";
        var localAppDataRootPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var localAppDataPath = Path.Combine(localAppDataRootPath, myAppName);
        var virtualizedLocalAppDataPath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "Local", myAppName);

        // 従来のAppDataパスを使ってファイルを作成・書き込み
        var sampleFilePath = Path.Combine(localAppDataPath, fileName);
        Console.WriteLine($"★ [{sampleFilePath}]に追記（無ければ作成）\n");
        Directory.CreateDirectory(localAppDataPath);
        using (var sw = new StreamWriter(sampleFilePath, append: true))
        {
            sw.WriteLine("テスト書き込み");
        }

        // dirコマンドでファイル一覧を表示
        Console.WriteLine("★ dirコマンドでファイル確認");
        // C:\Users\biz\AppData\Local\MSIX_TEST
        ListFilesByDirCommand(localAppDataPath);
        // 仮想化されたLocalAppData
        ListFilesByDirCommand(virtualizedLocalAppDataPath);

        // ファイルを確認
        Console.WriteLine($"★ ファイルを読み込み");
        // C:\Users\biz\AppData\Local\MSIX_TEST
        Console.WriteLine($"[{sampleFilePath}]");
        Console.WriteLine($"File.Exists：{File.Exists(sampleFilePath)}");
        Console.WriteLine("File.ReadAllText：");
        if (File.Exists(sampleFilePath))
        {
            Console.WriteLine($"{File.ReadAllText(sampleFilePath)}");
        }
        // 仮想化されたLocalAppData
        var virtualizedSampleFilePath = Path.Combine(virtualizedLocalAppDataPath, fileName);
        Console.WriteLine($"[{virtualizedSampleFilePath}]");
        Console.WriteLine($"File.Exists：{File.Exists(virtualizedSampleFilePath)}");
        Console.WriteLine("File.ReadAllText：");
        if (File.Exists(virtualizedSampleFilePath))
        {
            Console.WriteLine($"{File.ReadAllText(virtualizedSampleFilePath)}");
        }
    }

    // コマンドプロンプトでdirコマンドを実行し、ファイル一覧を標準出力
    void ListFilesByDirCommand(string path)
    {
        Console.WriteLine($"[{path}]のファイル一覧");
        var psi = new ProcessStartInfo("cmd.exe")
        {
            Arguments = $"/c dir /b \"{path}",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        using var process = Process.Start(psi);
        string output = process!.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
    }
}
