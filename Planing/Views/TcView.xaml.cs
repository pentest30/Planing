using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Planing.Core.DbImport;
using Planing.Core.Models;
using Planing.ModelView;
using Planing.UI.Helpers;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for TcView.xaml
    /// </summary>
    public partial class TcView
    {
        readonly DbModel _db = new DbModel();
        private int _semestre;
        private int _anS;
        private int _fId;

        public TcView()
        {
            InitializeComponent();
            CbFacutlte.ItemsSource = _db.Facultes.ToList();
            CbAs.ItemsSource = _db.AnneeScolaires.ToList();
           
        }

       

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem == null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var result = MessageBox.Show("Est vous sure!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (!result.ToString().Equals("Yes")) return;
              
            if (DataGrid.SelectedItems.Count > 0)
            {
                ProgressBar.Maximum = 0;
                ProgressBar.Maximum = DataGrid.SelectedItems.Count;
                PBar pBar = new PBar(ProgressBar);
                 var tcViewModels = DataGrid.SelectedItems.Cast<TcViewModel>();
                foreach (var tcViewModel in tcViewModels)
                {
                    //var deleted = DataGrid.SelectedItem as TcViewModel;
                    if (tcViewModel == null) return;
                    var converted = _db.Tcs.Find(tcViewModel.Id);
                    _db.Entry(converted).State = EntityState.Deleted;
                    _db.SaveChanges();
                    pBar.IncPb();
                }
                GetDg();
                ProgressBar .Minimum=0;
            }
           
        }

        private void GetDg()
        {
            List<TcViewModel> result = AutoMapper.Mapper.Map<List<Tc>, List<TcViewModel>>(_db.Tcs.Include("Teacher").
                Include("Course").
                Include("Section").Include("Section.Specialite").
                Include("Groupe").Include("ClassRoomType").
                Include("AnneeScolaire").Where(x=>x.AnneeScolaireId ==_anS&& x.Semestre ==_semestre&&x.Section.Specialite.FaculteId==_fId).ToList());
            DataGrid.ItemsSource = result;

              
        }

    

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            //AddButton.Visibility = Visibility.Hidden;
            //var list = DataGrid.ItemsSource.OfType<TcViewModel>().ToList();
            //list.Add(new TcViewModel());
            //Grid.DataContext =  AutoMapper.Mapper.Map<TcViewModel, Tc>(list.Last()); 
            var frm = new TcWind(0, _fId, _anS, _semestre);
           
            frm.Show();
            frm.UpdateDataDg += GetDg;
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = DataGrid.SelectedItem as TcViewModel;
            if (item == null)
            {
                MessageBox.Show("Selectionner un champ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var frm = new TcWind(item.Id , _fId, _anS, _semestre);
            
            frm.Show();
            frm.UpdateDataDg += GetDg;

        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            var item1 = Grid.DataContext as TcViewModel;
            Tc item = AutoMapper.Mapper.Map<TcViewModel, Tc>(item1);
          
            if (item.ScheduleWieght <= 0)
            {

                MessageBox.Show("désigner le nombre de séances  ");
                return;
            }


           

            using (var db =new DbModel())
            {
                if (item.Id <= 0)
                {
                    db.Tcs.Add(item);
                   
                }
                else
                {

                    db.Entry(item).State = EntityState.Modified;
                }
                try
                {
                    db.SaveChanges();  
                    GetDg();
                   
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Erreurs pendant l'enregistrement", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    //((ObservableCollection<Article>)DataGrid.ItemsSource).Remove(item);
                }
            }

            AddButton.Visibility = Visibility.Visible;
          
            var binding = new Binding {ElementName = "DataGrid", Path = new PropertyPath("SelectedItem")};
            Grid.SetBinding(DataContextProperty, binding);
            //item.Id = 0;
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;

        }



        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Visible;
            UpdateButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
            var binding = new Binding { ElementName = "DataGrid", Path = new PropertyPath("SelectedItem") };
            Grid.SetBinding(DataContextProperty, binding);
        }
        
        private void DupliquButton_OnClick(object sender, RoutedEventArgs e)
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
            var liste = DbAcceess.GetPlaning(cheminExcel, 3);
            var secId = 0;
            ProgressBar.Minimum = 0;
            var tcModels = liste as TcModel[] ?? liste.Distinct().ToArray();
            ProgressBar.Maximum = tcModels.Count();
            PBar pBar = new PBar(ProgressBar);
            TextBlock.Text = "Initiation et conversion des données";
            var temps = new List<Tc>();
            var count = (from p in tcModels 
                         where  !string.IsNullOrEmpty(p.Section) 
                         select p.Section);
            var enumerable = count as string[] ?? count.ToArray();
            var ids = new int[enumerable.Count()];  
            int o = 0, j = 0;
            foreach (var item in enumerable)
            {
                var firstOrDefault = _db.Sections.FirstOrDefault(x => x.Name.StartsWith(item));
                if (firstOrDefault != null)
                    ids[o] = firstOrDefault.Id;
                o++;
            }
            foreach (var tcModel in tcModels)
            {
                
                if (!string.IsNullOrEmpty(tcModel.Section))
                {
                    var firstOrDefault = _db.Sections.FirstOrDefault(x => x.Name.Equals(tcModel.Section));
                    if (firstOrDefault != null)
                        secId = ids[j];
                    j++;
                }
                else
                {
                    if (secId == 0 ||tcModel.Prof==null|| tcModel.Matiere==null)
                    {
                        pBar.IncPb();
                        continue;
                    }
                    var tc = new Tc(); 
                    tc.ScheduleWieght = (tcModel.Seances>0)? tcModel.Seances:1;
                    tc.Semestre = 1;
                    tc.AnneeScolaireId = 1;
                    tc.SectionId = secId;
                    //var c = (from c in _db.Courses where c.Code.Contains(tcModel.Matiere) select c).Last();
                    var course = (from c in _db.Courses 
                                  where c.Code.Contains(tcModel.Matiere) 
                                  select c).FirstOrDefault();
                    if (course != null)
                        tc.CourseId = course.Id;
                    else
                    {
                        pBar.IncPb();
                        continue;
                    }
                    var prof = _db.Teachers.FirstOrDefault(x => x.Nom.StartsWith(tcModel.Prof.Trim()));
                    if (prof != null)
                        tc.TeacherId = prof.Id;
                    else
                    {
                        pBar.IncPb();
                        continue;
                    }
                   
                    if (!string.IsNullOrEmpty(tcModel.Group))
                    {
                        var gr = tcModel.Group[1].ToString(CultureInfo.InvariantCulture) +((tcModel.Group.Length>2)? tcModel.Group[2].ToString(CultureInfo.InvariantCulture):"");
                        var groupe = _db.Groupes.FirstOrDefault(
                            x => x.Code.Contains(gr) && x.SectionId == secId);
                        if (groupe != null)
                            tc.GroupeId =
                                groupe.Id;
                    }
                    if (string.IsNullOrEmpty(tcModel.Classe))
                    {
                        pBar.IncPb();
                        continue;
                        
                    }
                    if (tcModel.Classe.Contains(','))tcModel.Classe= tcModel.Classe.Split(',')[0]; 
                    var firstOrDefault = _db.ClassRooms.FirstOrDefault(x => x.Code.Trim().Contains(tcModel.Classe.Trim()));
                    if (firstOrDefault != null)
                    {
                        tc.ClassRoomTypeId = Convert.ToInt32(firstOrDefault.ClassRoomTypeId);
                       
                    }
                    else
                    {
                        pBar.IncPb();
                        continue;
                    }
                      
                    try
                    {
                        //var item = new Tc();
                        bool f = false;
                        if (temps.Count == 0)
                        {
                            temps.Add(tc);
                        }
                        else
                        {
                            if (tc.GroupeId > 0)
                            {
                                foreach (var temp in temps)
                                {
                                    if (temp.CourseId == tc.CourseId && temp.TeacherId == tc.TeacherId &&
                                        tc.SectionId == temp.SectionId &&tc.GroupeId == temp.GroupeId)
                                    {
                                        f = true;
                                        break;
                                    }
                                }

                            }
                            else
                            {
                                foreach (var temp in temps)
                                {
                                    if (temp.CourseId == tc.CourseId && temp.TeacherId == tc.TeacherId &&
                                        tc.SectionId == temp.SectionId)
                                    {
                                        f = true;
                                        break;
                                    }
                                }

                            }
                        }
                        if (!f)
                        {
                            temps.Add(tc);

                        }
                        else
                        {
                            throw new Exception();

                        }
                    }
                    catch (Exception)
                    {
                        pBar.IncPb();
                        continue;
                    }
                }
                pBar.IncPb();
            }
            using (var db = new DbModel())
            {

                ProgressBar.Minimum = 0;
                ProgressBar.Maximum = temps.Count();
                pBar = new PBar(ProgressBar);
                TextBlock.Text = "Sauvegarde des données";
                foreach (var temp in temps.Distinct())
                {
                    db.Tcs.Add(temp);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    pBar.IncPb();
                }

            }
            //SaveButton.Click += SaveButton_OnClick;
        }

        private void SemestreTxt_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            CbAs.SelectedIndex = -1;
        }

        private void CbAs_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var f = CbFacutlte.SelectedItem as Faculte;
                var a = CbAs.SelectedItem as AnneeScolaire;
                _semestre = (SemestreTxt.Text != "") ? Convert.ToInt32(SemestreTxt.Text) : 0;
                if (f != null) _fId = f.Id;
                if (a != null) _anS = a.Id;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                   
                    GetDg();
                    Mouse.OverrideCursor = null;
                });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                //  throw;
            }

        }
    }
}
