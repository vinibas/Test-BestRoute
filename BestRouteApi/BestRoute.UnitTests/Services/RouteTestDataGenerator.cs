namespace BestRoute.UnitTests.Services;

public static class RouteTestDataGenerator
{
    private static readonly RouteTestData documentationExample = new ("GRU", "CDG",
        [
            new("GRU", "BRC", 10),
            new("BRC", "SCL", 5),
            new("GRU", "CDG", 75),
            new("GRU", "SCL", 20),
            new("GRU", "ORL", 56),
            new("ORL", "CDG", 5),
            new("SCL", "ORL", 20),
        ], ["GRU", "BRC", "SCL", "ORL", "CDG"], 40);

    private static readonly RouteTestData[] additionalExamples = [
        new ("GRU", "BRC", [ new("GRU", "BRC", 10) ], [ "GRU", "BRC" ], 10),
        new ("GRU", "ORL", [ new("GRU", "BRC", 10), new ("BRC", "ORL", 7) ], [ "GRU", "BRC", "ORL" ], 17),
        new ("ORL", "CDG",
            [
                new("GRU", "BRC", 10),
                new("BRC", "SCL", 5),
                new("GRU", "CDG", 75),
                new("GRU", "SCL", 20),
                new("GRU", "ORL", 56),
                new("ORL", "CDG", 5),
                new("SCL", "ORL", 20),
            ], ["ORL", "CDG"], 5),
        new ("GRU", "SCL",
            [
                new("GRU", "BRC", 10),
                new("BRC", "SCL", 5),
                new("GRU", "CDG", 75),
                new("GRU", "SCL", 20),
                new("GRU", "ORL", 56),
                new("ORL", "CDG", 5),
                new("SCL", "ORL", 20),
            ], ["GRU", "BRC", "SCL"], 15)
    ];


    public static IEnumerable<object[]> GetRouteTestData()
    {
        yield return new object[] { documentationExample };
        foreach (var example in additionalExamples)
            yield return new object[] { example };
    }
}