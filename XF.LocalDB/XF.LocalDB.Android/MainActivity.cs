using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using XF;

namespace XF.LocalDB.Droid
{
    [Activity(Label = "XF.LocalDB", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            App.Init((IAuthenticate)this);
        }

		private MobileServiceUser user;
		public async Task<bool> Authenticate()
		{
			var success = false;
			var message = string.Empty;
			try
			{

                user = await AlunoManager.DefaultManager.CurrentClient.LoginAsync(this,
					MobileServiceAuthenticationProvider.Twitter);
				if (user != null)
				{
					message = string.Format("você está autenticado como {0}.",
						user.UserId);
					success = true;
				}
			}
			catch (Exception ex)
			{
				message = ex.Message;
			}

			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetMessage(message);
			builder.SetTitle("Resultado Autenticação");
			builder.Create().Show();

			return success;
		}
    }
}

