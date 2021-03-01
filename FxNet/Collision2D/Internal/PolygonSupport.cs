using FxNet.Math;

namespace FxNet.Collision2D.Internal {
  internal static class PolygonSupport {
    public static unsafe FxVec2 GetMaxInDirection(FxVec2* points, int pointsCount, in FxVec2 direction) {
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