using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Text;

namespace Compil
{
    struct TextPosition
    {
        public uint lineNumber; // номер строки
        public byte charNumber; // номер позиции в строке

        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }

    struct Err
    {
        public TextPosition errorPosition;
        public byte errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }


    class InputOutput
    {
        const byte ERRMAX = 9;
        public static char Ch { get; set; }
        public static TextPosition positionNow = new TextPosition();
        static string? line;
        static byte lastInLine = 0;
        public static List<Err> err;
        static StreamReader File { get; set; }
        static uint errCount = 0;
        public static bool EndOfFile
        {
            get
            {
                return File.EndOfStream && (positionNow.charNumber >= lastInLine);
            }
        }

        static Func<bool>? onNewLine;
        static bool NewLine = false;

        public static Func<bool>? OnNewLine
        {
            get
            {
                return onNewLine;
            }
            set
            {
                onNewLine = value;
            }
        }

        static public void Init(string filename)
        {
            File = new StreamReader(filename, Encoding.UTF8);
            err = new List<Err>(); ;
            ReadNextLine();
        }

        static public void SetPosition(uint row, byte column)
        {
            if (File.BaseStream.CanSeek)
            {
                File.BaseStream.Seek(0, SeekOrigin.Begin);
                File.DiscardBufferedData();
                for (uint i = 0; i < row + 1; i++)
                {
                    line = File.ReadLine();
                }
                err.Clear();
                positionNow.charNumber = column;
                positionNow.lineNumber = row;
            }
        }
        static public void NextCh()
        {
            if (NewLine)
            {
                if (OnNewLine != null)
                {
                    OnNewLine();
                }
                NewLine = false;
            }
            if (positionNow.charNumber >= lastInLine)
            {
                ListThisLine();
                if (err.Count > 0)
                    ListErrors();
                positionNow.lineNumber++;
                positionNow.charNumber = 0;
                ReadNextLine();
                NewLine = true;
            }
            else ++positionNow.charNumber;
            if (line != null && lastInLine > 0)
            {
                try
                {
                    Ch = line[positionNow.charNumber];
                }
                catch (System.Exception)
                {
                    Console.WriteLine($"{line} {positionNow.charNumber}");
                    throw;
                }
            }
        }

        private static void ListThisLine()
        {
            Console.WriteLine(line);
        }

        private static void ReadNextLine()
        {
            if (!File.EndOfStream)
            {
                line = File.ReadLine();
                err.Clear();
                if (line.Length == 0)
                {
                    Ch = ' ';
                    lastInLine = 0;
                }
                else
                {
                    lastInLine = (byte)(line.Length - 1);
                    Ch = line[positionNow.charNumber];
                }
            }
        }

        public static void End()
        {
            if (err.Count > 0)
                ListErrors();
            Console.WriteLine($"Компиляция завершена: : ошибок — {errCount}!");
        }

        static void ListErrors()
        {
            int pos = 6 - $"{positionNow.lineNumber} ".Length;
            string s;
            foreach (Err item in err)
            {
                ++errCount;
                s = "**";
                if (errCount < 10) s += "0";
                s += $"{errCount}**";
                while (s.Length - 1 < pos + item.errorPosition.charNumber) s += " ";
                s += $"^ ошибка {ErrorsTable.errors[item.errorCode]} в строке {item.errorPosition.lineNumber + 1}:{item.errorPosition.charNumber + 1}";
                Console.WriteLine(s);
            }
        }

        static public void Error(byte errorCode, TextPosition position)
        {
            Err e;
            if (err.Count <= ERRMAX)
            {
                e = new Err(position, errorCode);
                err.Add(e);
            }
        }
    }
}