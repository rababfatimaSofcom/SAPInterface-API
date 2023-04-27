using SApInterface.API.Model.Domain;
using Sofcom.CoreServices;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Principal;
using SApInterface.API.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace SApInterface.API.Repositry
{


    public class ProductRepositry : IProductRepositry
    {
  
        private Sofcom.CoreServices.IDbManager _dbManager;
        public List<Section> sectionResult = new List<Section>();
       public List<SectionDetail> sectiondet = new List<SectionDetail>(); 
        public Section sectionresponse = new Section();
        System.DateTime MDateTime;
        private IConfiguration _configuration;
        protected Sofcom.CoreServices.IDbManager dbManager
        {
            get
            {
                return _dbManager;
            }
            set 
            {
                _dbManager = value;
            }
        }
        public ProductRepositry(IConfiguration configuration)
        {
            _configuration = configuration;
            SF0001G.LocalDBCredentialsProvider.MIntCompID = configuration.GetValue<int>("MySettings:ComId");
            SF0001G.LocalDBCredentialsProvider.GstrEnv = configuration.GetValue<string>("MySettings:Env");
            SF0001G.GClsGeneral.GStrDBType = "SQL";

            SF0001G.GClsConnection_Sql.LAppType = "Web";
            SF0001G.GClsConnection_Sql.MIntCompID = configuration.GetValue<int>("MySettings:ComId");//1011;
            SF0001G.GClsConnection_Sql.GstrEnv = configuration.GetValue<string>("MySettings:Env"); //"Live";
            SF0001G.GClsConnection_Sql.GstrServer  = configuration.GetValue<string>("MySettings:Server");  //"SofsrvDB01\\Client2014";

            this.dbManager = SF0001G.Factory.GetDBManager();

        }

        public async Task<List<Section>> GetAsync()
        {
            try
            {
                StringBuilder selectCommand = new StringBuilder();
                DataTable dt = new DataTable();

                selectCommand.Append("Select * from SPBSSectionPRM ");
                //selectCommand.Append(" where ProdCode = @ProdCode");

                //List<DbParameter> dbParameters = new List<DbParameter>()
                //{
                //    new SqlParameter() {ParameterName = "ProdCode", DbType = DbType.String, Value = ""}
                //};
                dt = this.dbManager.FetchData(selectCommand.ToString());

                if (dt.Rows.Count > 0)
                {
                    //{
                    //    new Section()
                    //    {
                    //        sectionCode = dt.Rows[0]["SectionCode"].ToString().Trim(),
                    //        sectionName = dt.Rows[0]["SectionDesc"].ToString().Trim()
                    //    };
                    //};
                    Section section = new Section()
                    {
                        sectionCode = dt.Rows[0]["SectionCode"].ToString().Trim(),
                        sectionName = dt.Rows[0]["SectionDesc"].ToString().Trim()
                    };
                    sectionResult.Add(section);
                }
            }
            catch (Exception ex)
            {

                //throw ex.Message;
            }
            return sectionResult.ToList();
            //throw new NotImplementedException();
        }

        public async Task<Section> GetSectionAsync(string code)
        {
            try
            {
                StringBuilder selectCommand = new StringBuilder();
                DataTable dt = new DataTable();

                selectCommand.Append("Select * from SPBSCOUNTRYPRM ");
                selectCommand.Append(" where CountryCode = @CountryCode");

                List<DbParameter> dbParameters = new List<DbParameter>()
                {
                    new SqlParameter() {ParameterName = "CountryCode", DbType = DbType.String, Value = code}
                };
                dt = this.dbManager.FetchData(selectCommand.ToString(), dbParameters.ToArray());

                if (dt.Rows.Count > 0)
                {
                  
                    await Task.Run(() => sectionresponse = new Section()
                    {
                        sectionCode = dt.Rows[0]["CountryCode"].ToString().Trim(),
                        sectionName = dt.Rows[0]["CountryName"].ToString().Trim()
                    });
                   
                }
            }
            catch (Exception)
            {

                //throw ex.Message;
            }
            return sectionresponse;
        }

        public string AddAsync(Product prod, DataSet Mdsdataset)
        {
            string LStrErr;
            SF0001G.GClsGeneral LClsGeneral = new SF0001G.GClsGeneral();
            MDateTime = DateTime.Now;
            StringBuilder insertCommand = new StringBuilder();
            StringBuilder updateCommand = new StringBuilder();
            StringBuilder deleteCommand = new StringBuilder();
            List<DbParameter> parameters = new List<DbParameter>();

           
           
            DataTable dt=new DataTable();
            dt = Mdsdataset.Tables["Product"];
           
            LStrErr = ClsFncValidate(Mdsdataset);
            if ((LStrErr.Length) != 0)
            {
                return LStrErr;
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        for (int i = 0; i < dt.Rows.Count - 1; i++)
                        {

                            if (dt.Rows[i]["Status"].ToString()=="Y")
                            { 
                            insertCommand.Append("Insert into SPBSProdPRM (PRODCODE,PRODSHTNAME,PRODLNGNAME,PRODCATECODE,");
                            insertCommand.Append(" REGISTRATIONNO,BATCHSIZE,STATUS,STORAGECON,UNIT,");
                            insertCommand.Append(" LASTUSER, LASTUPDATE,CREATEDBY,CREATEDON) Values(");
                            insertCommand.Append(" @PRODCODE,@PRODSHTNAME,@PRODLNGNAME,@PRODCATECODE,");
                            insertCommand.Append(" @REGISTRATIONNO,@BATCHSIZE,@STATUS,@STORAGECON,@UNIT,");
                            insertCommand.Append(" @LASTUSER, @LASTUPDATE,@CREATEDBY,@CREATEDON)");

                                        parameters = new List<DbParameter>()
                                {
                                new SqlParameter() {ParameterName = "PRODCODE", DbType = DbType.String, Value = dt.Rows[i]["PRODCODE"]},
                                new SqlParameter() {ParameterName = "PRODSHTNAME", DbType = DbType.String, Value = dt.Rows[i]["PRODSHTNAME"]},
                                new SqlParameter() {ParameterName = "PRODLNGNAME", DbType = DbType.String, Value = dt.Rows[i]["PRODLNGNAME"]},
                                new SqlParameter() {ParameterName = "PRODCATECODE", DbType = DbType.String, Value = dt.Rows[i]["PRODCATECODE"]},
                                new SqlParameter() {ParameterName = "REGISTRATIONNO", DbType = DbType.String, Value = ""},
                                new SqlParameter() {ParameterName = "BATCHSIZE", DbType = DbType.String, Value = dt.Rows[i]["BATCHSIZE"]},
                                new SqlParameter() {ParameterName = "STATUS", DbType = DbType.String, Value = "A"},
                                new SqlParameter() {ParameterName = "STORAGECON", DbType = DbType.String, Value = ""},
                                new SqlParameter() {ParameterName = "UNIT", DbType = DbType.String, Value = dt.Rows[i]["UNIT"]},
                                new SqlParameter() {ParameterName = "LASTUSER", DbType = DbType.String, Value = "SAPUSER"},
                                new SqlParameter() {ParameterName = "LASTUPDATE", DbType = DbType.DateTime, Value = MDateTime},
                                new SqlParameter() {ParameterName = "CREATEDBY", DbType = DbType.String, Value = "SAPUSER"},
                                new SqlParameter() {ParameterName = "CREATEDON", DbType = DbType.DateTime, Value = MDateTime}
                                };
                                        this.dbManager.ExecuteCommand(insertCommand.ToString(), parameters.ToArray());
                                    }
                            }
                        transaction.Complete();
                    }
                    catch (Exception)
                    {

                        transaction.Dispose();
                    }


                }
            }
            return "";
        }

        public string ClsFncValidate(DataSet Mdsdataset)
        {
            try
            {
               
              
             
                    string Lstr_1stApprove = string.Empty;
                    string Lstr_2ndApprove;
                    string Lstr_1stReject;
                    string Lstr_2ndReject = string.Empty;
                    int Lint_StartValue = 0;
                    int Lint_EndValue = 0;

                    for (int Lint_Loop = 0; Lint_Loop <= Mdsdataset.Tables["Product"].Rows.Count - 1; Lint_Loop++)
                    {
                    if (string.IsNullOrEmpty(Mdsdataset.Tables["Product"].Rows[Lint_Loop]["PRODCODE"].ToString()))
                    {

                        Mdsdataset.Tables["Product"].Rows[Lint_Loop]["Status"] = "N";
                        Mdsdataset.Tables["Product"].AcceptChanges();

                    }
                    else if (string.IsNullOrEmpty(Mdsdataset.Tables["Product"].Rows[Lint_Loop]["PRODSHTNAME"].ToString()))
                    {

                        Mdsdataset.Tables["Product"].Rows[Lint_Loop]["Status"] = "N";
                        Mdsdataset.Tables["Product"].AcceptChanges();
                    }
                    else if (string.IsNullOrEmpty(Mdsdataset.Tables["Product"].Rows[Lint_Loop]["PRODLNGNAME"].ToString()))
                    {

                        Mdsdataset.Tables["Product"].Rows[Lint_Loop]["Status"] = "N";
                        Mdsdataset.Tables["Product"].AcceptChanges();
                    }
                    else if (string.IsNullOrEmpty(Mdsdataset.Tables["Product"].Rows[Lint_Loop]["PRODCATECODE"].ToString()))
                    {

                        Mdsdataset.Tables["Product"].Rows[Lint_Loop]["Status"] = "N";
                        Mdsdataset.Tables["Product"].AcceptChanges();
                    }
                    else if (string.IsNullOrEmpty(Mdsdataset.Tables["Product"].Rows[Lint_Loop]["BATCHSIZE"].ToString()))
                    {

                        Mdsdataset.Tables["Product"].Rows[Lint_Loop]["Status"] = "N";
                        Mdsdataset.Tables["Product"].AcceptChanges();
                    }
                    
                    StringBuilder selectCommand = new StringBuilder();
                    DataTable dt = new DataTable();

                    selectCommand.Append("Select * from SPBSProdPRM ");
                    selectCommand.Append("  where ProdCode = @ProdCode");
                   

                    List<DbParameter> dbParameters = new List<DbParameter>()
                    {
                        new SqlParameter() {ParameterName = "ProdCode", Value =  Mdsdataset.Tables["Product"].Rows[Lint_Loop]["PRODCODE"]}
                    };
                    dt = this.dbManager.FetchData(selectCommand.ToString(), dbParameters.ToArray());

                    if (dt.Rows.Count > 0)
                    {
                        Mdsdataset.Tables["Product"].Rows[Lint_Loop]["Status"] = "N";
                        Mdsdataset.Tables["Product"].AcceptChanges();
                    }

                  
                }
            }
            catch (Exception)
            {
                return "";
            }
            return "";
        }
    }
}
