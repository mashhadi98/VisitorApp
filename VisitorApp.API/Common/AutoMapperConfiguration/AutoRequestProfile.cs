using System.Reflection;

namespace VisitorApp.API.Common.AutoMapperConfiguration;

public class AutoRequestProfile : Profile
{
    public AutoRequestProfile()
    {
        var apiAssembly = Assembly.GetAssembly(typeof(AssemblyReference));         // اسمبلی API
        var appAssembly = Assembly.GetAssembly(typeof(Application.AssemblyReference)); // اسمبلی Application

        var appRequests = appAssembly.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Request"))
            .ToList();

        foreach (var request in appRequests)
        {
            if (request.Name.EndsWith("CommandRequest"))
            {
                var commandName = request.Name.Replace("CommandRequest", "Request");

                var commandType = apiAssembly.GetTypes()
                    .FirstOrDefault(t => t.IsClass && t.Name == commandName);

                if (commandType != null)
                {
                    // ایجاد مپ دینامیک
                    CreateMap(commandType, request);
                }
            }
            else if (request.Name.EndsWith("QueryRequest"))
            {
                var endpointName = request.Name.Replace("QueryRequest", "Request");

                var endpointType = apiAssembly.GetTypes()
                    .FirstOrDefault(t => t.IsClass && t.Name == endpointName);

                if (endpointType != null)
                {
                    // ایجاد مپ دینامیک
                    CreateMap(endpointType, request);
                }
            }
        }
    }
}
