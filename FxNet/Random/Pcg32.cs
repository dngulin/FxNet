using System.Runtime.InteropServices;

namespace FxNet.Random {
  [StructLayout(LayoutKind.Sequential)]
  public struct Pcg32State {
    public ulong State;
    public ulong Inc;
  }

  public static class Pcg32 {
    public static void Init(this ref Pcg32State rng, ulong initState, ulong initSeq) {
      rng.State = 0;
      rng.Inc = (initSeq << 1) | 1ul;
      rng.NextUInt();
      rng.State += initState;
      rng.NextUInt();
    }

    public static uint NextUInt(this ref Pcg32State rng) {
      var oldState = rng.State;
      rng.State = oldState * 6364136223846793005ul + rng.Inc;

      var xor = (uint) (((oldState >> 18) ^ oldState) >> 27);
      var rot = (int) (oldState >> 59);

      return (xor >> rot) | (xor << (-rot & 31));
    }

    public static uint NextUintBounded(this ref Pcg32State rng, uint bound) {
      var threshold = (~bound + 1u) % bound;
      while (true) {
        var value = rng.NextUInt();
        if (value >= threshold)
          return value % bound;
      }
    }
  }
}