using System.IO;
using System;
using Avalonia.Controls;
using System.Collections.Generic;


namespace Infraestructure.ReadingFile;


public class SheetRead
{
    float discount;

    public void GetFileRead(TextBox inputRef, ListBox listRef)
    {
        string? path = inputRef.Text;
        var textReference = listRef;

        if (File.Exists(path))
        {
            string[] allLines = File.ReadAllLines(path);
            
            textReference.ItemsSource = allLines;
            
        }
        else
        {
            textReference.ItemsSource = new string[]{"ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO"};
            Console.WriteLine("ERRO: O CAMINHO DO ARQUIVO QUE VOCÊ DIGITOU NÃO EXISTE OU ESTÁ VAZIO");
        }
    }  


    public void CalculatePrice(TextBox inputRef, TextBox discountRef)   
    {
        string? path = inputRef.Text;

        if (discountRef != null && float.TryParse(discountRef.Text, out float newDiscount))
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

            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "novos-descontos.csv");
            File.WriteAllLines(savePath, newLines);
        }
    }

}
