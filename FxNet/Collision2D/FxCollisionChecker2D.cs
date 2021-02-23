using FxNet.Collision2D.Shapes;
using FxNet.Math;

namespace FxNet.Collision2D {
  public class FxCollisionChecker2D :
    ISupport2D<FxCircle>,
    ISupport2D<FxCapsule>,
    ISupport2D<FxPoly3>,
    ISupport2D<FxPoly4> {
    public FxVec2 GetMaxInDirection(in FxCircle shape, in FxVec2 direction) {
      return shape.Center + direction.Normalized() * shape.Radius;
    }

    public FxVec2 GetMaxInDirection(in FxCapsule shape, in FxVec2 direction) {
      var norm = direction.Normalized();

      var a = shape.Start + norm * shape.Radius;
      var b = shape.End + norm * shape.Radius;

      var dotA = FxVec2.Dot(a, norm);
      var dotB = FxVec2.Dot(b, norm);

      return dotA > dotB ? a : b;
    }

    public unsafe FxVec2 GetMaxInDirection(in FxPoly3 shape, in FxVec2 direction) {
      fixed (void* points = &shape) {
        return GetMaxInDirection((FxVec2*) points, 3, direction);
      }
    }

    public unsafe FxVec2 GetMaxInDirection(in FxPoly4 shape, in FxVec2 direction) {
      fixed (void* points = &shape) {
        return GetMaxInDirection((FxVec2*) points, 4, direction);
      }
    }

    private static unsafe FxVec2 GetMaxInDirection(FxVec2* points, int pointsCount, in FxVec2 direction) {
      var index = 0;
      var maxDot = FxNum.FromRaw(long.MinValue);

      for (var i = 0; i < pointsCount; i++) {
        var dot = FxVec2.Dot(direction, points[i]);
        if (dot <= maxDot) continue;

        maxDot = dot;
        index = i;
      }

      return points[index];
    }
  }
}