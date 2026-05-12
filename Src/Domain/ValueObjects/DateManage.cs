using System;
using System.Globalization;

namespace Coreapi.Domain.ValueObjects;

public class DateManage
{
    private DateManage(){}


    public DateManage(DateTime dateTime)
    {
        DateTime = dateTime;
    }

    public string getShamsi(string shamsi)
    {

        var sh = shamsi;
        PersianCalendar persianCalendar = new PersianCalendar();
        //DateTime dateT = p.ToDateTime(DateTime.Year, DateTime.Month, DateTime.Day, 0, 0, 0, 0);
       // return dateT.ToShortDateString();
       return persianCalendar.GetYear(DateTime) + "/" + persianCalendar.GetMonth(DateTime) + "/" +
              persianCalendar.GetDayOfMonth(DateTime);


    }
    public DateTime DateTime { get; set; }
}