using System.Collections.Generic;

namespace Compil
{
    class SemanticAnalyzer
    {
        Dictionary<string, string> variables = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> records = new Dictionary<string, Dictionary<string, string>>();

        public void DeclareVariable(string name, string type)
        {
            if (variables.ContainsKey(name))
                InputOutput.Error(11, InputOutput.positionNow); // повторное объявление
            variables[name] = type;
        }

        public string GetVariableType(string name)
        {
            if (!variables.TryGetValue(name, out string type))
                InputOutput.Error(9, InputOutput.positionNow); // переменная не объявлена
            return type;
        }

        public void AssignVariable(string name, string exprType)
        {
            string varType = GetVariableType(name);
            if (varType != exprType)
                InputOutput.Error(13, InputOutput.positionNow); // несовместимость
        }

        public void AssignRecordField(string recordName, string fieldName, string exprType)
        {
            string recordType = GetVariableType(recordName);
            if (!records.TryGetValue(recordType, out var fields))
                InputOutput.Error(14, InputOutput.positionNow); // тип не record

            if (!fields.TryGetValue(fieldName, out string fieldType))
                InputOutput.Error(14, InputOutput.positionNow); // поле не найдено

            if (fieldType != exprType)
                InputOutput.Error(13, InputOutput.positionNow); // несовместимость
        }

        public string GetRecordFieldType(string recordType, string fieldName)
        {
            if (!records.TryGetValue(recordType, out var fields))
                InputOutput.Error(14, InputOutput.positionNow); // тип не record

            if (!fields.TryGetValue(fieldName, out string fieldType))
                InputOutput.Error(14, InputOutput.positionNow); // поле не найдено

            return fieldType;
        }
    }
}