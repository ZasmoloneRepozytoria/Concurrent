using NUnit.Framework;

namespace Concurrent.Tests;

[TestFixture]
[TestOf(typeof(Math))]
public class MathTest
{

    [Test]
    public void TestAddTwoNumbers()
    {
        var result = Math.AddTwoNumbers(1, 2);
        Assert.That(result, Is.EqualTo(3));
    }
}