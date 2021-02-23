using System.Runtime.InteropServices;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxMat3x3 {
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

    public FxMat3x3(
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

    public static FxMat3x3 operator*(in FxMat3x3 l, in FxMat3x3 r)
    {
      return new FxMat3x3(
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

    public static FxVec3 operator*(in FxMat3x3 l, in FxVec3 r)
    {
      return new FxVec3(
        l.M00 * r.X + l.M01 * r.Y + l.M02 * r.Z,
        l.M10 * r.X + l.M11 * r.Y + l.M12 * r.Z,
        l.M20 * r.X + l.M21 * r.Y + l.M22 * r.Z
      );
    }

    public static FxVec2 operator*(in FxMat3x3 l, in FxVec2 r)
    {
      return new FxVec2(
        l.M00 * r.X + l.M01 * r.Y + l.M02,
        l.M10 * r.X + l.M11 * r.Y + l.M12
      );
    }
  }

  public static class FxMat3X3Extensions {
    public static FxVec2 MultiplyPoint2x3(this in FxMat3x3 m, in FxVec2 v) {
      var x = m.M00 * v.X + m.M01 * v.Y + m.M02;
      var y = m.M10 * v.X + m.M11 * v.Y + m.M12;
      return new FxVec2(x, y);
    }
  }
}