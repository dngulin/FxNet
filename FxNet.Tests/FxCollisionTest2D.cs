using System.Collections.Generic;
using FxNet.Collision2D;
using FxNet.Collision2D.Shapes;
using FxNet.Math;
using Xunit;

namespace FxNet.Tests {
  public class FxCollisionTest2D {
    private readonly FxCollisionChecker2D _collisionChecker = new FxCollisionChecker2D();

    public static IEnumerable<object[]> GetCapsuleToCapsuleData() {
      yield return new object[] { new FxVec2(-1, -1),  new FxVec2(1, 1), new FxVec2(-1, 1), new FxVec2(1, -1), true };
      yield return new object[] { new FxVec2(-2, -2),  new FxVec2(-2, 2), new FxVec2(2, -2), new FxVec2(2, 2), false };
      yield return new object[] { new FxVec2(-10, 1),  new FxVec2(10, 1), new FxVec2(0, -2), new FxVec2(0, 1), true };
      yield return new object[] { new FxVec2(-10, 3),  new FxVec2(10, 3), new FxVec2(0, -2), new FxVec2(0, 0), false };
      yield return new object[] { new FxVec2(0, 0),  new FxVec2(-3, 7), new FxVec2(-1, 0), new FxVec2(2, 4), true };
    }

    [Theory]
    [MemberData(nameof(GetCapsuleToCapsuleData))]
    public void TestCapsuleToCapsuleCollision(FxVec2 a1, FxVec2 a2, FxVec2 b1, FxVec2 b2, bool expected) {
      var a = new FxCapsule(a1, a2, 1);
      var b = new FxCapsule(b1, b2, 1);

      Assert.Equal(expected, _collisionChecker.Check(a, b, FxVec2.Up));
      Assert.Equal(expected, _collisionChecker.Check(b, a, FxVec2.Up));
    }
  }
}