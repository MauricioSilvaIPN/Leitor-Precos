using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Metadata;
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
    private TextBox? discountText;
    private float discount;

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
            CreatingElements();
        }

        else
        {
            textReference.ItemsSource = new string[]{"ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO"};
            Console.WriteLine("ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO");
        }
        
    }


    private void CreatingElements()
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

        if (discountText == null)
        {
            discountText = new TextBox()
            {
                PlaceholderText = "Digite o desconto em que quer aplicar..."
            };
        }

        if (!DataContainer.Children.Contains(calcButton) && discountText != null)
        {
            DataContainer.Children.Add(calcButton); 
            DataContainer.Children.Add(discountText);
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
        string? path = Input.Text;

        if (discountText != null && float.TryParse(discountText.Text, out float newDiscount))
        {
            discount = newDiscount;
        }
        

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
                        double firstValue = (originalPrice * discount) / 100f;
                        double discountedPrice = originalPrice - firstValue;

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

            SucessMessage();
            Console.WriteLine("Sucesso!! Seus dados foram salvos e escrito em um novo arquivo");
        }
    }

    private void SucessMessage()
    {
        var sucessText = new TextBlock()
        {
            Text = "Sucesso!! Seus dados foram salvos e escrito em um novo arquivo"
        };

        DataContainer.Children.Add(sucessText);
    }
#endregion
}