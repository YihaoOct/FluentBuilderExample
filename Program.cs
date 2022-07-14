var bookKeeper = new BookKeeper(
    "My Journals",
    new DateOnly(2022, 1, 1),
    new DateOnly(2022, 1, 7),
    new Entry[] 
    {
        new Entry(200m, new DateOnly(2022, 1, 1), "Found on road"),
        new Entry(-20.5m, new DateOnly(2022, 1, 2), "Lunch"),
        new Entry(-4m, new DateOnly(2022, 1, 3), "Coffee"),
        new Entry(-4m, new DateOnly(2022, 1, 4), "Coffee"),
        new Entry(-4m, new DateOnly(2022, 1, 5), "Coffee"),
        new Entry(-100m, new DateOnly(2022, 1, 6), "Lost"),
        new Entry(-20m, new DateOnly(2022, 1, 7), "Gave to charity"),
    }
);

bookKeeper.Print();