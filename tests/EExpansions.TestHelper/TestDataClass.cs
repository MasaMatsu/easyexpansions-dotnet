﻿using System.Collections;

namespace EExpansions.TestHelper;

public abstract class TestDataClass : IEnumerable<object?[]>
{
    protected IList<object?[]> TestData = [];

    public TestDataClass()
    {
        Initialize();
    }

    protected abstract void Initialize();

    public IEnumerator<object?[]> GetEnumerator() => TestData.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
