using FxNet.Collision2D.Internal;
using FxNet.Math;

namespace FxNet.Collision2D {
  public static class FxCollisionChecker2D {
    private const int IterationsLimit = 16;

    public static unsafe bool Check<T1, T2>(in T1 a, in T2 b, FxVec2 dir) where T1: IShape2D where T2 : IShape2D {
      var points = stackalloc FxVec2[Simplex2D.Capacity];
      var simplex = new Simplex2D(points);

      for (var i = 0; i < IterationsLimit; i++) {
        var csoPoint = a.GetMaxInDirection(dir) - b.GetMaxInDirection(-dir);
        simplex.PushPoint(csoPoint);

        if (FxVec2.Dot(csoPoint, dir).Raw <= 0)
          return false;

        if (simplex.IsFull) {
          if (simplex.CheckForOrigin(out dir, out var pointToRemoveIndex))
            return true;

          simplex.RemovePoint(pointToRemoveIndex);
        }
        else {
          dir = simplex.GetDirectionToOrigin();
        }
      }

      return false;
    }

    public static FxVec2 GetPenetration<T1, T2>(in T1 a, in T2 b, FxVec2 dir) where T1: IShape2D where T2 : IShape2D {
      var normalizedDir = dir.Normalized();
      var result = a.GetMaxInDirection(normalizedDir) - b.GetMaxInDirection(-normalizedDir);

      var length = FxVec2.Dot(result, normalizedDir);
      return normalizedDir * length;
    }
  }
}