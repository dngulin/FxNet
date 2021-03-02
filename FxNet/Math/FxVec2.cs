using System.Runtime.InteropServices;
using System.Text;

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

    public override string ToString() => this.ToStr();

    public static FxNum Dot(FxVec2 a, FxVec2 b) => a.X * b.X + a.Y * b.Y;

    public static FxVec2 Lerp(in FxVec2 a, in FxVec2 b, in FxNum t) {
      var k = FxMath.Clamp01(t);
      return new FxVec2(
        a.X + (b.X - a.X) * k,
        a.Y + (b.Y - a.Y) * k
      );
    }

    public static FxVec2 LerpUnclamped(in FxVec2 a, in FxVec2 b, in FxNum t) {
      return new FxVec2(
        a.X + (b.X - a.X) * t,
        a.Y + (b.Y - a.Y) * t
      );
    }

    public static FxVec2 ClampMagnitude(in FxVec2 vector, in FxNum maxMagnitude) {
      var sqrMag = vector.SqrMagnitude();

      if (sqrMag > maxMagnitude * maxMagnitude) {
        var mag = FxMath.Sqrt(sqrMag);
        return vector * (maxMagnitude / mag);
      }

      return vector;
    }

    public static FxNum Angle(in FxVec2 from, in FxVec2 to) {
      var d = FxMath.Sqrt(from.SqrMagnitude() * to.SqrMagnitude());
      if (d < FxNum.FromRaw(1 << 2))
        return 0;

      var dot = FxMath.Clamp(Dot(from, to) / d, -1, 1);
      return FxMath.Acos(dot) * FxMath.Rad2Deg;
    }

    public static FxNum SignedAngle(in FxVec2 from, in FxVec2 to)
    {
      var angle = Angle(from, to);
      var sign = FxMath.Sign(from.X * to.Y - from.Y * to.X);
      return angle * sign;
    }

    public static FxVec2 Min(in FxVec2 a, in FxVec2 b) => new FxVec2(FxMath.Min(a.X, b.X), FxMath.Min(a.Y, b.Y));
    public static FxVec2 Max(in FxVec2 a, in FxVec2 b) => new FxVec2(FxMath.Max(a.X, b.X), FxMath.Max(a.Y, b.Y));

    public static FxVec2 SmoothDamp(in FxVec2 curr, in FxVec2 target, ref FxVec2 velocity, FxNum smoothTime, in FxNum maxSpeed, in FxNum dt) {
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

    public static FxVec2 Reflect(in FxVec2 inDirection, in FxVec2 inNormal)
    {
      var factor = -2 * Dot(inNormal, inDirection);
      return new FxVec2(
        factor * inNormal.X + inDirection.X,
        factor * inNormal.Y + inDirection.Y);
    }
  }

  public static class FxVec2Extensions {
    public static FxNum SqrMagnitude(this in FxVec2 v) => v.X * v.X + v.Y * v.Y;

    public static FxNum Magnitude(this in FxVec2 v) => FxMath.Sqrt(v.SqrMagnitude());

    public static FxVec2 Normalized(this in FxVec2 v) {
      var m = v.Magnitude();

      if (m.Raw == 0)
        return FxVec2.Zero;

      return v * (1 / m);
    }

    public static FxVec2 Clamped(this in FxVec2 v, in FxNum mag) => FxVec2.ClampMagnitude(v, mag);

    public static string ToStr(this in FxVec2 vec) {
      var sb = new StringBuilder(64);
      sb.AppendFxVec2(vec);
      return sb.ToString();
    }

    public static void AppendFxVec2(this StringBuilder sb, in FxVec2 vec) {
      sb.Append('{');
      sb.AppendFxNum(vec.X);
      sb.Append(", ");
      sb.AppendFxNum(vec.Y);
      sb.Append('}');
    }
  }
}