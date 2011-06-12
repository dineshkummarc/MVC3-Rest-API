using System;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace REST
{
    public class ObjectResult<T> : ActionResult
    {
        private static UTF8Encoding UTF8 = new UTF8Encoding(false);

        public T Data { get; set; }

        public Type[] IncludedTypes = new[] { typeof(object) };

        public ObjectResult(T data)
        {
            this.Data = data;
        }

        public ObjectResult(T data, Type[] extraTypes)
        {
            this.Data = data;
            this.IncludedTypes = extraTypes;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            // If ContentType is not expected to be application/json, then return XML
            if (context.HttpContext.Request["jsoncallback"] != null)
            {
                SerializeToJsonp(context);
            }
            else if (context.HttpContext.Request["json"] != null)
            {
                SerializeToJson(context);
            }
            else if ((context.HttpContext.Request.ContentType ?? string.Empty).Contains("application/json"))
            {
                SerializeToJson(context);
            }
            else
            {
                SerializeToXml(context);
            }
        }

        private void SerializeToJson(ControllerContext context)
        {
            var result = JsonConvert.SerializeObject(this.Data, Formatting.Indented);
            new ContentResult
            {
                ContentType = "application/json",
                Content = result,
                ContentEncoding = UTF8
            }
            .ExecuteResult(context);
        }

        private void SerializeToJsonp(ControllerContext context)
        {
            var result = HttpContext.Current.Request.Params["jsoncallback"] + "(" +
                JsonConvert.SerializeObject(this.Data, Formatting.Indented) + ")";
            new ContentResult
            {
                ContentType = "application/javascript",
                Content = result,
                ContentEncoding = UTF8
            }
            .ExecuteResult(context);
        }

        private void SerializeToXml(ControllerContext context)
        {
            using (var stream = new MemoryStream(500))
            {
                using (var xmlWriter =
                    XmlTextWriter.Create(stream,
                                         new XmlWriterSettings()
                                         {
                                             OmitXmlDeclaration = true,
                                             Encoding = UTF8,
                                             Indent = true
                                         }))
                {
                    new XmlSerializer(typeof(T), IncludedTypes)
                        .Serialize(xmlWriter, this.Data);
                }
                // NOTE: We need to cache XmlSerializer for specific type. Probably use the 
                // GenerateSerializer to generate compiled custom made serializer for specific
                // types and then cache the reference
                new ContentResult
                {
                    ContentType = "text/xml",
                    Content = UTF8.GetString(stream.ToArray()),
                    ContentEncoding = UTF8
                }
                    .ExecuteResult(context);
            }
        }


    }
}