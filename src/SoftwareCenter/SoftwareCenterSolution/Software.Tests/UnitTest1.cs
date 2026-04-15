namespace Software.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
        int a = 10; int b = 20;
        int answer = a + b;
        Assert.Equal(30, answer);
    }
}