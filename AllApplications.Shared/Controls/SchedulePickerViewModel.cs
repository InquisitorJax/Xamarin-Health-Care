using System;
using Xamarin.Forms;

namespace Core.Controls
{
    public interface IScheduleAppointment
    {
        Color Color { get; }
        DateTime? EndDateTime { get; set; }

        //use mappings for syncfusion schedule control

        string Name { get; set; }
        DateTime? StartDateTime { get; set; }
    }
}