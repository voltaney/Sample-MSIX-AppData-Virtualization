using Microsoft.UI.Xaml;

namespace PackageTypeCheck;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if PACKAGED
        // Packaged app
        MyText.Text = "Packaged App";
#else
        // Unpackaged app
        MyText.Text = "Unpackaged App";
#endif
    }
}
