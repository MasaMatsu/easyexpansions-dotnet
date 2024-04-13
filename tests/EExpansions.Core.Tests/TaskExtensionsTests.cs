namespace EExpansions;

public class TaskExtensionsTests
{
    [Fact]
    public async Task WhenAll()
    {
        static async Task func(IList<int> list, int i)
        {
            await Task.Delay(i * 100);
            list.Add(i);
        }
        var actual = new List<int>();
        var expected = new List<int>();
        var actualTasks = new List<Task>
        {
            func(actual, 1),
            func(actual, 2),
            func(actual, 3),
            func(actual, 4),
            func(actual, 5),
        };
        var expectedTasks = new List<Task>
        {
            func(expected, 1),
            func(expected, 2),
            func(expected, 3),
            func(expected, 4),
            func(expected, 5),
        };

        await actualTasks.WhenAll();
        await Task.WhenAll(expectedTasks);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task WhenAll_WithResult()
    {
        var actual =
            await Enumerable.Range(1, 5)
            .Select(i => Task.FromResult(i))
            .WhenAll();
        var expected =
            await Task.WhenAll(
                Enumerable.Range(1, 5)
                .Select(i => Task.FromResult(i))
            );
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task WhenAllSync()
    {
        static async Task func(IList<int> list, int i)
        {
            await Task.Delay(i * 100);
            list.Add(i);
        }
        var actual = new List<int>();
        var expected = new List<int>();
        var actualTasks = new List<Task>
        {
            func(actual, 1),
            func(actual, 2),
            func(actual, 3),
            func(actual, 4),
            func(actual, 5),
        };
        var expectedTasks = new List<Task>
        {
            func(expected, 1),
            func(expected, 2),
            func(expected, 3),
            func(expected, 4),
            func(expected, 5),
        };

        await actualTasks.WhenAllSync();
        await Task.WhenAll(expectedTasks);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task WhenAllSync_WithResult()
    {
        var actual =
            await Enumerable.Range(1, 5)
            .Select(i => Task.FromResult(i))
            .WhenAllSync();
        var expected =
            await Task.WhenAll(
                Enumerable.Range(1, 5)
                .Select(i => Task.FromResult(i))
            );
        Assert.Equal(expected, actual);
    }

    [Fact]
    public Task WhenAny()
    {
        // TODO: Implement.
        Assert.True(true);
        return Task.CompletedTask;
    }
}
