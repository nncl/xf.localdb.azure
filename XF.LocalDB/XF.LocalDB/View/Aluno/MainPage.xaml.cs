using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.LocalDB.ViewModel;

namespace XF.LocalDB.View.Aluno
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            App.AlunoVM.Carregar();
            base.OnAppearing();
        }

        private void OnNovo(object sender, EventArgs args)
        {
            App.AlunoVM.Selecionado = new Model.Aluno();
            Navigation.PushAsync(new NovoAlunoView() { BindingContext = App.AlunoVM });
        }

        private void OnEditar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NovoAlunoView() { BindingContext = App.AlunoVM });
        }

        private void OnSair(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
