using System;
using System.Collections.Generic;

namespace Planing.Views
{
    public class ConvertToDay
    {
        public int Seance { get; set; }
        public string Jour { get; set; }
        public string Time { get; set; }

        public static IEnumerable<ConvertToDay> ListConvertToDays()
        {
            var result = new List<ConvertToDay>();
          
            for (double i = 1; i < 37; i++) 
            {
                double r = Convert.ToDouble(i / 6);
                if (r <= 1)
                {
                    var item = new ConvertToDay();
                    item.Jour = "Samedi";
                    item.Seance = Convert.ToInt32(i)
                        ;
                    switch (Convert.ToInt32(i))
                    {
                        case 1:
                        {
                            item.Time = "08:00 - 09:30";

                        }
                            break;
                        case 2:
                        {
                            item.Time = "09:30 - 11:00";
                        }
                            break;
                        case 3:
                        {
                            item.Time = "11:00 - 12:30";
                        }
                            break;
                        case 4:
                        {
                            item.Time = "12:30 - 14:00";
                        }
                            break;
                        case 5:
                        {
                            item.Time = "14:00 - 15:30";
                        }
                            break;
                        case 6:
                        {
                            item.Time = "15:30 - 17:00";
                        }
                            break;
                    }
                    result.Add(item);
                }
                else if (r <= 2)
                {
                    var item = new ConvertToDay();
                    item.Jour = "Dimanche";
                    item.Seance = Convert.ToInt32(i);
                    switch (Convert.ToInt32(i))
                    {
                        case 7:
                        {
                            item.Time = "08:00 - 09:30";
                        }
                            break;
                        case 8:
                        {
                            item.Time = "09:30 - 11:00";
                        }
                            break;
                        case 9:
                        {
                            item.Time = "11:00 - 12:30";
                        }
                            break;
                        case 10:
                        {
                            item.Time = "12:30 - 14:00";
                        }
                            break;
                        case 11:
                        {
                            item.Time = "14:00 - 15:30";
                        }
                            break;
                        case 12:
                        {
                            item.Time = "15:30 - 17:00";
                        }
                            break;
                    }
                    result.Add(item);
                }

                else if (r <= 3)
                {
                    var item = new ConvertToDay();
                    item.Jour = "Lundi";
                    item.Seance = Convert.ToInt32(i);
                    switch (Convert.ToInt32(i))
                    {
                        case 13:
                        {
                            item.Time = "08:00 - 09:30";
                        }
                            break;
                        case 14:
                        {
                            item.Time = "09:30 - 11:00";
                        }
                            break;
                        case 15:
                        {
                            item.Time = "11:00 - 12:30";
                        }
                            break;
                        case 16:
                        {
                            item.Time = "12:30 - 14:00";
                        }
                            break;
                        case 17:
                        {
                            item.Time = "14:00 - 15:30";
                        }
                            break;
                        case 18:
                        {
                            item.Time = "15:30 - 17:00";
                        }
                            break;
                    }
                    result.Add(item);
                }
                else if (r <= 4)
                {
                    var item = new ConvertToDay();
                    item.Jour = "Mardi";
                    item.Seance = Convert.ToInt32(i);
                    switch (Convert.ToInt32(i))
                    {
                        case 19:
                        {
                            item.Time = "08:00 - 09:30";
                        }
                            break;
                        case 20:
                        {
                            item.Time = "09:30 - 11:00";
                        }
                            break;
                        case 21:
                        {
                            item.Time = "11:00 - 12:30";
                        }
                            break;
                        case 22:
                        {
                            item.Time = "12:30 - 14:00";
                        }
                            break;
                        case 23:
                        {
                            item.Time = "14:00 - 15:30";
                        }
                            break;
                        case 24:
                        {
                            item.Time = "15:30 - 17:00";
                        }
                            break;
                    }
                    result.Add(item);
                }
                else if (r <= 5)
                {
                    var item = new ConvertToDay();
                    item.Jour = "Mercredi";
                    item.Seance = Convert.ToInt32(i);
                    switch (Convert.ToInt32(i))
                    {
                        case 25:
                        {
                            item.Time = "08:00 - 09:30";
                        }
                            break;
                        case 26:
                        {
                            item.Time = "09:30 - 11:00";
                        }
                            break;
                        case 27:
                        {
                            item.Time = "11:00 - 12:30";
                        }
                            break;
                        case 28:
                        {
                            item.Time = "12:30 - 14:00";
                        }
                            break;
                        case 29:
                        {
                            item.Time = "14:00 - 15:30";
                        }
                            break;
                        case 30:
                        {
                            item.Time = "15:30 - 17:00";
                        }
                            break;
                    }
                    result.Add(item);
                }
                else if (r <= 6)
                {
                    var item = new ConvertToDay();
                    item.Jour = "Jeudi";
                    item.Seance = Convert.ToInt32(i);
                    switch (Convert.ToInt32(i))
                    {
                        case 31:
                        {
                            item.Time = "08:00 - 09:30";
                        }
                            break;
                        case 32:
                        {
                            item.Time = "09:30 - 11:00";
                        }
                            break;
                        case 33:
                        {
                            item.Time = "11:00 - 12:30";
                        }
                            break;
                        case 34:
                        {
                            item.Time = "12:30 - 14:00";
                        }
                            break;
                        case 35:
                        {
                            item.Time = "14:00 - 15:30";
                        }
                            break;
                        case 36:
                        {
                            item.Time = "15:30 - 17:00";
                        }
                            break;
                    }
                    result.Add(item);
                }

            }
            return result;
        } 
    }
}
