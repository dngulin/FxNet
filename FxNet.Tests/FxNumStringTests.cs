using FxNet.Math;
using Xunit;

namespace FxNet.Tests {
  public class FxNumStringTests {
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(18436)]
    [InlineData(-12345)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void TestIntegerToString(int num) {
      Assert.Equal(num.ToString(), FxNum.FromInt(num).ToStr());
    }

    [Theory]
    [InlineData(1, 2, "0.5")]
    [InlineData(1, 3, "0.33333")]
    [InlineData(2, 3, "0.66666")]
    [InlineData(-22, 7, "-3.14285")]
    public void TestFractionalToString(int numerator, int denominator, string expected) {
      var num = FxNum.FromInt(numerator) / denominator;
      Assert.Equal(expected, num.ToStr());
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(1, 1000)]
    [InlineData(22, 7)]
    [InlineData(3, 7)]
    [InlineData(128, 233)]
    public void TestToStringAndParse(int numerator, int denominator) {
      var expected = FxNum.FromInt(numerator) / denominator;
      var actual = FxNum.Parse(expected.ToStr());
      FxAssert.Equal(expected, actual, FxNum.FromMillis(1));
    }
  }
}