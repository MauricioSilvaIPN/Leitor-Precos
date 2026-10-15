using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeitorPrecos;

public partial class MainWindow : Window
{
    private Button? calcButton;
    List<double> myPrices = new List<double>();

    public MainWindow()
    {
        InitializeComponent();
    }

    #region READ FILES
    public void GettingFileRead(object sender, RoutedEventArgs args)
    {
        string path = Input.Text;
        var textReference = ListContent;

        if (File.Exists(path))
        {
            string[] allLines = File.ReadAllLines(path);
            
            textReference.ItemsSource = allLines;
            CreatingButton();

            /*using (FileStream fs = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                   textReference.Text = temp.GetString(b);
                   CreatingButton();
                }
            }*/
        }

        else
        {
            textReference.ItemsSource = new string[]{"ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO"};
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
        SeparateChar();

        foreach (double j in myPrices)
        {
            Console.WriteLine(j);
            Console.WriteLine(myPrices.Count);
        }
    }


    private void SeparateChar()
    {
        var textReference = ListContent;

        foreach (string itemS in textReference.ItemsSource)
        {
            string[] textSeparator = itemS.Split('$');
            
            
            for(int i = 0; i < textSeparator.Length; i++)
            {
                string removeSpace = textSeparator[i].Trim( new Char[] {' '} );
                
                bool isConverted = double.TryParse(removeSpace, out double result);

                if (isConverted)
                {
                    myPrices.Add(result);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    #endregion
}