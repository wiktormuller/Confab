using Microsoft.AspNetCore.Mvc;

namespace Confab.Shared.Infrastructure.Api;

public class ProducesDefaultContentTypeAttribute : ProducesAttribute
{
    public ProducesDefaultContentTypeAttribute(Type type) : base(type)
    {
    }

    public ProducesDefaultContentTypeAttribute() : this("application/json")
    {
    }
    
    private ProducesDefaultContentTypeAttribute(string contentType, params string[] additionalContentTypes)
        : base(contentType, additionalContentTypes)
    {
    }
}