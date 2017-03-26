using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Planing.Core.Models;
using Planing.Models;
using Planing.ModelView;
using Planing.PL.Generator;
using Planing.UI.Helpers;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for GenePlan.xaml
    /// </summary>
    public partial class GenePlan
    {
        readonly DbModel _db = new DbModel();
        public GenePlan()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.Facultes.ToList();
            CbAs.ItemsSource = _db.AnneeScolaires.ToList();
            //CbArticle.ItemsSource = _db.Specialites.ToList();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fid = (CbCategorie.SelectedItem as Faculte);
            var asid = (CbAs.SelectedItem as AnneeScolaire);
          //  var secid = (CbArticle.SelectedItem as Specialite).Id;
            
            int resltatCount;

            var s =(!string.IsNullOrEmpty(SemestreTxt.Text))? Convert.ToInt32(SemestreTxt.Text):0;
            if (fid == null || asid == null || s == 0)
            {
                MessageBox.Show("remplir les champs vides!", "Warning", MessageBoxButton.OK,
                 MessageBoxImage.Warning);
                return;
            } 
            var count = _db.Lectures.Count(x => x.FaculteId == fid.Id

                                                && x.Section.Semestre == s
                                                && x.Section.AnneeScolaireId == asid.Id
                );
           

            if ( count!= 0)
            {
                ProgressBar.Minimum = 0;
                ProgressBar.Maximum = count;
                var pBar= new PBar(ProgressBar);

                var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
                if (!result.ToString().Equals("Yes")) return;
                string sp = "Suppression de l'ancien planing ";
                foreach (var source in _db.Lectures.Where(x => x.FaculteId == fid.Id

                                        && x.Section.Semestre == s
                                        && x.Section.AnneeScolaireId == asid.Id
                ))
                {
                    UpdateStatuText(sp + "--");
                    _db.Entry(source).State = EntityState.Deleted;
                    _db.SaveChanges();
                    pBar.IncPb();
                }var sc = _db.ClassSeances.Where(x => x.Semestre == s
                                                     && x.AnneeScolaireId == asid.Id);
                ProgressBar.Minimum = 0;
                ProgressBar.Maximum = sc.Count();
                foreach (var classSeance in sc)
                {
                    UpdateStatuText(sp + "--");
                    _db.Entry(classSeance).State = EntityState.Deleted;
                    _db.SaveChanges();
                    pBar.IncPb();
                }
                PlaningGenerator.UpdateDataDg += UpdateStatuText;
                resltatCount=PlaningGenerator.GeneratePopulations(fid.Id, s, asid.Id, ProgressBar, GridStat.DataContext as UpdateStatus );
            }
            else
            {
                PlaningGenerator.UpdateDataDg += UpdateStatuText;
                resltatCount = PlaningGenerator.GeneratePopulations(fid.Id, s, asid.Id, ProgressBar, GridStat.DataContext as UpdateStatus);
            }

            if (resltatCount > 0) MessageBox.Show("Génération de planing est terminée");
            //foreach (var nosolution in nosolutions)
            //{
            //    _db.Lectures.Add(nosolution);
            //    _db.SaveChanges();
            //}
            DataGrid.ItemsSource = _db.Lectures.
                            Include("Teacher").
                            Include("Course").
                            Include("ClassRoom").
                            Include("Section").
                            Include("Groupe").
                            Where(x => x.FaculteId == fid.Id
                                     &&x.Solved ==false
                                       && x.Section.Semestre == s
                                       && x.Section.AnneeScolaireId == asid.Id
                                       ).ToList();
        }

        private void UpdateStatuText(string mesage)
        {
            TextBlock.Text = mesage;
        }
    }
}
