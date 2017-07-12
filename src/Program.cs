using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using CsvHelper;
using CsvHelper.Configuration;

namespace WheelLogMerge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dir = new DirectoryInfo(args[0]);
            var rows = new SortedList<long, Row>();
            foreach (var file in dir.EnumerateFiles("*.csv"))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                using (var streamReader = new StreamReader(file.FullName))
                {
                    var csvReader = new CsvReader(streamReader, new CsvConfiguration { HasHeaderRecord = true });
                    while (csvReader.Read())
                    {
                        var record = csvReader.CurrentRecord;
                        Debug.Assert(record.Length == 9);
                        var speed = float.Parse(record[2]);
                        // if (speed < 1) continue;
                        var row = new Row
                        {
                            Date = DateTime.Parse($"{record[0]} {record[1]}"),
                            Speed = speed,
                            Voltage = float.Parse(record[3]),
                            Current = float.Parse(record[4]),
                            Power = float.Parse(record[5]),
                            BatteryLevel = int.Parse(record[6]),
                            Distance = float.Parse(record[7]),
                            Temperature = int.Parse(record[8])
                        };
                        rows.Add(row.Date.Ticks, row);
                    }
                }
            }

            using (var textWriter = File.CreateText("out.csv"))
            {
                var csvWriter = new CsvWriter(textWriter);
                csvWriter.WriteHeader(typeof(Row));
                csvWriter.WriteRecords(rows.Values);
            }
        }
    }
}
