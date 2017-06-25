using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.LocalDB.View.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

		#region Authenticator
		bool authenticated = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();

			if (authenticated == true)
			{
				this.btnLogin.IsVisible = false;
			}
        }

        private async void Autenticar_Clicked(object sender, EventArgs e)
        {
			if (App.Authenticator != null)
				authenticated = await App.Authenticator.Authenticate();

            if (authenticated == true) {
                await Navigation.PushAsync(new Aluno.MainPage());
            }
        }

		#endregion
	}
}