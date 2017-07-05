using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompetencyMatrix.ViewModels
{
    public class ButtonPanelModel
    {
        public List<ButtonModel> Buttons { get; set; }


        public string Css { get; set; } = "pull-right";

    }
}