using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MappingExtensions
    {
        public static void PatchValues<TSource, TDestination>(TDestination destination, TSource source)
        {
            var sourceProperties = typeof(TSource).GetProperties();
           
            var destProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProp in sourceProperties)
            {
              
                var destProp = destProperties.FirstOrDefault(p => p.Name == sourceProp.Name);

              
                if (destProp != null && destProp.CanWrite)
                {
                    var newValue = sourceProp.GetValue(source);

                   
                    if (newValue != null)
                    {
                        destProp.SetValue(destination, newValue);
                    }
                }
                }
        }
        private static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
  
}
