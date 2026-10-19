using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Text;

namespace LeitorPrecos;

public partial class MainWindow : Window
{
    private Button? calcButton;
    List<double> defaultPrices = new List<double>();
    List<double> discountPrices = new List<double>();
    private float discount = 0.5f;

    public MainWindow()
    {
        InitializeComponent();
    }

    #region FILES SOURCE
    public void GettingFileRead(object sender, RoutedEventArgs args)
    {
        string? path = Input.Text;
        var textReference = ListContent;

        if (File.Exists(path))
        {
            string[] allLines = File.ReadAllLines(path);
            
            textReference.ItemsSource = allLines;
            CreatingButton();
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
#endregion

#region ORGANIZE PRICES
    private void CalculatePrice(object? sender, RoutedEventArgs args)
    {
        SeparateChar();
        
        string? path = Input.Text;

        if (File.Exists(path))
        {
            string[] allLines = File.ReadAllLines(path);
            List<string> newLines = new List<string>();

            foreach(string line in allLines)
            {
                if (line.Contains('$'))
                {
                    string[] parts = line.Split('$');
                    string prefix = parts[0];
                    string priceString = parts[1].Trim( new Char[] {' '} );

                    if (double.TryParse(priceString, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("pt-BR"), out double originalPrice))
                    {
                        double discountedPrice = originalPrice * discount;

                        string reconstructedLine = prefix + "$" + discountedPrice.ToString("F2", new System.Globalization.CultureInfo("pt-BR"));
                        
                        newLines.Add(reconstructedLine);
                    }
                    else
                    {
                        newLines.Add(line);
                    }
                }
                else
                {
                    newLines.Add(line);
                }
            }

            string savePath = "/home/mauricio/Downloads/novos-descontos.csv";
            File.WriteAllLines(savePath, newLines);
            Console.WriteLine("Sucesso!! Seus dados foram salvos e escrito em um novo arquivo");
        }
    }


    private void SeparateChar()
    {
        var textReference = ListContent;

        if (textReference.ItemsSource != null)
        {
            foreach (string itemS in textReference.ItemsSource)
            {
                string[] textSeparator = itemS.Split('$');
                
                
                for(int i = 0; i < textSeparator.Length; i++)
                {
                    string removeSpace = textSeparator[i].Trim( new Char[] {' '} );
                    
                    bool isConverted = double.TryParse(removeSpace, out double result);

                    if (isConverted)
                    {
                        defaultPrices.Add(result);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }

    #endregion
}