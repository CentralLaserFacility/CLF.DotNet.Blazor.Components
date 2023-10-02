using Clf.Common.ImageProcessing;

namespace Clf.Blazor.Basic.Components.Controls.Models
{
    public record LedState(string? Label = null, Colour? Color = null, string? IconPath = null)
    {
        public string Label { get; init; } = Label ?? "";
        public Colour Color { get; init; } = Color ?? MultiStateLedStyle.DEFAULT_COLOR;
        public string IconPath { get; init; } = IconPath ?? "";
    }
}
