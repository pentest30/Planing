using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using Planing.Core.Models;
using Planing.Models;
using Planing.PL.Generator;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for SchedulView.xaml
    /// </summary>
    public partial class SchedulView
    {
        private readonly DbModel _db = new DbModel();
        private List<Lecture> _lectures = new List<Lecture>();

        public SchedulView()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.Facultes.ToList();
            CbAs.ItemsSource = _db.AnneeScolaires.ToList();
            CbEnseignant.ItemsSource = _db.Teachers.ToList();


        }

        private void GridControl_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var item = GridControl.SelectedItem as Lecture;

            var seances = new List<int>();
            if (item != null && item.GroupeId != null)
            {
                seances =
                    _db.Lectures.Where(
                        x => x.SectionId == item.SectionId && (x.GroupeId == null) || (x.GroupeId == item.GroupeId)||x.TeacherId==item.TeacherId)
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
            if (item != null)
            {
                var firstOrDefault = _db.ClassRooms.FirstOrDefault(x => x.Id == item.ClassRoomId);
                if (firstOrDefault != null)
                 item.ClassRoomTypeId = Convert.ToInt32(firstOrDefault.ClassRoomTypeId);
                var results = _db.ClassSeances.Where(s => s.ClassRoomTypeId == item.ClassRoomTypeId).ToList();
                foreach (var seance in seances)
                {
                    int seance1 = seance;
                    var item2 = results.FirstOrDefault(x => x.Seance == seance1);
                    results.Remove(item2);
                }
                var times = ConvertToDay.ListConvertToDays();
                var convertToDays = times as ConvertToDay[] ?? times.ToArray();
                foreach (var classSeance in results)
                {
                    var sd = convertToDays.FirstOrDefault(x => x.Seance == classSeance.Seance);
                    if (sd != null)
                    {
                        classSeance.Jour = sd.Jour;
                        classSeance.Time = sd.Time;
                    }
                }
                GridControl2.ItemsSource = results.Distinct().ToList();
            }

           
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CbAnnee_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ab = CbAs.SelectedItem as AnneeScolaire;
            if (ab != null)
            {
                var s = string.IsNullOrEmpty(SemestreTxt.Text) ? 0 : Convert.ToInt32(SemestreTxt.Text);
                var queries =
                    _db.Lectures.Include("Teacher")
                        .Include("Course")
                        .Include("ClassRoom")
                        .Include("Section")
                        .Include("Groupe")
                        .Where(x => x.Section.AnneeScolaireId == ab.Id && x.Section.Semestre == s)
                        .ToList();
                var times = ConvertToDay.ListConvertToDays();
                var convertToDays = times as ConvertToDay[] ?? times.ToArray();
                foreach (var lecture in queries)
                {
                    var item = convertToDays.FirstOrDefault(x => x.Seance == lecture.Seance);
                    if (item != null)
                    {
                        lecture.Jour = item.Jour;
                        lecture.Time = item.Time;
                    }
                }
                try
                {
                    GridControl.ItemsSource = queries;
                    _lectures = new List<Lecture>();
                    _lectures.AddRange(queries);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    // throw;
                }
            }
        }

        private void SemestreTxt_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            CbAs.SelectedIndex = -1;
        }

        private void CbEnseignant_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CbEnseignant.SelectedItem as Teacher;

            var filterdList = _lectures;
            if (filterdList != null)
                try
                {
                    GridControl.ItemsSource = item != null ? filterdList.Where(x =>  x.TeacherId == item.Id).ToList() : _lectures;
                }
                catch (Exception)
                {
                    GridControl.ItemsSource = item == null ? _lectures : null;

                }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var item = GridControl.SelectedItem as Lecture;
            if (item==null)return;
            var sx = new SchedulGenerator();
            var frm = new SchduemWInd(sx.PopulateList(item.SectionId));
            frm.Show();
        }
    }
}
