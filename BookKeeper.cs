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

    public BookKeeper(BookKeeperContext context) : this(context.Configuration.Title, context.Configuration.StartDate, context.Configuration.EndDate, context.Entries, context.Configuration.EntryFormatter) { }

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

public class BookKeeperConfiguration
{
    public string Title { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Action<Entry> EntryFormatter { get; set; }
    public BookKeeperContext Context { get; set; }

    public BookKeeperConfiguration(string title, DateOnly startDate, DateOnly endDate, Action<Entry> entryFormatter, BookKeeperContext context)
    {
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        EntryFormatter = entryFormatter;
        Context = context;
    }
}

public class BookKeeperConfigurationBuilder
{
    string title = "Untitled";
    DateOnly startDate;
    DateOnly endDate;
    Action<Entry> entryFormatter = entry => Console.WriteLine($"{entry.date}\t$ {entry.amount}\t{entry.description}");
    BookKeeperContext context;

    public BookKeeperConfigurationBuilder(BookKeeperContext context)
    {
        this.context = context;
    }

    public BookKeeperConfigurationBuilder WithTitle(string title)
    {
        this.title = title;
        return this;
    }

    public BookKeeperConfigurationBuilder WithTitle(Func<string, string> titleProvider)
    {
        this.title = titleProvider(this.title);
        return this;
    }

    public BookKeeperConfigurationBuilder WithTitle(Func<string, BookKeeperContext, string> titleProvider)
    {
        this.title = titleProvider(this.title, context);
        return this;
    }

    public BookKeeperConfigurationBuilder From(DateOnly date)
    {
        this.startDate = date;
        return this;
    }

    public BookKeeperConfigurationBuilder From(int year, int month, int day) => From(new DateOnly(year, month, day));

    public BookKeeperConfigurationBuilder To(DateOnly date)
    {
        this.endDate = date;
        return this;
    }

    public BookKeeperConfigurationBuilder To(int year, int month, int day) => To(new DateOnly(year, month, day));

    public BookKeeperConfigurationBuilder WithEntryFormatter(Action<Entry> entryFormatter) 
    {
        this.entryFormatter = entryFormatter;
        return this;
    }

    public BookKeeperConfigurationBuilder WithEntryFormatter(Action<Entry, Action<Entry>> entryFormatter)
    {
        var oldFormatter = this.entryFormatter;
        this.entryFormatter = entry => entryFormatter(entry, oldFormatter);
        return this;
    }

    public BookKeeperConfiguration Build() => new BookKeeperConfiguration(title, startDate, endDate, entryFormatter, context);
}

public class BookKeeperContext
{
    public List<Entry> Entries { get; set;} = new List<Entry>();
    public BookKeeperConfiguration Configuration { get; set; }
    public BookKeeperContext() => this.Configuration = new BookKeeperConfigurationBuilder(this).Build();
}

public class BookKeeperBuilder
{
    BookKeeperContext context = new BookKeeperContext();

    public BookKeeperBuilder Configure(Action<BookKeeperConfigurationBuilder> configuration)
    {
        var configurationBuilder = new BookKeeperConfigurationBuilder(context);
        configuration(configurationBuilder);
        context.Configuration = configurationBuilder.Build();
        return this;
    }

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
    
    public BookKeeper Build() 
    {
        return new BookKeeper(context);
    }
}