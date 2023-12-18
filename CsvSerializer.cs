using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace CsvSerialization
{
    /// <summary>
    /// Сериализатор / десериализатор данных любого типа в формат CSV.
    /// </summary>
    internal static class CsvSerializer<T> where T : class, new()
    {
        private static readonly IEnumerable<FieldInfo> _fields = typeof(T).GetFields(
            BindingFlags.Public | BindingFlags.Instance).OrderBy(_ => _.Name);

        /// <summary>
        /// Метод сериализации
        /// </summary>
        /// <param name="obj">Объект для сериализации.</param>
        /// <param name="opt">Опции сериализатора CSV.</param>
        /// <returns>Строковое представление значения сериализации.</returns>
        public static string Serialize(T obj, CsvSerializerOptions opt)
        {
            var sb = new StringBuilder();
            // Заголовок из названий полей
            sb.AppendLine(string.Join(opt.Separator, _fields.Select(_ => _.Name)));
            var fieldValues = new List<string?>();
            foreach (FieldInfo f in _fields)
            {
                object? value = f.GetValue(obj);
                fieldValues.Add(value == null ? string.Empty : value.ToString());
            }
            // Строка из значений полей
            sb.AppendLine(string.Join(opt.Separator, fieldValues));
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Метод десериализации
        /// </summary>
        /// <param name="csv">Строковое представление CSV.</param>
        /// <returns>Объект десериализации класса <typeparamref name="T" /></returns>
        public static T Deserialize(string csv, CsvSerializerOptions opt)
        {
            var obj = new T();
            string[] lines = csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length == 2)
            {
                string[] fieldNames = lines[0].Split(opt.Separator),
                         fieldValues = lines[1].Split(opt.Separator);
                if (fieldNames.Length == fieldValues.Length)
                {
                    for (int i = 0; i < fieldValues.Length; i++)
                    {
                        string fieldName = fieldNames[i],
                               fieldValue = fieldValues[i];
                        FieldInfo? f = _fields.FirstOrDefault(_ => _.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
                        if (f != null)
                        {
                            TypeConverter converter = TypeDescriptor.GetConverter(f.FieldType);
                            var convertedvalue = converter.ConvertFrom(fieldValue);
                            f.SetValue(obj, convertedvalue);
                        }
                    }
                }
            }
            return obj;
        }
    }
}
