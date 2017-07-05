using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class ButtonModel
    {
        public string DataTarget { get; set; }

        public ButtonType Type { get; set; }

        public string Name { get; set; }

        public string OnClick { get; set; }

        public string Title { get; set; }

        public string Id { get; set; }

        public string Css { get; set; }

        public bool Disabled { get; set; }

        public bool Visible { get; set; } = true;
    }

    public enum ButtonType
    {
        Save,
        Cancel,
        Approve, 
        Reject,
        Remove,
        Edit,
        Add
    }
}
