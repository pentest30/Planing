using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DevExpress.Xpf.Grid;
using Planing.Core.DbImport;
using Planing.Core.Models;
using Planing.Models;
using Planing.UI.Helpers;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for EnseignantView.xaml
    /// </summary>
    public partial class EnseignantView
    {
        private readonly DbModel _db = new DbModel();
        public EnseignantView()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.Facultes.ToList();
            DataGrid.ItemsSource = _db.Teachers.Include("Faculte").ToList();
        }

       

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Hidden;
            var list = DataGrid.ItemsSource as List<Teacher>;
            if (list != null)
            {
                list.Add(new Teacher());
                Grid.DataContext = list.Last();
            }
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem == null)
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
            var item = (Teacher)Grid.DataContext;
            if (item.Id <= 0)
            {
                _db.Teachers.Add(item);
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
            DataGrid.ItemsSource = new ObservableCollection<Teacher>(_db.Teachers.Include("Faculte").ToList());
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
            var deleted = DataGrid.SelectedItem as Teacher;
            if (deleted == null) return;
            _db.Entry(deleted).State = EntityState.Deleted;
            _db.SaveChanges();
            DataGrid.ItemsSource = _db.Teachers.Include("Faculte").ToList();
        }

        private void LBonAddBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckSelectedItem()) return;
            var teacher = DataGrid.SelectedItem as Teacher;
            if (teacher == null|| teacher.Id==0) return;
            //var frm = new HoraireWnd(teacher.Id);
            //try
            //{
            //    frm.UpdateDataDg += UpdateDg;
            //    frm.ShowDialog();
            //}
            //catch (Exception exception)
            //{
                
            //    MessageBox.Show(exception.Message);
            //}
        }

        private bool CheckSelectedItem()
        {
            if (DataGrid.SelectedItem == null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }

        private void UpdateDg(int id)
        
        {
            DataGridLignes.ItemsSource = _db.Seances.Where(x => x.TeacherId == id).Include("Teacher").Include("AnneeScolaire").ToList();
        }

        private void DeleteBeLignesButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGridLignes.SelectedItem == null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (!result.ToString().Equals("Yes")) return;
            var deleted = DataGridLignes.SelectedItem as Seance;

            if (deleted == null) return;
            var id = deleted.TeacherId;
            _db.Entry(deleted).State = EntityState.Deleted;
            _db.SaveChanges();
            UpdateDg(id);
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == false) return;
            string cheminExcel = ofd.FileName;
            if (!cheminExcel.Split('\\').Last().Contains(".xlsx"))
            {
                MessageBox.Show("Le fichier que vous avez selectioné ce n'est un fichier Excel");
                return;
            }
            var liste = DbAcceess.GetTeachers(cheminExcel, 5);
            if (liste != null)
            {
                var list = liste.ToList();
                var specialites = liste as IList<Teacher> ?? list.ToList().Distinct();
                ProgressBar.Minimum = 0;
                var enumerable = specialites as Teacher[] ?? specialites.Where(x => !string.IsNullOrEmpty(x.Nom)).ToArray();
                ProgressBar.Maximum = enumerable.Count();
                PBar pBar = new PBar(ProgressBar);
             
                foreach (var teacher in enumerable)
                {
                    if (teacher != null && !string.IsNullOrEmpty(teacher.Nom))
                    {
                        var item = new Teacher();
                        item.Nom = teacher.Nom;
                        item.Prenom = teacher.Nom.Split(' ').LastOrDefault();
                        item.FaculteId = 1;
                        try
                        {
                            _db.Teachers.Add(item);
                            _db.SaveChanges();
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    pBar.IncPb();
                }
                GetDg();
            }
        }

        private void GetDg()
        {
            DataGrid.ItemsSource = _db.Teachers.Include("Faculte").ToList();
        }

        private void DataGridLignes_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //var item = DataGrid.SelectedItem as Teacher;
            //if (item != null)
            //{
            //    DataGridLignes.ItemsSource = _db.Seances.Include("AnneeScolaire").Where(x => x.TeacherId == item.Id).ToList();
            //}
        }

        private void DataGrid_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var item = DataGrid.SelectedItem as Teacher;
            if (item != null)
            {
                DataGridLignes.ItemsSource = _db.Seances.Include("AnneeScolaire").Where(x => x.TeacherId == item.Id).ToList();
            }
        }
    }
}
