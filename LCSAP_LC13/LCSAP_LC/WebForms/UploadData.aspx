<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadData.aspx.cs" Inherits="LCSAP_LC.UploadData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title>Update Data</title>
    <!-- Bootstrap -->
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
            <div class="row">
                <div class="col-md-6">
                    <h3>Choose Student Info File (EnQ_Student_info.csv)<asp:FileUpload ID="FileUpload1" runat="server" Width="701px" />
                    </h3>
                     <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload" />
                    <asp:Label ID="LabelProgress" runat="server" Text="0"></asp:Label> Completed
                </div>
            </div>
            <div class="row">
            
                <br />
            <a href="../">Back to Home</a>
            </div>
            <div class="row">
                <h3 class="alert alert-warning">Error Log:</h3>
                <asp:ListBox ID="ListBox1" runat="server" Width="1096px" Height="329px"></asp:ListBox>
            </div>

        
        </form>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../Scripts/bootstrap.min.js"></script>
        </div>
</body>
</html>
