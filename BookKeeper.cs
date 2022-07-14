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

    public BookKeeper(BookKeeperContext context) : this(context.Title, context.StartDate, context.EndDate, context.Entries, context.EntryFormatter) { }

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

public class BookKeeperContext
{
    public string Title { get; set; } = "Untitled";
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<Entry> Entries { get; set;} = new List<Entry>();
    public Action<Entry> EntryFormatter { get; set; } = entry => Console.WriteLine($"{entry.date}\t$ {entry.amount}\t{entry.description}");
}

public class BookKeeperBuilder
{
    BookKeeperContext context = new BookKeeperContext();

    public BookKeeperBuilder WithTitle(string title)
    {
        context.Title = title;
        return this;
    }

    public BookKeeperBuilder WithTitle(Func<string, string> titleProvider)
    {
        context.Title = titleProvider(context.Title);
        return this;
    }

    public BookKeeperBuilder WithTitle(Func<string, BookKeeperContext, string> titleProvider)
    {
        context.Title = titleProvider(context.Title, context);
        return this;
    }

    public BookKeeperBuilder From(DateOnly date)
    {
        context.StartDate = date;
        return this;
    }

    public BookKeeperBuilder From(int year, int month, int day) => From(new DateOnly(year, month, day));

    public BookKeeperBuilder To(DateOnly date)
    {
        context.EndDate = date;
        return this;
    }

    public BookKeeperBuilder To(int year, int month, int day) => To(new DateOnly(year, month, day));

    public BookKeeperBuilder WithEntries(IEnumerable<Entry> entries) 
    {
        context.Entries.AddRange(entries);
        return this;
    }

    public BookKeeperBuilder WithEntry(Entry entry) 
    {
        context.Entries.Add(entry);
        return this;
    }

    public BookKeeperBuilder WithEntry(decimal amount, DateOnly date, string? description) => WithEntry(new Entry(amount, date, description));
    
    public BookKeeperBuilder WithEntryFormatter(Action<Entry> entryFormatter) 
    {
        context.EntryFormatter = entryFormatter;
        return this;
    }

    public BookKeeperBuilder WithEntryFormatter(Action<Entry, Action<Entry>> entryFormatter)
    {
        var oldFormatter = context.EntryFormatter;
        context.EntryFormatter = entry => entryFormatter(entry, oldFormatter);
        return this;
    }

    public BookKeeper Build() 
    {
        return new BookKeeper(context);
    }
}