using FxNet.Math;

namespace FxNet.Collision2D {
  public interface IShape2D {
    FxVec2 GetMaxInDirection(in FxVec2 direction);
  }
}