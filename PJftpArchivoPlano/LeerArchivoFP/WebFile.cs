using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace PJftpArchivoPlano
{

    /*
     En esta clase utilizada para subir el archivo al Servidor FTP
         
         */
    class WebFile
    {


        public WebFile()
        {


        }

        public void SubirArchivoWeb(string FTP,string user, string pass, string nombreFile)
        {

            try
            {
              

                FtpWebRequest dirFtp = ((FtpWebRequest)FtpWebRequest.Create(FTP + "/" + nombreFile));

                // Los datos del usuario (credenciales)
                NetworkCredential cr = new NetworkCredential(user, pass);
                dirFtp.Credentials = cr;

                dirFtp.UsePassive = true;
                dirFtp.UseBinary = true;
                dirFtp.KeepAlive = true;

                dirFtp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream stream = File.OpenRead(@"C:\\xxxxx\\" + nombreFile + "");

                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                stream.Close();

                Stream reqStream = dirFtp.GetRequestStream();

                reqStream.Write(buffer, 0, buffer.Length);

                reqStream.Flush();

                reqStream.Close();

                //MessageBox.Show("Archivo Subido");
            }
            catch (Exception ex)
            {

                MessageBox.Show("El servidor esta apagado para realizar Esta Tarea Programada"+ex.Message);
            }
        }
    }
}
