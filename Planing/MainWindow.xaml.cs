using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Planing.Views;
using ZonaTools.XPlorerBar;

namespace Planing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var sw = "";
            if (sender != null)
            {

                try
                {
                    sw = ((XPlorerItem) sender).ItemText.ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception )
                {
                    sw = ((MenuItem) sender).Header.ToString();
                    //ingnore
                }
                finally
                {
                    
                    switch (sw)
                    {
                        case "Modules": ContentControl.Content = new ArticleView();
                            break;
                        case "Séctions": ContentControl.Content = new SectionView();
                            break;
                        case "Salles": ContentControl.Content = new SalleView();
                            break;
                        case "Spécialité": ContentControl.Content = new SpecialiteView();
                            break;
                        case "Groupes": ContentControl.Content = new SpecialiteView();
                            break;
                        case "Enseignants": ContentControl.Content = new EnseignantView();
                            break;
                    }
                }
            }
          
        }
    }
}
