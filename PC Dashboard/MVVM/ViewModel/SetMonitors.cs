using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsDisplayAPI;
using WindowsDisplayAPI.DisplayConfig;
using WindowsDisplayAPI.Native.DisplayConfig;

namespace PC_Dashboard.MVVM.ViewModel
{
    public class SetMonitors
    {
        public static void DisableAllMonitorsExceptSelected(Display displayToSet)
        {
            var displays = Display.GetDisplays();

            foreach (var display in displays)
            {
                if(display.DisplayName != displayToSet.DisplayName)
                {
                    Debug.WriteLine(display.DisplayName);
                }
            }
        }
    }
}
