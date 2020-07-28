using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Data;
using System.Text;

namespace UnitTestProject
{
    internal class Scripts : Singleton<Scripts>
    {
        private string DROP_SCRIPT
        {
            get
            {
                return BuildScript(new string[] {
                    "DELETE FROM Subscribe",
                    "DELETE FROM React",
                    "DELETE FROM ReactionType",
                    "DELETE FROM Follow",
                    "DELETE FROM Importance",
                    "DELETE FROM Comment",
                    "DELETE FROM Share",
                    "DELETE FROM Article",
                    "DELETE FROM Blog",
                    "DELETE FROM Credential",
                    "DELETE FROM Connexion",
                    "DELETE FROM Notification",
                    "DELETE FROM Kind",
                    "DELETE FROM Follow",
                    "DELETE FROM Message",
                    "DELETE FROM Account",
                    "DELETE FROM Importance",
                    "DELETE FROM Status",
                    "DELETE FROM State",
                    "DELETE FROM Visibility"
                });
            }
        }

        private string UNLINK_SCRIPT
        {
            get
            {
                return BuildScript(new string[] {
                    "UPDATE Comment SET AnswerId = NULL",
                    "UPDATE Message SET AnswerId = NULL",
                    "UPDATE Message SET AnswerFromId = NULL",
                    "UPDATE Message SET AnswerToId = NULL"
                });
            }
        }

        private string INSERT_ALL_DISTINGUISHING_TYPES
        {
            get
            {
                return BuildScript(new string[] {
                    "INSERT INTO `visibility` VALUES" +
                    "('1', 'Public')," +
                    "('2','Friends')," +
                    "('3','Personal')," +
                    "('4','Hidden')," +
                    "('99','Removed')",

                    "INSERT INTO `Status` VALUES" +
                    "('1', 'Friend')," +
                    "('2','PendingAlpha')," +
                    "('3','Following'),"+
                    "('4','PendingBeta'),"+
                    "('99','Blocked')",

                    "INSERT INTO `State` VALUES" +
                    "('1', 'Sending')," +
                    "('2','Sent')," +
                    "('3','Unsent'),"+
                    "('4','Removed')",

                    "INSERT INTO `Importance` VALUES" +
                    "('1', 'Default')," +
                    "('2','First')," +
                    "('3','Hidden')",

                    "INSERT INTO `Kind` VALUES" +
                    "('1', 'System')," +
                    "('2','BLog')," +
                    "('3','Social')," +
                    "('4','Messenger')",

                    "INSERT INTO `ReactionType` VALUES" +
                    "('1','like')," +
                    "('2','love')," +
                    "('3','haha')," +
                    "('4','surprised')," +
                    "('5','sad')," +
                    "('6','angry')," +
                    "('7','confused')"
                });
            }
        }

        private Boolean Once { get; set; }

        private string BuildScript(string[] tablesActions)
        {
            StringBuilder SB = new StringBuilder();
            foreach (string ta in tablesActions)
            {
                SB.Append(ta).Append(";\n");
            }
            string result = SB.ToString();
            return result;
        }

        public void SetStartupDatabase(string connectionString)
        {
            if (!Once)
            {
                Once = true; // initialization done
                var options = new DbContextOptionsBuilder().UseMySql(connectionString).Options;
                using (DbContext context = new DbContext(options))
                {
                    RelationalDatabaseFacadeExtensions.BeginTransaction(
                    context.Database,
                    IsolationLevel.ReadUncommitted
                );

                    context.Database.ExecuteSqlRaw(UNLINK_SCRIPT);
                    context.Database.ExecuteSqlRaw(DROP_SCRIPT);
                    context.Database.ExecuteSqlRaw(INSERT_ALL_DISTINGUISHING_TYPES);

                    context.Database.CommitTransaction();
                }
            }
        }
    }
}