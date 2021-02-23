using System.Runtime.InteropServices;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxMat4x4 {
    private const int ItemCount = 4 * 4;

    // column 0
    public FxNum M00;
    public FxNum M10;
    public FxNum M20;
    public FxNum M30;

    // column 1
    public FxNum M01;
    public FxNum M11;
    public FxNum M21;
    public FxNum M31;

    // column 2
    public FxNum M02;
    public FxNum M12;
    public FxNum M22;
    public FxNum M32;

    // column 3
    public FxNum M03;
    public FxNum M13;
    public FxNum M23;
    public FxNum M33;

    public static FxMat4x4 Identity => new FxMat4x4(
      1, 0, 0, 0,
      0, 1, 0, 0,
      0, 0, 1, 0,
      0, 0, 0, 1);

    public FxMat4x4(
      in FxNum m00, in FxNum m01, in FxNum m02, in FxNum m03,
      in FxNum m10, in FxNum m11, in FxNum m12, in FxNum m13,
      in FxNum m20, in FxNum m21, in FxNum m22, in FxNum m23,
      in FxNum m30, in FxNum m31, in FxNum m32, in FxNum m33) {
      M00 = m00;
      M01 = m01;
      M02 = m02;
      M03 = m03;

      M10 = m10;
      M11 = m11;
      M12 = m12;
      M13 = m13;

      M20 = m20;
      M21 = m21;
      M22 = m22;
      M23 = m23;

      M30 = m30;
      M31 = m31;
      M32 = m32;
      M33 = m33;
    }

    public static FxMat4x4 operator *(in FxMat4x4 l, in FxMat4x4 r) {
      return new FxMat4x4(
        l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20 + l.M03 * r.M30,
        l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21 + l.M03 * r.M31,
        l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22 + l.M03 * r.M32,
        l.M00 * r.M03 + l.M01 * r.M13 + l.M02 * r.M23 + l.M03 * r.M33,
        l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20 + l.M13 * r.M30,
        l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21 + l.M13 * r.M31,
        l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22 + l.M13 * r.M32,
        l.M10 * r.M03 + l.M11 * r.M13 + l.M12 * r.M23 + l.M13 * r.M33,
        l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20 + l.M23 * r.M30,
        l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21 + l.M23 * r.M31,
        l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22 + l.M23 * r.M32,
        l.M20 * r.M03 + l.M21 * r.M13 + l.M22 * r.M23 + l.M23 * r.M33,
        l.M30 * r.M00 + l.M31 * r.M10 + l.M32 * r.M20 + l.M33 * r.M30,
        l.M30 * r.M01 + l.M31 * r.M11 + l.M32 * r.M21 + l.M33 * r.M31,
        l.M30 * r.M02 + l.M31 * r.M12 + l.M32 * r.M22 + l.M33 * r.M32,
        l.M30 * r.M03 + l.M31 * r.M13 + l.M32 * r.M23 + l.M33 * r.M33
      );
    }

    public static unsafe bool operator ==(in FxMat4x4 l, in FxMat4x4 r) {
      fixed (FxNum* pl = &l.M00, pr = &r.M00) {
        for (var i = 0; i < ItemCount; i++) {
          if (pl[i] != pr[i]) return false;
        }
      }

      return true;
    }

    public static unsafe bool operator !=(in FxMat4x4 l, in FxMat4x4 r) {
      fixed (FxNum* pl = &l.M00, pr = &r.M00) {
        for (var i = 0; i < ItemCount; i++) {
          if (pl[i] == pr[i]) return false;
        }
      }

      return true;
    }

    public override bool Equals(object obj) => obj is FxMat4x4 other && this == other;
    public override int GetHashCode() => throw new System.NotSupportedException();
  }

  public static class FxMat4X4Extensions {
    public static FxVec3 MultiplyPoint3x4(this in FxMat4x4 m, in FxVec3 v) {
      return new FxVec3(
        m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z + m.M03,
        m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z + m.M13,
        m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z + m.M23);
    }

    public static bool TryInverse(in this FxMat4x4 mat, out FxMat4x4 inv) {
      var a2323 = mat.M22 * mat.M33 - mat.M23 * mat.M32;
      var a1323 = mat.M21 * mat.M33 - mat.M23 * mat.M31;
      var a1223 = mat.M21 * mat.M32 - mat.M22 * mat.M31;
      var a0323 = mat.M20 * mat.M33 - mat.M23 * mat.M30;
      var a0223 = mat.M20 * mat.M32 - mat.M22 * mat.M30;
      var a0123 = mat.M20 * mat.M31 - mat.M21 * mat.M30;
      var a2313 = mat.M12 * mat.M33 - mat.M13 * mat.M32;
      var a1313 = mat.M11 * mat.M33 - mat.M13 * mat.M31;
      var a1213 = mat.M11 * mat.M32 - mat.M12 * mat.M31;
      var a2312 = mat.M12 * mat.M23 - mat.M13 * mat.M22;
      var a1312 = mat.M11 * mat.M23 - mat.M13 * mat.M21;
      var a1212 = mat.M11 * mat.M22 - mat.M12 * mat.M21;
      var a0313 = mat.M10 * mat.M33 - mat.M13 * mat.M30;
      var a0213 = mat.M10 * mat.M32 - mat.M12 * mat.M30;
      var a0312 = mat.M10 * mat.M23 - mat.M13 * mat.M20;
      var a0212 = mat.M10 * mat.M22 - mat.M12 * mat.M20;
      var a0113 = mat.M10 * mat.M31 - mat.M11 * mat.M30;
      var a0112 = mat.M10 * mat.M21 - mat.M11 * mat.M20;

      var det = mat.M00 * (mat.M11 * a2323 - mat.M12 * a1323 + mat.M13 * a1223) -
                mat.M01 * (mat.M10 * a2323 - mat.M12 * a0323 + mat.M13 * a0223) +
                mat.M02 * (mat.M10 * a1323 - mat.M11 * a0323 + mat.M13 * a0123) -
                mat.M03 * (mat.M10 * a1223 - mat.M11 * a0223 + mat.M12 * a0123);

      if (det.Raw == 0) {
        inv = FxMat4x4.Identity;
        return false;
      }

      inv = new FxMat4x4(
         (mat.M11 * a2323 - mat.M12 * a1323 + mat.M13 * a1223) / det,
        -(mat.M01 * a2323 - mat.M02 * a1323 + mat.M03 * a1223) / det,
         (mat.M01 * a2313 - mat.M02 * a1313 + mat.M03 * a1213) / det,
        -(mat.M01 * a2312 - mat.M02 * a1312 + mat.M03 * a1212) / det,
        -(mat.M10 * a2323 - mat.M12 * a0323 + mat.M13 * a0223) / det,
         (mat.M00 * a2323 - mat.M02 * a0323 + mat.M03 * a0223) / det,
        -(mat.M00 * a2313 - mat.M02 * a0313 + mat.M03 * a0213) / det,
         (mat.M00 * a2312 - mat.M02 * a0312 + mat.M03 * a0212) / det,
         (mat.M10 * a1323 - mat.M11 * a0323 + mat.M13 * a0123) / det,
        -(mat.M00 * a1323 - mat.M01 * a0323 + mat.M03 * a0123) / det,
         (mat.M00 * a1313 - mat.M01 * a0313 + mat.M03 * a0113) / det,
        -(mat.M00 * a1312 - mat.M01 * a0312 + mat.M03 * a0112) / det,
        -(mat.M10 * a1223 - mat.M11 * a0223 + mat.M12 * a0123) / det,
         (mat.M00 * a1223 - mat.M01 * a0223 + mat.M02 * a0123) / det,
        -(mat.M00 * a1213 - mat.M01 * a0213 + mat.M02 * a0113) / det,
         (mat.M00 * a1212 - mat.M01 * a0212 + mat.M02 * a0112) / det);

      return true;
    }
  }
}