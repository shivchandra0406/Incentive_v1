using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Incentive.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                if (methodInfo == null)
                {
                    var mapFromInterface = type.GetInterface("IMapFrom`1");
                    if (mapFromInterface != null)
                    {
                        methodInfo = mapFromInterface.GetMethod("Mapping");
                    }
                }

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
