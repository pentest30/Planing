using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Planing.Models;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for SpecialiteView.xaml
    /// </summary>
    public partial class SpecialiteView
    {
        DbModel db = new DbModel();
        public SpecialiteView()
        {
            InitializeComponent();
           // CbSousCategorie.ItemsSource = db.Annees.ToList();
            CbCategorie.ItemsSource = db.Facultes.ToList();
                CbNieau.ItemsSource = db.Niveaus.ToList();
            DataGrid.ItemsSource = db.Specialites.Include("Niveau").Include("Faculte").ToList();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedIndex < 0)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (!result.ToString().Equals("Yes")) return;
            var deleted = DataGrid.SelectedItem as Specialite;
            if (deleted == null) return;
            db.Entry(deleted).State =EntityState.Deleted;
            db.SaveChanges();
            DataGrid.ItemsSource = db.Specialites.Include("Niveau").Include("Faculte").ToList();
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
            var item = (Specialite)Grid.DataContext;
             if (item.Id <= 0)
             {
                 db.Specialites.Add(item);
                 // ((ObservableCollection<Article>)DataGrid.ItemsSource).Add(item);
             }
            else
            {
                db.Entry(item).State = EntityState.Modified;
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erreurs pendant l'enregistrement", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //((ObservableCollection<Article>)DataGrid.ItemsSource).Remove(item);
            }

            AddButton.Visibility = Visibility.Visible;
            DataGrid.ItemsSource = new ObservableCollection<Specialite>(db.Specialites.ToList());
            var binding = new Binding { ElementName = "DataGrid", Path = new PropertyPath("SelectedItem") };
            Grid.SetBinding(DataContextProperty, binding);
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedIndex < 0)
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
            var list = DataGrid.ItemsSource.OfType<Specialite>().ToList();
            list.Add(new Specialite());
            Grid.DataContext = list.Last();  
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
