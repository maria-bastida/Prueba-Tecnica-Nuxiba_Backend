using CCenterApi.DTOs;
using CsvHelper;
using System.Globalization;

namespace CCenterApi.Utils
{
    public static class CsvGenerator
    {
        public static byte[] GenerateCsv(IEnumerable<UserCsvReportDto> data)
        {
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(data);
            writer.Flush();
            return ms.ToArray();
        }
    }
}
