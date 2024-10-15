using System.ComponentModel.Composition;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevToys.Api;
using static DevToys.Api.GUI;

namespace DevToysMongoDbExtension;

[Export(typeof(IGuiTool))]
[Name("MongoDb Extension")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uF658',
    GroupName = PredefinedCommonToolGroupNames.Generators,
    ResourceManagerAssemblyIdentifier = nameof(ResourceAssemblyIdentifier),
    ResourceManagerBaseName = "DevToysMongoDbExtension.DevToysMongoDbExtension",
    ShortDisplayTitleResourceName = nameof(DevToysMongoDbExtension.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(DevToysMongoDbExtension.LongDisplayTitle),
    AccessibleNameResourceName = nameof(DevToysMongoDbExtension.AccessibleName)
    )]
internal sealed class MongoDbTab : IGuiTool
{
    private readonly IUIButton _generateButton = Button();
    private readonly IUIMultiLineTextInput _output = MultiLineTextInput();
    private readonly IUINumberInput _numberInput = NumberInput();
    private readonly IUIInfoBar _infoBar = InfoBar();
    private HashSet<string> _objectIds = new();
    private int _objectIdAmount = 3;

    public MongoDbTab()
    {
        GenerateObjectIds();
    }

    public UIToolView View => new(
        Stack()
            .Vertical()
            .WithChildren(
                _infoBar.Title("Error").Error(),
                _generateButton.Text("Generate ObjectId")
                    .OnClick(GenerateObjectIds),
                _numberInput.Title("Amount of ObjectId's").Minimum(1).Value(_objectIdAmount)
                    .Maximum(int.MaxValue).OnValueChanged(OnNumberInputValueChanged),
                _output.Title("ObjectId's:").Language("plaintext")
                    .ReadOnly().AutoLineNumber().AlwaysWrap().Extendable())
        );

    private void OnNumberInputValueChanged(double obj)
    {
        if (!int.TryParse(obj.ToString(CultureInfo.InvariantCulture), out _objectIdAmount))
        {
            _infoBar.Show();
        }
        // Generate new ObjectId's
        GenerateObjectIds();
    }
    
    /// <summary>
    /// Generate the given amount of ObjectId's
    /// </summary>
    private void GenerateObjectIds()
    {
        _objectIds.Clear();
        for (var i = 0; i < _objectIdAmount; i++)
        {
            _objectIds.Add(ObjectIdGenerator.Generate());
        }

        var outVal = _objectIds.Aggregate("", (current, objectId) => current + (objectId + "\n"));

        _output.Text(outVal);
    }
    
    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        // Not used
    }
}
