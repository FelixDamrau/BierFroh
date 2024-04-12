using Microsoft.AspNetCore.Components;

namespace BierFroh.Components;
public partial class InsertToSqlTable
{
    private IEnumerable<string> headers = [];
    [Parameter]
    public IEnumerable<string> Headers
    {
        get => headers;
        set
        {
            var countDifferent = value.Count() != headers.Count();
            if (countDifferent)
            {
                activeColumnIndices = new bool[value.Count()];
            }
            if (countDifferent || value.Intersect(headers).Count() != headers.Count())
            {
                Array.Fill(activeColumnIndices, true);
                headers = value;
            }
        }
    }

    [Parameter]
    public IReadOnlyList<IList<string>> Content { get; set; } = [];

    [Parameter]
    public EventCallback<bool[]> ActiveColumnsChanged { get; set; }

    private bool[] activeColumnIndices = [];

    private async Task CheckedChanged(bool isActive, int index)
    {
        activeColumnIndices[index] = isActive;
        await ActiveColumnsChanged.InvokeAsync(activeColumnIndices);
    }
}
