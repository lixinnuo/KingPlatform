namespace Basic.Code
{
    public class DBConnection
    {
        public static bool Encrypt { get; set; }

        public DBConnection(bool encrypt)
        {
            Encrypt = encrypt;
        }

        public static string connectionString
        {
            get
            {
                string connection = System.Configuration.ConfigurationManager.ConnectionStrings["KingDbContext"].ConnectionString;
                if (Encrypt == true)
                {
                    return DESEncrypt.Decrypt(connection);
                }
                else
                {
                    return connection;
                }
            }
        }


    }
}
