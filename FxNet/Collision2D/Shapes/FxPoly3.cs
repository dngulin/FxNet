using System.Runtime.InteropServices;
using FxNet.Collision2D.Internal;
using FxNet.Math;

namespace FxNet.Collision2D {
  [StructLayout(LayoutKind.Sequential)]
  public readonly struct FxPoly3 : IShape2D {
    public readonly FxVec2 A;
    public readonly FxVec2 B;
    public readonly FxVec2 C;

    public FxPoly3(in FxVec2 a, in FxVec2 b, in FxVec2 c) {
      A = a;
      B = b;
      C = c;
    }

    public unsafe FxVec2 GetMaxInDirection(in FxVec2 direction) {
      fixed (FxVec2* points = &A) {
        return PolygonSupport.GetMaxInDirection(points, 4, direction);
      }
    }
  }
}