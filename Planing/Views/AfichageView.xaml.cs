using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Planing.Core.DbImport;
using Planing.Core.Models;

using Planing.Models;
using Planing.PL.Generator;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for AfichageView.xaml
    /// </summary>
    public partial class AfichageView
    {
        readonly DbModel _db = new DbModel();

        public AfichageView()
        {
            InitializeComponent();
            Application.Current.Dispatcher.Invoke(() =>
            {
                CbCategorie.ItemsSource = _db.Facultes.ToList();
                CbAs.ItemsSource = _db.AnneeScolaires.ToList();
                CbAnnee.ItemsSource = _db.Annees.ToList();
            });

        }





        private void CbArticle_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            //var res = PlaningGenerator.GenerateLectures(fid, s, asid, ProgressBar);
            //if (res.Count > 0) MessageBox.Show("don!");
            CbSection.SelectedIndex = -1;
            //CbSousCategorie.SelectedIndex = -1;
            if (CbArticle.SelectedIndex != -1)
            {
               
                try
                {
                    var item = (Specialite)CbArticle.SelectedItem;
                    var anneeScolaire = CbAs.SelectedItem as AnneeScolaire;
                    if (anneeScolaire != null)
                    {
                        var asid = anneeScolaire.Id;
                        var s = Convert.ToInt32(SemestreTxt.Text);
                        var annee = CbAnnee.SelectedItem as Annee;
                        if (annee != null)
                        {
                            var an = annee.Id;
                            CbSection.ItemsSource =
                                _db.Sections.Where(
                                    x =>
                                        x.SpecialiteId == item.Id && x.AnneeScolaireId == asid && x.Semestre == s && x.AnneeId == an)
                                    .ToList();
                        }
                    }
                    CbSection.SelectedIndex = -1;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    //ignore
                }
           
            }

        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var result =
            //    _db.Lectures.
            //        Include("Teacher").
            //        Include("Course").
            //        Include("ClassRoom").
            //        Include("Section").
            //        Include("Groupe").
            //        Where(x => x.GroupeId == null).OrderBy(x => x.SectionId).ToList();
        }

        private void CbAnnee_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private static List<String> ConstructList(List<Lecture> lectures, int j)
        {
            var result = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                var item = lectures.FirstOrDefault(w => w.Seance == j);
                result.Add(item != null ? item.Display : "");
                j ++;
            }
            return result;

        }

        private void CbSection_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            var section = CbSection.SelectedItem as Section;
            if (section != null)
            {
                var sectionId = section.Id;
              
                 GetData(sectionId);
            }
        }

        private void GetData( int sectionId)
        {
            dg.ItemsSource = null;
            var final = new List<Lecture>();
            List<Lecture> result;
            using (var db = new DbModel())
            {
                result =
                    db.Lectures.
                        Include("Teacher").
                        Include("Course").
                        Include("ClassRoom").
                        Include("Section").
                        Include("Groupe").
                        Where(x => x.SectionId == sectionId).ToList();

            }
            var dictionary = new Dictionary<string, List<string>>();
            foreach (var lecture in result.GroupBy(w => w.Seance))
            {
                int j = 0;
                string display = "";
                foreach (var lecture1 in lecture)
                {
                    var g = (lecture1.Groupe != null) ? lecture1.Groupe.Code : "";
                    if (j == 0)
                    {
                        display = lecture1.Teacher.Nom +
                                  Environment.NewLine + lecture1.Course.Code +
                                  Environment.NewLine + lecture1.ClassRoom.Code + "\n" + g;
                    }
                    else
                    {
                        display = display + "\n" + "____________" + "\n" + lecture1.Teacher.Nom +
                                  Environment.NewLine + lecture1.Course.Code +
                                  Environment.NewLine + lecture1.ClassRoom.Code + "\n" + g;
                    }
                    j++;
                }
                var firstOrDefault = lecture.FirstOrDefault();
                Lecture lec;
                lec = AutoMapper.Mapper.Map<Lecture>(firstOrDefault);
                lec.Display = display;
                final.Add(lec);
                //Environment.NewLine + g;
            }
            //var q = result.Where(w => w.Seance <= 6);
            dictionary.Add("Samedi", ConstructList(final.Where(w => w.Seance <= 6).ToList(), 1));
            dictionary.Add("Dimanche", ConstructList(final.Where(w => w.Seance >= 7 && w.Seance < 13).ToList(), 7));
            dictionary.Add("Lundi", ConstructList(final.Where(w => w.Seance >= 13 && w.Seance < 19).ToList(), 13));
            dictionary.Add("Mardi", ConstructList(final.Where(w => w.Seance >= 19 && w.Seance < 25).ToList(), 19));
            dictionary.Add("Mercredi", ConstructList(final.Where(w => w.Seance >= 25 && w.Seance < 31).ToList(), 25));
            dictionary.Add("Jeudi", ConstructList(final.Where(w => w.Seance >= 31).ToList(), 31));
            dg.ItemsSource = dictionary;
        }

        //private Dictionary<string, List<string>> Dictionary(int fid, int secid, int sectionId, int s, int asid, int an)
        //{
        //    var result =
        //        _db.Lectures.
        //            Include("Teacher").
        //            Include("Course").
        //            Include("ClassRoom").
        //            Include("Section").
        //            Include("Groupe").
        //            Where(x => x.FaculteId == fid
        //                       && x.SpecialiteId == secid && x.SectionId == sectionId
        //                       && x.Section.Semestre == s
        //                       && x.Section.AnneeScolaireId == asid
        //                       && x.AnneeId == an).ToList();
        //    var dictionary = new Dictionary<string, List<string>>();
        //    foreach (var lecture in result.GroupBy(w=>w.Seance))
        //    {
        //        //var g = (lecture.Groupe != null) ? lecture.Groupe.Code : lecture.Section.Code;
        //        foreach (var lecture1 in lecture)
        //        {
        //            lecture1.Display =lecture1.Display + lecture1.Teacher.Nom +
        //                          Environment.NewLine + lecture1.Course.Code +
        //                          Environment.NewLine + lecture1.ClassRoom.Code;
        //        }
        //        //Environment.NewLine + g;
        //    }
        //    //var q = result.Where(w => w.Seance <= 6);
        //    dictionary.Add("Samedi", ConstructList(result.Where(w => w.Seance <= 6).ToList(), 1));
        //    dictionary.Add("Dimanche", ConstructList(result.Where(w => w.Seance >= 7 && w.Seance < 13).ToList(), 7));
        //    dictionary.Add("Lundi", ConstructList(result.Where(w => w.Seance >= 13 && w.Seance < 19).ToList(), 13));
        //    dictionary.Add("Mardi", ConstructList(result.Where(w => w.Seance >= 19 && w.Seance < 25).ToList(), 19));
        //    dictionary.Add("Mercredi", ConstructList(result.Where(w => w.Seance >= 25 && w.Seance < 31).ToList(), 25));
        //    dictionary.Add("Jeudi", ConstructList(result.Where(w => w.Seance >= 31).ToList(), 31));
        //    return dictionary;
        //}

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbArticle.SelectedIndex = -1;
            if (CbCategorie.SelectedIndex != -1)
            {
                var f = CbCategorie.SelectedItem as Faculte;
                CbArticle.ItemsSource = _db.Specialites.Where(x=>x.FaculteId ==f.Id).ToList();
            }
        }

        private void PrintButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fid = (CbCategorie.SelectedItem as Faculte);
            var asid = (CbAs.SelectedItem as AnneeScolaire);
            var secid = (CbArticle.SelectedItem as Specialite);
            var s = !string.IsNullOrEmpty(SemestreTxt.Text)?Convert.ToInt32(SemestreTxt.Text):0;
            var an = (CbAnnee.SelectedItem as Annee);
            var sectionId = (CbSection.SelectedItem as Section);
            //var gId = (CbSousCategorie.SelectedItem as Groupe);
            var r = (Dictionary<string, List<string>>)dg.ItemsSource;

            if (an == null) return;
            if (secid == null) return;
            if (sectionId == null) return;
            if (asid == null) return;
            if (fid == null) return;
            System.Data.DataTable table = DbAcceess.ToDictionary(r, an.Name, s, secid.Name, sectionId.Name, asid.Name, fid.Libelle);
            var frmReport = new AffReportWind(table);
            frmReport.Show();
        }

        private void SchButton_OnClick(object sender, RoutedEventArgs e)
        {
            var faculte = CbCategorie.SelectedItem as Faculte;
            if (faculte != null)
            {
                var fid = faculte.Id;
                var anneeScolaire = CbAs.SelectedItem as AnneeScolaire;
                if (anneeScolaire != null)
                {
                    var asid = anneeScolaire.Id;
                    var specialite = CbSection.SelectedItem as Section;
                    if (specialite != null)
                    {
                        var secid = specialite.Id;
                        var s = Convert.ToInt32(SemestreTxt.Text);
                        var annee = CbAnnee.SelectedItem as Annee;
                        if (annee != null)
                        {
                            var an = annee.Id;
                            var sx = new SchedulGenerator();
                            var frm = new SchduemWInd(sx.PopulateList(fid, an, asid, secid, s));
                            frm.UpdateDataDg += GetData;
                            frm.Show();
                                            
                        }
                    }
                }
            }
        }
    }
}
