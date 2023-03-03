using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReleaseDateReNamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Populate and initialzie the combo box
            cmbChangeType.ItemsSource = new object[] {
            "Add REL Date",
            "Add Obsolete",
            "Add RefOnly",
            "Remove all formatting"};
            cmbChangeType.SelectedIndex = 0;
        }

        private void btnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            if (Cal.SelectedDate.HasValue)
            {
                // Create OpenFileDialog

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



                // Set filter for file extension and default file extension

                dlg.DefaultExt = ".";
                //dlg.InitialDirectory = "c:\\";
                dlg.Filter = "All Documents (.)|*.*";
                dlg.Multiselect = true;
                


                // Display OpenFileDialog by calling ShowDialog method

                Nullable<bool> result = dlg.ShowDialog();



                // Get the selected file name and display in a TextBox

                if (result == true)
                {

                    foreach (string obj in dlg.FileNames)
                    {

                        string filename = obj;
                        string newfilename = filename;
                        int dot = newfilename.LastIndexOf(".");
                        int slash = filename.LastIndexOf("\\");


                        switch (cmbChangeType.SelectedIndex)//start of combo switch
                        {
                            case 0://REL STUFF
                                if (String.IsNullOrEmpty(Cal.SelectedDate.Value.ToString()))
                                {
                                    //select date first box
                                    MessageBox.Show("No date selected, closing program.");
                                }
                                else
                                {
                                    string relDate = "_REL" + Cal.SelectedDate.Value.ToString("MMddyy");

                                    //Replace replaces all occurences.  

                                    // filename.substing change to remove

                                    // use insert

                                    //
                                    if (filename.Substring(slash).ToUpper().Contains("_REL"))
                                    {
                                        int iRelIndex = filename.ToUpper().LastIndexOf("_REL");
                                        int iExtentionIndex = filename.LastIndexOf('.');
                                        string sOldRelDate = filename.Substring(iRelIndex, iExtentionIndex - iRelIndex);

                                        newfilename = filename.Substring(0, iRelIndex) + relDate + filename.Substring(iExtentionIndex);
                                        // newfilename = filename.Replace(sOldRelDate, relDate);
                                        try
                                        {
                                            File.Move(filename, newfilename);
                                        }
                                        catch (IOException ex1)
                                        {
                                            MessageBox.Show("Duplicate file name found.  Aborting Operation.\n"
                                                + filename.Substring(filename.LastIndexOf("\\")) + ex1);
                                            return;
                                        }

                                    }

                                    else
                                    {
                                        int iExtensionIndex = filename.LastIndexOf(".");
                                        newfilename = filename.Insert(iExtensionIndex, relDate);
                                        File.Move(filename, newfilename);
                                    }
                                }


                                break;
                            case 1: //OBSOLETE STUFF
                                if (filename.Substring(slash).ToUpper().Contains("OBSOLETE_"))
                                {
                                    newfilename = filename.Replace("OBSOLETE_", "OBSOLETE_");
                                }
                                else
                                {
                                    newfilename = filename.Insert(slash + 1, "OBSOLETE_");
                                }
                                if (File.Exists(newfilename))
                                {
                                    MessageBox.Show("Duplicate file name found.  Aborting Operation.\n"
                                        + filename.Substring(filename.LastIndexOf("\\")));
                                    return;
                                }
                                File.Move(filename, newfilename);
                                break;

                            case 2: //REFONLY STUFF
                                if (filename.Substring(slash).ToUpper().Contains("REFONLY_"))
                                {
                                    newfilename = filename.Replace("RefOnly_", "RefOnly_");

                                }
                                else
                                {
                                    newfilename = filename.Insert(slash + 1, "RefOnly_");
                                }

                                if (File.Exists(newfilename))
                                {
                                    MessageBox.Show("Duplicate file name found.  Aborting Operation.\n"
                                           + filename.Substring(filename.LastIndexOf("\\")));
                                    return;
                                }
                                File.Move(filename, newfilename);
                                break;


                            case 3: //REMOVE ALL STUFF
                                //with stringbuilder - not working
                                //StringBuilder removedFileName = new StringBuilder();
                                //string builtString;
                                //do
                                //{
                                //    int rstart = filename.ToUpper().LastIndexOf("_REL");
                                //    removedFileName.Remove(rstart, 10);
                                //    removedFileName.Replace("obsolete_".ToUpper(), null);
                                //    removedFileName.Replace("RefOnly_", null);

                                //    builtString = removedFileName.ToString();
                                //    File.Move(filename, builtString);
                                //} while (filename != builtString);


                                /************* Old code using same string****************/

                                int rstart = filename.ToUpper().LastIndexOf("_REL");
                                string oldfilename = filename;

                                string file;
                                file = filename.Substring(slash);
                                if (filename.Substring(slash).ToUpper().Contains("_REL"))
                                {
                                    filename = filename.Remove(rstart, 10);
                                    file = filename.Substring(slash);
                                }
                                if (filename.Substring(slash).ToUpper().Contains("OBSOLETE_"))
                                {
                                    filename = filename.Substring(0, slash) + file.Replace("obsolete_".ToUpper(), null);
                                }
                                if (filename.Substring(slash).ToUpper().Contains("REFONLY_"))
                                {
                                    filename = filename.Substring(0, slash) + file.Replace("RefOnly_", null);
                                }

                                File.Move(oldfilename, filename);



                                break;

                                //default:
                                //    System.Windows.Forms.MessageBox.Show("please select an option first");
                                // should not happen
                                //end of combo switch

                        }

                    }
                }
            }
            else
            {
                MessageBox.Show("Please select release date first.");
            }
        }
    }
}
