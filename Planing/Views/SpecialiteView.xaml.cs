using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for SpecialiteView.xaml
    /// </summary>
    public partial class SpecialiteView
    {
        readonly DbModel _db = new DbModel();

        public SpecialiteView()
        {
            InitializeComponent();
            // CbSousCategorie.ItemsSource = db.Annees.ToList();
            CbCategorie.ItemsSource = _db.Facultes.ToList();
            CbNieau.ItemsSource = _db.Niveaus.ToList();
            CbFilliere.ItemsSource = _db.Fillieres.ToList();
            GetDg();
        }

        private void GetDg()
        {
            DataGrid.ItemsSource = _db.Specialites.Include("Niveau").Include("Faculte").ToList();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem ==null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (!result.ToString().Equals("Yes")) return;
            var deleted = DataGrid.SelectedItem as Specialite;
            if (deleted == null) return;
            _db.Entry(deleted).State =EntityState.Deleted;
            try
            {
                _db.SaveChanges();
           
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            GetDg();
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
            var item = (Specialite) Grid.DataContext;
            if (item.Id <= 0)
            {
                _db.Specialites.Add(item);
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
            GetDg();
            var binding = new Binding {ElementName = "DataGrid", Path = new PropertyPath("SelectedItem")};
            Grid.SetBinding(DataContextProperty, binding);
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem ==null)
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
            var list = DataGrid.ItemsSource as List<Specialite>;
            if (list != null)
            {
                list.Add(new Specialite());
                Grid.DataContext = list.Last();
            }
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCategorie.SelectedIndex != -1)
            {
                //CbDepartement.SelectedIndex = -1;
                var id = ((Faculte) CbCategorie.SelectedItem).Id;
                CbDepartement.ItemsSource = _db.Departements.Where(x=>x.FaculteId == id).ToList();
            }
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
            var liste = DbAcceess.GetSpecialites(cheminExcel,1);

            if (liste != null)
            {
                var specialites = liste as IList<Specialite> ?? liste.ToList().Distinct();
                ProgressBar.Minimum = 0;
                var enumerable = specialites as Specialite[] ?? specialites.ToArray();
                ProgressBar.Maximum = enumerable.Count();
                PBar pBar = new PBar(ProgressBar);
                foreach (var specialite in enumerable)
                {
                    if (specialite != null && !string.IsNullOrEmpty(specialite.Name))
                    {
                        var item = new Specialite();
                        item.FaculteId = 1;
                        var n = specialite.Name.Split(' ')[0]; 
                        Niveau firstOrDefault;
                        if (n.Contains("L"))
                        {
                            firstOrDefault = _db.Niveaus.FirstOrDefault(x => x.Libelle.Equals("LISSENCE"));
                        }
                        else firstOrDefault = _db.Niveaus.FirstOrDefault(x => x.Libelle.Equals("MASTER"));
                        if (firstOrDefault != null)
                            item.NiveauId = firstOrDefault.Id;
                        item.Name = specialite.Name;
                        if (!_db.Specialites.Any(x => x.Name.Equals(item.Name)))
                        {
                            _db.Specialites.Add(item);
                            _db.SaveChanges();
                        }
                        
                    }
                    pBar.IncPb();
                }
                GetDg();
            }
        }
    }
}
