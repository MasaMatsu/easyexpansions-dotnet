namespace EExpansions;

public class EnumerableExtensionsTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AllAsync(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .Where(i => i % 2 == 0)
            .AllAsync(sync, i => Task.FromResult(i % 2 == 0));
        var expected =
            Enumerable.Range(1, 10)
            .Where(i => i % 2 == 0)
            .All(i => i % 2 == 0);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AnyAsync()
    {
        var actual =
            await Enumerable.Range(1, 10)
            .AnyAsync(i => Task.FromResult(i % 2 == 0));
        var expected =
            Enumerable.Range(1, 10)
            .Any(i => i % 2 == 0);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CountAsync(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .CountAsync(sync, i => Task.FromResult(i % 2 == 0));
        var expected =
            Enumerable.Range(1, 10)
            .Count(i => i % 2 == 0);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task LongCountAsync(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .LongCountAsync(sync, i => Task.FromResult(i % 2 == 0));
        var expected =
            Enumerable.Range(1, 10)
            .LongCount(i => i % 2 == 0);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(11, 10, false)]
    public async Task FirstAsync(int value, int max, bool success)
    {
        if (success)
        {
            var actual =
                await Enumerable.Range(1, max)
                .FirstAsync(i => Task.FromResult(i == value));
            var expected =
                Enumerable.Range(1, max)
                .First(i => i == value);
            Assert.Equal(expected, actual);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await Enumerable.Range(1, max)
                .FirstAsync(i => Task.FromResult(i == value))
            );
        }
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(11, 10)]
    public async Task FirstOrDefaultAsync(int value, int max)
    {
        var actual =
            await Enumerable.Range(1, max)
            .Select(i => i as int?)
            .FirstOrDefaultAsync(i => Task.FromResult(i == value));
        var expected =
            Enumerable.Range(1, max)
            .Select(i => i as int?)
            .FirstOrDefault(i => i == value);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(11, 10)]
    public async Task FirstOrDefaultAsync_WithDefaultValue(int value, int max)
    {
        var actual =
            await Enumerable.Range(1, max)
            .Select(i => i as int?)
            .FirstOrDefaultAsync(i => Task.FromResult(i == value), -1);
        var expected =
            Enumerable.Range(1, max)
            .Select(i => i as int?)
            .FirstOrDefault(i => i == value, -1);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(11, 10, false)]
    public async Task LastAsync(int value, int max, bool success)
    {
        if (success)
        {
            var actual =
                await Enumerable.Range(1, max)
                .LastAsync(i => Task.FromResult(i == value));
            var expected =
                Enumerable.Range(1, max)
                .Last(i => i == value);
            Assert.Equal(expected, actual);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await Enumerable.Range(1, max)
                .LastAsync(i => Task.FromResult(i == value))
            );
        }
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(11, 10)]
    public async Task LastOrDefaultAsync(int value, int max)
    {
        var actual =
            await Enumerable.Range(1, max)
            .Select(i => i as int?)
            .LastOrDefaultAsync(i => Task.FromResult(i == value));
        var expected =
            Enumerable.Range(1, max)
            .Select(i => i as int?)
            .LastOrDefault(i => i == value);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(11, 10)]
    public async Task LastOrDefaultAsync_WithDefaultValue(int value, int max)
    {
        var actual =
            await Enumerable.Range(1, max)
            .Select(i => i as int?)
            .LastOrDefaultAsync(i => Task.FromResult(i == value), -1);
        var expected =
            Enumerable.Range(1, max)
            .Select(i => i as int?)
            .LastOrDefault(i => i == value, -1);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(6, 10, true)]
    [InlineData(11, 10, false)]
    [InlineData(2, 10, false)]
    public async Task SingleAsync(int value, int max, bool success)
    {
        if (success)
        {
            var actual =
                await Enumerable.Range(1, max)
                .SingleAsync(i => Task.FromResult(i % value == 0));
            var expected =
                Enumerable.Range(1, max)
                .Single(i => i % value == 0);
            Assert.Equal(expected, actual);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await Enumerable.Range(1, max)
                .SingleAsync(i => Task.FromResult(i % value == 0))
            );
        }
    }

    [Theory]
    [InlineData(6, 10, true)]
    [InlineData(11, 10, true)]
    [InlineData(2, 10, false)]
    public async Task SingleOrDefaultAsync(int value, int max, bool success)
    {
        if (success)
        {
            var actual =
                await Enumerable.Range(1, max)
                .Select(i => i as int?)
                .SingleOrDefaultAsync(i => Task.FromResult(i % value == 0));
            var expected =
                Enumerable.Range(1, max)
                .Select(i => i as int?)
                .SingleOrDefault(i => i % value == 0);
            Assert.Equal(expected, actual);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await Enumerable.Range(1, max)
                .Select(i => i as int?)
                .SingleOrDefaultAsync(i => Task.FromResult(i % value == 0))
            );
        }
    }

    [Theory]
    [InlineData(6, 10, true)]
    [InlineData(11, 10, true)]
    [InlineData(2, 10, false)]
    public async Task SingleOrDefaultAsync_WithDefaultValue(int value, int max, bool success)
    {
        if (success)
        {
            var actual =
                await Enumerable.Range(1, max)
                .Select(i => i as int?)
                .SingleOrDefaultAsync(i => Task.FromResult(i % value == 0), -1);
            var expected =
                Enumerable.Range(1, max)
                .Select(i => i as int?)
                .SingleOrDefault(i => i % value == 0, -1);
            Assert.Equal(expected, actual);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await Enumerable.Range(1, max)
                .Select(i => i as int?)
                .SingleOrDefaultAsync(i => Task.FromResult(i % value == 0), -1)
            );
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SkipWhileAsync(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .SkipWhileAsync(sync, i => Task.FromResult(i == 5));
        var expected =
            Enumerable.Range(1, 10)
            .SkipWhile(i => i == 5);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SkipWhileAsync_WithIndex(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .Select(i => i.ToString())
            .SkipWhileAsync(sync, (_, i) => Task.FromResult(i == 5));
        var expected =
            Enumerable.Range(1, 10)
            .Select(i => i.ToString())
            .SkipWhile((_, i) => i == 5);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task TakeWhileAsync(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .TakeWhileAsync(sync, i => Task.FromResult(i == 5));
        var expected =
            Enumerable.Range(1, 10)
            .TakeWhile(i => i == 5);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task TakeWhileAsync_WithIndex(bool sync)
    {
        var actual =
            await Enumerable.Range(1, 10)
            .Select(i => i.ToString())
            .TakeWhileAsync(sync, (_, i) => Task.FromResult(i == 5));
        var expected =
            Enumerable.Range(1, 10)
            .Select(i => i.ToString())
            .TakeWhile((_, i) => i == 5);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task WhereAsync(bool sync)
    {
        var random = new Random();
        var actual =
            await Enumerable.Range(1, 10)
            .WhereAsync(sync, async i =>
            {
                await Task.Delay(random.Next(1, 500));
                return i % 2 == 0;
            });
        var expected =
            Enumerable.Range(1, 10)
            .Where(i => i % 2 == 0);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task WhereAsync_WithIndex(bool sync)
    {
        var random = new Random();
        var actual =
            await Enumerable.Range(1, 10)
            .Select(i => i.ToString())
            .WhereAsync(sync, async (_, i) =>
            {
                await Task.Delay(random.Next(1, 500));
                return i % 2 == 0;
            });
        var expected =
            Enumerable.Range(1, 10)
            .Select(i => i.ToString())
            .Where((_, i) => i % 2 == 0);
        Assert.Equal(expected, actual);
    }
}
