using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.LocalDB.Model;

namespace XF.LocalDB.ViewModel
{
    public class UsuarioViewModel
    {
        public UsuarioViewModel()
        {
            this.GetUsuarios("http://wopek.com/xml/usuarios.xml");
        }

        #region Propriedade
        public string Nome { get; set; }
        public string Stream { get; set; }

        // UI Events
        public IsAutenticarCMD IsAutenticarCMD { get; }

        #endregion

        public bool IsAutenticar(Usuario paramUser)
        {
            this.Nome = paramUser.Username;
            return UsuarioRepository.IsAutorizado(paramUser);
        }

        public bool IsAutenticar(string username, string password)
        {
            this.Nome = username;
            return UsuarioRepository.IsAutorizado(new Usuario() { Username = username, Password = password });
        }

        private async void GetUsuarios(string paramURL)
        {
            var httpRequest = new HttpClient();
            Stream = await httpRequest.GetStringAsync(paramURL);
        }

    }

    public class IsAutenticarCMD : ICommand
    {
        private UsuarioViewModel usuarioVM;
        public IsAutenticarCMD(UsuarioViewModel paramVM)
        {
            usuarioVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter)
        {
            if (parameter != null) return true;

            return false;
        }
        public void Execute(object parameter)
        {
            usuarioVM.IsAutenticar(parameter as Usuario);
        }
    }
}
