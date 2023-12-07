using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls.Models
{
    public record AxisRange(double Min, double Max)
    {
        public double Span => Max - Min;
    }
}
