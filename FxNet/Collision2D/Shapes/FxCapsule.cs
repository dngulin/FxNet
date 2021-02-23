using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Collision2D.Shapes {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxCapsule {
    public FxVec2 Start;
    public FxVec2 End;
    public FxNum Radius;

    public FxCapsule(in FxVec2 start, in FxVec2 end, in FxNum radius) {
      Start = start;
      End = end;
      Radius = radius;
    }
  }
}