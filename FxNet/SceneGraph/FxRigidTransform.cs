using FxNet.Math;

namespace FxNet.SceneGraph {
  public static class FxRigidTransform {
    public static FxMat4 CombineMatrix(in FxVec3 translation, in FxQuat rotation) {
      var zero = (FxNum) 0;
      var one = (FxNum) 1;

      var xx = rotation.X * rotation.X << 1;
      var xy = rotation.X * rotation.Y << 1;
      var xz = rotation.X * rotation.Z << 1;
      var xw = rotation.X * rotation.W << 1;

      var yy = rotation.Y * rotation.Y << 1;
      var yz = rotation.Y * rotation.Z << 1;
      var yw = rotation.Y * rotation.W << 1;

      var zz = rotation.Z * rotation.Z << 1;
      var zw = rotation.Z * rotation.W << 1;

      var result = new FxMat4(
        one - (yy + zz), xy - zw, xz + yw, translation.X,
        xy + zw, one - (xx + zz), yz - xw, translation.Y,
        xz - yw, yz + xw, one - (xx + yy), translation.Z,
        zero, zero, zero, one);

      return result;
    }

    public static FxMat4 Inverse(in FxMat4 m) {
      var zero = (FxNum) 0;
      var one = (FxNum) 1;

      var inv = new FxMat4(
        m.M00, m.M10, m.M20, zero,
        m.M01, m.M11, m.M21, zero,
        m.M02, m.M12, m.M22, zero,
        zero, zero, zero, one);

      var translation = -inv.MultiplyPoint(ExtractTranslation(m));

      inv.M03 = translation.X;
      inv.M13 = translation.Y;
      inv.M23 = translation.Z;

      return inv;
    }

    public static FxVec3 ExtractTranslation(in FxMat4 m) => new FxVec3(m.M03, m.M13, m.M23);

    public static FxQuat ExtractRotation(in FxMat4 m) {
      FxNum x, y, z, w, s;
      var one = (FxNum) 1;

      var trace = m.M00 + m.M11 + m.M22;
      if (trace > 0) {
        w = FxMath.Sqrt(one + trace) >> 1;
        s = w << 2;
        x = (m.M21 - m.M12) / s;
        y = (m.M02 - m.M20) / s;
        z = (m.M10 - m.M01) / s;
      }
      else if (m.M00 > m.M11 && m.M00 > m.M22) {
        x = FxMath.Sqrt(one + m.M00 - m.M11 - m.M22) >> 1;
        s = x << 2;
        y = (m.M10 + m.M01) / s;
        z = (m.M02 + m.M20) / s;
        w = (m.M21 - m.M12) / s;
      }
      else if (m.M11 > m.M22) {
        y = FxMath.Sqrt(one - m.M00 + m.M11 - m.M22) >> 1;
        s = y << 2;
        x = (m.M10 + m.M01) / s;
        z = (m.M21 + m.M12) / s;
        w = (m.M02 - m.M20) / s;
      }
      else {
        z = FxMath.Sqrt(one - m.M00 - m.M11 + m.M22) >> 1;
        s = z << 2;
        x = (m.M02 + m.M20) / s;
        y = (m.M21 + m.M12) / s;
        w = (m.M10 - m.M01) / s;
      }

      return new FxQuat(x, y, z, w);
    }

    public static FxBiQuat CombineBiQuaternion(in FxVec3 translation, in FxQuat rotation) {
      var dualPart = (new FxQuat(translation.X, translation.Y, translation.Z, 0) * rotation) >> 1;
      return new FxBiQuat(rotation, dualPart);
    }

    public static FxVec3 ExtractTranslation(in FxBiQuat bq) {
      var t = (bq.Dual << 1) * bq.Real.Conjugation();
      return new FxVec3(t.X, t.Y, t.Z);
    }

    public static FxQuat ExtractRotation(in FxBiQuat bq) => bq.Real;

    public static FxBiQuat Inverse(in FxBiQuat bq) => bq.Inverse();

    public static FxQuat EulerToQuaternion(in FxVec3 euler) {
      var half = (euler >> 1) * FxMath.Deg2Rad;

      var cx = FxMath.Cos(half.X);
      var cy = FxMath.Cos(half.Y);
      var cz = FxMath.Cos(half.Z);

      var sx = FxMath.Sin(half.X);
      var sy = FxMath.Sin(half.Y);
      var sz = FxMath.Sin(half.Z);

      var x = cy * sx * cz + sy * cx * sz;
      var y = sy * cx * cz - cy * sx * sz;
      var z = cy * cx * sz - sy * sx * cz;
      var w = cy * cx * cz + sy * sx * sz;

      return new FxQuat(x, y, z, w);
    }

    public static FxVec3 QuaternionToEuler(in FxQuat quat) {
      var rotationMatrix = CombineMatrix(FxVec3.Zero, quat);

      var x = FxMath.Asin(FxMath.Clamp(-rotationMatrix.M12, -1, 1));
      FxNum y, z;

      if (FxMath.Abs(rotationMatrix.M21) < 1) {
        y = FxMath.Atan2(rotationMatrix.M02, rotationMatrix.M22);
        z = FxMath.Atan2(rotationMatrix.M10, rotationMatrix.M11);
      }
      else {
        y = FxMath.Atan2(-rotationMatrix.M20, rotationMatrix.M00);
        z = 0;
      }

      return new FxVec3(x, y, z) * FxMath.Rad2Deg;
    }
  }
}