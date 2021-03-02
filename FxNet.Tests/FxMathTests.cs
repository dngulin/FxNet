using FxNet.Math;
using Xunit;

namespace FxNet.Tests {
  public class FxMathTests {
    [Theory]
    [InlineData(0, 1)]
    [InlineData(0, 4)]
    [InlineData(0, 50)]
    [InlineData(50, 500)]
    [InlineData(100, 1000)]
    [InlineData(8000, 10000)]
    public void TestSqrt(int from, int to) {
      const int steps = 1000;
      var precision = FxNum.FromRatio(1, 1000);

      var min = (FxNum) from;
      var max = (FxNum) to;
      var step = (max - min) / steps;

      for (var i = 0; i <= steps; i++) {
        var value = min + i * step;
        var sqrt = FxMath.Sqrt(value);
        FxAssert.Equal(value, sqrt * sqrt, precision);
      }
    }

    [Fact]
    public void TestSinCosSquaresSum() {
      const int steps = 10000;
      var step = 360 * FxMath.Deg2Rad / steps;
      var precision = FxNum.FromRatio(1, 10000);

      for (var i = 0; i <= steps; i++) {
        var angle = i * step;
        var sin = FxMath.Sin(angle);
        var cos = FxMath.Cos(angle);

        // sin(a)^2 + cos(a)^2 == 1
        FxAssert.Equal(1, sin * sin + cos * cos, precision);
      }
    }

    [Fact]
    public void TestSinDualAngle() {
      const int steps = 10000;
      var step = 360 * FxMath.Deg2Rad / steps;
      var precision = FxNum.FromRatio(5, 10000);

      for (var i = 0; i <= steps; i++) {
        var angle = i * step;
        var sin = FxMath.Sin(angle);
        var cos = FxMath.Cos(angle);

        // sin(2a) == 2 * sin(a) * cos(a)
        FxAssert.Equal(FxMath.Sin(angle * 2), 2 * sin * cos, precision);
      }
    }

    [Fact]
    public void TestCosDualAngle() {
      const int steps = 10000;
      var step = 360 * FxMath.Deg2Rad / steps;
      var precision = FxNum.FromRatio(5, 10000);

      for (var i = 0; i <= steps; i++) {
        var angle = i * step;
        var sin = FxMath.Sin(angle);
        var cos = FxMath.Cos(angle);

        // cos(2a) == cos(a)^2 * sin(a)^2
        FxAssert.Equal(FxMath.Cos(angle * 2), cos * cos - sin * sin, precision);

        // cos(2a) == 2 * cos(a)^2 - 1
        FxAssert.Equal(FxMath.Cos(angle * 2), 2 * cos * cos - 1, precision);
      }
    }

    [Fact]
    public void TestAsin() {
      const int steps = 10000;
      var precision = FxNum.FromRatio(5, 10000);

      var min = (FxNum) (-1);
      var max = (FxNum) 1;
      var step = (max - min) / steps;

      for (var i = 0; i <= steps; i++) {
        var value = min + i * step;
        FxAssert.Equal(value, FxMath.Sin(FxMath.Asin(value)), precision);
      }
    }

    [Fact]
    public void TestAcos() {
      const int steps = 10000;
      var precision = FxNum.FromRatio(5, 10000);

      var min = (FxNum) (-1);
      var max = (FxNum) 1;
      var step = (max - min) / steps;

      for (var i = 0; i <= steps; i++) {
        var value = min + i * step;
        FxAssert.Equal(value, FxMath.Cos(FxMath.Acos(value)), precision);
      }
    }
  }
}