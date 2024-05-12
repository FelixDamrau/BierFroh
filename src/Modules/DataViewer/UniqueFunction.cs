using DevLab.JmesPath.Functions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BierFroh.Modules.DataViewer;

public class UniqueFunction : JmesPathFunction
{
    public UniqueFunction() : base("Unique", 1)
    {
    }

    public override JToken Execute(params JmesPathFunctionArgument[] args)
    {
        var jArray = (JArray)args[0].Token;
        var list = JsonConvert
            .DeserializeObject<List<object>>(jArray.ToString())!
            .ToHashSet();
        return new JArray(list);
    }

    public override void Validate(params JmesPathFunctionArgument[] args)
    {
        base.Validate(args);
        var token = args[0].Token;
        if (token is not JArray)
        {
            var message = $"""
                The custom unique function expects a single array.
                The provided type was '{token.Type}'.
                """;
            throw new InvalidOperationException(message);
        }
    }
}
