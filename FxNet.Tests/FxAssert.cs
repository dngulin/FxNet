using FxNet.Math;
using Xunit.Sdk;

namespace FxNet.Tests {
  public static class FxAssert {
    public static void Equal(in FxNum expected, in FxNum actual, in FxNum threshold) {
      if (FxMath.Abs(expected - actual) > threshold)
        throw new EqualException((double) expected, (double) actual);
    }
  }
}