using System.Linq;
using System.Windows;
using Planing.Core.Models;
using Planing.Models;


namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for AddClasseView.xaml
    /// </summary>
    public partial class AddClasseView
    {
        readonly DbModel _db =new DbModel();
        private readonly Section _section;
        private readonly Groupe _groupe;
        public delegate void UpdateDg(int id);
        public UpdateDg UpdateDataDg;
        public AddClasseView(Section section , Groupe groupe)
        {
            InitializeComponent();
            _section = section;
            _groupe = groupe;
            CbEns.ItemsSource = (_section != null) ? _db.ClassRooms.Where(x => x.FaculteId == section.Specialite.FaculteId).ToList() : _db.ClassRooms.Where(x => x.FaculteId == groupe.Section.Specialite.FaculteId).ToList();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var item = new SalleClasse();
            if (_section != null) item.SectionId = _section.Id;
            else
            {
                item.GroupeId = _groupe.Id;
                //item.SectionId = _groupe.SectionId;
            }
            item.ClassRoomId = ((ClassRoom) CbEns.SelectedItem).Id;
            _db.SalleClasses.Add(item);
            _db.SaveChanges();
            if (UpdateDataDg != null&&_section!=null) UpdateDataDg(_section.Id);
            else if (UpdateDataDg != null && _groupe != null) UpdateDataDg(_groupe.Id);

        }


        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
