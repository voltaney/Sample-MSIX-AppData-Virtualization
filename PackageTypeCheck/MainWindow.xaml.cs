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

        // �p�X�̐ݒ�
        var myAppName = "MSIX_TEST";
        var fileName = "sample.txt";
        var localAppDataRootPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var localAppDataPath = Path.Combine(localAppDataRootPath, myAppName);
        var virtualizedLocalAppDataPath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "Local", myAppName);

        // �]����AppData�p�X���g���ăt�@�C�����쐬�E��������
        var sampleFilePath = Path.Combine(localAppDataPath, fileName);
        Console.WriteLine($"�� [{sampleFilePath}]�ɒǋL�i������΍쐬�j\n");
        Directory.CreateDirectory(localAppDataPath);
        using (var sw = new StreamWriter(sampleFilePath, append: true))
        {
            sw.WriteLine("�e�X�g��������");
        }

        // dir�R�}���h�Ńt�@�C���ꗗ��\��
        Console.WriteLine("�� dir�R�}���h�Ńt�@�C���m�F");
        // C:\Users\biz\AppData\Local\MSIX_TEST
        ListFilesByDirCommand(localAppDataPath);
        // ���z�����ꂽLocalAppData
        ListFilesByDirCommand(virtualizedLocalAppDataPath);

        // �t�@�C�����m�F
        Console.WriteLine($"�� �t�@�C����ǂݍ���");
        // C:\Users\biz\AppData\Local\MSIX_TEST
        Console.WriteLine($"[{sampleFilePath}]");
        Console.WriteLine($"File.Exists�F{File.Exists(sampleFilePath)}");
        Console.WriteLine("File.ReadAllText�F");
        if (File.Exists(sampleFilePath))
        {
            Console.WriteLine($"{File.ReadAllText(sampleFilePath)}");
        }
        // ���z�����ꂽLocalAppData
        var virtualizedSampleFilePath = Path.Combine(virtualizedLocalAppDataPath, fileName);
        Console.WriteLine($"[{virtualizedSampleFilePath}]");
        Console.WriteLine($"File.Exists�F{File.Exists(virtualizedSampleFilePath)}");
        Console.WriteLine("File.ReadAllText�F");
        if (File.Exists(virtualizedSampleFilePath))
        {
            Console.WriteLine($"{File.ReadAllText(virtualizedSampleFilePath)}");
        }
    }

    // �R�}���h�v�����v�g��dir�R�}���h�����s���A�t�@�C���ꗗ��W���o��
    void ListFilesByDirCommand(string path)
    {
        Console.WriteLine($"[{path}]�̃t�@�C���ꗗ");
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
