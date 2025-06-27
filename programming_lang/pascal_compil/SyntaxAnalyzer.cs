using System;
using System.Collections.Generic;
using System.Linq;

namespace Compil
{
    class SyntaxAnalyzer
    {
        LexicalAnalyzer lex = new LexicalAnalyzer();
        SemanticAnalyzer sem = new SemanticAnalyzer();

        public void Analyze()
        {
            // program → PROGRAM ident ';' block '.' 
            Match(LexicalAnalyzer.programsy);
            Match(LexicalAnalyzer.ident); // имя программы
            Match(LexicalAnalyzer.semicolon);

            Block();

            Match(LexicalAnalyzer.dot); // точка в конце
        }

        void Block()
        {
            if (lex.CurrentSymbol == LexicalAnalyzer.constsy)
                ConstantsSection();
            if (lex.CurrentSymbol == LexicalAnalyzer.typesy)
                TypesSection();
            if (lex.CurrentSymbol == LexicalAnalyzer.varsy)
                VariablesSection();
            StatementList();
        }

        void ConstantsSection() { /* TODO */ }
        void TypesSection() { /* TODO */ }

        void VariablesSection()
        {
            Match(LexicalAnalyzer.varsy);

            do
            {
                List<string> vars = new List<string>();
                vars.Add(lex.AddrName);
                Match(LexicalAnalyzer.ident);

                while (lex.CurrentSymbol == LexicalAnalyzer.comma)
                {
                    Match(LexicalAnalyzer.comma);
                    vars.Add(lex.AddrName);
                    Match(LexicalAnalyzer.ident);
                }

                Match(LexicalAnalyzer.colon);

                string type = lex.AddrName;
                MatchType(); // integer, real, record...

                foreach (string varName in vars)
                    sem.DeclareVariable(varName, type);

                Match(LexicalAnalyzer.semicolon);
            } while (lex.CurrentSymbol == LexicalAnalyzer.ident);
        }

        void MatchType()
        {
            switch (lex.CurrentSymbol)
            {
                case LexicalAnalyzer.tinteger:
                case LexicalAnalyzer.treal:
                case LexicalAnalyzer.tchar:
                case LexicalAnalyzer.tstring:
                case LexicalAnalyzer.tboolean:
                    lex.NextSym();
                    break;
                case LexicalAnalyzer.recordsy:
                    RecordType();
                    break;
                default:
                    InputOutput.Error(10, lex.CurrentToken); // ожидается тип
                    lex.NextSym();
                    break;
            }
        }

        void RecordType()
        {
            Match(LexicalAnalyzer.recordsy);
            Dictionary<string, string> fields = new Dictionary<string, string>();

            while (lex.CurrentSymbol != LexicalAnalyzer.endsy)
            {
                List<string> names = new List<string>();
                names.Add(lex.AddrName);
                Match(LexicalAnalyzer.ident);

                while (lex.CurrentSymbol == LexicalAnalyzer.comma)
                {
                    Match(LexicalAnalyzer.comma);
                    names.Add(lex.AddrName);
                    Match(LexicalAnalyzer.ident);
                }

                Match(LexicalAnalyzer.colon);
                string type = lex.AddrName;
                MatchType();

                foreach (string name in names)
                    fields[name] = type;

                Match(LexicalAnalyzer.semicolon);
            }

            Match(LexicalAnalyzer.endsy);
        }

        void StatementList()
        {
            if (lex.CurrentSymbol == LexicalAnalyzer.beginsy)
            {
                Match(LexicalAnalyzer.beginsy);
                do
                {
                    Statement();
                } while (lex.CurrentSymbol != LexicalAnalyzer.endsy);
                Match(LexicalAnalyzer.endsy);
            }
            else
            {
                Statement();
            }
        }

        void Statement()
        {
            if (lex.CurrentSymbol == LexicalAnalyzer.ident)
            {
                string varName = lex.AddrName;
                Match(LexicalAnalyzer.ident);

                if (lex.CurrentSymbol == LexicalAnalyzer.dot)
                {
                    Match(LexicalAnalyzer.dot);
                    string fieldName = lex.AddrName;
                    Match(LexicalAnalyzer.ident);
                    Match(LexicalAnalyzer.assign);
                    string exprType = Expression();
                    sem.AssignRecordField(varName, fieldName, exprType);
                }
                else if (lex.CurrentSymbol == LexicalAnalyzer.assign)
                {
                    Match(LexicalAnalyzer.assign);
                    string exprType = Expression();
                    sem.AssignVariable(varName, exprType);
                }
                else
                {
                    InputOutput.Error(13, lex.CurrentToken); // несовместимость типов
                }

                Match(LexicalAnalyzer.semicolon);
            }
            else
            {
                InputOutput.Error(9, lex.CurrentToken); // ожидается идентификатор
                lex.NextSym();
            }
        }

        string Expression()
        {
            string type = Term();
            while (lex.CurrentSymbol == LexicalAnalyzer.plus || lex.CurrentSymbol == LexicalAnalyzer.minus)
            {
                byte op = lex.CurrentSymbol;
                lex.NextSym();
                string type2 = Term();
                if (type != type2)
                    InputOutput.Error(13, lex.CurrentToken); // несовместимость
            }
            return type;
        }

        string Term()
        {
            string type = Factor();
            while (lex.CurrentSymbol == LexicalAnalyzer.star || lex.CurrentSymbol == LexicalAnalyzer.slash)
            {
                byte op = lex.CurrentSymbol;
                lex.NextSym();
                string type2 = Factor();
                if (type != type2)
                    InputOutput.Error(13, lex.CurrentToken); // несовместимость
            }
            return type;
        }

        string Factor()
        {
            string type = null;
            switch (lex.CurrentSymbol)
            {
                case LexicalAnalyzer.ident:
                    type = sem.GetVariableType(lex.AddrName);
                    lex.NextSym();
                    if (lex.CurrentSymbol == LexicalAnalyzer.dot)
                    {
                        lex.NextSym();
                        string field = lex.AddrName;
                        lex.NextSym();
                        type = sem.GetRecordFieldType(type, field);
                    }
                    break;
                case LexicalAnalyzer.intc:
                    type = "integer";
                    lex.NextSym();
                    break;
                case LexicalAnalyzer.floatc:
                    type = "real";
                    lex.NextSym();
                    break;
                case LexicalAnalyzer.leftpar:
                    lex.NextSym();
                    type = Expression();
                    Match(LexicalAnalyzer.rightpar);
                    break;
                default:
                    InputOutput.Error(3, lex.CurrentToken); // ошибка в выражении
                    lex.NextSym();
                    break;
            }
            return type;
        }

        void Match(byte expected)
        {
            if (lex.CurrentSymbol == expected)
                lex.NextSym();
            else
                InputOutput.Error((byte)100, lex.CurrentToken); // непредвиденный токен
        }
    }
}