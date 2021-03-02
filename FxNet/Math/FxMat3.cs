using System.Runtime.InteropServices;
using System.Text;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxMat3 {
    private const int ItemCount = 3 * 3;

    // column 0
    public FxNum M00;
    public FxNum M10;
    public FxNum M20;

    // column 1
    public FxNum M01;
    public FxNum M11;
    public FxNum M21;

    // column 2
    public FxNum M02;
    public FxNum M12;
    public FxNum M22;

    public FxMat3(
      in FxNum m00, in FxNum m01, in FxNum m02,
      in FxNum m10, in FxNum m11, in FxNum m12,
      in FxNum m20, in FxNum m21, in FxNum m22) {
      M00 = m00;
      M01 = m01;
      M02 = m02;

      M10 = m10;
      M11 = m11;
      M12 = m12;

      M20 = m20;
      M21 = m21;
      M22 = m22;
    }

    public static FxMat3 operator *(in FxMat3 l, in FxMat3 r) {
      return new FxMat3(
        l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20,
        l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21,
        l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22,
        l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20,
        l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21,
        l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22,
        l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20,
        l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21,
        l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22
      );
    }

    public static FxVec3 operator *(in FxMat3 l, in FxVec3 r) {
      return new FxVec3(
        l.M00 * r.X + l.M01 * r.Y + l.M02 * r.Z,
        l.M10 * r.X + l.M11 * r.Y + l.M12 * r.Z,
        l.M20 * r.X + l.M21 * r.Y + l.M22 * r.Z
      );
    }

    public static unsafe bool operator ==(in FxMat3 l, in FxMat3 r) {
      fixed (FxNum* pl = &l.M00, pr = &r.M00) {
        for (var i = 0; i < ItemCount; i++) {
          if (pl[i] != pr[i]) return false;
        }
      }

      return true;
    }

    public static unsafe bool operator !=(in FxMat3 l, in FxMat3 r) {
      fixed (FxNum* pl = &l.M00, pr = &r.M00) {
        for (var i = 0; i < ItemCount; i++) {
          if (pl[i] == pr[i]) return false;
        }
      }

      return true;
    }

    public override bool Equals(object obj) => obj is FxMat3 other && this == other;
    public override int GetHashCode() => throw new System.NotSupportedException();

    public override string ToString() => this.ToStr();
  }

  public static class FxMat3Extensions {
    public static FxVec2 MultiplyPoint(this in FxMat3 m, in FxVec2 v) {
      return new FxVec2(
        m.M00 * v.X + m.M01 * v.Y + m.M02,
        m.M10 * v.X + m.M11 * v.Y + m.M12);
    }

    public static string ToStr(this in FxMat3 mat) {
      var sb = new StringBuilder(256);
      sb.AppendFxMat3(mat);
      return sb.ToString();
    }

    public static void AppendFxMat3(this StringBuilder sb, in FxMat3 mat) {
      sb.Append('{');
      sb.AppendFxVec3(new FxVec3(mat.M00, mat.M01, mat.M02));
      sb.Append(", ");
      sb.AppendFxVec3(new FxVec3(mat.M10, mat.M11, mat.M12));
      sb.Append(", ");
      sb.AppendFxVec3(new FxVec3(mat.M20, mat.M21, mat.M22));
      sb.Append('}');
    }
  }
}