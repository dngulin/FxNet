using System.Runtime.InteropServices;
using System.Text;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxQuat {
    public FxNum X;
    public FxNum Y;
    public FxNum Z;
    public FxNum W;

    public static FxQuat Identity => new FxQuat(0, 0, 0, 1);

    public FxQuat(in FxNum x, in FxNum y, in FxNum z, in FxNum w) {
      X = x;
      Y = y;
      Z = z;
      W = w;
    }

    public static FxNum Dot(in FxQuat a, in FxQuat b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

    public static FxQuat AngleAxis(in FxNum degrees, in FxVec3 axis) {
      var halfAngle = (degrees * FxMath.Deg2Rad) >> 1;

      var sin = FxMath.Sin(halfAngle);
      var cos = FxMath.Cos(halfAngle);
      var xyz = axis.Normalized() * sin;

      return new FxQuat(xyz.X, xyz.Y, xyz.Z, cos);
    }

    public static FxQuat Slerp(in FxQuat a, in FxQuat b, in FxNum t) {
      var one = FxNum.FromRaw(FxNum.OneRaw);

      var cosHalfTheta = Dot(a, b);
      if (FxMath.Abs(cosHalfTheta) >= one)
        return a;

      var sinHalfTheta = FxMath.Sqrt(one - cosHalfTheta * cosHalfTheta);
      if (FxMath.Abs(sinHalfTheta) < FxNum.FromMillis(1))
        return (a + b) >> 1;

      var halfTheta = FxMath.Asin(sinHalfTheta);
      var invSinHalfTheta = one / sinHalfTheta;

      var k1 = FxMath.Sin((one - t) * halfTheta) * invSinHalfTheta;
      var k2 = FxMath.Sin(t * halfTheta) * invSinHalfTheta;

      return a * k1 + b * k2;
    }

    public static FxQuat operator *(in FxQuat l, in FxQuat r) {
      return new FxQuat(
        FxNum.MulRounding(l.W, r.X) + FxNum.MulRounding(l.X, r.W) +
        FxNum.MulRounding(l.Y, r.Z) - FxNum.MulRounding(l.Z, r.Y),
        FxNum.MulRounding(l.W, r.Y) + FxNum.MulRounding(l.Y, r.W) +
        FxNum.MulRounding(l.Z, r.X) - FxNum.MulRounding(l.X, r.Z),
        FxNum.MulRounding(l.W, r.Z) + FxNum.MulRounding(l.Z, r.W) +
        FxNum.MulRounding(l.X, r.Y) - FxNum.MulRounding(l.Y, r.X),
        FxNum.MulRounding(l.W, r.W) - FxNum.MulRounding(l.X, r.X) -
        FxNum.MulRounding(l.Y, r.Y) - FxNum.MulRounding(l.Z, r.Z));
    }

    public static FxQuat operator +(in FxQuat l, in FxQuat r) => new FxQuat(l.X + r.X, l.Y + r.Y, l.Z + r.Z, l.W + r.W);

    public static FxQuat operator -(in FxQuat q) => new FxQuat(-q.X, -q.Y, -q.Z, -q.W);

    public static FxQuat operator *(in FxQuat q, in FxNum n) => new FxQuat(q.X * n, q.Y * n, q.Z * n, q.W * n);
    public static FxQuat operator *(in FxNum n, in FxQuat q) => new FxQuat(n * q.X, n * q.Y, n * q.Z, n * q.W);

    public static FxQuat operator *(in FxQuat q, int n) => new FxQuat(q.X * n, q.Y * n, q.Z * n, q.W * n);
    public static FxQuat operator *(int n, in FxQuat q) => new FxQuat(n * q.X, n * q.Y, n * q.Z, n * q.W);

    public static FxQuat operator *(in FxQuat q, in long n) => new FxQuat(q.X * n, q.Y * n, q.Z * n, q.W * n);
    public static FxQuat operator *(in long n, in FxQuat q) => new FxQuat(n * q.X, n * q.Y, n * q.Z, n * q.W);

    public static FxQuat operator /(in FxQuat q, in FxNum n) => new FxQuat(q.X / n, q.Y / n, q.Z / n, q.W / n);
    public static FxQuat operator /(in FxQuat q, int n) => new FxQuat(q.X / n, q.Y / n, q.Z / n, q.W / n);
    public static FxQuat operator /(in FxQuat q, in long n) => new FxQuat(q.X / n, q.Y / n, q.Z / n, q.W / n);

    public static FxQuat operator >>(in FxQuat q, int n) => new FxQuat(q.X >> n, q.Y >> n, q.Z >> n, q.W >> n);
    public static FxQuat operator <<(in FxQuat q, int n) => new FxQuat(q.X << n, q.Y << n, q.Z << n, q.W << n);

    public static bool operator ==(in FxQuat l, in FxQuat r) => l.X == r.X && l.Y == r.Y && l.Z == r.Z && l.W == r.W;
    public static bool operator !=(in FxQuat l, in FxQuat r) => l.X != r.X && l.Y != r.Y && l.Z != r.Z && l.W != r.W;

    public override bool Equals(object obj) => obj is FxQuat other && this == other;
    public override int GetHashCode() => throw new System.NotSupportedException();

    public override string ToString() => this.ToStr();
  }

  public static class FxQuatExtensions {
    public static FxQuat Conjugation(this in FxQuat q) => new FxQuat(-q.X, -q.Y, -q.Z, q.W);

    public static FxQuat Inverted(this in FxQuat q) => q.Conjugation() * (1 / FxQuat.Dot(q, q));

    public static FxQuat Normalized(this in FxQuat q) {
      var m = FxMath.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);

      if (m.Raw == 0)
        return FxQuat.Identity;

      return q * (1 / m);
    }

    public static string ToStr(this in FxQuat quat) {
      var sb = new StringBuilder(64);
      sb.AppendFxQuat(quat);
      return sb.ToString();
    }

    public static void AppendFxQuat(this StringBuilder sb, in FxQuat quat) {
      sb.Append('{');
      sb.AppendFxNum(quat.X);
      sb.Append(", ");
      sb.AppendFxNum(quat.Y);
      sb.Append(", ");
      sb.AppendFxNum(quat.Z);
      sb.Append(", ");
      sb.AppendFxNum(quat.W);
      sb.Append('}');
    }
  }
}