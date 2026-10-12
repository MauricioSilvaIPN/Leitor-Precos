using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LeitorPrecos;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    #region READ FILES

    public void GettingFileRead(object sender, RoutedEventArgs args)
    {
        string path = @"/home/mauricio/Downloads/Planilha Teste - Produtos e Preços(Planilha1).csv";

        if (File.Exists(path))
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                }
            }
        }

        else
        {
            Console.WriteLine("ERROR: THE CURRENT FILE PATH DOES NOT EXISTS");
        }
        
    }


    #endregion
}