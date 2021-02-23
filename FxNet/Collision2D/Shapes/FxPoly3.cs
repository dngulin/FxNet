using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Collision2D.Shapes {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxPoly3 {
    public FxVec2 A;
    public FxVec2 B;
    public FxVec2 C;

    public FxPoly3(in FxVec2 a, in FxVec2 b, in FxVec2 c) {
      A = a;
      B = b;
      C = c;
    }
  }
}