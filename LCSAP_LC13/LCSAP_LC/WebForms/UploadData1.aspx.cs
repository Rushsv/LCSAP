using LCSAP_LC.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LCSAP_LC

{
    public partial class UploadData1 : System.Web.UI.Page
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
                    int linesCounter = 0;
                    using (System.IO.StreamWriter file =
                         new System.IO.StreamWriter(savePath = appPath + saveDir + "CRNerrorLog.txt"))
                    {
                        //Adding New Subjects
                        foreach (string line in lines)
                        {
                            linesCounter++;
                            ncolumns = line.Split(',');


                            if (ncolumns.Length == 7 )
                            {
                                try
                                {
                                    using (var db = new Models.LCSAPDbContext())
                                    {
                                        var s = db.Subjects.Find(ncolumns[0]);
                                        if (s == null)
                                        {
                                            Models.Subject subject = new Models.Subject();
                                            subject.Subject_Id = ncolumns[0];
                                            subject.Subject_Name = ncolumns[3];
                                            db.Subjects.Add(subject);
                                            db.SaveChanges();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    file.WriteLine("{0} {1} - {2}", ncolumns[0] + ncolumns[1].PadLeft(3, '0'), ncolumns[3], ex.Message);
                                    ListBox1.Items.Add(string.Format("{0}: {1} {2} - {3}",linesCounter, ncolumns[0] + ncolumns[1].PadLeft(3, '0'), ncolumns[3], ex.Message));
                                    //save to a log file if neccesary
                                }
                            }
                            else
                            {
                                file.WriteLine("{0}", line);
                                ListBox1.Items.Add(string.Format("{0}: {1} Wrong number of parameters or commas",linesCounter, line));

                            }

                            processCounter++;
                            percentCompleted = (processCounter / linesToProcess) * 100;
                            LabelProgress.Text = string.Format("{0}%", percentCompleted);
                        }

                        //Adding New Courses
                        linesCounter = 0;
                        foreach (string line in lines)
                        {
                            linesCounter++;
                            ncolumns = line.Split(',');


                            if (ncolumns.Length == 7 )
                            {
                                try
                                {
                                    using (var db = new Models.LCSAPDbContext())
                                    {
                                        string c_id = ncolumns[0] + ncolumns[1].PadLeft(3, '0');
                                        var c = db.Courses.Find(c_id);
                                        if (c == null)
                                        {
                                            Models.Course course = new Models.Course();
                                            course.Course_Id = ncolumns[0] + ncolumns[1].PadLeft(3, '0');
                                            course.Course_Title = ncolumns[3];
                                            course.Subject_Id = ncolumns[0];
                                            db.Courses.Add(course);
                                            db.SaveChanges();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    file.WriteLine("{0} {1} - {2}", ncolumns[0] + ncolumns[1].PadLeft(3, '0'),ncolumns[3], ex.Message);
                                    ListBox1.Items.Add(string.Format("{0}: {1} {2} - {3}", linesCounter, ncolumns[0] + ncolumns[1].PadLeft(3, '0'), ncolumns[3], ex.Message));
                                    //save to a log file if neccesary
                                }
                            }
                            processCounter++;
                            percentCompleted = (processCounter / linesToProcess) * 100;
                            LabelProgress.Text = string.Format("{0}%", percentCompleted);
                        }
                        //Adding Sections per Term.
                        processCounter = 0;
                        linesCounter = 0;

                        foreach (string line in lines)
                        {
                            linesCounter++;
                            ncolumns = line.Split(',');


                            if (ncolumns.Length == 7 && lcServices.SeekTerm(TextBox1.Text))
                            {
                                try
                                {
                                    using (var db = new Models.LCSAPDbContext())
                                    {
                                        var r = db.CRNSections.Find(int.Parse(ncolumns[2]));
                                        if (r == null)
                                        {
                                            Models.CRNSection crnSection = new Models.CRNSection();
                                            crnSection.Course_Id = ncolumns[0] + ncolumns[1].PadLeft(3, '0');
                                            crnSection.CRN_Id = int.Parse(ncolumns[2]);
                                            crnSection.Term_Id = TextBox1.Text.ToUpper();
                                            db.CRNSections.Add(crnSection);
                                            db.SaveChanges();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    file.WriteLine("{0} {1} {2} - {3}",ncolumns[0]+ncolumns[1].PadLeft(3,'0'),ncolumns[2],TextBox1.Text, ex.Message);
                                    ListBox1.Items.Add(string.Format("{0}: {0} {1} {2} - {3}",linesCounter, ncolumns[0] + ncolumns[1].PadLeft(3, '0'), ncolumns[2], TextBox1.Text, ex.Message));
                                    //save to a log file if neccesary
                                }
                            }

                            processCounter++;
                            percentCompleted = (processCounter / linesToProcess) * 100;
                            LabelProgress.Text = string.Format("{0}%", percentCompleted);
                        }
                        try
                        {
                            using (var db = new Models.LCSAPDbContext())
                            {
                                Models.CRNSection crnSection = new Models.CRNSection();
                                int labSupportExists = db.CRNSections.Where(l => l.CRN_Id == 0).Count();
                                if (labSupportExists == 1)
                                {
                                    crnSection = db.CRNSections.Find(0);
                                    db.Entry(crnSection).State = EntityState.Modified;
                                    crnSection.Term_Id = TextBox1.Text.ToUpper();
                                    db.SaveChanges();
                                }
                                else
                                {
                                    crnSection.Course_Id = "LAB000";
                                    crnSection.CRN_Id = 000;
                                    crnSection.Term_Id = TextBox1.Text.ToUpper();
                                    db.CRNSections.Add(crnSection);
                                    db.SaveChanges();
                                }

                            }
                        }
                        catch { }

                    }
                }
            }
        }
    }
}