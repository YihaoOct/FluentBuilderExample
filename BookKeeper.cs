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

public class BookKeeperBuilder
{
    string title = "Untitled";
    DateOnly startDate;
    DateOnly endDate;
    List<Entry> entries = new List<Entry>();

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
    
    public BookKeeper Build() 
    {
        return new BookKeeper(title, startDate, endDate, entries);
    }
}