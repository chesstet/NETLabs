﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrivateLibrary.Util
{
    public class TimespanConverter : JsonConverter<TimeSpan>
    {
        /// <summary>
        /// Format: Hours:Minutes
        /// </summary>
        public const string TimeSpanFormatString = @"hh\:mm";

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TimeSpan.TryParseExact(reader.ValueSpan.ToString(), TimeSpanFormatString, null, out var parsedTimeSpan);
            return parsedTimeSpan;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            var timespanFormatted = $"{value.ToString(TimeSpanFormatString)}";
            writer.WriteRawValue(timespanFormatted);
        }
    }
}
