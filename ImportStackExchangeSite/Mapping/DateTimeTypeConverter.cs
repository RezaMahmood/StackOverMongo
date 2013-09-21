using System;
using AutoMapper;

namespace StackOverMongo.Import.Mapping
{
    internal class DateTimeTypeConverter : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(ResolutionContext context)
        {
            if(!string.IsNullOrEmpty((string)context.SourceValue))
            {
                DateTime result = default(DateTime);
                if(DateTime.TryParse((string)context.SourceValue, out result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}