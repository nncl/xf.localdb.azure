using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.LocalDB.Data;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace XF.LocalDB.Model
{
    public class Aluno
    {
        [PrimaryKey, AutoIncrement]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "rm")]
        public string RM { get; set; }
        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "aprovado")]
        public bool Aprovado { get; set; }
        public string IsAprovado
        {
            get
            {
                return (Aprovado) ? "Aprovado" : "Reprovado";
            }
        }
    }

    public class AlunoRepository
    {
        private AlunoRepository() { }

        private static SQLiteConnection database;
        private static readonly AlunoRepository instance = new AlunoRepository();
        public static AlunoRepository Instance
        {
            get
            {
                if (database == null)
                {
                    database = DependencyService.Get<IDependencyServiceSQLite>().GetConexao();
                    database.CreateTable<Aluno>();
                }
                return instance;
            }
        }

        static object locker = new object();

        public static async Task<bool> SalvarAluno(Aluno aluno)
        {
            /*lock (locker)
            {
                if (aluno.Id != 0)
                {
                    database.Update(aluno);
                    return aluno.Id;
                }
                else return database.Insert(aluno);
            }*/

            if (aluno.Id == null) return false;

			var httpRequest = new HttpClient();
			httpRequest.BaseAddress = new Uri(Constants.ApplicationURL);
            httpRequest.DefaultRequestHeaders.Accept.Clear();

            httpRequest.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string alunoJson = Newtonsoft.Json.JsonConvert.SerializeObject(aluno);

            var response = await httpRequest.PostAsync("api/alunos", new StringContent(alunoJson, System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public static async Task<List<Aluno>> GetAlunos()
        {
            /*
            lock (locker)
            {
                return (from c in database.Table<Aluno>()
                        select c).ToList();
            }
            */

            var httpRequest = new HttpClient();
            var stream = await httpRequest.GetStreamAsync(Constants.ApplicationURL);
            var alunoSerializer = new DataContractJsonSerializer(typeof(List<Aluno>));

            alunoSerializer = (List<Aluno>)alunoSerializer.ReadObject(stream);

            return alunoSerializer;
        }

        public static async Task<Aluno> GetAluno(int Id)
        {
			/*lock (locker)
            {
                // return database.Query<Aluno>("SELECT * FROM [Aluno] WHERE [Id] = " + Id);
                return database.Table<Aluno>().Where(c => c.Id == Id).FirstOrDefault();
            }*/

			var httpRequest = new HttpClient();
			httpRequest.BaseAddress = new Uri(Constants.ApplicationURL);

			httpRequest.DefaultRequestHeaders.Accept.Clear();

			httpRequest.DefaultRequestHeaders.Accept.Add
			(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			var stream = await httpRequest.GetStreamAsync(string.Format("api/alunos/{0}", Id));

            var alunoSerializer = new DataContractJsonSerializer(typeof(List<Aluno>));
            alunoSerializer = (Aluno)alunoSerializer.ReadObject(stream);

			return alunoSerializer;
        }

        public static async Task<bool> RemoverAluno(int Id)
        {
			/*lock (locker)
            {
                return database.Delete<Aluno>(Id);
            }*/

			var httpRequest = new HttpClient();
			httpRequest.BaseAddress = new Uri(Constants.ApplicationURL);


			httpRequest.DefaultRequestHeaders.Accept.Clear();

			httpRequest.DefaultRequestHeaders.Accept.Add
			(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			var response = await httpRequest.DeleteAsync(string.Format("api/alunos/{0}", Id));

			if (response.IsSuccessStatusCode) return true;
			return false;
        }
    }
}