using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Planing.Core.Models;
using Planing.Models;
using Planing.ModelView;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for TcWind.xaml
    /// </summary>
    public partial class TcWind
    {
        readonly DbModel _db = new DbModel();
        public delegate void UpdateDg();
        public UpdateDg UpdateDataDg;
        private readonly int _semestre;
        private readonly int _anSc;

        public TcWind(int id, int fid, int anS, int s)
        {
            InitializeComponent();
            _semestre = s;
            _anSc = anS;
            CbAnnee.ItemsSource = _db.Annees.ToList();
            CbCategorie.ItemsSource = _db.Specialites.Where(x => x.FaculteId == fid).ToList();
            CbOptions.ItemsSource = PeriodeOption();
            CbTypeCourse.ItemsSource = _db.ClassRoomTypes.ToList();
            CbEnseignant.ItemsSource = _db.Teachers.Where(x => x.FaculteId == fid).ToList();
            Grid.DataContext = (id == 0)
                ? new TcViewModel()
                : AutoMapper.Mapper.Map<TcViewModel>(_db.Tcs.Include("Teacher").
                    Include("Course").
                    Include("Section").Include("Section.Specialite").
                    Include("Groupe").Include("ClassRoomType").
                    Include("AnneeScolaire").FirstOrDefault(x => x.Id == id));
        }

        private Dictionary<int, string> PeriodeOption()
        {
            var dc = new Dictionary<int, string>();
            dc.Add(0, "Toute la journé");
            dc.Add(1, "1 ere demi journé");
            dc.Add(2, "2 eme demi journé");
            return dc;
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CbArticle_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbSousCategorie.SelectedIndex = -1;
            if (CbArticle.SelectedIndex != -1)
            {
                var item = CbArticle.SelectedItem as Section;
                CbSousCategorie.ItemsSource = _db.Groupes.Where(x => x.SectionId == item.Id).ToList();
            }
        }

        private void CbCours_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCours.SelectedIndex != -1)
            {
                var item = CbCours.SelectedItem as Course;
                var item2 = CbCategorie.SelectedItem as Specialite;
                if (item != null)
                {
                    CbArticle.ItemsSource = _db.Sections.Include("AnneeScolaire").Where(x =>
                     x.SpecialiteId == item2.Id
                     &&x.Semestre ==_semestre&& x.AnneeScolaireId == _anSc
                    ).ToList();
                }
            }
        }

        private void CbAnnee_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbAnnee.SelectedIndex != -1)
            {
                var item = CbAnnee.SelectedItem as Annee;
                var item2 = CbCategorie.SelectedItem as Specialite;
                if (item != null && item2 != null)
                {
                    CbCours.ItemsSource =
                        _db.Courses.Where(x => x.AnneeId == item.Id && item2.Id == x.SpecialiteId && x.Semestre == _semestre).ToList();
                }
                else
                {
                    MessageBox.Show("Selectionner l'année et la specialité");
                    //return;
                }
            }
        }

   

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var item1 = Grid.DataContext as TcViewModel;
            if (item1 != null)
            {
                Tc item = new Tc()
                {
                    Id = item1.Id,
                    AnneeScolaireId = item1.AnneeScolaireId,
                    ClassRoomTypeId = item1.ClassRoomTypeId,
                    Periode = item1.Periode,
                    TeacherId = item1.TeacherId,
                    CourseId = item1.CourseId,
                    SectionId = item1.SectionId,
                    GroupeId = item1.GroupeId, 
                    ScheduleWieght = item1.ScheduleWieght, 
                    Semestre = item1.Semestre

                };
                if (item1 != null) item1.Id =0;
                var firstOrDefault = _db.Sections.FirstOrDefault(x => x.Id == item.SectionId);
                if (firstOrDefault != null)
                {
                    item.AnneeScolaireId = firstOrDefault.AnneeScolaireId;
                    item.Semestre = firstOrDefault.Semestre;
                }
                if (item.ScheduleWieght == 0)
                {
                    MessageBox.Show("Nbr de seances doit etre superieur a zero");
                    return;
                }
           
                using (var db = new DbModel())
                {
                    if (item.Id == 0)
                    {
                        if (
                            db.Tcs.Any(
                                x =>
                                    x.AnneeScolaireId == item.AnneeScolaireId && x.CourseId == item.CourseId &&
                                    x.TeacherId == item.TeacherId && x.Semestre == item.Semestre &&
                                    x.SectionId == item.SectionId)&&item.GroupeId==null)
                        {
                            MessageBox.Show("L'enregistrement existe deja dans la base de données ");
                            return;
                        }
                        if (db.Tcs.Any(
                            x =>
                                x.AnneeScolaireId == item.AnneeScolaireId && x.CourseId == item.CourseId &&
                                x.TeacherId == item.TeacherId && x.Semestre == item.Semestre &&
                                x.SectionId == item.SectionId &&x.GroupeId == item.GroupeId))
                        {
                            MessageBox.Show("L'enregistrement existe deja dans la base de données ");
                            return;
                        }
                        db.Tcs.Add(item);

                    }
                    else
                    {
                        //    db.Tcs.Attach(item);
                        db.Entry(item).State = EntityState.Modified;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception exception)
                    {

                        MessageBox.Show(exception.Message);
                    }
                }
            }
            if (UpdateDataDg != null ) UpdateDataDg();
        }
    }
}
