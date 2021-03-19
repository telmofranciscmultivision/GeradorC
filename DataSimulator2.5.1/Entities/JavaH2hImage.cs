using Newtonsoft.Json;

namespace EdpSimulator.Entities
{
    public class JavaH2hImage
    {
        public long IDMSGBAN_1 {get;set;}
        public int IDMSGBAN_2 {get;set;}
        public long CONTA_CLI {get;set;}
        public string CODMSG_BS {get;set;} = "1111";
        public int VERMSG {get;set;} = 123456789;
        public long DHMSG {get;set;} = 123456789;
        public int CODRESPS {get;set;} = 123456789;
        public int DT_RECOLHA {get;set;} = 123456789;
        public string ESTADO_ANT {get;set;} = "1111";
        public string ESTADO_ACT {get;set;} = "1111";
        public int RETURN_CODE {get;set;} = 123456789;
        public string RETURN_MSG {get;set;} = "1111";
        public int TIPO_PEDIDO {get;set;} = 123456789;
        public int TIP_PAG_SER {get;set;} = 123456789;
        public string NIB_ORD {get;set;} = "1111";
        public int ENTIDADE {get;set;} = 123456789;
        public int REFERENCIA {get;set;} = 123456789;
        public long REF_ESP {get;set;} = 123456789;
        public int REF_ADICIONAL {get;set;} = 123456789;
        public int REF_RESPOSTA {get;set;} = 123456789;
        public string MONTANTE {get;set;} = "1111";
        public string REDE_ORIGEM {get;set;} = "1111";
        public string CANAL_ORIGEM {get;set;} = "1111";
        public string USERID_CLI {get;set;} = "1111";
        public string CAT_NR {get;set;} = "1111";
        public string CAT_CARTAO {get;set;} = "1111";
        public string LOCMORTER {get;set;} = "1111";
        public string CHAVE_TRF {get;set;} = "1111";
        public string APLIC_N {get;set;} = "1111";
        public int ID_LOG_SIBS {get;set;} = 123456789;
        public int NR_LOG_SIBS {get;set;} = 123456789;
        public int NR_CONTRIBUINTE {get;set;} = 123456789;
        public int NR_FACTURA {get;set;} = 123456789;
        public string DESCENT {get;set;} = "1111";
        public int TIPSER {get;set;} = 123456789;
        public string CODMOEDA {get;set;} = "1111";
        public string CODRECS {get;set;} = "1111";
        public string RESP1 {get;set;} = "1111";
        public string RESP2 {get;set;} = "1111";
        public int BANCO_CLI {get;set;} = 123456789;
        public int BALCAO_CLI {get;set;} = 123456789;
        public int CKD_CLI {get;set;} = 123456789;
        public string DH_INS {get;set;} = "1111";
        public string CONFIRMACAO_SIBS {get;set;} = "1111";
        public string DH_CONFIRMACAO {get;set;} = "1111";
        public string USERID_ALT {get;set;} = "1111";
        public string DH_ALT {get;set;} = "1111";
        public string SIBS_PEDIDO {get;set;} = "1111";
        public string SIBS_RESPOSTA {get;set;} = "1111";
        public string MEIO_PAGAMENTO {get;set;} = "1111";
        public long PAN_CARTAO {get;set;} = 123456789;
        public string FATURA_SIMPLIF {get;set;} = "1111";
        public string RODAPE_FAT_SIMP {get;set;} = "1111";
        public string COD_JANELA {get;set;} = "1111";


        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}