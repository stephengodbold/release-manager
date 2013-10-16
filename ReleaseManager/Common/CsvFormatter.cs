using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using ReleaseManager.Models;

namespace ReleaseManager.Common
{
    public class CsvFormatter : BufferedMediaTypeFormatter
    {
        public CsvFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var notes = value as ReleaseNotes;
            if (notes == null)
            {
                throw new InvalidOperationException("Cannot serialize type");
            }

            using (var writer = new StreamWriter(writeStream))
            {
                WriteNotes(notes, writer);
            }

            writeStream.Close();
        }

        private void WriteNotes(ReleaseNotes notes, TextWriter writer)
        {
            foreach (var workItem in notes.Items)
            {
                writer.Write("{0}, {1}", workItem.Id, workItem.State);
            }
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(ReleaseNotes);
        }
    }
}