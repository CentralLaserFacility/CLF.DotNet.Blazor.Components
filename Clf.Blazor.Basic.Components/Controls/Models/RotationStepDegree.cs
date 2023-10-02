using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Clf.Blazor.Basic.Components.Controls.Models
{
  public enum RotationStepDegree
  {
    [XmlEnum("0")]
    [Display(Name = "0")]
    _0 = 0,
    [XmlEnum("1")]
    [Display(Name = "90")]
    _90 = 1,
    [XmlEnum("2")]
    [Display(Name = "180")]
    _180 = 2,
    [XmlEnum("3")]
    [Display(Name = "-90")]
    _Minus90 = 3
  }
}
