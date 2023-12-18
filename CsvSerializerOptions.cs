namespace CsvSerialization
{
    /// <summary>
    /// Опции для работы сериализатора CSV
    /// </summary>
    internal class CsvSerializerOptions
    {
        private char _separator = ',';

        /// <summary>
        /// Разделитель в CSV
        /// </summary>
        public char Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }
    }
}
