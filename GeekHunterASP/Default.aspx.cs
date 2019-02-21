using System;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GeekHunterASP
{
    public partial class _Default : Page
    {
        /// <summary>
        /// Load or refresh data. Ensure refreshes retain correct filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            if (!string.IsNullOrWhiteSpace(skillFilter.Text))
                SkillFilter_TextChanged(this, null);
        }

        /// <summary>
        /// Construct the connection object. ONLY change here if the file is moved.
        /// </summary>
        /// <returns></returns>
        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=|DataDirectory|\\GeekHunter.sqlite");
        }

        /// <summary>
        /// Load the data and display. Ignore ID's as the user doesn't need them in this context.
        /// </summary>
        private void LoadData()
        {
            using (SQLiteConnection cn = GetConnection())
            {
                cn.Open();
                string command = "select FirstName as [First Name], LastName as [Last Name], Skill.Name as Skill from Candidate INNER JOIN Skill on Candidate.SkillId = Skill.Id";
                DataSet candidateSet = new DataSet();
                SQLiteDataAdapter da = new SQLiteDataAdapter(command, cn);
                da.Fill(candidateSet, "Candidate");
                DataView dvCandidate = candidateSet.Tables[0].DefaultView;

                gridGeeks.CellPadding = 5;
                gridGeeks.DataSource = dvCandidate;
                gridGeeks.DataBind();

                DataSet skillSet = new DataSet();
                string skillCommand = "select Name, Id from Skill";
                SQLiteDataAdapter skillNames = new SQLiteDataAdapter(skillCommand, cn);
                skillNames.Fill(skillSet, "Skill");
                DataView skillView = skillSet.Tables[0].DefaultView;
                newSkill.DataSource = skillView;
                newSkill.DataTextField = "Name";
                newSkill.DataValueField = "Id";
                newSkill.DataBind();

            }
        }

        /// <summary>
        /// Update the displayed results with the filter whenever filter is updated. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SkillFilter_TextChanged(object sender, EventArgs e)
        {
            if (gridGeeks.DataSource is DataView view)
            {
                if (string.IsNullOrWhiteSpace(skillFilter.Text))
                    view.RowFilter = "";
                else
                    view.RowFilter = string.Format("Skill LIKE '{0}%' OR Skill LIKE '%{0}%'", skillFilter.Text);
                gridGeeks.DataBind();
            }
        }

        /// <summary>
        /// Use RegEx to ensure only valid characters, and a conditional to avoid looping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SanitizeNames(object sender, EventArgs e)
        {
            if (sender is TextBox box)
            {
                string newText = Regex.Replace(box.Text, @"[^a-zA-Z ]", "");
                if (box.Text != newText)
                    box.Text = newText;
            }
        }

        /// <summary>
        /// Add new user. Next step would be to avoid duplicates, but since these exist in real life
        /// they are allowed - consider adding photo ID to avoid confusion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(newName.Text))
            {
                if (!string.IsNullOrWhiteSpace(newSurname.Text))
                {
                    using (SQLiteConnection cn = GetConnection())
                    {
                        cn.Open();
                        SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Candidate (FirstName, LastName, SkillId) VALUES (?,?,?)", cn);
                        insertSQL.Parameters.AddWithValue("FirstName", newName.Text);
                        insertSQL.Parameters.AddWithValue("LastName", newSurname.Text);
                        insertSQL.Parameters.AddWithValue("SkillId", newSkill.SelectedValue);
                        try
                        {
                            insertSQL.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
        }
    }
}