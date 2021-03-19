using Newtonsoft.Json;

namespace EdpSimulator.Entities
{
    public class JavaCardsImage
    {

        public string TXN_DT{get;set;}
        public int TXN_TM{get;set;}
        public string TXN_AMT{get;set;}
        public string TXN_SINAL{get;set;}
        public string ORIG_PLAS{get;set;}
        //public int ACCT_COID{get;set;} = 123456789;
        public string ACCT_PROD{get;set;} = "DDA";
        //public string ACCT_NBR{get;set;} = "1111";
        public string FILE_ORIG{get;set;} = "1111";
        public int PSTG_DT{get;set;} = 123456789;
        public long PSTG_SEQ{get;set;} = 123456789;
        public string TIPO_CRT{get;set;} = "1111";
        public string TXN_FMT_IND{get;set;} = "1111";
        public string TXN_SRCE{get;set;} = "1111";
        public string TXN_CATG{get;set;} = "1111";
        public string TXN_3RD_LVL{get;set;} = "1111";
        public string TXN_4TH_LVL{get;set;} = "1111";
        public string TXN_5TH_LVL{get;set;} = "1111";
        public string PRT_ON_STMT{get;set;} = "1111";
        public int TXN_CD{get;set;} = 123456789;
        public string TXN_DESC{get;set;} = "1111";
        public string TXN_CNTRY_CD{get;set;} = "1111";
        public string TXN_FGN_CUR_CD{get;set;} = "1111";
        public string TXN_FGN_AMT{get;set;} = "1111";
        public string TXN_FGN_TC{get;set;} = "1111";
        public string TXN_OPER{get;set;} = "1111";
        public string TXN_MCC{get;set;} = "1111";
        public string TXN_TERM_ID{get;set;} = "1111";
        public string TIPO_OPER{get;set;} = "1111";
        public string TIPO_TERM{get;set;} = "1111";
        public string TIPO_AUTH_CLI{get;set;} = "1111";
        public string TIPO_AUTH_CRT{get;set;} = "1111";
        public string MRCH_DESC{get;set;} = "1111";
        public int NAT_OPER{get;set;} = 123456789;
        public string TOKEN_LVL{get;set;} = "1111";
        public string WALTIP{get;set;} = "1111";
        public string APLIC_TXN{get;set;} = "1111";
        public string AVAIL_CR_LN{get;set;} = "1111";
        public string EMB_NM_LN_1{get;set;} = "1111";
        public string PLAS_TENANT{get;set;} = "1111";
        public int NUM_CIS{get;set;} = 123456789;
        public string ACCT_FIID{get;set;} = "1111";
        public string ACCT_TYP_CD{get;set;} = "1111";
        public string CTL_ACCT_IND{get;set;} = "1111";
        public string ACCT_CUR_CD{get;set;} = "1111";
        public string OFF{get;set;} = "1111";
        public int CTL_ACCT_COID{get;set;} = 123456789;
        public string CTL_ACCT_PROD{get;set;} = "1111";
        public string CTL_ACCT_NBR{get;set;} = "1111";
        public string TIPO_MIG{get;set;} = "1111";
        public string RA_IBAN{get;set;} = "1111";
        public string BANCO{get;set;} = "1111";
        public string REF_SIST_ORIG{get;set;} = "1111";
        public string IBAN_ORDEN{get;set;} = "1111";
        public string NOME_ORDEN{get;set;} = "1111";
        public string IBAN_DEST{get;set;} = "1111";
        public string NOME_DEST{get;set;} = "1111";
        public int ENTIDADE{get;set;} = 123456789;
        public int REFERENCIA{get;set;} = 123456789;
        public long REF_PAG_ESTADO{get;set;} = 123456789;
        public string USER_INS{get;set;} = "1111";
        public string TIMESTAMP_INS{get;set;} = "1111";
        public string USER_ALT{get;set;} = "1111";
        public string TIMESTAMP_ALT{get;set;} = "1111";
        public string LOG_ACCAO{get;set;} = "1111";
        public int ODS_PSTG_DT{get;set;} = 123456789;
        public long ODS_PSTG_SEQ{get;set;} = 123456789;
        public int CAM_AU_PSTG_DT{get;set;} = 123456789;
        public long CAM_AU_PSTG_SEQ{get;set;} = 123456789;
        public int CAM_PSTG_DT{get;set;} = 123456789;
        public long CAM_PSTG_SEQ{get;set;} = 123456789;
        public long SEQ_ID{get;set;} = 123456789;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}