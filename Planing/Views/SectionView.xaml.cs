using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DevExpress.Xpf.Grid;
using Planing.Core.DbImport;
using Planing.Core.Models;
using Planing.Models;
using Planing.UI.Helpers;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for SectionView.xaml
    /// </summary>
    public partial class SectionView
    {
        readonly DbModel _db = new DbModel();
        public SectionView()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.Specialites.ToList();
            CbSousCategorie.ItemsSource = _db.Annees.ToList();
            CbAnneeScolaire.ItemsSource = _db.AnneeScolaires.ToList();
            DataGrid.ItemsSource = _db.Sections.Include("Specialite").Include("Annee").Include("AnneeScolaire").ToList();
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var item = CbCategorie.SelectedItem as Specialite;
            //CbEns.ItemsSource = _db.ClassRooms.Where(c=>c.FaculteId == item.FaculteId).ToList();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Hidden;
            var list = DataGrid.ItemsSource as List<Section>;
            if (list != null)
            {
                list.Add(new Section());
                Grid.DataContext = list.Last();
            }
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
            var deleted = DataGrid.SelectedItem as Specialite;
            if (deleted == null) return;
            _db.Entry(deleted).State = EntityState.Deleted;
            _db.SaveChanges();
            DataGrid.ItemsSource = _db.Sections.Include("Specialite").Include("Annee").Include("AnneeScolaire").ToList();
            
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (Section)Grid.DataContext;
            if (item.Id <= 0)
            {
                _db.Sections.Add(item);
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
            DataGrid.ItemsSource = _db.Sections.Include("Specialite").Include("Annee").Include("AnneeScolaire").ToList();
            var binding = new Binding { ElementName = "DataGrid", Path = new PropertyPath("SelectedItem") };
            Grid.SetBinding(DataContextProperty, binding);
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
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
            var liste = DbAcceess.GetSpecialites(cheminExcel, 0);

            if (liste != null)
            {
                var specialites = liste as IList<Specialite> ?? liste.ToList().Distinct();
                ProgressBar.Minimum = 0;
                var enumerable = specialites as Specialite[] ?? specialites.Where(x=>!string.IsNullOrEmpty(x.Name)).ToArray();
                ProgressBar.Maximum = enumerable.Count();
                PBar pBar = new PBar(ProgressBar);
                foreach (var specialite in enumerable)
                {
                    
                    if (specialite != null && !string.IsNullOrEmpty(specialite.Name))
                    {
                        var item = new Section();
                        item.Semestre = 1;
                        item.Code = specialite.Code;
                        var n = specialite.Name.Split(' ')[0];
                        item.Name = specialite.Name;
                        Specialite firstOrDefault = _db.Specialites.FirstOrDefault(x => specialite.Name.Contains(x.Name));
                        item.AnneeScolaireId = 1;
                        if (firstOrDefault != null) item.SpecialiteId= firstOrDefault.Id;
                        switch (n)
                        {
                            case "L1":
                                item.AnneeId = 1;break;
                            case "L2":
                                item.AnneeId = 2; break;
                            case "L3":
                                item.AnneeId = 3; break;
                            case "M1":
                                item.AnneeId = 1; break;
                            case "M2":
                                item.AnneeId = 2; break;

                        }
                        _db.Sections.Add(item);
                        _db.SaveChanges();
                      

                    }
                    pBar.IncPb();
                }
                GetDg();
            }
        }

        private void GetDg()
        {
            DataGrid.ItemsSource = _db.Sections.Include("Specialite").Include("Annee").Include("AnneeScolaire").ToList();
        
        }

        private void GetDgSalle(int id)
        {
            DataGridSalle.ItemsSource = _db.SalleClasses.Where(x => x.SectionId == id).Include("Section").Include("ClassRoom").Include("ClassRoom.ClassRoomType").ToList();
        }


        private void LBonAddBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var item = DataGrid.SelectedItem as Section;
            if (item == null)
            {
                MessageBox.Show("Sélectioner un champ");
                return;
            }
            var frm = new AddClasseView(item,null);
            frm.UpdateDataDg += GetDgSalle;
            frm.Show();
        }

       

        private void DataGrid_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var item = DataGrid.SelectedItem as Section;
            if (item != null) GetDgSalle(item.Id);
        }
    }
}
