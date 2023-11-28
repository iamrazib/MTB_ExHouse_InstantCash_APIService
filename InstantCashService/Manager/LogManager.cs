using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InstantCashService.Manager
{
    public class LogManager
    {
        static string remittanceConnectionString = Global.Utility.DecryptString(ConfigurationManager.ConnectionStrings["RemittanceDBConnectionString"].ConnectionString.Trim());
        //MTBDBManager dbManager = null;

        public int RQRSLogger(int logid, string exHouse, int typeId, int isAuthenticated, string requestCode, string reqDtls, string respDtls)
        {
            int returnId = 0;

            SqlConnection openCon = new SqlConnection(remittanceConnectionString);
            SqlCommand cmdUpdateData = new SqlCommand();
            if (openCon.State.Equals(ConnectionState.Closed))
            {
                openCon.Open();
            }

            try
            {
                cmdUpdateData.CommandType = CommandType.StoredProcedure;
                cmdUpdateData.CommandText = "SP_ExHouse_RequestLog";
                cmdUpdateData.Connection = openCon;

                cmdUpdateData.Parameters.Add("@LogId", SqlDbType.Int).Value = logid;
                cmdUpdateData.Parameters.Add("@UserId", SqlDbType.VarChar).Value = exHouse;
                cmdUpdateData.Parameters.Add("@RequestTypeId", SqlDbType.Int).Value = typeId;
                cmdUpdateData.Parameters.Add("@Authenticated", SqlDbType.Bit).Value = isAuthenticated;
                cmdUpdateData.Parameters.Add("@RequestCode", SqlDbType.VarChar).Value = requestCode;
                cmdUpdateData.Parameters.Add("@RequestDetails", SqlDbType.VarChar).Value = reqDtls;
                cmdUpdateData.Parameters.Add("@ResponseDetails", SqlDbType.VarChar).Value = respDtls;

                SqlDataReader exQ = cmdUpdateData.ExecuteReader();
                while (exQ.Read())
                {
                    returnId = exQ.GetInt32(0);
                    return returnId;
                }
                return returnId;
            }
            catch (Exception ex)
            {
                string errorCode = "ERR000 : LogManager";
                string errorMsg = ex.Message;
                string errorStackTrace = ex.StackTrace;
                InsertIntoExHouseAPIErrorLog(exHouse, errorCode, errorMsg, errorStackTrace);
                return returnId;
            }
            finally
            {
                if (openCon.State.Equals(ConnectionState.Open))
                {
                    openCon.Close();
                }
            }

        }

        internal void InsertIntoExHouseAPIErrorLog(string refCode, string errorCode, string errorMsg, string errorStackTrace)
        {
            SqlConnection openCon = new SqlConnection(remittanceConnectionString);
            SqlCommand cmdSaveAcData = new SqlCommand();
            string sqlQuery = string.Empty;

            if (openCon.State.Equals(ConnectionState.Closed))
            {
                openCon.Open();
            }

            try
            {
                cmdSaveAcData.CommandType = CommandType.Text;
                cmdSaveAcData.Connection = openCon;

                sqlQuery = "INSERT INTO [dbo].[ExHouseAPIErrorLog] ([ReferenceCode], [ErrorCode], [ErrorMsg], [ErrorStackTrace]) VALUES(@ReferenceCode, @ErrorCode, @ErrorMsg, @ErrorStackTrace)";

                cmdSaveAcData.Parameters.Add("@ReferenceCode", SqlDbType.VarChar).Value = refCode;
                cmdSaveAcData.Parameters.Add("@ErrorCode", SqlDbType.VarChar).Value = errorCode;
                cmdSaveAcData.Parameters.Add("@ErrorMsg", SqlDbType.VarChar).Value = errorMsg;
                cmdSaveAcData.Parameters.Add("@ErrorStackTrace", SqlDbType.VarChar).Value = errorStackTrace;
                cmdSaveAcData.CommandText = sqlQuery;

                int result = cmdSaveAcData.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                openCon.Close();
                openCon.Dispose();
            }
        }


        /*
        public int RQRSLogger(int logid, int typeId, int isAuthenticated, string requestCode, string responseCode)
        {
            int returnId = 0;

            SqlConnection openCon = new SqlConnection(conString.RemittanceDataContext.ToString());
            SqlCommand cmdUpdateData = new SqlCommand();

            if (openCon.State.Equals(ConnectionState.Closed))
            {
                openCon.Open();
            }

            cmdUpdateData.CommandType = CommandType.StoredProcedure;
            cmdUpdateData.CommandText = "ICTC_RQ_Log";
            cmdUpdateData.Connection = openCon;

            cmdUpdateData.Parameters.Add("@LogId", SqlDbType.Int).Value = logid;
            cmdUpdateData.Parameters.Add("@RequestTypeId", SqlDbType.Int).Value = typeId;
            cmdUpdateData.Parameters.Add("@Authenticated", SqlDbType.Bit).Value = isAuthenticated;
            cmdUpdateData.Parameters.Add("@RequestCode", SqlDbType.VarChar).Value = requestCode;
            cmdUpdateData.Parameters.Add("@ResponseCode", SqlDbType.VarChar).Value = responseCode;


            try
            {
                SqlDataReader exQ = cmdUpdateData.ExecuteReader();
                while (exQ.Read())
                {
                    returnId = exQ.GetInt32(0);
                    return returnId;
                }
                return returnId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (openCon.State.Equals(ConnectionState.Open))
                {
                    openCon.Close();
                }
            }

        } */


    }
}