using System.Runtime.InteropServices;

namespace FxNet.Math {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxBiQuat {
    public FxQuat Real;
    public FxQuat Dual;

    public static FxBiQuat Identity => new FxBiQuat(FxQuat.Identity, new FxQuat(0, 0, 0, 0));

    public FxBiQuat(in FxQuat real, in FxQuat dual) {
      Real = real;
      Dual = dual;
    }

    public static FxBiQuat operator *(in FxBiQuat l, in FxBiQuat r) {
      return new FxBiQuat(l.Real * r.Real, l.Real * r.Dual + l.Dual * r.Real);
    }

    public static bool operator ==(in FxBiQuat l, in FxBiQuat r) => l.Real == r.Real && l.Dual == r.Dual;
    public static bool operator !=(in FxBiQuat l, in FxBiQuat r) => l.Real != r.Real && l.Dual != r.Dual;

    public override bool Equals(object obj) => obj is FxBiQuat other && this == other;
    public override int GetHashCode() => throw new System.NotSupportedException();
  }

  public static class FxBiQuatExtensions {
    public static FxBiQuat Inverse(in this FxBiQuat bq) {
      var real = bq.Real.Conjugation();
      var dual = -real * bq.Dual * real;
      return new FxBiQuat(real, dual);
    }
  }
}