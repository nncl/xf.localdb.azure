using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XF.LocalDB.Model;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace XF.LocalDB
{
    public partial class AlunoManager
    {

        static AlunoManager defaultInstance = new AlunoManager();
        MobileServiceClient client;

		#if OFFLINE_SYNC_ENABLED
		        IMobileServiceSyncTable<Aluno> alunoTable;
		#else
				IMobileServiceTable<Aluno> alunoTable;
		#endif

		private AlunoManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
        }

		public static AlunoManager DefaultManager
		{
			get
			{
				return defaultInstance;
			}
			private set
			{
				defaultInstance = value;
			}
		}

		public MobileServiceClient CurrentClient
		{
			get { return client; }
		}
    }
}
