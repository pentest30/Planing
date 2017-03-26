using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DevExpress.Xpf.Grid;
using Planing.Core.DbImport;
using Planing.Core.Models;
using Planing.UI.Helpers;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for SalleView.xaml
    /// </summary>
    /// readonly DbModel _db = new DbModel();
    public partial class SalleView
    {
        readonly DbModel _db = new DbModel();
        public SalleView()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.ClassRoomTypes.ToList();
            CbSousCategorie.ItemsSource = _db.Facultes.ToList();
            //CbTypeCourse.ItemsSource = _db.CourseTypes.ToList();
            DataGrid.ItemsSource = _db.ClassRooms.Include("Faculte").Include("ClassRoomType").ToList();
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Visible;
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
            var binding = new Binding { ElementName = "DataGrid", Path = new PropertyPath("SelectedItem") };
            Grid.SetBinding(DataContextProperty, binding);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (ClassRoom)Grid.DataContext;
            if (item.Id <= 0)
            {
                _db.ClassRooms.Add(item);
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
            DataGrid.ItemsSource = new ObservableCollection<ClassRoom>(_db.ClassRooms.ToList());
            var binding = new Binding { ElementName = "DataGrid", Path = new PropertyPath("SelectedItem") };
            Grid.SetBinding(DataContextProperty, binding);
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
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

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Hidden;
            var list = DataGrid.ItemsSource as List<ClassRoom>;
            if (list != null)
            {
                list.Add(new ClassRoom());
                Grid.DataContext = list.Last();
            }
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
            var deleted = DataGrid.SelectedItem as ClassRoom;
            if (deleted == null) return;
            _db.Entry(deleted).State = EntityState.Deleted;
            _db.SaveChanges();
            DataGrid.ItemsSource = _db.ClassRooms.Include("Faculte").Include("ClassRoomType").ToList();
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
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
            var liste = DbAcceess.GetClassRooms(cheminExcel, 6);

            if (liste != null)
            {
                var list = liste.ToList();
                var specialites = liste as IList<ClassRoom> ?? list.ToList().Distinct();
                ProgressBar.Minimum = 0;
                var enumerable = specialites as ClassRoom[] ?? specialites.Where(x =>!string.IsNullOrEmpty( x.Name)).ToArray();
                ProgressBar.Maximum = enumerable.Count();
                PBar pBar = new PBar(ProgressBar);
                var typeClasses = _db.ClassRoomTypes.ToList();
                foreach (var classRoom in enumerable)
                {
                    if (classRoom != null && !string.IsNullOrEmpty(classRoom.Name))
                    {
                        var item = new ClassRoom();
                        item.Name = classRoom.Name;
                        item.Code = classRoom.Code;
                        item.FaculteId = 1;
                        var firstOrDefault = typeClasses.LastOrDefault(x => classRoom.Name.Contains(x.Name));
                        if (firstOrDefault != null)
                            item.ClassRoomTypeId =
                                firstOrDefault.Id;
                        try
                        {
                            _db.ClassRooms.Add(item);
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
            DataGrid.ItemsSource = _db.ClassRooms.Include("Faculte").Include("ClassRoomType").ToList();
        }

        private void LBonAddBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckSelectedItem()) return;
            var classRoom = DataGrid.SelectedItem as ClassRoom;
            if (classRoom == null || classRoom.Id == 0) return;
            var frm = new HrLbrSalleView(classRoom.Id);
            frm.UpdateDataDg += UpdateDg;
            frm.ShowDialog();
        }private bool CheckSelectedItem()
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
            DataGridLignes.ItemsSource = _db.SeanceLbrSalles.Where(x => x.SalleId == id).Include("Salle").Include("AnneeScolaire").ToList();
        }

        private void DeleteBeLignesButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = DataGridLignes.SelectedItem as SeanceLbrSalle;
            if (item == null)
            {
                MessageBox.Show("Selectionner un champ!");
            }
            else
            {
                var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (!result.ToString().Equals("Yes")) return;
                _db.Entry(item).State = EntityState.Deleted;
                _db.SaveChanges();
                DataGridLignes.ItemsSource =
                    _db.SeanceLbrSalles.Where(x => x.SalleId == item.SalleId)
                        .Include("Salle")
                        .Include("AnneeScolaire")
                        .ToList();
            }
        }


      

      

        private void DataGrid_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = DataGrid.SelectedItem as ClassRoom;
                DataGridLignes.ItemsSource = _db.SeanceLbrSalles.Where(x => x.SalleId == item.Id).
                    Include("Salle").
                    Include("AnneeScolaire").ToList();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {

                //throw;
            }
        }
    }
}
