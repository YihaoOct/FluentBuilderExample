var bookKeeper = BookKeeper.CreateBuilder()
    .WithEntries(new Entry[]
    {
        new Entry(200m, new DateOnly(2022, 1, 1), "Found on road"),
        new Entry(-20.5m, new DateOnly(2022, 1, 2), "Lunch"),
        new Entry(-4m, new DateOnly(2022, 1, 3), "Coffee"),
        new Entry(-4m, new DateOnly(2022, 1, 4), "Coffee"),
        new Entry(-4m, new DateOnly(2022, 1, 5), "Coffee"),
        new Entry(-100m, new DateOnly(2022, 1, 6), "Lost"),
        new Entry(-20m, new DateOnly(2022, 1, 7), "Gave to charity"),
    })
    .Configure(builder => 
    {
        builder.From(2022, 1, 1)
            .To(2022, 1, 7)
            .WithTitle("My Journals")
            .WithTitle((oldTitle, context) => 
            {
                var total = context.Entries.Sum(entry => entry.amount);
                return $"{oldTitle.ToUpperInvariant()} ($ {total})";
            })
            .WithEntryFormatter((entry, oldFormatter) => 
            {
                Console.ForegroundColor = entry.amount >= 0 ? ConsoleColor.Green : ConsoleColor.Yellow;
                oldFormatter(entry);
                Console.ResetColor();
            });
    })
    .Build();

bookKeeper.Print();