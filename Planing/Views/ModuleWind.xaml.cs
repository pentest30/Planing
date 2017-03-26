using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Planing.Core.Models;
using Planing.Models;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for ModuleWind.xaml
    /// </summary>
    public partial class ModuleWind
    {
        readonly DbModel _db = new DbModel();
        public delegate void UpdateDg();
        public AddGroupWind.UpdateDg UpdateDataDg;
        public ModuleWind()
        {
            InitializeComponent();
            CbCategorie.ItemsSource = _db.Specialites.ToList();
            CbSousCategorie.ItemsSource = _db.Annees.ToList();
            CbTypeCourse.ItemsSource = _db.CourseTypes.ToList();
            //GetDg();
            Grid.DataContext = new Course();
        }

        private void CbCategorie_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (Course) Grid.DataContext;
            _db.Courses.Add(item);
            _db.SaveChanges();
            if (UpdateDataDg != null && item != null) UpdateDataDg();
        }
    }
}
