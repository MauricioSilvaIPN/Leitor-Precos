using Avalonia.Controls;
using Avalonia.Interactivity;
using Infraestructure.ReadingFile;
using System;

namespace LeitorPrecos;

public partial class MainWindow : Window
{
    private Button? calcButton;
    private TextBox? discountText;
    private SheetRead? sheetRead;

    public MainWindow()
    {
        InitializeComponent();
    }

#region FILES SOURCE
    public void GettingFileRead(object sender, RoutedEventArgs args)
    {
        sheetRead = new SheetRead();

        sheetRead.GetFileRead(InputBox, ListContent);
        CreatingElements();
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


            calcButton.Click += ShowPrice;
        }

        if (discountText == null)
        {
            discountText = new TextBox()
            {
                PlaceholderText = "Digite o desconto em que quer aplicar..."
            };

            discountText.KeyDown += TextBox_KeyDown;
        }

        if (!DataContainer.Children.Contains(calcButton) && discountText != null)
        {
            DataContainer.Children.Add(discountText);
            DataContainer.Children.Add(calcButton); 
        }
        else
        {
            return;
        }
    }
#endregion

#region ORGANIZE PRICES
    private void ShowPrice(object? sender, RoutedEventArgs args)
    {
        sheetRead = new SheetRead();

        if (discountText != null)
        {
            sheetRead.CalculatePrice(InputBox, discountText);
            SucessMessage();
            Console.WriteLine("Sucesso!! Seus dados foram salvos e escritos em um novo arquivo");    
        }

    }

    private void SucessMessage()
    {
        var sucessText = new TextBlock()
        {
            Text = "Sucesso!! Seus dados foram salvos e escritos em um novo arquivo"
        };

        DataContainer.Children.Add(sucessText);
    }
#endregion    

private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Back || 
            e.Key == Avalonia.Input.Key.Delete ||
            e.Key == Avalonia.Input.Key.Left || 
            e.Key == Avalonia.Input.Key.Right ||
            e.Key == Avalonia.Input.Key.Tab)
        {
            return;
        }

        string? letter = e.KeySymbol;

        if (string.IsNullOrEmpty(letter) || !char.IsDigit(letter[0]))
        {
            e.Handled = true;
        }
    }
}