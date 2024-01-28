using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Create a scripture memorizer instance
        var memorizer = new ScriptureMemorizer();

        // Display the scripture and hide words until all are hidden
        memorizer.StartMemorizing();

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }
}

class ScriptureMemorizer
{
    private readonly Scripture _scripture;

    public ScriptureMemorizer()
    {
        // You can initialize your scripture here
        _scripture = new Scripture("John 3:16", "For God so loved the world, that he gave his only Son, that whoever believes in him should not perish but have eternal life.");
    }

    public void StartMemorizing()
    {
        do
        {
            Console.Clear();
            _scripture.Display();
            Console.WriteLine("\nPress Enter to continue or type 'quit' to exit.");
            string input = Console.ReadLine().ToLower();

            if (input == "quit")
                break;

            _scripture.HideRandomWord();
        } while (!_scripture.AllWordsHidden);

        Console.WriteLine("Congratulations! You have memorized the scripture.");
    }
}

class Scripture
{
    private readonly string _reference;
    private readonly List<Word> _words;

    public bool AllWordsHidden => _words.All(word => word.IsHidden);

    public Scripture(string reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void Display()
    {
        Console.WriteLine($"{_reference}: {string.Join(" ", _words.Select(word => word.IsHidden ? "_____" : word.Text))}");
    }

    public void HideRandomWord()
    {
        var random = new Random();
        var visibleWords = _words.Where(word => !word.IsHidden).ToList();

        if (visibleWords.Any())
        {
            var wordToHide = visibleWords[random.Next(visibleWords.Count)];
            wordToHide.Hide();
        }
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
