using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Planing.Core.DbImport;
using Planing.Core.Models;

using Planing.Models;
using Planing.UI.Helpers;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for ArticleView.xaml
    /// </summary>
    public partial class ArticleView
    {

        //private readonly Repository<Article> _articleRepository;
        readonly DbModel _db = new DbModel();
        public ArticleView()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.Specialites.ToList();
            CbSousCategorie.ItemsSource = _db.Annees.ToList();
            CbTypeCourse.ItemsSource = _db.CourseTypes.ToList();
           GetDg();
        }



        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            //AddButton.Visibility = Visibility.Hidden;
            //var list = DataGrid.ItemsSource.OfType<Course>().ToList();
            //list.Add(new Course());
            //Grid.DataContext = list.Last();  
            var modFrm = new ModuleWind();
            modFrm.Show();
            modFrm.UpdateDataDg += GetDg;
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem== null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AddButton.Visibility = Visibility.Hidden;
            UpdateButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (Course)Grid.DataContext;
            if (item.Id <= 0)
            {
                _db.Courses.Add(item);
                // ((ObservableCollection<Article>)DataGrid.ItemsSource).Add(item);
            }
            else
            {
                _db.Entry(item).State = EntityState.Modified;
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erreurs pendant l'enregistrement", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //((ObservableCollection<Article>)DataGrid.ItemsSource).Remove(item);
            }

            AddButton.Visibility = Visibility.Visible;
            DataGrid.ItemsSource = _db.Courses.Include("Specialite").Include("Annee").ToList();
            var binding = new Binding { ElementName = "DataGrid", Path = new PropertyPath("SelectedItem") };
            Grid.SetBinding(DataContextProperty, binding);
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Visible;
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
            var binding = new Binding {ElementName = "DataGrid", Path = new PropertyPath("SelectedItem")};
            Grid.SetBinding(DataContextProperty, binding);
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem == null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (!result.ToString().Equals("Yes")) return;
            var deleted = DataGrid.SelectedItem as Course;
            if (deleted == null) return;
            _db.Entry(deleted).State = EntityState.Deleted;
            _db.SaveChanges();
            GetDg();
        }

        private void GetDg()
        {
            DataGrid.ItemsSource = _db.Courses.Include("Specialite").Include("Annee").ToList();
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == false) return;
            string cheminExcel = ofd.FileName;
            if (!cheminExcel.Split('\\').Last().Contains(".xlsx"))
            {
                MessageBox.Show("Le fichier que vous avez selectioné  n'est pas un fichier Excel");
                return;
            }
            var liste = DbAcceess.GetCourses(cheminExcel, 4);
            var list2 = DbAcceess.GetModule(cheminExcel, 1);
            var listOfCourses = new List<Course>();
            if (liste == null || list2 == null) return;
            int q = 0;
            var sc = liste as IList<Course> ?? liste.ToList().Where(x => !string.IsNullOrEmpty(x.Name)).Distinct();
            var sc2 = list2 as IList<Module> ?? list2.ToList().Where(x => !string.IsNullOrEmpty(x.Nom)).Distinct();
            ProgressBar.Minimum = 0;

            var courses = sc as Course[] ?? sc.ToArray();
            ProgressBar.Maximum = courses.Count();
            PBar pBar = new PBar(ProgressBar);
            var enumerable = sc2 as Module[] ?? sc2.ToArray();

            foreach (var course in courses)
            {
            
                var firstOrDefault = enumerable.FirstOrDefault(x => x.Code == course.Code);
                if (firstOrDefault != null)
                {

                    var item = firstOrDefault.Specialite;
                    var id = _db.Specialites.FirstOrDefault(w => item.StartsWith(w.Name));
                    if (id != null)
                    {
                        course.SpecialiteId = id.Id;
                        switch (id.Name.Split(' ')[0])
                        {
                            case "L1":
                                {
                                    var orDefault = _db.Annees.FirstOrDefault(w => w.Name == 1);
                                    if (orDefault != null)
                                        course.AnneeId = orDefault.Id;
                                    course.Semestre = 1;
                                }
                                break;
                            case "M1":
                                {
                                    var orDefault = _db.Annees.FirstOrDefault(w => w.Name == 1);
                                    if (orDefault != null)
                                        course.AnneeId = orDefault.Id;
                                    course.Semestre = 1;
                                }
                                break;
                            case "L2":
                                {
                                    var orDefault = _db.Annees.FirstOrDefault(w => w.Name == 2);
                                    if (orDefault != null)
                                        course.AnneeId = orDefault.Id;
                                    course.Semestre = 1;
                                }
                                break;
                            case "M2":
                                {
                                    var orDefault = _db.Annees.FirstOrDefault(w => w.Name == 2);
                                    if (orDefault != null)
                                        course.AnneeId = orDefault.Id;
                                    course.Semestre = 1;
                                }
                                break;
                            case "L3":
                                {
                                    var orDefault = _db.Annees.FirstOrDefault(w => w.Name == 3);
                                    if (orDefault != null)
                                        course.AnneeId = orDefault.Id;
                                    course.Semestre = 1;
                                }
                                break;

                        }
                        listOfCourses.Add(course);
                      //  DataGrid.ItemsSource = listOfCourses;

                        q++;
                    }
                    pBar.IncPb();
                }

            }
               
          
            _db.Courses.AddRange(listOfCourses);
            _db.SaveChanges();
            MessageBox.Show(q.ToString(CultureInfo.InvariantCulture));
        }
    }
}
