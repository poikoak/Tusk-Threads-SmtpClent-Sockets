using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamDllThreadSearchByMask
{
    public partial class Form1 : Form
    {
        public string FileSearch = "";
        public static ListViewItem item;
        public static ListViewItem itemIMG;
        private DriveInfo[] Drives = DriveInfo.GetDrives();
        private string[] strDrive = new string[5];
        private int count = 0;
        public int curent = 0;
        Type[] types;
        public int Maximum { get; set; }
        private bool isInProgress = false;
        private bool _forceClose = false;
        private readonly Form2 _form2 = new Form2();
        public Form1()
        {

            InitializeComponent();
            foreach (DriveInfo d in Drives)
            {
                if (d.IsReady)
                {
                    strDrive[count] = d.Name; count++;
                    cbDrive.Items.Add(d.VolumeLabel + "( " + d.Name + " )");
                }
            }
            cbDrive.Text = (string)cbDrive.Items[0];



        }





        private void bSearch_Click(object sender, EventArgs e)
        {
            if (isInProgress)
            {
                ChangeControlState(true);
                return;
            }
            lstFiles.Items.Clear();
            this.Text = "Search ...";
            FileSearch = txtSearch.Text + "*";





            var threadStart = new ThreadStart(delegate
            {
                try
                {
                    ChangeControlState(false);
                    GetAllFiles(new DirectoryInfo(txtPath.Text));
                }
                finally
                {
                    ChangeControlState(true);
                }
            });

            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        private void cbDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            curent = cbDrive.SelectedIndex;
        }

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Execute(lstFiles.SelectedItems[0].SubItems[1].Text);
        }







        #region ::  Methods   ::
        public void GetAllFiles(DirectoryInfo di)
        {
            /* statusLabel.Foreground = new SolidColorBrush(Colors.Green);*/

            Application.DoEvents();
            foreach (FileInfo file in di.GetFiles(FileSearch))
            {

                if ((file.Attributes & FileAttributes.Hidden) == 0)
                {

                    imageList1.Images.Add(Icon.ExtractAssociatedIcon(file.FullName));
                    item = lstFiles.Items.Add(System.IO.Path.GetFileName(file.Name), imageList1.Images.Count - 1);
                    item.SubItems.Add(file.FullName);
                    item.SubItems.Add((file.Length / (1024 ^ 3)).ToString());
                    item.SubItems.Add(file.LastWriteTime.ToString());
                    this.Text = string.Format("Found = {0} items", lstFiles.Items.Count);


                }

            }
            foreach (DirectoryInfo director in di.GetDirectories())
            {
                if ((director.Attributes & FileAttributes.Hidden) == 0) GetAllFiles(director);
            }


            for (var i = 0; i < lstFiles.Items.Count; i++)
            {
                if (_forceClose || !isInProgress)
                {
                    Thread.CurrentThread.Abort();
                }
                try
                {
                    if (progressBar1.Value < progressBar1.Maximum)
                    {
                        progressBar1.Value++;
                        progressBar1.Maximum--;
                        Task.Delay(1000);
                    }

                    /*UpdateProgress(i);*/

                }
                catch (IOException)
                {
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        public void Execute(string cmd)
        {
            if (cmd != string.Empty)
            {
                Process proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = cmd;
                proc.Start(); proc.Close();
                proc.Dispose();
            }
        }


        private void UpdateProgress(int value)
        {
            progressBar1.PerformSafely(() => progressBar1.Value = value);

        }

        private void ChangeControlState(bool state)
        {
            isInProgress = !state;
            bSearch.PerformSafely(() => bSearch.Text = !state ? "Stop" : "Start");
            progressBar1.PerformSafely(() => progressBar1.Visible = !state);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _forceClose = true;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {

            /* _form2.ShowDialog();*/
         
            try
            {
                // динамическая загрузка .NET сборки
                Assembly asm = Assembly.LoadFrom("ClassLibrary1.dll");

                // получить все типы данных, находящиеся в указанной сборке
                types = asm.GetTypes();
                foreach (Type t in types)
                {
                    if (t.Name == "FirstClass")
                    {
                        // перебор методов в текущем типе
                        MethodInfo[] mi = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);



                        // вывод обнаруженных методов
                        foreach (MethodInfo m in mi)
                        {


                            // если обнаружен целевой метод - запустить его
                            if (m.Name == "SetBg1")
                            {
                                try
                                {
                                    // создание экземпляра класса, который выбрал пользователь в списке
                                    // обнаруженных в сборке классов на основе информации в переменной типа Type
                                    Object obj = Activator.CreateInstance(t);

                                    // запуск целевого метода
                                    object[] arg = new object[1];
                                    arg[0] = this;

                                    // obj - объект, для которого запускается метод
                                    // arg - список аргументов метода
                                    m.Invoke(obj, arg);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // динамическая загрузка .NET сборки
                Assembly asm = Assembly.LoadFrom("ClassLibrary1.dll");

                // получить все типы данных, находящиеся в указанной сборке
                types = asm.GetTypes();
                foreach (Type t in types)
                {
                    if (t.Name == "SecondClass")
                    {
                        // перебор методов в текущем типе
                        MethodInfo[] mi = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);



                        // вывод обнаруженных методов
                        foreach (MethodInfo m in mi)
                        {


                            // если обнаружен целевой метод - запустить его
                            if (m.Name == "SetBg2")
                            {
                                try
                                {
                                    // создание экземпляра класса, который выбрал пользователь в списке
                                    // обнаруженных в сборке классов на основе информации в переменной типа Type
                                    Object obj = Activator.CreateInstance(t);

                                    // запуск целевого метода
                                    object[] arg = new object[1];
                                    arg[0] = this;

                                    // obj - объект, для которого запускается метод
                                    // arg - список аргументов метода
                                    m.Invoke(obj, arg);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();

            /* OpenFileDialog OD = new OpenFileDialog();
             OD.FileName = "";

             if (OD.ShowDialog() == DialogResult.OK)
             {

                 txtPath.Text = OD.FileName;
                 Properties.Settings.Default. = OD.FileName;
                 Properties.Settings.Default.Save();
             }*/

           

            if (Directory.Exists(txtPath.Text))
            {
                dlg.SelectedPath = txtPath.Text;
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = dlg.SelectedPath;
              
                Properties.Settings.Default.Save();
            }

        }
    }


 
    public static class FormHelper
    {
        public static void ShowInvisible(this Form form)
        {
            // сохраняем параметры окна
            bool needToShowInTaskbar = form.ShowInTaskbar;
            FormWindowState initialWindowState = form.WindowState;

            // делаем окно невидимым
            form.ShowInTaskbar = false;
            form.WindowState = FormWindowState.Minimized;

            // показываем и скрываем окно
            form.Show();
            form.Hide();

            // восстанавливаем параметры окна
            form.ShowInTaskbar = needToShowInTaskbar;
            form.WindowState = initialWindowState;
        }
    }
}
