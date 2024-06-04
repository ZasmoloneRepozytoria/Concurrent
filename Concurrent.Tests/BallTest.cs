using Concurrent.Data;
using NUnit.Framework;
namespace Concurrent.Tests;

[TestFixture]
public class BallTest
{

    [Test]
    public void TestBallConstructor()
    {
        Ball ball = new Ball(3.5, 4.6, 1.2);

        Assert.Equals(ball.PositionX, 3.5);
        Assert.Equals(ball.PositionY, 4.6);
        Assert.Equals(ball.Radius, 5);
    }
}