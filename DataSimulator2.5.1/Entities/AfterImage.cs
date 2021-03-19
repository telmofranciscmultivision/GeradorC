using Newtonsoft.Json;

namespace EdpSimulator.Entities
{
    public class AfterImage
    {
        public int HDDA_KEY_COMP {get;set;} = 20;
        public int HDDA_KEY_CONTA {get;set;} = 680;
        public string HDDA_KEY_DATA_LANCAMENTO {get;set;} = "2020-03-16";
        public string HDDA_KEY_ORIGEM {get;set;} = "3";
        public string HDDA_KEY_DATA_HORA {get;set;} = "2020-03-05T12:17:07.190353000000";
        public string HDDA_HST_REVERSAL {get;set;} = "N";
        public int HDDA_HST_CURR_DG {get;set;} = 3201;
        public string HDDA_HST_SUBPRD {get;set;} = "MG";
        public int HDDA_HST_TRAN_SIGN {get;set;} = -1;
        public string HDDA_HST_TRAN_TYPE {get;set;} = "D";
        public string HDDA_HST_GLI_SOURCE {get;set;} = "1";
        public int HDDA_HST_FUNC_ID {get;set;} = 8054;
        public int HDDA_HST_SOURCE_TYPE {get;set;} = 3;
        public int HDDA_HST_ENVIRONMENT {get;set;} = 1;
        public string HDDA_HST_EFF_DATE {get;set;} = "2020-03-05";
        public int HDDA_HST_TRAN_COD_REVERS {get;set;} = 8055;
        public string HDDA_HST_DEP_WD {get;set;} = "W";
        public string HDDA_HST_TAMT {get;set;} = "10.01";
        public int HDDA_HST_BRANCH {get;set;} = 105;
        public int HDDA_HST_TRANS_SEQ {get;set;} = 4;
        public string HDDA_HST_PAID_INTO_OD {get;set;} = " ";
        public string HDDA_HST_LEDG_BAL_AFP {get;set;} = "255612469.95";
        public string HDDA_HST_COLL_BAL_AFP {get;set;} = "255612469.95";
        public int HDDA_HST_TRANS_NO {get;set;} = 7734;
        public int HDDA_HST_TCK_SERIAL_NO {get;set;} = 3;
        public string HDDA_HST_TRACE_ID {get;set;} = "TNP200303105222O8905";
        public string HDDA_HST_CASH_AMT {get;set;} = "10.01";
        public string HDDA_HST_INTERN_GEN {get;set;} = " ";
        public int HDDA_HST_HORA {get;set;} = 121706;
        public string HDDA_HST_DATA_SISTEMA {get;set;} = "2020-03-05";
        public string HDDA_HST_SOURCE {get;set;} = "#12     ";
        public string HDDA_HST_OPER {get;set;} = "OPCDDES             ";
        public string HDDA_HST_DESCR {get;set;} = "TRF P/ NOME DST                                ";  // len 47
        public int HDDA_HST_ACT_NO_TO {get;set;} = 0;
        public int HDDA_HST_ACT_NO_FROM {get;set;} = 0;
        public string HDDA_HST_VAMT_FLOAT_0 {get;set;} = "0.00";
        public int HDDA_HST_VDAY_FLOAT_0 {get;set;} = 0;
        public string HDDA_HST_VAMT_FLOAT_1 {get;set;} = "0.00";
        public int HDDA_HST_VDAY_FLOAT_1 {get;set;} = 0;
        public string HDDA_HST_VAMT_FLOAT_2 {get;set;} = "0.00";
        public int HDDA_HST_VDAY_FLOAT_2 {get;set;} = 0;
        public string HDDA_HST_VAMT_FLOAT_3 {get;set;} = "0.00";
        public int HDDA_HST_VDAY_FLOAT_3 {get;set;} = 0;
        public string HDDA_HST_TRACE_ID_CONT {get;set;} = "          ";
        public string HDDA_HST_INPUT_MOEDA {get;set;} = "EUR";
        public string HDDA_HST_ORIG_MOEDA {get;set;} = "EUR";
        public string HDDA_HST_ORIG_CURR_TAMT {get;set;} = "10.01";
        public int HDDA_HST_ORIG_BRANCH {get;set;} = 0;
        public string HDDA_HST_STMT_REVERS_CODE {get;set;} = " ";
        public string HDDA_HST_LIM_TOL_USADO {get;set;} = "0.00";
        public string HDDA_HST_CANAL {get;set;} = "XPPI";
        public int HDDA_HST_ID_PRIORIT {get;set;} = 0;
        public string HDDA_HST_CANAL2 {get;set;} = "    ";
        public string HDDA_HST_TRATADO {get;set;} = " ";
        public string HDDA_HST_AVAILABLE_BALANCE {get;set;} = "255612469.95";
        public string HDDA_HST_ADJ_BAL {get;set;} = "0.00";
        public string HDDA_HST_COLL_BAL {get;set;} = "0.00";
        public string HDDA_HST_SALDO_AUTORIZADO {get;set;} = "0.00";
        public string HDDA_HST_SLD_DISP_BD {get;set;} = "255596764.95";
        public string HDDA_HST_LINE_AMT {get;set;} = "0.00";
        public string HDDA_HST_MEMO_DR {get;set;} = "0.00";
        public string HDDA_HST_MEMO_CR {get;set;} = "0.00";
        public string HDDA_HST_CATIVOS_DEBITO {get;set;} = "15715.01";
        public string HDDA_HST_CATIVOS_CREDITO {get;set;} = "0.00";
        public string HDDA_HST_SLD_DISP_LCA {get;set;} = "0.00";
        public string HDDA_HST_INVESTMENT_BAL {get;set;} = "0.00";
        public string HDDA_HST_AVAIL_LOC {get;set;} = "0.00";
        public string HDDA_HST_SSV_LIM_AUTORIZ {get;set;} = "0.00";
        public string HDDA_HST_SLD_CONTAB {get;set;} = "255612469.95";
        public string HDDA_HST_DESCR_CONT {get;set;} = "                                                     ";  // len 53

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}