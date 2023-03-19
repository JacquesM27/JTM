using Microsoft.Data.SqlClient;

namespace JTM.IntegrationTests.Helpers
{
    public class CleanTestDatabaseToTest
    {
        private static async Task CleanDb(string connectionString)
        {
            string query = @"ALTER TABLE [dbo].[WorkingTimes] DROP CONSTRAINT [FK_WorkingTimes_Users_EmployeeId]

                            truncate table dbo.users

                            ALTER TABLE [dbo].[WorkingTimes]  WITH CHECK ADD  CONSTRAINT [FK_WorkingTimes_Users_EmployeeId] FOREIGN KEY([EmployeeId])
                            REFERENCES [dbo].[Users] ([Id])
                            ON DELETE CASCADE
                            ALTER TABLE [dbo].[WorkingTimes] CHECK CONSTRAINT [FK_WorkingTimes_Users_EmployeeId]";
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(query, connection);
            await command.Connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
