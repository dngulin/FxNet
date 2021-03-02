using System;
using System.Text;

namespace FxNet.Math {
  public struct FxNum {
    public const int Precision = 18;
    public const long OneRaw = 1 << Precision;
    public const long HalfRaw = OneRaw / 2;
    public const long AlmostHalfRaw = HalfRaw - 1;

    public long Raw;
    private FxNum(in long rawValue) => Raw = rawValue;

    public static FxNum FromRaw(in long rawValue) => new FxNum(rawValue);
    public static FxNum FromInt(in long value) => new FxNum(value << Precision);

    public static FxNum FromCents(int cents) => FromInt(cents) / 100;
    public static FxNum FromCents(in long cents) => FromInt(cents) / 100;

    public static FxNum FromMillis(int millis) => FromInt(millis) / 1000;
    public static FxNum FromMillis(in long millis) => FromInt(millis) / 1000;

    public static FxNum FromFloat(float value) => new FxNum((long) (value * OneRaw));
    public static FxNum FromFloat(in double value) => new FxNum((long) (value * OneRaw));

    public static implicit operator FxNum(int value) => FromInt(value);
    public static implicit operator FxNum(uint value) => FromInt(value);
    public static implicit operator FxNum(in long value) => FromInt(value);

    public static explicit operator FxNum(float value) => FromFloat(value);
    public static explicit operator FxNum(in double value) => FromFloat(value);

    public static explicit operator int(in FxNum value) => (int) (value.Raw >> Precision);
    public static explicit operator long(in FxNum value) => value.Raw >> Precision;

    public static explicit operator float(in FxNum value) => (float) value.Raw / OneRaw;
    public static explicit operator double(in FxNum value) => (double) value.Raw / OneRaw;

    public static bool operator ==(in FxNum l, in FxNum r) => l.Raw == r.Raw;
    public static bool operator !=(in FxNum l, in FxNum r) => l.Raw != r.Raw;

    public static bool operator >(in FxNum l, in FxNum r) => l.Raw > r.Raw;
    public static bool operator <(in FxNum l, in FxNum r) => l.Raw < r.Raw;

    public static bool operator >=(in FxNum l, in FxNum r) => l.Raw >= r.Raw;
    public static bool operator <=(in FxNum l, in FxNum r) => l.Raw <= r.Raw;

    public static FxNum operator +(in FxNum l, in FxNum r) => new FxNum(l.Raw + r.Raw);
    public static FxNum operator -(in FxNum l, in FxNum r) => new FxNum(l.Raw - r.Raw);

    public static FxNum operator *(in FxNum l, in FxNum r) => new FxNum((l.Raw * r.Raw) >> Precision);

    public static FxNum operator *(in FxNum l, int r) => new FxNum(l.Raw * r);
    public static FxNum operator *(int l, in FxNum r) => new FxNum(l * r.Raw);

    public static FxNum operator *(in FxNum l, in long r) => new FxNum(l.Raw * r);
    public static FxNum operator *(in long l, in FxNum r) => new FxNum(l * r.Raw);

    public static FxNum operator /(in FxNum l, in FxNum r) => new FxNum((l.Raw << Precision) / r.Raw);

    public static FxNum operator /(in FxNum l, int r) => new FxNum(l.Raw / r);
    public static FxNum operator /(in FxNum l, in long r) => new FxNum(l.Raw / r);

    public static FxNum operator -(in FxNum v) => new FxNum(-v.Raw);

    public static FxNum operator >>(in FxNum v, int n) => new FxNum(v.Raw >> n);
    public static FxNum operator <<(in FxNum v, int n) => new FxNum(v.Raw << n);

    public override bool Equals(object obj) => obj is FxNum other && this == other;
    public override int GetHashCode() => throw new NotSupportedException();

    public override string ToString() => this.ToStr();

    public static FxNum MulRounding(in FxNum l, in FxNum r) => new FxNum((l.Raw * r.Raw + AlmostHalfRaw) >> Precision);

    public static FxNum MulBigValues(in FxNum l, in FxNum r) {
      var r1 = (long) r;
      var r2 = r - r1;
      return l * r1 + l * r2;
    }

    public static FxNum DivBigValue(in FxNum l, in FxNum r) => new FxNum((l.Raw / r.Raw) << Precision);

    public static FxNum Parse(string value) {
      if (TryParse(value, out var result))
        return result;

      throw new ArgumentException();
    }

    public static bool TryParse(string value, out FxNum result) => TryParse(value, 0, value?.Length ?? 0, out result);

    public static bool TryParse(string value, int beginPos, int endPos, out FxNum result) {
      result = new FxNum(0);

      if (string.IsNullOrEmpty(value))
        return false;

      var negative = value[beginPos] == '-';
      var startPos = negative ? beginPos + 1 : beginPos;

      var pointPos = endPos;
      for (var i = startPos; i < endPos; i++) {
        if (value[i] == '.')
          pointPos = i;
      }

      var power = 1L;
      for (var i = pointPos - 1; i >= startPos; i--) {
        if (!TryGetDigit(value[i], out var digit))
          return false;

        result += digit * power;
        power *= 10;
      }

      power = 10;
      for (var i = pointPos + 1; i < endPos; i++) {
        if (!TryGetDigit(value[i], out var digit))
          return false;

        result += FromInt(digit) / power;
        power *= 10;
      }

      if (negative)
        result = -result;

      return true;
    }

    private static bool TryGetDigit(char c, out int digit) {
      digit = c - '0';
      return digit >= 0 && digit <= 9;
    }
  }

  public static class FxNumExtensions {
    public static string ToStr(this in FxNum num) {
      var sb = new StringBuilder(32);
      sb.AppendFxNum(num);
      return sb.ToString();
    }

    public static unsafe void AppendFxNum(this StringBuilder sb, in FxNum num) {
      const int buffSize = 16;
      var buffer = stackalloc char[buffSize];

      var abs = FxMath.Abs(num);

      {
        var value = (long) abs;
        var rangeStart = buffSize;

        while (value > 0 && rangeStart > 0) {
          var digit = (int) (value % 10);
          buffer[--rangeStart] = (char) ('0' + digit);
          value /= 10;
        }

        if (num.Raw < 0) sb.Append('-');

        if (rangeStart < buffSize) {
          sb.Append(buffer + rangeStart, buffSize - rangeStart);
        }
        else {
          sb.Append('0');
        }
      }

      {
        var value = abs - (long) abs;
        var rangeEnd = 0;

        while (value.Raw > 0 && rangeEnd < 5) {
          value *= 10;
          var digit = (int) value;
          buffer[rangeEnd++] = (char) ('0' + digit);
          value -= digit;
        }

        if (rangeEnd == 0)
          return;

        sb.Append('.');
        sb.Append(buffer, rangeEnd);
      }
    }
  }
}