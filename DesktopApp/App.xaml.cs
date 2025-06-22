using System.Windows;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DesktopApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseFacade facade = new DatabaseFacade(new DataContext());
            facade.EnsureCreated();
            //base.OnStartup(e);
            //var login = new LoginWindow();
            //login.Show();
        }
    }

}
