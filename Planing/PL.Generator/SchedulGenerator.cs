using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Planing.Core.Models;
using Planing.ModelView;

namespace Planing.PL.Generator
{
    public class SchedulGenerator
    {
        #region Properties
        readonly DbModel _db = new DbModel();
        public BindingList<LectureModelView> LectureModelViews { get; set; }
        #endregion

        #region Functions
        public  BindingList<LectureModelView> PopulateList(int fId,int an, int a,  int section , int semestre)
        {
            LectureModelViews = new BindingList<LectureModelView>();
            var result =
                _db.Lectures.
                    Include("Teacher").
                    Include("Course").
                    Include("ClassRoom").
                    Include("Section").
                    Include("Groupe").
                    Where(x => x.FaculteId == fId
                       && x.Section.Semestre == semestre
                       && x.Section.AnneeScolaireId == a
                       && x.AnneeId == an
                       && x.SectionId == section).OrderBy(w=>w.Seance).ThenBy(x=>x.CourseId).ToList();
            int j = 1;
            foreach (var lecture in result.GroupBy(w=>w.CourseId))
            {
               
                foreach (var lecture1 in lecture)
                {
                    var item =(CreateEvent(lecture1));
                    item.Label = j;
                    LectureModelViews.Add(item);
                }
               if (j == 10) j = 0;
               
                j++;
            }
            return LectureModelViews;

        }
        public BindingList<LectureModelView> PopulateList(ObservableCollection<Lecture> result )
        {
            LectureModelViews = new BindingList<LectureModelView>();
            
            int j = 1;
            foreach (var lecture in result.GroupBy(w => w.CourseId))
            {

                foreach (var lecture1 in lecture)
                {
                    var item = (CreateEvent(lecture1));
                    item.Label = j;
                    LectureModelViews.Add(item);
                }
                if (j == 10) j = 0;

                j++;
            }
            return LectureModelViews;

        }
        public BindingList<LectureModelView> PopulateList(int section)
        {
            LectureModelViews = new BindingList<LectureModelView>();
            var result =
                _db.Lectures.
                    Include("Teacher").
                    Include("Course").
                    Include("ClassRoom").
                    Include("Section").
                    Include("Groupe").
                    Where(x =>
                       x.SectionId == section).OrderBy(w => w.Seance).ThenBy(x => x.CourseId).ToList();
            int j = 1;
            foreach (var lecture in result.GroupBy(w => w.CourseId))
            {

                foreach (var lecture1 in lecture)
                {
                    var item = (CreateEvent(lecture1));
                    item.Label = j;
                    LectureModelViews.Add(item);
                }
                if (j == 10) j = 0;

                j++;
            }
            return LectureModelViews;

        }
        public static int GetSeance(DateTime t)
        {
            int seance =0;
            if (t.Hour == 8 && (t.Minute == 0 ||t.Minute==30))
            {
                switch (t.Day)
                {
                    case 1:
                        {
                            seance = 1;
                        }
                        break;
                    case 2:
                        {
                            seance = 7;
                        }
                        break;
                    case 3:
                        {
                            seance = 13;
                        }
                        break;
                    case 4:
                        {
                            seance = 19;
                        }
                        break;
                    case 5:
                        {
                            seance = 25;
                        }
                        break;
                    case 6:
                        {
                            seance = 31;
                        }
                        break;
                }
            }
            else if ((t.Hour == 9&&t.Minute==30)  || (t.Hour == 10 && (t.Minute == 0)))
            {
                switch (t.Day)
                {
                    case 1:
                        {
                            seance = 2;
                        }
                        break;
                    case 2:
                        {
                            seance = 8;
                        }
                        break;
                    case 3:
                        {
                            seance = 14;
                        }
                        break;
                    case 4:
                        {
                            seance = 20;
                        }
                        break;
                    case 5:
                        {
                            seance = 26;
                        }
                        break;
                    case 6:
                        {
                            seance = 32;
                        }
                        break;
                }
            }
            else if (t.Hour == 11 && (t.Minute == 0 || t.Minute == 30))
            {
                switch (t.Day)
                {
                    case 1:
                        {
                            seance = 3;
                        }
                        break;
                    case 2:
                        {
                            seance = 9;
                        }
                        break;
                    case 3:
                        {
                            seance = 15;
                        }
                        break;
                    case 4:
                        {
                            seance = 21;
                        }
                        break;
                    case 5:
                        {
                            seance = 27;
                        }
                        break;
                    case 6:
                        {
                            seance = 33;
                        }
                        break;
                }
            }
            else if ((t.Hour == 12 && t.Minute == 30) ||(t.Hour==13&& t.Minute==00))
            {
                switch (t.Day)
                {
                    case 1:
                        {
                            seance = 4;
                        }
                        break;
                    case 2:
                        {
                            seance = 10;
                        }
                        break;
                    case 3:
                        {
                            seance = 16;
                        }
                        break;
                    case 4:
                        {
                            seance = 22;
                        }
                        break;
                    case 5:
                        {
                            seance = 28;
                        }
                        break;
                    case 6:
                        {
                            seance = 34;
                        }
                        break;
                }
            }
            else if (t.Hour == 14 && (t.Minute == 0 || t.Minute == 30))
            {
                switch (t.Day)
                {
                    case 1:
                        {
                            seance = 5;
                        }
                        break;
                    case 2:
                        {
                            seance = 11;
                        }
                        break;
                    case 3:
                        {
                            seance = 17;
                        }
                        break;
                    case 4:
                        {
                            seance = 23;
                        }
                        break;
                    case 5:
                        {
                            seance = 29;
                        }
                        break;
                    case 6:
                        {
                            seance = 35;
                        }
                        break;
                }
            }
            else if ((t.Hour == 15 &&t.Minute==30)|| (t.Hour == 16 && t.Minute == 00))
            {
                switch (t.Day)
                {
                    case 1:
                        {
                            seance = 6;
                        }
                        break;
                    case 2:
                        {
                            seance = 12;
                        }
                        break;
                    case 3:
                        {
                            seance = 18;
                        }
                        break;
                    case 4:
                        {
                            seance = 24;
                        }
                        break;
                    case 5:
                        {
                            seance = 30;
                        }
                        break;
                    case 6:
                        {
                            seance = 36;
                        }
                        break;
                }
            }
            return seance;
        }
        private LectureModelView CreateEvent(Lecture lecture)
        {
            var apt = new LectureModelView();
            var g = (lecture.Groupe != null) ? lecture.Groupe.Code : "";
            apt.Subject = lecture.TeacherId +
                          Environment.NewLine + lecture.CourseId;
                         // Environment.NewLine + lecture.ClassRoom.Code + "\n" + g;
            apt.SectionId = lecture.SectionId;
            apt.CourseId = Convert.ToInt32(lecture.ClassRoomId);
            apt.TeacherId = lecture.TeacherId;
            apt.Id = (lecture.Id);

            double d = Convert.ToDouble(lecture.Seance / 6.00);
            int month = FindMonth();
          
            if (d <= 1)
            {
                switch (lecture.Seance)
                {
                    case 1:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,1, 8, 0,0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month,1, 9, 30,0);
                    }
                        break;
                    case 2:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,1, 9, 30,0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 1, 11, 0, 0);
                    }
                        break;
                    case 3:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 1, 11, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 1, 12, 30, 0);
                    }
                        break;
                    case 4:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 1, 12, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 1, 14, 00, 0);
                    }
                        break;
                    case 5:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 1, 14, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 1, 15, 30, 0);
                    }
                        break;
                    case 6:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 1, 15, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 1, 17, 0, 0);
                    }
                        break;
                  
                }
            }
            else if (d <= 2)
            {
                switch (lecture.Seance)
                {
                    case 7:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 2, 8, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 2, 9, 30, 0);
                    }
                        break;
                    case 8:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 2, 9, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 2, 11, 00, 0);
                    }
                        break;
                    case 9:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 2, 11, 00, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 2, 12, 30, 0);
                    }
                        break;
                    case 10:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 2, 12, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 2, 14, 00, 0);
                    }
                        break;
                    case 11:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 2, 14, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 2, 15, 30, 0);
                    }
                        break;
                    case 12:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 2, 15, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 2, 17, 00, 0);
                    }
                        break;

                }
            }
            else if (d <= 3)
            {
                switch (lecture.Seance)
                {
                    case 13:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 3, 8, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 3, 9, 30, 0);
                    }
                        break;
                    case 14:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 3, 9, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 3, 11, 0, 0);
                    }
                        break;
                    case 15:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 3, 11, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month,3, 12, 30, 0);
                    }
                        break;
                    case 16:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 3, 12, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 3, 14, 00, 0);
                    }
                        break;
                    case 17:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,3, 14, 00, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month,3, 15, 30, 0);
                    }
                        break;
                    case 18:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 3, 15, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 3, 17, 00, 0);
                    }
                        break;

                }
            }
            else if (d <= 4)
            {
                switch (lecture.Seance)
                {
                    case 19:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,4, 8, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 4, 9, 30, 0);
                    }
                        break;
                    case 20:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 4, 9, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 4, 11, 0, 0);
                    }
                        break;
                    case 21:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 4, 11, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 4, 12, 30, 0);
                    }
                        break;
                    case 22:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 4, 12, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 4,  14, 0,0);
                    }
                        break;
                    case 23:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 4, 14, 00, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 4, 15, 30, 0);
                    }
                        break;
                    case 24:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 4, 15, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 4, 17, 00, 0);
                    }
                        break;

                }
            }
            else if (d <=5)
            {
                switch (lecture.Seance)
                {
                    case 25:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,5, 8, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 5, 9, 30, 0);
                    }
                        break;
                    case 26:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 5, 9, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 5, 11, 0, 0);
                    }
                        break;
                    case 27:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 5, 11, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 5, 12, 30, 0);
                    }
                        break;
                    case 28:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 5, 12, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 5,  14, 0,0);
                    }
                        break;
                    case 29:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 5, 14, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month,5, 15, 30, 0);
                    }
                        break;
                    case 30:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 5, 15, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 5, 17, 00, 0);
                    }
                        break;

                }
            }
            else if (d <= 6)
            {
                switch (lecture.Seance)
                {
                    case 31:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 6, 8, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 6, 9, 30, 0);
                    }
                        break;
                    case 32:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,6, 9, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 6, 11, 0, 0);
                    }
                        break;
                    case 33:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 6, 11, 0, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 6, 12, 30, 0);
                    }
                        break;
                    case 34:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 6, 12, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month,6, 14, 0,0);
                    }
                        break;
                    case 35:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month, 6, 14, 00, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 6, 15, 30, 0);
                    }
                        break;
                    case 36:
                    {
                        apt.StartTime = new DateTime(DateTime.Now.Year, month,  6, 15, 30, 0);
                        apt.EndTime = new DateTime(DateTime.Now.Year, month, 6, 17, 00, 0);
                    }
                        break;

                }
            }
            //apt.StartTime = lecture
          
            return apt;
        }

        private int FindMonth()
        {
            var permitedDays= new int[3] ;

            for (int i = 1; i < 13; i++)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, i);
                switch (days)
                {
                    case 28:
                        permitedDays = new[] {1, 2, 28};
                        break;

                    case 29:
                        permitedDays = new[] { 1, 2, 29 };
                        break;

                    case 30:
                        permitedDays = new[] { 1, 2, 30 };
                        break;

                    case 31:
                        permitedDays = new[] { 1, 2, 31 };
                        break;
                }
                foreach (var t in permitedDays)
                {
                    var d = new DateTime(DateTime.Now.Year, i, t).ToLocalTime();
                    var w = GetWeekNumberOfMonth(d);
                    if (w == 1&& (t==1||t==2)) return i;
                    if (w == 1 && (t == 28 || t == 29 || t == 30 || t == 31))
                    {
                        return i + 1;
                    }
                }
               

                
            }
            return 0;
        }
        //from http://stackoverflow.com/questions/23353703/calculate-the-number-of-weeks-in-a-month
        static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            var firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
        #endregion
    }
}