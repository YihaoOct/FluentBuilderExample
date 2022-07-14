public record Entry(decimal amount, DateOnly date, string? description);

public class BookKeeper 
{
    string title;
    DateOnly startDate;
    DateOnly endDate;
    Entry[] entries;
    
    public BookKeeper(string title, DateOnly startDate, DateOnly endDate, IEnumerable<Entry> entries) 
    {
        this.title = title;
        this.startDate = startDate;
        this.endDate = endDate;
        this.entries = entries.ToArray();
    }
    
    public void Print() 
    {
        PrintTitle();
        PrintPeriod();
        PrintEntries();
        PrintTotal();
    }

    void PrintTitle() 
    {
        Console.WriteLine();
        Console.WriteLine(this.title);
        Console.WriteLine();
    }

    void PrintPeriod() 
    {
        Console.WriteLine($"From {startDate} to {endDate}");
    }

    void PrintEntries() 
    {
        Console.WriteLine("----------------------------------------------------------");
        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry.date}\t$ {entry.amount}\t{entry.description}");
        }
        Console.WriteLine("----------------------------------------------------------");
    }

    void PrintTotal()
    {
        var total = entries.Sum(entry => entry.amount);
        Console.WriteLine($"Total\t$ {total}");
    }
}