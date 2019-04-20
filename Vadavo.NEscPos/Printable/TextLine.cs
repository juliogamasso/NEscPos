using System;
using System.Linq;
using System.Text;
using Vadavo.NEscPos.Helpers;

namespace Vadavo.NEscPos.Printable
{
    public class TextLine : IPrintable
    {
        private readonly string _line;
        private readonly bool _feed;

        public TextLine(string line = "", bool feed = true)
        {
            _line = line;
            _feed = feed;
        }
        
        public byte[] GetBytes()
        {
            var builder = new PrintableBuilder();
            
            var lineBytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding("ISO-8859-1"),
                Encoding.Unicode.GetBytes(_line));
            
            builder
                .Add(IsoCodePage)
                .Add(lineBytes);

            if (_feed)
                builder.Add(new Feed());

            builder.Add(DefaultCodePage);

            return builder.ToArray();
        }

        private byte[] IsoCodePage => new[] {(byte) Control.Escape, (byte) 't', (byte) 37};
        private byte[] DefaultCodePage => new[] {(byte) Control.Escape, (byte) 't', (byte) 0};
    }

    public static class TextLineExtensions
    {
        /// <summary>
        ///     Print a line of text using ISO-8859-3 codepage.
        /// </summary>
        public static void PrintLine(this IPrinter printer, string line = "")
        {
            if (printer == null)
                throw new ArgumentNullException(nameof(printer));
            
            printer.Print(new TextLine(line));
        }
    }
}