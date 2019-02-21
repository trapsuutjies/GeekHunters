<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GeekHunterASP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Welcome to Geek Hunters!</h1>
        <p class="lead">Add your new geek below, or filter the existing geeks by skill:
            <asp:TextBox runat="server" ID="skillFilter" OnTextChanged="SkillFilter_TextChanged" AutoPostBack="true" />
        </p>
        <p>
            New Geek: First Name: <asp:TextBox runat="server" ID="newName" OnTextChanged="SanitizeNames" AutoPostBack="true" /> 
            Last Name: <asp:TextBox runat="server" ID="newSurname" OnTextChanged="SanitizeNames" AutoPostBack="true"/> 
            Skill: <asp:DropDownList runat="server" ID="newSkill" />
            
            <asp:Button Text="Add" runat="server" OnClick="Add_Click"/>
        </p>
        <asp:DataGrid ID="gridGeeks" runat="server" CellPadding="5" CellSpacing="5"></asp:DataGrid>
    </div>

</asp:Content>
