public record Entry(decimal amount, DateOnly date, string? description);

public class BookKeeper 
{
    string title;
    DateOnly startDate;
    DateOnly endDate;
    Entry[] entries;
    Action<Entry> entryFormatter;
    
    public BookKeeper(string title, DateOnly startDate, DateOnly endDate, IEnumerable<Entry> entries, Action<Entry> entryFormatter) 
    {
        this.title = title;
        this.startDate = startDate;
        this.endDate = endDate;
        this.entries = entries.ToArray();
        this.entryFormatter = entryFormatter;
    }

    public static BookKeeperBuilder CreateBuilder() => new BookKeeperBuilder();
    
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
            entryFormatter(entry);
        }
        Console.WriteLine("----------------------------------------------------------");
    }

    void PrintTotal()
    {
        var total = entries.Sum(entry => entry.amount);
        Console.WriteLine($"Total\t$ {total}");
    }
}

public class BookKeeperBuilder
{
    string title = "Untitled";
    DateOnly startDate;
    DateOnly endDate;
    List<Entry> entries = new List<Entry>();
    Action<Entry> entryFormatter = entry => Console.WriteLine($"{entry.date}\t$ {entry.amount}\t{entry.description}");

    public BookKeeperBuilder WithTitle(string title)
    {
        this.title = title;
        return this;
    }

    public BookKeeperBuilder From(DateOnly date)
    {
        this.startDate = date;
        return this;
    }

    public BookKeeperBuilder From(int year, int month, int day) => From(new DateOnly(year, month, day));

    public BookKeeperBuilder To(DateOnly date)
    {
        this.endDate = date;
        return this;
    }

    public BookKeeperBuilder To(int year, int month, int day) => To(new DateOnly(year, month, day));

    public BookKeeperBuilder WithEntries(IEnumerable<Entry> entries) 
    {
        this.entries.AddRange(entries);
        return this;
    }

    public BookKeeperBuilder WithEntry(Entry entry) 
    {
        this.entries.Add(entry);
        return this;
    }

    public BookKeeperBuilder WithEntry(decimal amount, DateOnly date, string? description) => WithEntry(new Entry(amount, date, description));
    
    public BookKeeperBuilder WithEntryFormatter(Action<Entry> entryFormatter) 
    {
        this.entryFormatter = entryFormatter;
        return this;
    }

    public BookKeeper Build() 
    {
        return new BookKeeper(title, startDate, endDate, entries, entryFormatter);
    }
}