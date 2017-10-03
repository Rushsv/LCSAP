using LCSAP_LC.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LCSAP_LC

{
    public partial class UploadData2 : System.Web.UI.Page
    {
        private LCSAPServices lcServices = new LCSAPServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.Text = lcServices.CurrentTerm;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                TextBox1.Text = TextBox1.Text.ToUpper();
                string saveDir = @"\Uploads\";

                // Get the physical file system path for the currently
                // executing application.
                string appPath = Request.PhysicalApplicationPath;

                // Before attempting to save the file, verify
                // that the FileUpload control contains a file.
                if (FileUpload1.HasFile)
                {
                    string savePath = appPath + saveDir +
                        Server.HtmlEncode(FileUpload1.FileName);

                    // Call the SaveAs method to save the 
                    // uploaded file to the specified path.
                    // This example does not perform all
                    // the necessary error checking.               
                    // If a file with the same name
                    // already exists in the specified path,  
                    // the uploaded file overwrites it.
                    FileUpload1.SaveAs(savePath);

                    // Notify the user that the file was uploaded successfully.
                    //UploadStatusLabel.Text = "Your file was uploaded successfully.";

                    //Models.LCSAPDbContext db = new Models.LCSAPDbContext();

                    string[] lines = System.IO.File.ReadAllLines((savePath));
                    string[] ncolumns;
                    int percentCompleted = 0;
                    int processCounter = 0;
                    int linesToProcess = lines.Length;
                    string cTerm = lcServices.CurrentTerm;
                    using (System.IO.StreamWriter file =
                         new System.IO.StreamWriter(savePath = appPath + saveDir + "CRNStudentLog.txt"))
                    {
                        //Removing courses from students
                                /*try
                                {
                                    using (var db = new Models.LCSAPDbContext())
                                    {
                                        {
                                            //var termCRNSections = db.CRNSections;
                                            var termCRNSections = db.CRNSections.Where(s => s.Term_Id == cTerm);
                                            foreach (Models.CRNSection itemCrn in termCRNSections)
                                            {
                                                foreach (var itemStudent in termCRNSections.SelectMany(x => x.Students))
                                                {
                                                    itemCrn.Students.Remove(itemStudent);
                                                }
                                            }
                                            db.SaveChanges();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    file.WriteLine(ex.Message);
                                    //save to a log file if neccesary
                                }*/

                        //Adding Sections to students.
                        processCounter = 0;
                        int linesCounter = 0;
                        
                        foreach (string line in lines)
                        {
                            linesCounter++;
                            ncolumns = line.Split(',');


                            if (ncolumns.Length == 4 )
                            {
                                try
                                {
                                    using (var db = new Models.LCSAPDbContext())
                                    {
                                        Models.Student student = new Models.Student();
                                        Models.CRNSection crnSection = new Models.CRNSection();
                                        student = db.Students.Find(ncolumns[0].Trim());
                                        crnSection = db.CRNSections.Find(int.Parse(ncolumns[1]));
                                        db.CRNSections.Attach(crnSection);
                                        db.Students.Attach(student);
                                        student.CRNSections.Add(crnSection);
                                        db.SaveChanges();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    file.WriteLine("{0} {1} {2}", ncolumns[0], ncolumns[1], ex.Message);
                                    ListBox1.Items.Add(string.Format("{0}: {1} {2} {3}", linesCounter, ncolumns[0], ncolumns[1], ex.Message));
                                    //save to a log file if neccesary
                                }
                            }
                            processCounter++;
                            percentCompleted = (processCounter / linesToProcess) * 100;
                            LabelProgress.Text = string.Format("{0}%", percentCompleted);
                        }
                    }
                }
            }
        }
    }
}