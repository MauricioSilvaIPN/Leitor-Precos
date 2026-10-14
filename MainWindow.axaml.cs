using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System;
using System.IO;
using System.Text;

namespace LeitorPrecos;

public partial class MainWindow : Window
{
    private Button? calcButton;

    public MainWindow()
    {
        InitializeComponent();
    }

    #region READ FILES
    public void GettingFileRead(object sender, RoutedEventArgs args)
    {
        string path = Input.Text;
        var textReference = TextContent;

        if (File.Exists(path))
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                   textReference.Text = temp.GetString(b);
                   CreatingButton();
                }
            }
        }

        else
        {
            textReference.Text = "ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO";
            Console.WriteLine("ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO");
        }
        
    }


    private void CreatingButton()
    {
        if (calcButton == null)
        {
            calcButton = new Button()
            {
                Margin = new Avalonia.Thickness(0, 15, 0, 15),
                Content = "Calcular Preço"
            };


            calcButton.Click += CalculatePrice;
        }

        if (!DataContainer.Children.Contains(calcButton))
        {
            DataContainer.Children.Add(calcButton);    
        }
        else
        {
            return;
        }
    }


    private void CalculatePrice(object? sender, RoutedEventArgs args)
    {
        var textReference = TextContent;
        string[] textSeparator = textReference.Text.Split('$');

        for (int i = 0; i < textSeparator.Length; i++)
        {
            string changeChar = textSeparator[i].Replace(',', '.');
            string removeSpace = changeChar.Trim( new Char[] {' '} );
            //Console.WriteLine(removeSpace);
        }
    }

    #endregion
}