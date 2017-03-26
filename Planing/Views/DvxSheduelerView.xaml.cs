using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Opt.Alogrithms;
using Planing.Core.Models;
using Planing.ModelView;
using Planing.PL.Generator;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for DvxSheduelerView.xaml
    /// </summary>
    public partial class DvxSheduelerView
    {
        private ObservableCollection<LectureModelView> _bindingList;
         ObservableCollection<Lecture> _solution = new ObservableCollection<Lecture>();
        Thread t;

        public DvxSheduelerView()
        {
            InitializeComponent();
          
           
        }





        private void Scheduler_Drop(object sender, DragEventArgs e)
        {

        }

        private void GenBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var initial = PlaningGenerator.GeneratingPlanings(1, 1, 1, ProgressBar);
            s = new SumilatedAnnealing(initial);
            t = new Thread(() =>
            {

               
                s.UpdateDataDg += () =>
                {
                    var sx = new SchedulGenerator();
                    _bindingList = new ObservableCollection<LectureModelView>(sx.PopulateList(s.Solution));

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        Scheduler.Storage.RefreshData();
                        Scheduler.Storage.AppointmentStorage.DataSource = _bindingList;
                        TextBlock.Text = s.Fitness.ToString();

                    }));
                  


                };
                _solution = s.Run(20000000);
                Abort();
            });
            t.Start();
          
        }

        public SumilatedAnnealing s { get; set; }

        private void BtnStop_OnClick(object sender, RoutedEventArgs e)
        {
            t.Abort();
            s.Stop = true;
            _solution = s.Solution;
            Abort();
        }
        
        private void Abort()
        {
            var result = MessageBox.Show("la génération du planing est termninée! n voulez vous savegarder cette _solution ?",
                "Warning", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (!result.ToString().Equals("Yes")) return;
           
            using (var db = new DbModel())
            {
                foreach (var lecture in _solution)
                {
                    if (lecture.GroupeId == 0) lecture.GroupeId = null;
                   
                    var s = db.Sections.FirstOrDefault(x => x.Id == lecture.SectionId);
                    lecture.SpecialiteId = s.SpecialiteId;
                    lecture.AnneeId = s.AnneeId;

                }

                db.Lectures.AddRange(_solution);
                db.SaveChanges();
            }
            MessageBox.Show("Enregistrement terminé");
        }
    }
}
