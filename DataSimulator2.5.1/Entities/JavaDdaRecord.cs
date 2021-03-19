// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace EdpSimulator.Entities
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	[Serializable]
	public partial class JavaDdaRecord : IEdpRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"AuditRecord\",\"namespace\":\"EdpSimulator.Entities\",\"fields\":[{\"name" +
				"\":\"beforeImage\",\"type\":[\"null\",{\"type\":\"record\",\"name\":\"HMV_DDA\",\"namespace\":\"va" +
				"lue.SOURCEDB.DEP000PA.DEPPA\",\"fields\":[{\"name\":\"HDDA_KEY_COMP\",\"default\":0,\"type" +
				"\":\"int\"},{\"name\":\"HDDA_KEY_CONTA\",\"default\":0,\"type\":\"long\"},{\"name\":\"HDDA_KEY_D" +
				"ATA_LANCAMENTO\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_KEY_ORIGEM\",\"default" +
				"\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_KEY_DATA_HORA\",\"default\":\"\",\"type\":\"string\"}" +
				",{\"name\":\"HDDA_HST_REVERSAL\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_STM" +
				"T_REVERS\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_CURR_DG\",\"default\":0,\"" +
				"type\":\"int\"},{\"name\":\"HDDA_HST_SUBPRD\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HD" +
				"DA_HST_TRAN_SIGN\",\"default\":0,\"type\":\"int\"},{\"name\":\"HDDA_HST_TRAN_TYPE\",\"defaul" +
				"t\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_GLI_SOURCE\",\"default\":\"\",\"type\":\"string" +
				"\"},{\"name\":\"HDDA_HST_FUNC_ID\",\"default\":0,\"type\":\"int\"},{\"name\":\"HDDA_HST_SOURCE" +
				"_TYPE\",\"default\":0,\"type\":\"int\"},{\"name\":\"HDDA_HST_ENVIRONMENT\",\"default\":0,\"typ" +
				"e\":\"int\"},{\"name\":\"HDDA_HST_EFF_DATE\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDD" +
				"A_HST_TRAN_COD_REVERS\",\"default\":0,\"type\":\"int\"},{\"name\":\"HDDA_HST_DEP_WD\",\"defa" +
				"ult\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_TAMT\",\"default\":\"0\",\"type\":\"string\"}," +
				"{\"name\":\"HDDA_HST_BRANCH\",\"default\":0,\"type\":\"int\"},{\"name\":\"HDDA_HST_TRANS_SEQ\"" +
				",\"default\":0,\"type\":\"int\"},{\"name\":\"HDDA_HST_PAID_INTO_OD\",\"default\":\"\",\"type\":\"" +
				"string\"},{\"name\":\"HDDA_HST_LEDG_BAL_AFP\",\"default\":\"0\",\"type\":\"string\"},{\"name\":" +
				"\"HDDA_HST_COLL_BAL_AFP\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_TRANS_N" +
				"O\",\"default\":0,\"type\":\"long\"},{\"name\":\"HDDA_HST_TCK_SERIAL_NO\",\"default\":0,\"type" +
				"\":\"long\"},{\"name\":\"HDDA_HST_TRACE_ID\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDD" +
				"A_HST_CASH_AMT\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_INTERN_GEN\",\"de" +
				"fault\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_HORA\",\"default\":0,\"type\":\"int\"},{\"n" +
				"ame\":\"HDDA_HST_DATA_SISTEMA\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_SOU" +
				"RCE\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_OPER\",\"default\":\"\",\"type\":\"" +
				"string\"},{\"name\":\"HDDA_HST_DESCR\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HS" +
				"T_ACT_NO_TO\",\"default\":0,\"type\":\"long\"},{\"name\":\"HDDA_HST_ACT_NO_FROM\",\"default\"" +
				":0,\"type\":\"long\"},{\"name\":\"HDDA_HST_VAMT_FLOAT_0\",\"default\":\"0\",\"type\":\"string\"}" +
				",{\"name\":\"HDDA_HST_VDAY_FLOAT_0\",\"default\":0,\"type\":\"long\"},{\"name\":\"HDDA_HST_VA" +
				"MT_FLOAT_1\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_VDAY_FLOAT_1\",\"defa" +
				"ult\":0,\"type\":\"long\"},{\"name\":\"HDDA_HST_VAMT_FLOAT_2\",\"default\":\"0\",\"type\":\"stri" +
				"ng\"},{\"name\":\"HDDA_HST_VDAY_FLOAT_2\",\"default\":0,\"type\":\"long\"},{\"name\":\"HDDA_HS" +
				"T_VAMT_FLOAT_3\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_VDAY_FLOAT_3\",\"" +
				"default\":0,\"type\":\"long\"},{\"name\":\"HDDA_HST_TRACE_ID_CONT\",\"default\":\"\",\"type\":\"" +
				"string\"},{\"name\":\"HDDA_HST_INPUT_MOEDA\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"H" +
				"DDA_HST_ORIG_MOEDA\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_ORIG_CURR_TA" +
				"MT\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_ORIG_BRANCH\",\"default\":0,\"t" +
				"ype\":\"int\"},{\"name\":\"HDDA_HST_STMT_REVERS_CODE\",\"default\":\"\",\"type\":\"string\"},{\"" +
				"name\":\"HDDA_HST_LIM_TOL_USADO\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_" +
				"CANAL\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_ID_PRIORIT\",\"default\":0,\"" +
				"type\":\"int\"},{\"name\":\"HDDA_HST_CANAL2\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HD" +
				"DA_HST_TRATADO\",\"default\":\"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_AVAILABLE_BALANC" +
				"E\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_ADJ_BAL\",\"default\":\"0\",\"type" +
				"\":\"string\"},{\"name\":\"HDDA_HST_COLL_BAL\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"" +
				"HDDA_HST_SALDO_AUTORIZADO\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_SLD_" +
				"DISP_BD\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_LINE_AMT\",\"default\":\"0" +
				"\",\"type\":\"string\"},{\"name\":\"HDDA_HST_MEMO_DR\",\"default\":\"0\",\"type\":\"string\"},{\"n" +
				"ame\":\"HDDA_HST_MEMO_CR\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_CATIVOS" +
				"_DEBITO\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_CATIVOS_CREDITO\",\"defa" +
				"ult\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_SLD_DISP_LCA\",\"default\":\"0\",\"type\":\"" +
				"string\"},{\"name\":\"HDDA_HST_INVESTMENT_BAL\",\"default\":\"0\",\"type\":\"string\"},{\"name" +
				"\":\"HDDA_HST_AVAIL_LOC\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_SSV_LIM_" +
				"AUTORIZ\",\"default\":\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_SLD_CONTAB\",\"default\":" +
				"\"0\",\"type\":\"string\"},{\"name\":\"HDDA_HST_DESCR_CONT\",\"default\":\"\",\"type\":\"string\"}" +
				"]}]},{\"name\":\"afterImage\",\"type\":[\"null\",\"value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA\"" +
				"]},{\"name\":\"A_ENTTYP\",\"default\":\"\",\"type\":[\"string\",\"null\"]},{\"name\":\"A_CCID\",\"d" +
				"efault\":\"\",\"type\":[\"string\",\"null\"]},{\"name\":\"A_TIMSTAMP\",\"default\":\"\",\"type\":[\"string\",\"" +
				"null\"]}]}	");
		private @value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA _beforeImage;
		private @value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA _afterImage;
		private string _A_ENTTYP;
		private string _A_CCID;
		private string _A_TIMSTAMP;
		public virtual Schema Schema
		{
			get
			{
				return JavaDdaRecord._SCHEMA;
			}
		}
		public @value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA beforeImage
		{
			get
			{
				return this._beforeImage;
			}
			set
			{
				this._beforeImage = value;
			}
		}
		public @value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA afterImage
		{
			get
			{
				return this._afterImage;
			}
			set
			{
				this._afterImage = value;
			}
		}
		public string A_ENTTYP
		{
			get
			{
				return this._A_ENTTYP;
			}
			set
			{
				this._A_ENTTYP = value;
			}
		}
		public string A_CCID
		{
			get
			{
				return this._A_CCID;
			}
			set
			{
				this._A_CCID = value;
			}
		}
		public string A_TIMSTAMP
		{
			get
			{
				return this._A_TIMSTAMP;
			}
			set
			{
				this._A_TIMSTAMP = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.beforeImage;
			case 1: return this.afterImage;
			case 2: return this.A_ENTTYP;
			case 3: return this.A_CCID;
			case 4: return this.A_TIMSTAMP;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.beforeImage = (@value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA)fieldValue; break;
			case 1: this.afterImage = (@value.SOURCEDB.DEP000PA.DEPPA.HMV_DDA)fieldValue; break;
			case 2: this.A_ENTTYP = (System.String)fieldValue; break;
			case 3: this.A_CCID = (System.String)fieldValue; break;
			case 4: this.A_TIMSTAMP = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}

		public string GetTransactionId() =>
			afterImage.HDDA_KEY_COMP +
			afterImage.HDDA_KEY_CONTA +
			afterImage.HDDA_KEY_DATA_LANCAMENTO +
			afterImage.HDDA_KEY_ORIGEM +
			afterImage.HDDA_KEY_DATA_HORA;
		public string GetPartitionValue() => this.GetTransactionId();
		public string GetCosmosId() => Guid.NewGuid().ToString();
	}
}