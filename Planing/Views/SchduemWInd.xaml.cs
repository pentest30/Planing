using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Internal.Implementations;
using Planing.Core.Models;
using Planing.Models;
using Planing.ModelView;
using Planing.PL.Generator;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for SchduemWInd.xaml
    /// </summary>
    public partial class SchduemWInd
    {
        private  BindingList<LectureModelView> _bindingList;
        public delegate void UpdateDg(int id );
        public UpdateDg UpdateDataDg;
        readonly DbModel _db = new DbModel();
        Appointment _selecteItem = new AppointmentInstance();
        public SchduemWInd(BindingList<LectureModelView> list )
        {
            InitializeComponent();
            _bindingList = list;
            //Scheduler.MonthView.CompressWeekend = false;
            Scheduler.MonthView.WeekCount = 1;
            Scheduler.Storage.AppointmentStorage.DataSource = list;
        }

      
        


        private void Scheduler_AppointmentDrop(object sender, AppointmentDragEventArgs e)
        {
            DateTime t = e.HitInterval.Start;
            int seance = SchedulGenerator.GetSeance(t);
            _selecteItem = Scheduler.SelectedAppointments[0];
            var uniquId = Convert.ToInt64(_selecteItem.Id);
            var item = _db.Lectures.Include("Section").FirstOrDefault(x => x.Id == uniquId);
            var seances = new List<int>();
            if (item != null && item.GroupeId != null)
            {
                seances =
                    _db.Lectures.Where(
                        x => x.SectionId == item.SectionId && (x.GroupeId == null) || 
                            (x.GroupeId == item.GroupeId) ||(x.TeacherId ==item.TeacherId))
                        .Select(s => s.Seance)
                        .Distinct()
                        .ToList();
            }
            else if (item != null && item.GroupeId == null)
            {
                seances = _db.Lectures.Where(x => x.SectionId == item.SectionId)
                    .Select(s => s.Seance)
                    .Distinct()
                    .ToList();

            }
            if (seances.Any(x => x == seance) ||seance==0)
            {
                e.Allow = false;
            }
            else
            {
                var classType = _db.ClassRooms.FirstOrDefault(x => x.Id == item.ClassRoomId);
                var classes = _db.ClassSeances.Count(x => x.Seance == seance && x.ClassRoomId == item.ClassRoomId)>0?
                    _db.ClassSeances.Where(x => x.Seance == seance && x.ClassRoomId == item.ClassRoomId).ToList():
                    _db.ClassSeances.Where(x => x.Seance == seance && x.ClassRoomTypeId == classType.ClassRoomTypeId).ToList();
                if (!classes.Any()) e.Allow = false;
                else
                {
                    var rnd = new Random();
                    var r = rnd.Next(0, classes.Count( ));
                    var cs = classes[r];
                    var cs2 = new ClassSeance();
                    if (item != null)
                    {
                        if (classType != null) cs2.ClassRoomTypeId = Convert.ToInt32(classType.ClassRoomTypeId);
                        cs2.ClassRoomId = Convert.ToInt32(item.ClassRoomId);
                        cs2.Seance = item.Seance;
                        cs2.AnneeScolaireId = item.Section.AnneeScolaireId;
                        cs2.Semestre = Convert.ToInt32(item.Section.Semestre);
                        _db.ClassSeances.Add(cs2);
                        item.Seance = seance;
                        item.ClassRoomId = cs.ClassRoomId;
                        _db.Entry(cs).State = EntityState.Deleted;
                        _db.Entry(item).State = EntityState.Modified;
                        try
                        {
                            _db.SaveChanges();
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                            e.Allow = false;
                        }
                        var sg = new SchedulGenerator();
                        _bindingList = new BindingList<LectureModelView>();
                        _bindingList = sg.PopulateList(item.SectionId);
                        Scheduler.Storage.AppointmentStorage.DataSource = _bindingList;
                        if (UpdateDataDg != null) UpdateDataDg(item.SectionId);

                    }
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            _db.Dispose();
        }
    }
}
