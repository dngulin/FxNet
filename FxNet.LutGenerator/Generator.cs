using System;
using System.IO;
using FxNet.Math;

namespace FxNet.LutGenerator {
  internal static class Generator {
    private static void Main(string[] args) {
      var outPutPath = args[0];

      using (var writer = new StreamWriter(File.Create(outPutPath))) {
        // TODO: Generate IL2CPP attributes
        using (var nsScope = new Scope(writer, "namespace FxNet.Math"))
        using (var clsScope = nsScope.Sub("internal static unsafe class FxLut")) {
          WriteLutClass(clsScope);
        }
      }
    }

    private static void WriteLutClass(in Scope clsScope) {
      var pi = FxNum.FromFloat(System.Math.PI);
      clsScope.WriteLine($"public const long PiRaw = {pi.Raw};");
      clsScope.WriteLine($"public const long DegToRadRaw = {FxNum.FromFloat(System.Math.PI / 180d).Raw};");
      clsScope.WriteLine($"public const long RadToDegRaw = {FxNum.FromFloat(180d / System.Math.PI).Raw};");

      clsScope.WriteLine();
      WriteLut(clsScope, "Sqrt", 4, 1000, System.Math.Sqrt);

      clsScope.WriteLine();
      WriteLut(clsScope, "Cos", pi / 2, 1500, System.Math.Cos);

      clsScope.WriteLine();
      WriteLut(clsScope, "Asin", 1, 1000, System.Math.Asin);

      clsScope.WriteLine();
      WriteLut(clsScope, "Atan", 1, 1000, System.Math.Asin);
    }

    private static void WriteLut(in Scope scope, string name, in FxNum range, int lutSize, Func<double, double> gen) {
      var shift = (int) System.Math.Round(System.Math.Log2((range / lutSize).Raw));
      var step = FxNum.FromRaw(1 << shift);
      lutSize = (int) (range / step) + 2; // Include the last value & allow linear interpolation for it

      Console.WriteLine($"{name}Lut: {lutSize}");

      scope.WriteLine($"private const int {name}Shift = {shift};");
      scope.WriteLine($"private const long {name}StepMask = (long) (ulong.MaxValue >> (64 - {name}Shift));");

      scope.WriteLine($"private const long {name}StepRaw = 1 << {name}Shift;");
      scope.WriteLine($"private const long {name}InvStepRaw = (FxNum.OneRaw << FxNum.Precision) / {name}StepRaw;");


      scope.WriteLine();
      using (var fnScope = scope.Sub($"public static FxNum {name}(in long raw)"))
      using (var fixedScope = fnScope.Sub($"fixed(int* lut = Lut{name})")) {
        fixedScope.WriteLine($"var index = raw >> {name}Shift;");
        fixedScope.WriteLine("var bot = FxNum.FromRaw(lut[index]);");
        fixedScope.WriteLine("var top = FxNum.FromRaw(lut[index + 1]);");
        fixedScope.WriteLine($"var factor = FxNum.FromRaw(raw & {name}StepMask) * FxNum.FromRaw({name}InvStepRaw);");
        fixedScope.WriteLine("return bot + factor * (top - bot);");
      }

      const int cols = 8;
      var rows = lutSize / cols;

      if (lutSize % cols != 0)
        rows++;

      scope.WriteLine();
      using (var lutScope = scope.Sub($"private static readonly int[] Lut{name} = ", true)) {
        var i = 0;
        for (var row = 0; row < rows; row++) {
          lutScope.WriteIndent();

          for (var col = 0; col < cols; col++) {
            var input = FxNum.FromRaw(System.Math.Min(i, lutSize - 2) << shift);
            var value = (int) ((FxNum) gen((double) input)).Raw;

            var sep = col == cols - 1 || i == lutSize - 1 ? "," : ", ";
            lutScope.Write(value + sep);

            i++;
            if (i >= lutSize) break;
          }

          lutScope.WriteLine();
        }
      }
    }
  }
}