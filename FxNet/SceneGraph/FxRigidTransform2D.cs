using FxNet.Math;

namespace FxNet.SceneGraph {
  public static class FxRigidTransform2D {
    public static FxMat3 CombineMatrix(in FxVec2 translation, in FxNum rotation) {
      var zero = (FxNum) 0;
      var one = (FxNum) 1;

      var cos = FxMath.Cos(rotation);
      var sin = FxMath.Sin(rotation);

      return new FxMat3(
        cos, -sin, translation.X,
        sin, cos, translation.Y,
        zero, zero, one
      );
    }

    public static FxVec2 ExtractTranslation(in FxMat3 m) => new FxVec2(m.M02, m.M12);

    public static FxNum ExtractRotation(in FxMat3 m) => FxMath.Atan2(m.M10, m.M00);

    public static FxMat3 Inverse(in FxMat3 m) {
      var zero = (FxNum) 0;
      var one = (FxNum) 1;

      var inv = new FxMat3(
        m.M00, m.M10, zero,
        m.M01, m.M11, zero,
        zero, zero, one);

      var translation = -inv.MultiplyPoint(ExtractTranslation(m));

      inv.M02 = translation.X;
      inv.M12 = translation.Y;

      return inv;
    }
  }
}