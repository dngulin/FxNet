using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Collision2D.Shapes {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxPoly4 {
    public FxVec2 A;
    public FxVec2 B;
    public FxVec2 C;
    public FxVec2 D;

    public FxPoly4(in FxVec2 a, in FxVec2 b, in FxVec2 c, in FxVec2 d) {
      A = a;
      B = b;
      C = c;
      D = d;
    }
  }
}