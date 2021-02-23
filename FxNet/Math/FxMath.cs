namespace FxNet.Math {
  public static class FxMath {
    public const long PiRaw = FxLut.PiRaw;
    public const long TwoPiRaw = PiRaw * 2;
    public const long HalfPiRaw = PiRaw / 2;
    public const long ThreePiDivTwoRaw = PiRaw * 3 / 2;

    public static FxNum Pi => FxNum.FromRaw(FxLut.PiRaw);
    public static FxNum Rad2Deg => FxNum.FromRaw(FxLut.RadToDegRaw);
    public static FxNum Deg2Rad => FxNum.FromRaw(FxLut.DegToRadRaw);

    public static FxNum Floor(in FxNum value) => FxNum.FromInt((long) value);
    public static FxNum Ceil(in FxNum value) => Floor(value + FxNum.FromRaw(FxNum.OneRaw - 1));
    public static FxNum Round(in FxNum value) => Floor(value + FxNum.FromRaw(FxNum.OneRaw / 2));

    public static FxNum Abs(in FxNum value) => FxNum.FromRaw(System.Math.Abs(value.Raw));
    public static FxNum Sign(FxNum value) => System.Math.Sign(value.Raw);

    public static FxNum Min(in FxNum a, in FxNum b) => FxNum.FromRaw(System.Math.Min(a.Raw, b.Raw));
    public static FxNum Max(in FxNum a, in FxNum b) => FxNum.FromRaw(System.Math.Max(a.Raw, b.Raw));

    public static FxNum Clamp(in FxNum value, in FxNum min, in FxNum max) {
      if (value < min)
        return min;

      if (value > max)
        return max;

      return value;
    }

    public static FxNum Clamp01(in FxNum value) => Clamp(value, 0, 1);

    public static FxNum Sqrt(in FxNum value) {
      if (value.Raw <= 0)
        return 0;

      const long fourRaw = FxNum.OneRaw * 4;
      var fours = 0;
      var raw = value.Raw;

      while (raw > fourRaw) {
        raw >>= 2;
        fours++;
      }

      var root = FxLut.Sqrt(raw) << fours;

      root = (root + value / root) >> 1;
      root = (root + value / root) >> 1;

      return root;
    }

    public static FxNum Sin(in FxNum value) => Cos(FxNum.FromRaw(HalfPiRaw) - value);

    public static FxNum Cos(in FxNum value) {
      var raw = System.Math.Abs(value.Raw) % TwoPiRaw;

      if (raw > ThreePiDivTwoRaw)
        return FxLut.Cos(TwoPiRaw - raw);

      if (raw > PiRaw)
        return -FxLut.Cos(raw - PiRaw);

      if (raw > HalfPiRaw)
        return -FxLut.Cos(PiRaw - raw);

      return FxLut.Cos(raw);
    }

    public static FxNum Asin(in FxNum value) {
      var raw = Clamp(value, -1, 1).Raw;
      return raw > 0 ? FxLut.Asin(raw) : -FxLut.Asin(-raw);
    }

    public static FxNum Acos(in FxNum value) => FxNum.FromRaw(HalfPiRaw) - Asin(value);

    public static FxNum Atan2(in FxNum y, in FxNum x) {
      if (x.Raw == 0) {
        if (y.Raw > 0) return FxNum.FromRaw(HalfPiRaw);
        if (y.Raw < 0) return FxNum.FromRaw(-HalfPiRaw);
        return 0;
      }

      if (Abs(x) > Abs(y)) {
        var atan = FxLut.Atan((y / x).Raw);
        if (x.Raw > 0) return atan;
        return atan + FxNum.FromRaw(y.Raw >= 0 ? PiRaw : -PiRaw);
      }
      else {
        var atan = -FxLut.Atan((x / y).Raw);
        return atan + FxNum.FromRaw(y.Raw > 0 ? HalfPiRaw : -HalfPiRaw);
      }
    }
  }
}