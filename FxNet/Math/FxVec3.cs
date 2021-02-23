using System.Runtime.InteropServices;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxVec3 {
    public FxNum X;
    public FxNum Y;
    public FxNum Z;

    public static FxVec3 Zero => new FxVec3(0, 0, 0);
    public static FxVec3 One => new FxVec3(1, 1, 1);
    public static FxVec3 Forward => new FxVec3(0, 0, 1);
    public static FxVec3 Back => new FxVec3(0, 0, -1);
    public static FxVec3 Up => new FxVec3(0, 1, 0);
    public static FxVec3 Down => new FxVec3(0, -1, 0);
    public static FxVec3 Left => new FxVec3(-1, 0, 0);
    public static FxVec3 Right => new FxVec3(1, 0, 0);

    public FxVec3(in FxNum x, in FxNum y, in FxNum z) {
      X = x;
      Y = y;
      Z = z;
    }

    public static FxVec3 operator -(in FxVec3 v) => new FxVec3(-v.X, -v.Y, -v.Z);

    public static FxVec3 operator +(in FxVec3 l, in FxVec3 r) => new FxVec3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
    public static FxVec3 operator -(in FxVec3 l, in FxVec3 r) => new FxVec3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);

    public static FxVec3 operator *(in FxVec3 v, in FxNum n) => new FxVec3(v.X * n, v.Y * n, v.Z * n);
    public static FxVec3 operator *(in FxNum n, in FxVec3 v) => new FxVec3(n * v.X, n * v.Y, n * v.Z);

    public static FxVec3 operator *(in FxVec3 v, int n) => new FxVec3(v.X * n, v.Y * n, v.Z * n);
    public static FxVec3 operator *(int n, in FxVec3 v) => new FxVec3(n * v.X, n * v.Y, n * v.Z);

    public static FxVec3 operator *(in FxVec3 v, in long n) => new FxVec3(v.X * n, v.Y * n, v.Z * n);
    public static FxVec3 operator *(in long n, in FxVec3 v) => new FxVec3(n * v.X, n * v.Y, n * v.Z);

    public static FxVec3 operator /(in FxVec3 v, in FxNum n) => new FxVec3(v.X / n, v.Y / n, v.Z / n);
    public static FxVec3 operator /(in FxVec3 v, int n) => new FxVec3(v.X / n, v.Y / n, v.Z / n);
    public static FxVec3 operator /(in FxVec3 v, in long n) => new FxVec3(v.X / n, v.Y / n, v.Z / n);

    public static FxVec3 operator >>(in FxVec3 v, int n) => new FxVec3(v.X >> n, v.Y >> n, v.Z >> n);
    public static FxVec3 operator <<(in FxVec3 v, int n) => new FxVec3(v.X << n, v.Y << n, v.Z << n);

    public static bool operator ==(in FxVec3 l, in FxVec3 r) => l.X == r.X && l.Y == r.Y && l.Z == r.Z;
    public static bool operator !=(in FxVec3 l, in FxVec3 r) => l.X != r.X && l.Y != r.Y && l.Z != r.Z;

    public override bool Equals(object obj) => obj is FxVec3 other && this == other;
    public override int GetHashCode() => throw new System.NotSupportedException();

    public static FxNum Dot(in FxVec3 a, in FxVec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static FxVec3 Cross(in FxVec3 a, in FxVec3 b) {
      return new FxVec3(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X);
    }

    public static FxVec3 Lerp(in FxVec3 a, in FxVec3 b, in FxNum t) {
      var k = FxMath.Clamp01(t);
      return new FxVec3(
        a.X + (b.X - a.X) * k,
        a.Y + (b.Y - a.Y) * k,
        a.Z + (b.Z - a.Z) * k
      );
    }

    public static FxVec3 LerpUnclamped(in FxVec3 a, in FxVec3 b, in FxNum t) {
      return new FxVec3(
        a.X + (b.X - a.X) * t,
        a.Y + (b.Y - a.Y) * t,
        a.Z + (b.Z - a.Z) * t
      );
    }

    public static FxVec3 ClampMagnitude(in FxVec3 vector, in FxNum maxMagnitude) {
      var sqrMag = vector.SqrMagnitude();

      if (sqrMag > maxMagnitude * maxMagnitude) {
        var mag = FxMath.Sqrt(sqrMag);
        return vector / mag * maxMagnitude;
      }

      return vector;
    }

    public static FxNum Angle(in FxVec3 from, in FxVec3 to) {
      var d = FxMath.Sqrt(from.SqrMagnitude() * to.SqrMagnitude());
      if (d < FxNum.FromRaw(1 << 2))
        return 0;

      var dot = FxMath.Clamp(Dot(from, to) / d, -1, 1);
      return FxMath.Acos(dot) * FxMath.Rad2Deg;
    }

    public static FxNum SignedAngle(in FxVec3 from, in FxVec3 to, in FxVec3 axis)
    {
      var angle = Angle(from, to);
      var cross = Cross(from, to);
      var sign = FxMath.Sign(Dot(axis, cross));
      return angle * sign;
    }

    public static FxVec3 Min(in FxVec3 a, in FxVec3 b) {
      return new FxVec3(FxMath.Min(a.X, b.X), FxMath.Min(a.Y, b.Y), FxMath.Min(a.Z, b.Z));
    }

    public static FxVec3 Max(in FxVec3 a, in FxVec3 b) {
      return new FxVec3(FxMath.Max(a.X, b.X), FxMath.Max(a.Y, b.Y), FxMath.Max(a.Z, b.Z));
    }

    public static FxVec3 SmoothDamp(in FxVec3 curr, in FxVec3 target, ref FxVec3 velocity, FxNum smoothTime, in FxNum maxSpeed, in FxNum dt) {
      smoothTime = FxMath.Max(smoothTime, FxNum.FromMillis(1));

      var change = ClampMagnitude(curr - target, maxSpeed * smoothTime);

      var omega = 2 / smoothTime;
      var x = omega * dt;
      var exp = 1 / (1 + x + FxNum.FromCents(48) * x * x + FxNum.FromMillis(235) * x * x * x);

      var temp = (velocity + change * omega) * dt;
      velocity = (velocity - temp * omega) * exp;

      var output = curr - change + (change + temp) * exp;

      if (Dot(target - curr, output - target) > 0) {
        output = target;
        velocity = Zero;
      }

      return output;
    }

    public static FxVec3 Reflect(in FxVec3 inDirection, in FxVec3 inNormal)
    {
      var factor = -Dot(inNormal, inDirection) << 1;
      return new FxVec3(
        factor * inNormal.X + inDirection.X,
        factor * inNormal.Y + inDirection.Y,
        factor * inNormal.Z + inDirection.Z);
    }
  }

  public static class FxVec3Extensions {
    public static FxNum SqrMagnitude(this in FxVec3 v) => v.X * v.X + v.Y * v.Y + v.Z * v.Z;

    public static FxNum Magnitude(this in FxVec3 v) => FxMath.Sqrt(v.SqrMagnitude());

    public static FxVec3 Normalized(this in FxVec3 v) {
      var m = v.Magnitude();

      if (m.Raw == 0)
        return FxVec3.Zero;

      return v / m;
    }

    public static FxVec3 Clamped(this in FxVec3 v, in FxNum mag) => FxVec3.ClampMagnitude(v, mag);
  }
}