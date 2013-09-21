using AutoMapper;

namespace StackOverMongo.Import.Mapping
{
    internal class IntegerTypeConverter : ITypeConverter<string, int>
    {
        public int Convert(ResolutionContext context)
        {
            if(!string.IsNullOrEmpty((string)context.SourceValue))
            {
                int result = 0;
                if(int.TryParse((string)context.SourceValue, out result))
                {
                    return result;
                }
            }

            return 0;
        }
    }
}