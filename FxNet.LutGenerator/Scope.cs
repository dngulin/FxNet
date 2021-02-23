using System;
using System.IO;

namespace FxNet.LutGenerator {
  public readonly struct Scope : IDisposable {
    private const int IndentStep = 2;

    private readonly TextWriter _writer;
    private readonly int _indent;
    private readonly bool _closeWithSemicolon;

    public Scope(TextWriter writer, string prefix, int indent = 0, bool closeWithSemicolon = false) {
      _writer = writer;
      _indent = indent;
      _closeWithSemicolon = closeWithSemicolon;

      WriteIndent(_indent);
      _writer.WriteLine($"{prefix} {{");
    }

    public Scope Sub(string prefix, bool closeWithSemicolon = false) {
      return new Scope(_writer, prefix, _indent + IndentStep, closeWithSemicolon);
    }

    private void WriteIndent(int indent) {
      for (var i = 0 ; i < indent; i++)
        _writer.Write(' ');
    }

    public void WriteLine(string value) {
      WriteIndent(_indent + IndentStep);
      _writer.WriteLine(value);
    }

    public void WriteLine() => _writer.WriteLine();

    public void Dispose() {
      WriteIndent(_indent);
      _writer.WriteLine(_closeWithSemicolon ? "};" : "}");
    }

    public void WriteIndent() => WriteIndent(_indent + IndentStep);

    public void Write(string value) => _writer.Write(value);
  }
}