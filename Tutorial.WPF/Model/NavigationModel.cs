using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial.WPF.Model
{
    public class NavigationModel
    {
        public NavigationModel(string viewname)
        {
            Name = viewname;
        }
        public string Name { get; set; }

    }
}
