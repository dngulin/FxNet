using FxNet.Math;

namespace FxNet.Collision2D.Internal {
  internal unsafe ref struct Simplex2D {
    public const int Capacity = 3;

    private readonly FxVec2* _points;
    private int _count;

    public Simplex2D(FxVec2* points) {
      _points = points;
      _count = 0;
    }

    public bool IsFull => _count >= Capacity;

    public void PushPoint(in FxVec2 point) => _points[_count++] = point;

    public void RemovePoint(int index) {
      _count--;

      switch (index) {
        case 0:
          _points[0] = _points[1];
          _points[1] = _points[2];
          break;
        case 1:
          _points[1] = _points[2];
          break;
      }
    }

    public FxVec2 GetDirectionToOrigin() {
      if (_count == 1)
        return -_points[0];

      var a = _points[1];
      var b = _points[0];

      var ab = b - a;
      var ao = -a;

      var direction = TripleProduct(ab, ao, ab); // perpendicular to AB towards origin
      if (direction == FxVec2.Zero)
        direction = new FxVec2(ab.Y, -ab.X);

      return direction;
    }

    public bool CheckForOrigin(out FxVec2 nextDirection, out int pointToRemoveIndex) {
      var a = _points[2];
      var b = _points[1];
      var c = _points[0];

      var ao = -a;
      var ab = b - a;
      var ac = c - a;

      pointToRemoveIndex = 1;
      nextDirection = TripleProduct(ab, ac, ac);
      if (FxVec2.Dot(nextDirection, ao) >= 0)
        return false;

      pointToRemoveIndex = 0;
      nextDirection = TripleProduct(ac, ab, ab);
      return FxVec2.Dot(nextDirection, ao) < 0;
    }

    // Simplified version of the FxVec3.Cross(FxVec3.Cross(a, b), c) where each z = 0
    private static FxVec2 TripleProduct(in FxVec2 a, in FxVec2 b, in FxVec2 c) {
      var z0 = a.X * b.Y - a.Y * b.X;
      var x1 = z0 * c.Y;
      var y1 = z0 * c.X;
      return new FxVec2(-x1, y1);
    }
  }
}