using System.Runtime.InteropServices;
using FxNet.Math;

namespace FxNet.Random {
  [StructLayout(LayoutKind.Sequential)]
  public struct FxRandomState {
    public Pcg32State Hi;
    public Pcg32State Lo;
  }

  public static class FxRandom {
    public static FxRandomState CreteState(ulong seed) {
      var pcg32 = new Pcg32State();
      pcg32.Init(seed, seed + 1);

      var fxRandom = new FxRandomState();
      fxRandom.Init(
        ((ulong) pcg32.NextUInt() << 32) | pcg32.NextUInt(),
        ((ulong) pcg32.NextUInt() << 32) | pcg32.NextUInt(),
        ((ulong) pcg32.NextUInt() << 32) | pcg32.NextUInt(),
        ((ulong) pcg32.NextUInt() << 32) | pcg32.NextUInt());

      return fxRandom;
    }

    public static void Init(this ref FxRandomState rng, ulong seed1, ulong seed2, ulong seq1, ulong seq2) {
      const ulong mask = ~0ul >> 1;
      if ((seq1 & mask) == (seq2 & mask))
        seq2 = ~seq2;

      rng.Hi.Init(seed1, seq1);
      rng.Lo.Init(seed2, seq2);
    }

    public static ulong NextUlong(this ref FxRandomState rng) {
      return ((ulong) rng.Hi.NextUInt() << 32) | rng.Lo.NextUInt();
    }

    public static ulong NextUlongBounded(this ref FxRandomState rng, ulong bound) {
      var threshold = (~bound + 1ul) % bound;
      while (true) {
        var value = rng.NextUlong();
        if (value >= threshold)
          return value % bound;
      }
    }

    public static FxNum Next(this ref FxRandomState rng) => FxNum.FromRaw((long) (rng.NextUlong() >> 1));

    public static FxNum Next(this ref FxRandomState rng, in FxNum max) {
      var bound = (ulong) max.Raw;
      return FxNum.FromRaw((long) rng.NextUlongBounded(bound));
    }

    public static FxNum Next(this ref FxRandomState rng, in FxNum min, in FxNum max) {
      var bound = (ulong) (max - min).Raw;
      return min + FxNum.FromRaw((long) rng.NextUlongBounded(bound));
    }

    public static FxNum NextFactor(this ref FxRandomState rng) {
      return FxNum.FromRaw((long) rng.NextUlongBounded(FxNum.OneRaw + 1));
    }
  }
}