using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LCSAP_LC

{
    public partial class UploadData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
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
                    int linesCounter = 0;
                    int linesToProcess = lines.Length;
                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(savePath = appPath + saveDir + "StudentErrorLog.txt"))
                    {

                        foreach (string line in lines)
                        {
                            ncolumns = line.Split(',');

                            linesCounter ++;
                            if (ncolumns.Length == 15)
                            {
                                try
                                {
                                    using (var db = new Models.LCSAPDbContext())
                                    {
                                        
                                        var student = db.Students.Find(ncolumns[0].Trim());
                                        if (student == null)
                                        {
                                            int yearsToSubtract = int.Parse(ncolumns[12]);
                                            student = new Models.Student
                                            {
                                                Student_Id = ncolumns[0].Trim(),
                                                Student_FirstName = ncolumns[1],
                                                Student_MName = ncolumns[2],
                                                Student_LastName = ncolumns[3],
                                                Student_DOB = DateTime.Today.AddYears(yearsToSubtract * -1),
                                                Student_Gender = (ncolumns[13].Contains('M') || ncolumns[13].Contains('F') ? ncolumns[13] : "U"),
                                                Student_Major = ncolumns[10] + ncolumns[11]

                                        };
                                            
                                            db.Students.Add(student);
                                            db.SaveChanges();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    file.WriteLine("{0}: {1} ** {2}",linesCounter, line, ex.Message);
                                    ListBox1.Items.Add(string.Format("{0}: {1} ** {2}",linesCounter, line, ex.Message));
                                    //save to a log file if neccesary
                                }
                            }
                            else
                            {
                                file.WriteLine("{0}: {1}",linesCounter, line);
                                ListBox1.Items.Add(string.Format("{0}: {1}  Wrong number of parameters or commas",linesCounter, line));

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