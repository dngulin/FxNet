using System.Runtime.InteropServices;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxVec2 {
    public FxNum X;
    public FxNum Y;

    public static FxVec2 Zero => new FxVec2(0, 0);
    public static FxVec2 One => new FxVec2(1, 1);
    public static FxVec2 Up => new FxVec2(0, 1);
    public static FxVec2 Down => new FxVec2(0, -1);
    public static FxVec2 Left => new FxVec2(-1, 0);
    public static FxVec2 Right => new FxVec2(1, 0);

    public FxVec2(in FxNum x, in FxNum y) {
      X = x;
      Y = y;
    }

    public static FxVec2 operator -(in FxVec2 v) => new FxVec2(-v.X, -v.Y);

    public static FxVec2 operator +(in FxVec2 l, in FxVec2 r) => new FxVec2(l.X + r.X, l.Y + r.Y);
    public static FxVec2 operator -(in FxVec2 l, in FxVec2 r) => new FxVec2(l.X - r.X, l.Y - r.Y);

    public static FxVec2 operator *(in FxVec2 v, in FxNum n) => new FxVec2(v.X * n, v.Y * n);
    public static FxVec2 operator *(in FxNum n, in FxVec2 v) => new FxVec2(n * v.X, n * v.Y);

    public static FxVec2 operator *(in FxVec2 v, int n) => new FxVec2(v.X * n, v.Y * n);
    public static FxVec2 operator *(int n, in FxVec2 v) => new FxVec2(n * v.X, n * v.Y);

    public static FxVec2 operator *(in FxVec2 v, in long n) => new FxVec2(v.X * n, v.Y * n);
    public static FxVec2 operator *(in long n, in FxVec2 v) => new FxVec2(n * v.X, n * v.Y);

    public static FxVec2 operator /(in FxVec2 v, in FxNum n) => new FxVec2(v.X / n, v.Y / n);
    public static FxVec2 operator /(in FxVec2 v, int n) => new FxVec2(v.X / n, v.Y / n);
    public static FxVec2 operator /(in FxVec2 v, in long n) => new FxVec2(v.X / n, v.Y / n);

    public static FxVec2 operator >>(in FxVec2 v, int n) => new FxVec2(v.X >> n, v.Y >> n);
    public static FxVec2 operator <<(in FxVec2 v, int n) => new FxVec2(v.X << n, v.Y << n);

    public static bool operator ==(in FxVec2 l, in FxVec2 r) => l.X == r.X && l.Y == r.Y;
    public static bool operator !=(in FxVec2 l, in FxVec2 r) => l.X != r.X && l.Y != r.Y;

    public override bool Equals(object obj) => obj is FxVec2 other && this == other;
    public override int GetHashCode() => throw new System.NotSupportedException();

    public static FxVec2 Min(in FxVec2 a, in FxVec2 b) => new FxVec2(FxMath.Min(a.X, b.X), FxMath.Min(a.Y, b.Y));
    public static FxVec2 Max(in FxVec2 a, in FxVec2 b) => new FxVec2(FxMath.Max(a.X, b.X), FxMath.Max(a.Y, b.Y));

    public static FxNum Dot(FxVec2 a, FxVec2 b) => a.X * b.X + a.Y * b.Y;

    public static FxVec2 ClampMagnitude(in FxVec2 vector, in FxNum maxMagnitude) {
      var sqrMag = vector.SqrMagnitude();

      if (sqrMag > maxMagnitude * maxMagnitude) {
        var mag = FxMath.Sqrt(sqrMag);
        return vector / mag * maxMagnitude;
      }

      return vector;
    }
  }

  public static class FxVec2Extensions {
    public static FxNum SqrMagnitude(this in FxVec2 v) => v.X * v.X + v.Y * v.Y;

    public static FxNum Magnitude(this in FxVec2 v) => FxMath.Sqrt(v.SqrMagnitude());

    public static FxVec2 Normalized(this in FxVec2 v) {
      var m = v.Magnitude();

      if (m.Raw == 0)
        return FxVec2.Zero;

      return v / m;
    }

    public static FxVec2 Clamped(this in FxVec2 v, in FxNum mag) => FxVec2.ClampMagnitude(v, mag);
  }
}