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

    public static FxQuat operator *(in FxQuat l, in FxQuat r) {
      return new FxQuat(
        l.W * r.X + l.X * r.W + l.Y * r.Z - l.Z * r.Y,
        l.W * r.Y + l.Y * r.W + l.Z * r.X - l.X * r.Z,
        l.W * r.Z + l.Z * r.W + l.X * r.Y - l.Y * r.X,
        l.W * r.W - l.X * r.X - l.Y * r.Y - l.Z * r.Z);
    }

    public static FxQuat operator +(in FxQuat l, in FxQuat r) => new FxQuat(l.X + r.X, l.Y + r.Y, l.Z + r.Z, l.W + r.W);

    public static FxQuat operator -(in FxQuat q) => new FxQuat(-q.X, -q.Y, -q.Z, -q.W);

    public static FxQuat operator /(in FxQuat q, in FxNum s) => new FxQuat(q.X / s, q.Y / s, q.Z / s, q.W / s);
    public static FxQuat operator /(in FxQuat q, int s) => new FxQuat(q.X / s, q.Y / s, q.Z / s, q.W / s);
    public static FxQuat operator /(in FxQuat q, in long s) => new FxQuat(q.X / s, q.Y / s, q.Z / s, q.W / s);

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

    public static FxQuat Inverted(this in FxQuat q) => q.Conjugation() / FxQuat.Dot(q, q);

    public static FxQuat Normalized(this in FxQuat q) {
      var m = FxMath.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);

      if (m.Raw == 0)
        return FxQuat.Identity;

      return q / m;
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