using FxNet.Collision2D.Internal;
using FxNet.Math;

namespace FxNet.Collision2D {
  public interface ISupport2D<T> {
    FxVec2 GetMaxInDirection(in T shape, in FxVec2 direction);
  }

  public static class FxGjk2D {
    private const int IterationsLimit = 100;

    public static unsafe bool Check<TSup, TShape1, TShape2>(this TSup support, in TShape1 a, in TShape2 b, FxVec2 dir)
      where TSup : ISupport2D<TShape1>, ISupport2D<TShape2> {
      var points = stackalloc FxVec2[Simplex2D.Capacity];
      var simplex = new Simplex2D(points);

      var iterations = 0;
      var zero = (FxNum) 0;

      while (true) {
        var csoPoint = support.GetMaxInDirection(a, dir) - support.GetMaxInDirection(b, -dir);
        simplex.PushPoint(csoPoint);

        if (FxVec2.Dot(csoPoint, dir) <= zero)
          return false;

        if (simplex.IsFull) {
          if (simplex.CheckForOrigin(out dir, out var pointToRemoveIndex))
            return true;

          simplex.RemovePoint(pointToRemoveIndex);
        }
        else {
          dir = simplex.GetDirectionToOrigin();
        }

        iterations++;
        if (iterations >= IterationsLimit)
          break;
      }

      return false;
    }
  }
}