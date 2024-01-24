using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Create a list of scriptures by loading them from a file
        scriptures = ("Proverbs 3:5 Trust in the Lord with all thine heart; and lean not unto thine own understanding.


Proverbs 3:5
Old Testament
Scriptures


");

        foreach (var scripture in scriptures)
        {
            DisplayScripture(scripture);

            // Continue hiding words until all words are hidden
            while (!scripture.AllWordsHidden)
            {
                Console.WriteLine("Press Enter to hide more words or type 'quit' to end:");
                string input = Console.ReadLine();

                if (input.ToLower() == "quit")
                    return;

                HideRandomWords(scripture);
                Console.Clear();
                DisplayScripture(scripture);
            }
        }

        Console.WriteLine("All scriptures are now hidden. Press Enter to exit.");
        Console.ReadLine();
    }

    static void DisplayScripture(Scripture scripture)
    {
        Console.WriteLine($"Reference: {scripture.Reference}");
        Console.WriteLine("Scripture:");
        Console.WriteLine(scripture.GetVisibleText());
        Console.WriteLine();
    }

    static void HideRandomWords(Scripture scripture)
    {
        Random random = new Random();
        int wordsToHide = random.Next(1, Math.Max(1, scripture.VisibleWordCount() / 2));

        for (int i = 0; i < wordsToHide; i++)
        {
            scripture.HideRandomWord();
        }
    }

    static List<Scripture> LoadScripturesFromFile(string filePath)
    {
        List<Scripture> scriptures = new List<Scripture>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                string[] parts = line.Split('|');
                string reference = parts[0];
                string text = parts[1];
                scriptures.Add(new Scripture(new Reference(reference), text));
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Scripture file not found. Please make sure the file exists.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading scriptures: {ex.Message}");
        }

        return scriptures;
    }
}

class Scripture
{
    private Reference reference;
    private List<Word> words;

    public bool AllWordsHidden => words.TrueForAll(word => word.IsHidden);

    public string Reference => reference.ToString();

    public Scripture(Reference reference, string text)
    {
        this.reference = reference;
        words = new List<Word>();

        string[] textWords = text.Split(' ');

        foreach (var word in textWords)
        {
            words.Add(new Word(word));
        }
    }

    public void HideRandomWord()
    {
        Random random = new Random();
        int index = random.Next(0, words.Count);

        words[index].Hide();
    }

    public string GetVisibleText()
    {
        return string.Join(" ", words.ConvertAll(word => word.IsHidden ? "___" : word.Text));
    }

    public int VisibleWordCount()
    {
        return words.Count(word => !word.IsHidden);
    }
}

class Reference
{
    private string reference;

    public Reference(string reference)
    {
        this.reference = reference;
    }

    public override string ToString()
    {
        return reference;
    }
}

class Word
{
    public string Text { get; }
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }
}
