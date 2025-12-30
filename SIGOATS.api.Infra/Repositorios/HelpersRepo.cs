using LINQtoCSV;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Infra.Common;
using System.Text;

namespace SIGOATS.api.Infra.Repositorios
{
    public class HelpersRepo
    {
        public static async Task<Response<ODataServiceResult<T>, string>> OnDemandAsync<T>(ODataQueryOptions<T> opts, IQueryable<T> selectQuery)
        {
            try
            {
                var Query = (IQueryable<T>)opts.ApplyTo(selectQuery);
                var resultado = await Query.ToListAsync();

                var QueryCount = (IQueryable<T>)opts.ApplyTo(selectQuery, AllowedQueryOptions.Top);
                var count = await QueryCount.CountAsync() + opts.Skip.Value;

                return new() { Data = new() { Count = count, Value = resultado } };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] ConvertirLinqToCsv<T>(List<T>? data)
        {
            CsvFileDescription outputFileDescription = new()
            {
                SeparatorChar = Char.Parse(","), // tab delimited
                FileCultureName = "es-ES" // use formats used in The Netherlands
            };

            MemoryStream ms = new();
            var txt = new StreamWriter(ms, Encoding.UTF8);

            var cc = new CsvContext();
            cc.Write<T>(data, txt, outputFileDescription);
            txt.Flush();
            ms.Position = 0;

            return ms.ToArray();
        }

        public static byte[] ConvertirLinqToCsvWithOutHeaders<T>(List<T>? data)
        {
            CsvFileDescription outputFileDescription = new()
            {
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, // only use fields with [CsvColumn] attribute
                SeparatorChar = Char.Parse(","), // tab delimited
                FileCultureName = "es-ES" // use formats used in The Netherlands
            };

            MemoryStream ms = new();
            var txt = new StreamWriter(ms, Encoding.UTF8);

            var cc = new CsvContext();
            cc.Write<T>(data, txt, outputFileDescription);
            txt.Flush();
            ms.Position = 0;

            return ms.ToArray();
        }

    }
}
