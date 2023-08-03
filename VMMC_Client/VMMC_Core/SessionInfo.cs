using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace VMMC_Core
{
    public class SessionInfo : INotifyPropertyChanged
    {
        private string serverName;
        public string ServerName
        {
            get { return serverName; }
            set
            {
                serverName = value;
                OnConnectionPropertyChanged("ServerName");
                GetProjectId();
            }
        }


        private string dataBaseName;
        public string DataBaseName
        {
            get { return dataBaseName; }
            set
            {
                dataBaseName = value;
                OnConnectionPropertyChanged("DataBaseName");
                GetProjectId();
            }
        }


        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnConnectionPropertyChanged("UserName");
            }
        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;
                OnConnectionPropertyChanged("UserPassword");
            }
        }


        private string userFIO;
        public string UserFIO
        {
            get { return userFIO; }
            set
            {
                userFIO = value;
                OnConnectionPropertyChanged("UserFIO");
            }
        }


        private string hostName;
        public string HostName
        {
            get { return hostName; }
            set
            {
                hostName = value;
                OnConnectionPropertyChanged("HostName");
            }
        }


        private Guid projectId;
        public Guid ProjectId
        {
            get { return projectId; }
            set
            {
                projectId = value;
                OnConnectionPropertyChanged("ProjectId");
            }
        }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnConnectionPropertyChanged("ProjectName");
            }
        }

        private string projectCode;
        public string ProjectCode
        {
            get { return projectCode; }
            set
            {
                projectCode = value;
                OnConnectionPropertyChanged("ProjectCode");
            }
        }

        public string ConnectionString
        {
            get 
            {
                
                string result = "Server=" + ServerName;
                if (DataBaseName != null) result += ";Initial Catalog=" + DataBaseName;
                if (UserPassword != null) result += ";User ID="+UserName+"; Password=" + UserPassword;
                else result += ";Integrated security=SSPI";
                return result;
            }
            //set
            //{
            //    dataBaseName = value;
            //    OnConnectionPropertyChanged("ConnectionString");
            //}
        }
        public List<string> AvalibaleDataBaseList
        {
            get 
            {
                List<string> result = GetDatabasesHere(ConnectionString);
                if (result == null) 
                {
                    UserName = "User2"; UserPassword = "qwerty12345";
                    result = GetDatabasesHere(ConnectionString);
                }
                return GetDatabasesHere(ConnectionString); 
            
            }
            //set
            //{
            //    avalibaleDataBaseList = GetDatabasesHere(serverName);
            //    OnConnectionPropertyChanged("AvalibaleDataBaseList");
            //}
        }
        public static List<string> GetDatabasesHere(string connectionString)
        {
            if (connectionString != null)
            {
                List<string> dbList = new List<string>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();// устанавливаем соединение с БД
                    }
                    catch
                    {
                        return null;
                    }
                    

                    string sql = "SELECT name FROM sys.databases";

                    SqlCommand cmd = new SqlCommand(sql, connection);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dbList.Add(dr["name"].ToString());
                        }
                    }
                    return dbList;
                }
            }
            else return null;
        }
        public void GetProjectId()
        {
            if (ServerName != null && DataBaseName != null)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                    
                {
                    connection.Open();// устанавливаем соединение с БД

                    string sql = "SELECT [Id], [Name], [Description] FROM [dbo].[Projects]";

                    SqlCommand cmd = new SqlCommand(sql, connection);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ProjectId = Guid.Parse(dr["Id"].ToString());
                            ProjectName = dr["Name"].ToString();
                            ProjectCode = dr["Description"].ToString();
                            if (ProjectCode == "") ProjectCode = ProjectName;
                        }
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnConnectionPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
