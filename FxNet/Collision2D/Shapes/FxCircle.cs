using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Collision2D.Shapes {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxCircle {
    public FxVec2 Center;
    public FxNum Radius;

    public FxCircle(FxVec2 center, FxNum radius) {
      Center = center;
      Radius = radius;
    }
  }
}