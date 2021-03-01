using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Collision2D {
  [StructLayout(LayoutKind.Sequential)]
  public readonly struct FxCapsule : IShape2D {
    public readonly FxVec2 Start;
    public readonly FxVec2 End;
    public readonly FxNum Radius;

    public FxCapsule(in FxVec2 start, in FxVec2 end, in FxNum radius) {
      Start = start;
      End = end;
      Radius = radius;
    }

    public FxVec2 GetMaxInDirection(in FxVec2 direction) {
      var norm = direction.Normalized();

      var a = Start + norm * Radius;
      var b = End + norm * Radius;

      var dotA = FxVec2.Dot(a, norm);
      var dotB = FxVec2.Dot(b, norm);

      return dotA > dotB ? a : b;
    }
  }
}