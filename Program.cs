var bookKeeper = BookKeeper.CreateBuilder()
    .From(2022, 1, 1)
    .To(2022, 1, 7)
    .WithTitle("My Journals")
    .WithTitle(oldTitle => oldTitle.ToUpperInvariant())
    .WithEntry(200m, new DateOnly(2022, 1, 1), "Found on road")
    .WithEntry(-20.5m, new DateOnly(2022, 1, 2), "Lunch")
    .WithEntries(new Entry[] 
    {
        new Entry(-4m, new DateOnly(2022, 1, 3), "Coffee"),
        new Entry(-4m, new DateOnly(2022, 1, 4), "Coffee"),
        new Entry(-4m, new DateOnly(2022, 1, 5), "Coffee"),
    })
    .WithEntry(-100m, new DateOnly(2022, 1, 6), "Lost")
    .WithEntry(-20m, new DateOnly(2022, 1, 7), "Gave to charity")
    .WithEntryFormatter((entry, olderFormatter) => 
    {
        Console.ForegroundColor = entry.amount >= 0 ? ConsoleColor.Green : ConsoleColor.Yellow;
        olderFormatter(entry);
        Console.ResetColor();
    })
    .Build();

bookKeeper.Print();