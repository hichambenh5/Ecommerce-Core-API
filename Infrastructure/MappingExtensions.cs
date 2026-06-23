using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MappingExtensions
    {
        public static void PatchValues<T>(T existingEntity, T updatedDto)
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var newValue = property.GetValue(updatedDto);
                if(newValue!=null && !newValue.Equals(GetDefaultValue(property.PropertyType)))
                {
                    property.SetValue(existingEntity, newValue);
                }
            }
        }
        private static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
  
}
