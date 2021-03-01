using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Collision2D {
  [StructLayout(LayoutKind.Sequential)]
  public readonly struct FxCircle : IShape2D {
    public readonly FxVec2 Center;
    public readonly FxNum Radius;

    public FxCircle(FxVec2 center, FxNum radius) {
      Center = center;
      Radius = radius;
    }

    public FxVec2 GetMaxInDirection(in FxVec2 direction) => Center + direction.Normalized() * Radius;
  }
}