using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using DevExpress.Xpf.Grid;
using Planing.Core.Models;
using Planing.Models;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class GroupView
    {
        private readonly DbModel _db = new DbModel();

        public GroupView()
        {
            InitializeComponent();
            CbArticle.ItemsSource = _db.Sections.Include("Annee").Include("AnneeScolaire").Include("Specialite").ToList();
            UpdateDg();
        }

        private void UpdateDg()
        {
            DataGrid.ItemsSource = _db.Groupes.Include("Section").Include("Section.Specialite").ToList();
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
            var deleted = DataGrid.SelectedItem as Groupe;
            if (deleted == null) return;
            _db.Entry(deleted).State = EntityState.Deleted;
            _db.SaveChanges();
            UpdateDg();
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
            var item = (Groupe)Grid.DataContext;
            if (item.Id <= 0)
            {
                _db.Groupes.Add(item);
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
            UpdateDg();
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
            var list = DataGrid.ItemsSource as List<Groupe>;
            if (list != null)
            {
                list.Add(new Groupe());
                Grid.DataContext = list.Last();
            }
        }

        private void AddGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            var frm = new AddGroupWind();
            frm.UpdateDataDg += UpdateDg;
            frm.ShowDialog();
        }
        private void GetDgSalle(int id)
        {
            DataGridSalle.ItemsSource = _db.SalleClasses.Where(x => x.GroupeId == id).Include("Section").Include("Groupe").Include("ClassRoom").Include("ClassRoom.ClassRoomType").ToList();
        }


        private void LBonAddBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var item = DataGrid.SelectedItem as Groupe;
            if (item == null)
            {
                MessageBox.Show("Sélectioner un champ");
                return;
            }
            var frm = new AddClasseView(null,item);
            frm.UpdateDataDg += GetDgSalle;
            frm.Show();
        }

       

        private void DataGrid_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var item = DataGrid.SelectedItem as Groupe;
            if (item != null) GetDgSalle(item.Id);
        }
    }
}
