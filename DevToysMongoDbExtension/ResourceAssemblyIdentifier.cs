using System.ComponentModel.Composition;
using DevToys.Api;

namespace DevToysMongoDbExtension;

[Export(typeof(IResourceAssemblyIdentifier))]
[Name(nameof(ResourceAssemblyIdentifier))]
internal sealed class ResourceAssemblyIdentifier : IResourceAssemblyIdentifier
{
    public ValueTask<FontDefinition[]> GetFontDefinitionsAsync()
    {
        return new ValueTask<FontDefinition[]>(Array.Empty<FontDefinition>());
    }
}